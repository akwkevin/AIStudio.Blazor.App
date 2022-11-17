using AIStudio.Business.Base_Manage;
using AIStudio.Common.CurrentUser;
using AIStudio.Common.DI;
using AIStudio.Entity.DTO.OA_Manage;
using AIStudio.Entity.OA_Manage;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using AIStudio.Util.Helper;
using AutoMapper;
using LinqKit;
using SqlSugar;
using System.Collections.Concurrent;
using WorkflowCore.Interface;
using WorkflowCore.Services.DefinitionStorage;
using WorkflowCore.Services;
using AIStudio.Entity.Base_Manage;
using AIStudio.Business.OA_Manage.Steps;
using NetTaste;
using AIStudio.Common.Service;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace AIStudio.Business.OA_Manage
{
    public class OA_UserFormBusiness : BaseBusiness<OA_UserForm>, IOA_UserFormBusiness, ITransientDependency
    {
        private readonly IOA_UserFormStepBusiness _oA_UserFormStepBusiness;
        private readonly IBase_DepartmentBusiness _base_DepartmentBusiness;
        private readonly IMapper _mapper;

        private readonly IOperator _operator;
        private readonly IPersistenceProvider _workflowStore;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IWorkflowHost _workflowHost;

        public OA_UserFormBusiness(ISqlSugarClient db,
            IOA_UserFormStepBusiness oA_UserFormStepBusiness,
            IBase_DepartmentBusiness base_DepartmentBusiness,
            IMapper mapper,
            IOperator @operator,
            IServiceProvider serviceProvider)
            : base(db)
        {
            _oA_UserFormStepBusiness = oA_UserFormStepBusiness;
            _base_DepartmentBusiness = base_DepartmentBusiness;
            _mapper = mapper;
            _operator = @operator;
            _workflowStore = serviceProvider.GetRequiredService<IPersistenceProvider>();
            _workflowRegistry = serviceProvider.GetRequiredService<IWorkflowRegistry>();
            _workflowHost = serviceProvider.GetRequiredService<IWorkflowHost>();
        }

        private static ConcurrentBag<string> _queues = new ConcurrentBag<string>();

        public Task QueueWork(string id)
        {
            _queues.Add(id);
            return Task.CompletedTask;
        }

        public async Task<string> DequeueWork(string id)
        {
            for (int i = 0; i < 30; i++)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(100));

                if (_queues.Contains(id))
                {
                    if (_queues.TryTake(out id))//3m超时
                        return id;
                }
            }

            return null;
        }



        #region 外部接口

        public async Task<PageResult<OA_UserFormDTO>> GetDataListAsync(PageInput<OA_UserFormInputDTO> input)
        {
            var q = GetIQueryable(input.SearchKeyValues);

            if (!input.Search.userId.IsNullOrEmpty())
            {
                q = q.Where(p => p.UserIds.Contains("^" + input.Search.userId + "^") && p.Status == (int)OAStatus.Being);
            }

            if (!input.Search.applicantUserId.IsNullOrEmpty())
            {
                q = q.Where(p => p.ApplicantUserId == input.Search.applicantUserId && p.Status == (int)OAStatus.Being);
            }

            if (!input.Search.alreadyUserIds.IsNullOrEmpty())
            {
                q = q.Where(p => p.AlreadyUserIds.Contains("^" + input.Search.alreadyUserIds + "^"));
            }

            if (!input.Search.creatorId.IsNullOrEmpty())
            {
                q = q.Where(p => p.CreatorId == input.Search.creatorId);
            }

            return await q.Select<OA_UserFormDTO>().GetPageResultAsync(input);
        }

        public int GetDataListCount(List<string> jsonids, OAStatus status)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<OA_UserForm>();

            if (!jsonids.IsNullOrEmpty())
            {
                where = where.And(p => jsonids.Contains(p.DefFormJsonId));
            }

            if ((int)status >= 0)
            {
                where = where.And(p => p.Status == (int)status);
            }

            return q.Where(where).Count();
        }

        public new async Task<OA_UserFormDTO> GetTheDataAsync(string id)
        {
            var forms = await GetIQueryable().Where(o => o.Id == id).Select<OA_UserFormDTO>().ToListAsync();
            Db.ThenMapper(forms, async item =>
            {
                item.Comments = await Db.Queryable<OA_UserFormStep>().Select<OA_UserFormStepDTO>().SetContextAsync(x => x.UserFormId, () => item.Id, item);
            });

            var formdto = forms.FirstOrDefault();

            var workflow = await _workflowStore.GetWorkflowInstance(id);
            OAData data = workflow.Data as OAData;
            formdto.Steps = data.Steps;
            if (data.CurrentStepIds != null)
            {
                var stepid = data.CurrentStepIds.FirstOrDefault(p => p.ActRules.UserIds != null && p.ActRules.UserIds.Contains(_operator?.UserId));
                if (stepid != null)
                {
                    formdto.CurrentStepId = stepid.StepId;
                    formdto.CurrentStepIndex = data.Steps.Select((p, index) => new { p, index }).Where(p => p.p.Id == stepid.StepId).FirstOrDefault().index;
                }
                else
                {
                    var step = data.Steps.LastOrDefault(p => p.Status != 0);
                    if (step != null)
                    {
                        formdto.CurrentStepIndex = data.Steps.IndexOf(step);
                    }
                    else
                    {
                        formdto.CurrentStepIndex = data.Steps.Count - 1;
                    }
                }
            }
            else
            {
                var step = data.Steps.LastOrDefault(p => p.Status != 0);
                if (step != null)
                {
                    formdto.CurrentStepIndex = data.Steps.IndexOf(step);
                }
                else
                {
                    formdto.CurrentStepIndex = data.Steps.Count - 1;
                }
            }

            formdto.WorkflowJSON = data.ToJson();

            return formdto;
        }

        public async Task<List<OAStep>> PreStepAsync(OA_UserFormDTO data)
        {
            if (data.ContainsProperty("CreatorId"))
                data.SetPropertyValue("CreatorId", _operator?.UserId);

            OAData oAData = await OAExtension.InitOAStep(data);

            return oAData.Steps;
        }

        public async Task SaveDataAsync(OA_UserFormDTO data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                OAData oAData = await OAExtension.InitOAStep(data);

                //去掉事务，sqlite不支持
                //var res = await _oA_UserFormBus.RunTransactionAsync(async () =>
                //{
                var def = _workflowRegistry.GetDefinition(data.DefFormJsonId, data.DefFormJsonVersion);

                var workflowId = await _workflowHost.StartWorkflow(def.Id, oAData);
                data.Id = workflowId;
                data.Status = (int)OAStatus.Being;
                await AddDataAsync(data);

                //自动通过第一个节点

                OA_UserFormStep step = new OA_UserFormStep();
                step.UserFormId = workflowId;
                step.RoleNames = "发起人";
                step.Remarks = "发起了流程";
                step.Status = (int)OAStatus.Approve;

                await _oA_UserFormStepBusiness.AddDataAsync(step);
                //});
                //if (!res.Success)
                //    throw res.ex;
            }
            else
            {
                await UpdateDataAsync(data);
            }
        }

        public async Task<AjaxResult> EventDataAsync(MyEvent eventData)
        {
            await _workflowHost.PublishEvent(eventData.EventName, eventData.EventKey, eventData);
            var result = await DequeueWork(eventData.EventKey);
            AjaxResult res = new AjaxResult
            {
                Success = true,
                Msg = result == null ? "已提交审批请求！" : "审批成功",
            };

            return res;
        }

        public async Task DisCardDataAsync(DisCardInput input)
        {
            var data = await GetTheDataAsync(input.id);
            data.Status = (int)OAStatus.Discard;

            OA_UserFormStep step = new OA_UserFormStep();
            step.UserFormId = input.id;
            step.CreatorId = _operator?.UserId;
            step.CreatorName = _operator?.UserName;
            step.CreateTime = DateTime.Now;
            step.RoleNames = "创建者";
            step.Remarks = input.remark;
            step.Status = (int)OAStatus.Discard;

            //去掉事务，sqlite不支持
            //var res = await _oA_UserFormBus.RunTransactionAsync(async () =>
            //{
            await _workflowHost.TerminateWorkflow(input.id);
            await UpdateDataAsync(data);
            await _oA_UserFormStepBusiness.AddDataAsync(step);
            //});
            //if (!res.Success)
            //    throw res.ex;
        }

        public async Task<bool> SuspendAsync(IdInputDTO input)
        {
            return await _workflowHost.SuspendWorkflow(input.id);
        }

        public async Task<bool> ResumeAysnc(IdInputDTO input)
        {
            return await _workflowHost.ResumeWorkflow(input.id);
        }

        public async Task TerminateAsync(IdInputDTO input)
        {
            if (await _workflowHost.TerminateWorkflow(input.id))
            {
                await DeleteDataAsync(new List<string>() { input.id });
            }
            else
            {
                throw new Exception("停止失败");
            }
        }

        #endregion



    }


}