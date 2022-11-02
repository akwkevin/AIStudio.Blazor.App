using AIStudio.Business.OA_Manage;
using AIStudio.Business.OA_Manage.Step;
using AIStudio.Common.CurrentUser;
using AIStudio.Entity.DTO.OA_Manage;
using AIStudio.Entity.OA_Manage;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WorkflowCore.Interface;

namespace AIStudio.Api.Controllers.OA_Manage
{
    [Route("/OA_Manage/[controller]/[action]")]
    public class OA_UserFormController : ApiControllerBase
    {
        #region DI

        public OA_UserFormController(
            IOA_UserFormBusiness oA_UserFormBus,
            IOA_UserFormStepBusiness oA_UserFormStepBusiness,
            IBase_DepartmentBusiness base_DepartmentBusiness,
            IDefinitionLoader definitionLoader,
            IPersistenceProvider workflowStore,
            IWorkflowRegistry workflowRegistry,
            IWorkflowHost workflowHost,
            IOperator ioperator,
            IBase_UserBusiness userBus,
            IMapper mapper)
        {
            _oA_UserFormBus = oA_UserFormBus;
            _oA_UserFormStepBusiness = oA_UserFormStepBusiness;

            _base_DepartmentBusiness = base_DepartmentBusiness;

            _workflowRegistry = workflowRegistry;
            _definitionLoader = definitionLoader;
            _workflowStore = workflowStore;
            _workflowHost = workflowHost;
            _operator = ioperator;
            _userBus = userBus;
            _mapper = mapper;
        }

        IOA_UserFormBusiness _oA_UserFormBus { get; }
        IOA_UserFormStepBusiness _oA_UserFormStepBusiness { get; }
        IBase_DepartmentBusiness _base_DepartmentBusiness{get;}
        IDefinitionLoader _definitionLoader { get; }
        IPersistenceProvider _workflowStore { get; }
        IWorkflowRegistry _workflowRegistry { get; }
        IWorkflowHost _workflowHost { get; }
        IOperator _operator { get; }
        IBase_UserBusiness _userBus { get; }

        IMapper _mapper { get; }
        #endregion

        #region 获取

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<OA_UserFormDTO>> GetDataList(PageInput<OA_UserFormInputDTO> input)
        {
            var dataList = await _oA_UserFormBus.GetDataListAsync(input);

            return dataList;
        }

        //[HttpPost]
        //public async Task<PageResult<OA_UserFormDTO>> GetPageHistoryDataList(PageInput<OA_UserFormInputDTO> input)
        //{
        //    var dataList = await _oA_UserFormBus.GetPageHistoryDataListAsync(input);
        //    return dataList;
        //}

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<OA_UserFormDTO> GetTheData(IdInputDTO input)
        {
            var form = await _oA_UserFormBus.GetTheDataAsync(input.id);

            var workflow = await _workflowStore.GetWorkflowInstance(input.id);
            OAData data = workflow.Data as OAData;
            form.Steps = data.Steps;
            if (data.CurrentStepIds != null)
            {
                var stepid = data.CurrentStepIds.FirstOrDefault(p => p.ActRules.UserIds != null && p.ActRules.UserIds.Contains(_operator?.UserId));
                if (stepid != null)
                {
                    form.CurrentStepId = stepid.StepId;
                    form.CurrentStepIndex = data.Steps.Select((p, index) => new { p, index }).Where(p => p.p.Id == stepid.StepId).FirstOrDefault().index;
                }
                else
                {
                    var step = data.Steps.LastOrDefault(p => p.Status != 0);
                    if (step != null)
                    {
                        form.CurrentStepIndex = data.Steps.IndexOf(step);
                    }
                    else
                    {
                        form.CurrentStepIndex = data.Steps.Count - 1;
                    }
                }
            }
            else
            {
                var step = data.Steps.LastOrDefault(p => p.Status != 0);
                if (step != null)
                {
                    form.CurrentStepIndex = data.Steps.IndexOf(step);
                }
                else
                {
                    form.CurrentStepIndex = data.Steps.Count - 1;
                }
            }

            form.WorkflowJSON = data.ToJson();

            return form;
        }

        #endregion

        #region 提交

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data">保存的数据</param>
        [HttpPost]
        public async Task<List<OAStep>> PreStep(OA_UserFormDTO data)
        {
            OAData oAData = await OAExtension.InitOAStep(data);

            return oAData.Steps;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data">保存的数据</param>
        [HttpPost]
        public async Task SaveData(OA_UserFormDTO data)
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
                    await _oA_UserFormBus.AddDataAsync(data);

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
                await _oA_UserFormBus.UpdateDataAsync(data);
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids">id数组,JSON数组</param>
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _oA_UserFormBus.DeleteDataAsync(ids);
        }
        #endregion

        /// <summary>
        /// 审批数据
        /// </summary>
        [HttpPost]
        public async Task<AjaxResult> EventData(MyEvent eventData)
        {
            await _workflowHost.PublishEvent(eventData.EventName, eventData.EventKey, eventData);
            var result = await _oA_UserFormBus.DequeueWork(eventData.EventKey);
            AjaxResult res = new AjaxResult
            {
                Success = true,
                Msg = result == null ? "已提交审批请求！":"审批成功",
            };

            return res;
        }

        /// <summary>
        /// 审批数据
        /// </summary>
        [HttpPost]
        public async Task DisCardData(DisCardInput input)
        {
            var data = await _oA_UserFormBus.GetTheDataAsync(input.id);
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
                await _oA_UserFormBus.UpdateDataAsync(data);
                await _oA_UserFormStepBusiness.AddDataAsync(step);
            //});
            //if (!res.Success)
            //    throw res.ex;
        }

        [HttpPost]
        public async Task<bool> Suspend(IdInputDTO input)
        {
            return await _workflowHost.SuspendWorkflow(input.id);
        }

        [HttpPost]
        public async Task<bool> Resume(IdInputDTO input)
        {
            return await _workflowHost.ResumeWorkflow(input.id);
        }

        [HttpPost]
        public async Task Terminate(IdInputDTO input)
        {
            if (await _workflowHost.TerminateWorkflow(input.id))
            {
                await _oA_UserFormBus.DeleteDataAsync(new List<string>() { input.id });
            }
            else
            {
                throw new Exception("停止失败");
            }
        }

        [HttpPost]
        public void TestJsonData(string json)
        {
            string str = json;
        }
    }

    public class DisCardInput: IdInputDTO
    {
        public string remark { get; set; }
    }
}