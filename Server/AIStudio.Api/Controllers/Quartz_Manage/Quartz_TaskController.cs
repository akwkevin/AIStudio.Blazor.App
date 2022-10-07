using AIStudio.Business.Quartz_Manage;
using AIStudio.Entity.DTO.Quartz_Manage;
using AIStudio.Entity.Quartz_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Mvc;
using Quartz;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AIStudio.Api.Controllers.Quartz_Manage
{
    /// <summary>
    /// 作业调度
    /// </summary>
    [Route("/Quartz_Manage/[controller]/[action]")]
    public class Quartz_TaskController : ApiControllerBase
    {
        private readonly IQuartz_TaskBusiness _quartz_TaskBusiness;
        /// <summary>
        /// Quartz_TaskController
        /// </summary>
        /// <param name="quartz_TaskBusiness"></param>
        public Quartz_TaskController(IQuartz_TaskBusiness quartz_TaskBusiness)
        {
            _quartz_TaskBusiness = quartz_TaskBusiness;
        }

        /// <summary>
        /// 获取数据列表Quartz_Task
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<Quartz_Task>> GetDataList(PageInput input)
        {
            return await _quartz_TaskBusiness.GetDataListAsync(input);
        }

        /// <summary>
        /// 获取数据Quartz_Task
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Quartz_Task> GetTheData(IdInputDTO input)
        {
            return await _quartz_TaskBusiness.GetTheDataAsync(input.id) ?? new Quartz_Task();
        }

        /// <summary>
        /// 保存数据Quartz_Task
        /// </summary>
        /// <param name="theData"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task SaveData(Quartz_Task theData)
        {
            if (theData.Id.IsNullOrEmpty())
            {
                await _quartz_TaskBusiness.AddDataAsync(theData);
            }
            else
            {
                await _quartz_TaskBusiness.UpdateDataAsync(theData);
            }

        }

        /// <summary>
        /// 删除数据Quartz_Task
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _quartz_TaskBusiness.DeleteDataAsync(ids);
        }

        /// <summary>
        /// 暂停数据Quartz_Task
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task PauseData(List<string> ids)
        {
            await _quartz_TaskBusiness.PauseDataAsync(ids);
        }

        /// <summary>
        /// 启动数据Quartz_Task
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task StartData(List<string> ids)
        {
            await _quartz_TaskBusiness.StartDataAsync(ids);
        }

        /// <summary>
        /// 立即执行Quartz_Task
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
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
