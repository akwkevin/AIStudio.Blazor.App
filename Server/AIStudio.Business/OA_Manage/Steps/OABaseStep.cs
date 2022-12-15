using AIStudio.Common.CurrentUser;
using AIStudio.Common.IdGenerator;
using AIStudio.Entity.OA_Manage;
using AIStudio.Util;
using Microsoft.Extensions.DependencyInjection;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using AIStudio.Util.DiagramEntity;
using AIStudio.Entity.DTO.OA_Manage;

namespace AIStudio.Business.OA_Manage.Steps
{
    /// <summary>
    /// 基类
    /// </summary>
    public abstract class OABaseStep : StepBodyAsync
    {

        protected IOA_UserFormStepBusiness _userFormStepBusiness;
        protected IOA_UserFormBusiness _userFormBusiness;
        protected IWorkflowRegistry _registry;

        protected OA_Step OAStep { get; set; }

        /// <summary>
        /// 基类
        /// </summary>
        /// <param name="userFormStepBusiness"></param>
        /// <param name="userFormBusiness"></param>
        /// <param name="registry"></param>
        /// <param name="operator"></param>
        public OABaseStep(IOA_UserFormStepBusiness userFormStepBusiness, IOA_UserFormBusiness userFormBusiness, IWorkflowRegistry registry)
        {
            _userFormStepBusiness = userFormStepBusiness;
            _userFormBusiness = userFormBusiness;
            _registry = registry;
        }

        /// <summary>
        /// 节点触发
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            OA_Data oAData = GetStep(context);
            if (OAStep.Status == (int)OA_Status.Default)
            {
                OAStep.Status = (int)OA_Status.Being;
            }

            if (!context.ExecutionPointer.EventPublished)
            {
                if (OAStep.Status != (int)OA_Status.PartialApproval)
                {
                    var form = await _userFormBusiness.GetEntityAsync(context.Workflow.Id);
                    if (form == null)
                        throw new ArgumentException();

                    oAData.CurrentStepIds.Add(new CurrentStepId() { StepId = OAStep.Id, StepLabel = OAStep.Label, ActRules = OAStep.ActRules });

                    SetFormCurrentStepIds(form, oAData.CurrentStepIds);
                    await _userFormBusiness.UpdateDataAsync(form);

                    //改变流程图颜色
                    var node = oAData.Nodes.FirstOrDefault(p => p.Id == OAStep.Id);
                    if (node != null)
                    {
                        node.Color = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.Orange);
                    }
                }

                return ExecutionResult.WaitForEvent("MyEvent", context.Workflow.Id + OAStep.Id, DateTime.Now.ToUniversalTime());
            }

