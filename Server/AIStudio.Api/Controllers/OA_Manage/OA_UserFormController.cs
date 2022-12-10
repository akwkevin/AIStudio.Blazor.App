using AIStudio.Business.OA_Manage;
using AIStudio.Business.OA_Manage.Steps;
using AIStudio.Common.CurrentUser;
using AIStudio.Common.Swagger;
using AIStudio.Entity.DTO.OA_Manage;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using AIStudio.Util.DiagramEntity;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WorkflowCore.Interface;

namespace AIStudio.Api.Controllers.OA_Manage
{
    /// <summary>
    /// 流程控制
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/OA_Manage/[controller]/[action]")]
    public class OA_UserFormController : ApiControllerBase
    {
        #region DI
        /// <summary>
        /// 流程控制器
        /// </summary>
        /// <param name="oA_UserFormBus"></param>
        public OA_UserFormController(IOA_UserFormBusiness oA_UserFormBus)
        {
            _oA_UserFormBus = oA_UserFormBus;
        }

        IOA_UserFormBusiness _oA_UserFormBus { get; }

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

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<OA_UserFormDTO> GetTheData(IdInputDTO input)
        {
            var form = await _oA_UserFormBus.GetTheDataAsync(input.id);
            return form;
        }

        #endregion

        #region 提交

        /// <summary>
        /// 预览
        /// </summary>
        /// <param name="data">保存的数据</param>
        [HttpPost]
        public async Task<List<OA_Step>> PreStep(OA_UserFormDTO data)
        {
            return await _oA_UserFormBus.PreStepAsync(data);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data">保存的数据</param>
        [HttpPost]
        public async Task SaveData(OA_UserFormDTO data)
        {
            await _oA_UserFormBus.SaveDataAsync(data);
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
            return await _oA_UserFormBus.EventDataAsync(eventData);
        }

        /// <summary>
        /// 废弃数据
        /// </summary>
        [HttpPost]
        public async Task DisCardData(DisCardInput input)
        {
            await _oA_UserFormBus.DisCardDataAsync(input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Suspend(IdInputDTO input)
        {
            return await _oA_UserFormBus.SuspendAsync(input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Resume(IdInputDTO input)
        {
            return await _oA_UserFormBus.ResumeAysnc(input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Terminate(IdInputDTO input)
        {
            await _oA_UserFormBus.TerminateAsync(input);
        }
    }


}