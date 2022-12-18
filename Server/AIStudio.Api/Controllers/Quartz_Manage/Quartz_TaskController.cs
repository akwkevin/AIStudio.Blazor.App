using AIStudio.Business.Quartz_Manage;
using AIStudio.Common.Filter.FilterAttribute;
using AIStudio.Common.Swagger;
using AIStudio.Entity.DTO.Quartz_Manage;
using AIStudio.Entity.Quartz_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Simple.Common.Filters;
using static AIStudio.Common.Authentication.Jwt.JwtHelper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AIStudio.Api.Controllers.Quartz_Manage
{
    /// <summary>
    /// 作业调度
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Quartz_Manage/[controller]/[action]")]
    public class Quartz_TaskController : ApiControllerBase
    {
        private readonly IQuartz_TaskBusiness _quartz_TaskBusiness;
        /// <summary>
        /// 作业调度控制器
        /// </summary>
        /// <param name="quartz_TaskBusiness"></param>
        public Quartz_TaskController(IQuartz_TaskBusiness quartz_TaskBusiness)
        {
            _quartz_TaskBusiness = quartz_TaskBusiness;
        }

        /// <summary>
        /// 作业调度列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<Quartz_Task>> GetDataList(PageInput input)
        {
            return await _quartz_TaskBusiness.GetDataListAsync(input);
        }

        /// <summary>
        /// 获取指定Id的作业任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Quartz_Task> GetTheData(IdInputDTO input)
        {
            return await _quartz_TaskBusiness.GetTheDataAsync(input.id);
        }

        /// <summary>
        /// 保存作业任务
        /// </summary>
        /// <param name="theData"></param>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]
        [Authorize(Permissions.Auto)]
        public async Task SaveData(Quartz_Task theData)
        {
            await _quartz_TaskBusiness.SaveDataAsync(theData);
        }

        /// <summary>
        /// 删除作业任务
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]
        [Authorize(Permissions.Auto)]
        public async Task DeleteData(List<string> ids)
        {
            await _quartz_TaskBusiness.DeleteDataAsync(ids);
        }

        /// <summary>
        /// 暂停作业任务
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]
        [Authorize("Edit")]
        public async Task PauseData(List<string> ids)
        {
            await _quartz_TaskBusiness.PauseDataAsync(ids);
        }

        /// <summary>
        /// 启动作业任务
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]
        [Authorize("Edit")]
        public async Task StartData(List<string> ids)
        {
            await _quartz_TaskBusiness.StartDataAsync(ids);
        }

        /// <summary>
        /// 立即执行作业任务
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]
        [Authorize("Edit")]
        public async Task TodoData(List<string> ids)
        {
            await _quartz_TaskBusiness.TodoDataAsync(ids);
        }

        /// <summary>
        /// 获取作业类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<string> GetJobOptions()
        {
            return _quartz_TaskBusiness.GetJobOptions();
        }
    }
}