            if (!(context.ExecutionPointer.EventData is MyEvent))
                throw new ArgumentException();
            {
                MyEvent myEvent = context.ExecutionPointer.EventData as MyEvent;

                if (!context.ExecutionPointer.ExtensionAttributes.ContainsKey("ActionUser") || context.ExecutionPointer.ExtensionAttributes["ActionUser"]?.ToString() != myEvent.UserId)
                {
                    OA_UserFormStep step = new OA_UserFormStep()
                    {
                        Id = IdHelper.GetId(),
                        UserFormId = context.Workflow.Id,
                        CreatorId = myEvent.UserId,
                        CreatorName = myEvent.UserName,
                        RoleIds = string.Join(",", OAStep.ActRules?.RoleIds??new List<string>()),
                        RoleNames = string.Join(",", OAStep.ActRules?.RoleNames??new List<string>()),
                        Remarks = myEvent.Remarks,
                        Status = myEvent.Status,
                        CreateTime = DateTime.Now,
                    };

                    string error = CheckEvent(context, myEvent, oAData);
                    if (!string.IsNullOrEmpty(error))
                    {
                        step.Status = (int)OA_Status.Fail;
                        step.Remarks += error;
                    }
                    await _userFormStepBusiness.AddDataAsync(step);
                    await _userFormBusiness.QueueWork(context.Workflow.Id + OAStep.Id);
                    if (!string.IsNullOrEmpty(error))
                    {
                        context.ExecutionPointer.EventPublished = false;
                        return ExecutionResult.WaitForEvent("MyEvent", context.Workflow.Id + OAStep.Id, DateTime.Now.ToUniversalTime());
                    }                    

                    context.ExecutionPointer.ExtensionAttributes["ActionUser"] = myEvent.UserId;

                   
                }

                if (context.PersistenceData == null)
                {
                    var result = ExecutionResult.Branch(new List<object>() { null }, new ControlPersistenceData() { ChildrenActive = true });
                    result.OutcomeValue = myEvent.Status;
                    return result;
                }

                if ((context.PersistenceData is ControlPersistenceData) && ((context.PersistenceData as ControlPersistenceData).ChildrenActive))
                {
                    if (context.Workflow.IsBranchComplete(context.ExecutionPointer.Id))
                    {
                        if (await FinishStep(context, myEvent, oAData) == OA_Status.PartialApproval)
                        {
                            context.ExecutionPointer.EventPublished = false;
                            return ExecutionResult.WaitForEvent("MyEvent", context.Workflow.Id + OAStep.Id, DateTime.Now.ToUniversalTime());
                        }
                        else
                        {
                            context.ExecutionPointer.EventPublished = false;
                            return ExecutionResult.Next();
                        }
                    }
                    else
                    {
                        var result = ExecutionResult.Persist(context.PersistenceData);
                        result.OutcomeValue = myEvent.Status;
                        return result;
                    }
                }
            }
            throw new ArgumentException("PersistenceData");
        }

        protected async Task<OA_Status> FinishStep(IStepExecutionContext context, MyEvent myEvent, OA_Data oAData)
        {
            var form = await _userFormBusiness.GetEntityAsync(context.Workflow.Id);
            if (form == null)
                throw new ArgumentException();

            if (string.IsNullOrEmpty(form.AlreadyUserNames))
            {
                form.AlreadyUserNames = "^" + myEvent.UserName + "^";
                form.AlreadyUserIds = "^" + myEvent.UserId + "^";
            }
            else
            {
                form.AlreadyUserNames += myEvent.UserName + "^";
                form.AlreadyUserIds += myEvent.UserId + "^";
            }
            form.ModifyId = myEvent.UserId;
            form.ModifyName = myEvent.UserName;
            form.ModifyTime = DateTime.Now;

            var currentstepid = oAData.CurrentStepIds.FirstOrDefault(p => p.StepId == OAStep.Id);
            switch ((OA_Status)myEvent.Status)
            {
                case OA_Status.Approve:
                    {
                        if (OAStep.ActRules.ActType == "and")//与签
                        {
                            if (currentstepid.ActRules?.UserIds?.Count > 1)
                            {
                                //部分审批
                                myEvent.Status = (int)OA_Status.PartialApproval;

                                currentstepid.ActRules?.UserIds.Remove(myEvent.UserId);
                                currentstepid.ActRules?.UserNames.Remove(myEvent.UserName);
                                SetFormCurrentStepIds(form, oAData.CurrentStepIds);
                            }
                        }

                        if (context.Step.Outcomes.Count == 0 && myEvent.Status != (int)OA_Status.PartialApproval)
                        {
                            form.Status = myEvent.Status;
                        }
                        else if (context.Step.Outcomes.Count == 1 && context.Step.Outcomes[0].ExternalNextStepId != OAStep.NextStepId)
                        {
                            //修复下一个节点
                            var def = _registry.GetDefinition(context.Workflow.WorkflowDefinitionId, context.Workflow.Version);
                            var next = def.Steps.Find(p => p.ExternalId == OAStep.NextStepId);
                            context.Step.Outcomes[0].NextStep = next.Id;
                            context.Step.Outcomes[0].ExternalNextStepId = next.ExternalId;
                        }
                        break;
                    }
                case OA_Status.Discard:
                case OA_Status.Reject:
                    {
                        context.Step.Outcomes.Clear();
                        form.Status = myEvent.Status;
                        break;
                    }
                case OA_Status.Goback:
                    {
                        if (OAStep.PreStepId != null && OAStep.PreStepId.Count == 1 && context.Step.Outcomes.Count >= 1)
                        {
                            var def = _registry.GetDefinition(context.Workflow.WorkflowDefinitionId, context.Workflow.Version);
                            var pre = def.Steps.Find(p => p.ExternalId == OAStep.PreStepId[0]);
                            var step = oAData.Steps.FirstOrDefault(p => p.Id == pre.ExternalId);
                            if (step.StepType == StepType.COBegin || step.StepType == StepType.End)
                            {
                                throw new Exception("操作失败，该节点不支持驳回上一级");
                            }
                            context.Step.Outcomes[0].NextStep = pre.Id;
                            context.Step.Outcomes[0].ExternalNextStepId = pre.ExternalId;                           
                            step.Status = (int)OA_Status.Being;

                        }
                        else
                        {
                            throw new Exception("操作失败，该节点不支持驳回上一级");
                        }
                        break;
                    }
                case OA_Status.Restart:
                    {
                        var def = _registry.GetDefinition(context.Workflow.WorkflowDefinitionId, context.Workflow.Version);
                        var pre = def.Steps.Find(p => p.Id == 0);
                        context.Step.Outcomes[0].NextStep = pre.Id;
                        context.Step.Outcomes[0].ExternalNextStepId = pre.ExternalId;
                        var step = oAData.Steps.FirstOrDefault(p => p.Id == pre.ExternalId);
                        step.Status = (int)OA_Status.Being;
                        break;
                    }
            }

            //如果不是部分审批
            if (myEvent.Status != (int)OA_Status.PartialApproval)
            {
                oAData.CurrentStepIds.Remove(currentstepid);
            }
            OAStep.Status = myEvent.Status;

            await _userFormBusiness.UpdateAsync(form);

            //改变流程图颜色
            var node = oAData.Nodes.FirstOrDefault(p => p.Id == OAStep.Id);
            if (node != null)
            {
                switch ((OA_Status)myEvent.Status)
                {
                    case OA_Status.Approve:
                        {
                            node.Color = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.LightGreen); break;
                        }
                    case OA_Status.Discard:
                    case OA_Status.Reject:
                    case OA_Status.Goback:
                    case OA_Status.Restart:
                        {
                             node.Color = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.Red); break;
                        }
                }

            }

            return (OA_Status)myEvent.Status;
        }

        private string CheckEvent(IStepExecutionContext context, MyEvent myEvent, OA_Data oAData)
        {
            try
            {
                switch ((OA_Status)myEvent.Status)
                {
                    case OA_Status.Goback:
                        {
                            if (OAStep.PreStepId != null && OAStep.PreStepId.Count == 1 && context.Step.Outcomes.Count >= 1)
                            {
                                var step = oAData.Steps.FirstOrDefault(p => p.Id == OAStep.PreStepId[0]);
                                if (step.StepType == StepType.COBegin || step.StepType == StepType.End)
                                {
                                    throw new Exception(string.Format("\n{0}失败，该节点不支持{0}", ((OA_Status)myEvent.Status).GetDescription()));
                                }

                            }
                            else
                            {
                                throw new Exception(string.Format("\n{0}失败，该节点不支持{0}", ((OA_Status)myEvent.Status).GetDescription()));
                            }
                        }
                        break;
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        protected OA_Data GetStep(IStepExecutionContext context)
        {
            if (!(context.Workflow.Data is OA_Data))
                throw new ArgumentException();

            OA_Data oAData = context.Workflow.Data as OA_Data;
            if (OAStep == null)
            {
                OAStep = oAData.Steps.FirstOrDefault(p => p.Id == context.Step.ExternalId);
            }
            if (OAStep == null)
                throw new ArgumentException();

            return oAData;
        }

        private void SetFormCurrentStepIds(OA_UserForm form, List<CurrentStepId> currentStepIds)
        {
            var usernames = currentStepIds.SelectMany(p => p.ActRules?.UserNames??new List<string>());
            var userids = currentStepIds.SelectMany(p => p.ActRules?.UserIds ?? new List<string>());
            var rolenames = currentStepIds.SelectMany(p => p.ActRules?.RoleNames ?? new List<string>());
            var roleids = currentStepIds.SelectMany(p => p.ActRules?.RoleIds ?? new List<string>());

            form.UserNames = usernames.Count() > 0 ? "^" + string.Join("^", usernames) + "^" : null;
            form.UserIds = userids.Count() > 0 ? "^" + string.Join("^", userids) + "^" : null;
            form.UserRoleNames = rolenames.Count() > 0 ? "^" + string.Join("^", rolenames) + "^" : null;
            form.UserRoleIds = roleids.Count() > 0 ? "^" + string.Join("^", roleids) + "^" : null;

            form.CurrentNode = "^" + string.Join("^", currentStepIds.Select(p => p.StepLabel)) + "^";
        }
     
    }
}
