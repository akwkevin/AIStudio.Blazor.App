using AIStudio.Common.Service;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.OA_Manage;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace AIStudio.Business.OA_Manage.Steps
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public class OAExtension
    {
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="json"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string InitOAData(string json, string id)
        {
            var oaData = json.ToObject<OAData>();
            List<OAStep> oASteps = new List<OAStep>();
            if (oaData.nodes.Count(p => p.name == "开始节点") != 1)
            {
                throw new Exception("开始节点的个数不等于1个");
            }

            if (oaData.nodes.Count(p => p.name == "结束节点") != 1)
            {
                throw new Exception("结束节点的个数不等于1个");
            }

            if (oaData.nodes.Count(p => p.name.Contains("并行")) % 2 != 0)
            {
                throw new Exception("并行节点的个数不是成对出现");
            }

            oaData.Id = id;
            oaData.DataType = StepType.Data;
            oaData.Steps = new List<OAStep>();

            foreach (var node in oaData.nodes)
            {
                OAStep oAStep = new OAStep();
                oAStep.Id = node.id;
                oAStep.Label = node.label;
                oAStep.StepType = NameToType(node.name);
                oAStep.ActRules = new ActRule();
                oAStep.ActRules.UserIds = node.UserIds;
                oAStep.ActRules.RoleIds = node.RoleIds;
                oAStep.ActRules.ActType = node.ActType;
                oASteps.Add(oAStep);
            }

            foreach (var edge in oaData.edges)
            {
                var source = oASteps.FirstOrDefault(p => p.Id == edge.sourceId);
                if (source != null)
                {
                    if (source.StepType == StepType.Decide)
                    {
                        source.SelectNextStep.Add(edge.targetId, "data.Flag" + edge.label);
                    }
                    else if (source.StepType == StepType.COBegin)
                    {
                        source.SelectNextStep.Add(edge.targetId, "True");
                    }
                    else
                    {
                        source.NextStepId = edge.targetId;
                    }
                }
            }

            var oAStartStep = oASteps.Single(p => p.StepType == StepType.Start);
            if (string.IsNullOrEmpty(oAStartStep.NextStepId))
            {
                throw new Exception("开始节点没有下一个节点");
            }
            oaData.Steps.Add(oAStartStep);
            oASteps.Remove(oAStartStep);

            string nextstepid = oAStartStep.NextStepId;
            oaData.Steps.AddRange(GetNextStep(oASteps, nextstepid));

            //var oAEndStep = oaData.Steps.FirstOrDefault(p => p.StepType == StepType.End);
            //if (oAEndStep != null)
            //{
            //    oaData.Steps.Remove(oAEndStep);
            //    oaData.Steps.Add(oAEndStep);
            //}

            return JsonConvert.SerializeObject(oaData);
        }

        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="oASteps"></param>
        /// <param name="nextstepid"></param>
        /// <returns></returns>
        public static List<OAStep> GetNextStep(List<OAStep> oASteps, string nextstepid)
        {
            List<OAStep> outsteps = new List<OAStep>();
            List<string> nextids = new List<string>();
            var step = oASteps.FirstOrDefault(p => p.Id == nextstepid);
            if (step != null)
            {
                if (!string.IsNullOrEmpty(step.NextStepId))
                {
                    nextids.Add(step.NextStepId);
                }
                else if (step.SelectNextStep != null && step.SelectNextStep.Count > 0)
                {
                    nextids.AddRange(step.SelectNextStep.Keys);
                }

                outsteps.Add(step);
                oASteps.Remove(step);
            }

            int index = outsteps.IndexOf(step);

            nextids.Reverse();
            foreach (var next in nextids)
            {
                outsteps.InsertRange(index + 1, GetNextStep(oASteps, next));
            }
            return outsteps;
        }

        /// <summary>
        /// 名字转化成节点
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string NameToType(string name)
        {
            switch (name)
            {
                case "开始节点": return StepType.Start;
                case "中间节点": return StepType.Middle;
                case "结束节点": return StepType.End;
                case "条件节点": return StepType.Decide;
                case "并行开始节点": return StepType.COBegin;
                case "并行结束节点": return StepType.COEnd;
                default: return StepType.Normal;
            }
        }

        /// <summary>
        /// 初始化步骤
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<OAData> InitOAStep(OA_UserFormDTO data, IServiceProvider serviceProvider)
        {
            IBase_DepartmentBusiness _base_DepartmentBusiness = serviceProvider.GetRequiredService<IBase_DepartmentBusiness>();
            IBase_UserBusiness _base_UserBusiness = serviceProvider.GetRequiredService<IBase_UserBusiness>();
            IBase_RoleBusiness _base_RoleBusiness = serviceProvider.GetRequiredService<IBase_RoleBusiness>();
            IBase_UserRoleBusiness _base_UserRoleBusiness = serviceProvider.GetRequiredService<IBase_UserRoleBusiness>();
            Base_Department department = null;

            if (!string.IsNullOrEmpty(data.ApplicantDepartmentId))
            {
                department = await _base_DepartmentBusiness.GetEntityAsync(data.ApplicantDepartmentId);
                if (department != null)
                {
                    data.ApplicantDepartment = department.Name;
                }
            }
            var userlist = await _base_UserBusiness.GetListAsync();
            var rolelist = await _base_RoleBusiness.GetListAsync();
            var userrolelist = await _base_UserRoleBusiness.GetListAsync();
            OAData oAData = data.WorkflowJSON.ToObject<OAData>();
            oAData.Flag = data.Flag;

            foreach (var step in oAData.Steps)
            {
                //恢复指向上一个节点
                if (!string.IsNullOrEmpty(step.NextStepId))
                {
                    var nextstep = oAData.Steps.FirstOrDefault(p => p.Id == step.NextStepId);
                    if (nextstep == null)
                        throw new Exception(string.Format("流程异常，无法找到{0}的下一个流程节点{1}", step.Id, step.NextStepId));
                    if (nextstep.StepType == StepType.Decide)
                    {
                        var nsteps = oAData.Steps.Where(p => nextstep.SelectNextStep.Any(q => p.Id == q.Key));
                        if (nsteps == null || nsteps.Count() == 0)
                            throw new Exception(string.Format("流程异常，无法找到{0}的下一个流程节点{1}", step.Id, step.NextStepId));

                        //跳过Decide指向下面的子节点
                        foreach (var nstep in nsteps)
                        {
                            if (nstep.PreStepId == null)
                                nstep.PreStepId = new List<string>();
                            nstep.PreStepId.Add(step.Id);
                        }
                    }
                    else
                    {
                        if (nextstep.PreStepId == null)
                            nextstep.PreStepId = new List<string>();
                        nextstep.PreStepId.Add(step.Id);
                    }
                }
                //查找审批人
                if (step.ActRules?.UserIds != null && step.ActRules?.UserIds.Count > 0)
                {
                    var usernames = userlist.Where(p => step.ActRules.UserIds.Contains(p.Id)).ToList();
                    step.ActRules.UserNames = usernames.Select(p => p.UserName).ToList();
                    step.ActRules.UserIds = usernames.Select(p => p.Id).ToList();
                    continue;
                }

                //查找审批角色,采用该申请者所在部门的角色进行查找，找不到往上一级
                if (step.ActRules?.RoleIds != null && step.ActRules?.RoleIds.Count > 0)
                {
                    if (department == null)
                    {
                        throw new Exception(string.Format("流程异常，该申请者没有部门，无法找到{0}的角色为{1}的OA审批人", step.Id, step.ActRules?.RoleNames));
                    }
                    var theroles = rolelist.Where(p => step.ActRules.RoleIds.Contains(p.Id)).ToList();
                    step.ActRules.RoleNames = theroles.Select(p => p.RoleName).ToList();
                    step.ActRules.RoleIds = theroles.Select(p => p.Id).ToList();

                    //待处理部门审批
                    //var departmentids = department.ParentIds.Split(new string[] { "^" }, StringSplitOptions.RemoveEmptyEntries).Reverse().ToList();
                    //departmentids.Insert(0, department.Id);
                    //var userroles = userrolelist.Where(p => step.ActRules.RoleIds.Contains(p.RoleId)).ToList();

                    //bool success = false;
                    //foreach (var departmentid in departmentids)
                    //{
                    //    var roleuser = userlist.FirstOrDefault(p => p.DepartmentId == departmentid && userroles.Any(q => q.UserId == p.Id));
                    //    if (roleuser != null)
                    //    {
                    //        step.ActRules.UserNames = new List<string> { roleuser.UserName };
                    //        step.ActRules.UserIds = new List<string> { roleuser.Id };
                    //        success = true;
                    //        break;
                    //    }
                    //}

                    //if (success == false)
                    //{
                    //    throw new Exception(string.Format("流程异常，无法找到{0}的角色为{1}的OA审批人", step.Id, step.ActRules?.RoleNames));
                    //}
                    //else
                    //{
                    //    continue;
                    //}
                }

                Type type = Type.GetType(step.StepType, true, true);

                if (type.Name == nameof(OAStartStep))
                {
                    step.ActRules.UserNames = new List<string> { data.CreatorName };
                    step.ActRules.UserIds = new List<string> { data.CreatorId };
                }
                else if (type.IsSubclassOf(typeof(OABaseStep)) && !typeof(IEndStep).IsAssignableFrom(type))
                {
                    throw new Exception(string.Format("流程异常，{0}没有设置OA审批人", step.Id));
                }

                //最后找不到的，如果需要审批那么会指向系统管理员
            }

            return oAData;
        }


    }

}
