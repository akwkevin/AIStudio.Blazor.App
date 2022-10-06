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
    [Route("/Quartz_Manage/[controller]/[action]")]
    public class Quartz_TaskController : ApiControllerBase
    {
        private readonly IQuartz_TaskBusiness _quartz_TaskBusiness;
        public Quartz_TaskController(IQuartz_TaskBusiness quartz_TaskBusiness)
        {
            _quartz_TaskBusiness = quartz_TaskBusiness;
        }

        /// <summary>
        /// 获取所有的作业
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<Quartz_Task>> GetDataList(PageInput input)
        {
            return await _quartz_TaskBusiness.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<Quartz_Task> GetTheData(IdInputDTO input)
        {
            return await _quartz_TaskBusiness.GetTheDataAsync(input.id) ?? new Quartz_Task();
        }

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

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _quartz_TaskBusiness.DeleteDataAsync(ids);
        }

        [HttpPost]
        public async Task PauseData(List<string> ids)
        {
            await _quartz_TaskBusiness.PauseDataAsync(ids);
        }

        [HttpPost]
        public async Task StartData(List<string> ids)
        {
            await _quartz_TaskBusiness.StartDataAsync(ids);
        }

        [HttpPost]
        public async Task TodoData(List<string> ids)
        {
            await _quartz_TaskBusiness.TodoDataAsync(ids);
        }

        [HttpPost]
        public List<string> GetJobOptions()
        {
            return _quartz_TaskBusiness.GetJobOptions();
        }
    }
}
