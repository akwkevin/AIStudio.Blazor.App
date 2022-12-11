using AIStudio.Business.AOP;
using AIStudio.Common.DI;
using AIStudio.Common.Quartz;
using AIStudio.Common.Quartz.Models;
using AIStudio.Common.Types;
using AIStudio.Entity.DTO.Quartz_Manage;
using AIStudio.Entity.Quartz_Manage;
using AIStudio.Util.Common;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Quartz;
using Simple.Common;
using SqlSugar;
using System.Linq.Dynamic.Core;
using System.Runtime;
using System.Xml.Linq;
using static Quartz.Logging.OperationName;

namespace AIStudio.Business.Quartz_Manage
{
    public class Quartz_TaskBusiness : BaseBusiness<Quartz_Task>, IQuartz_TaskBusiness, ITransientDependency
    {
        private readonly IMapper _mapper;
        private readonly IQuartzManager _quartzManager;

        public Quartz_TaskBusiness(ISqlSugarClient db, IMapper mapper, IQuartzManager quartzManager) : base(db)
        {
            _mapper = mapper;
            _quartzManager = quartzManager;
        }

        #region 外部接口

        public override async Task<PageResult<Quartz_Task>> GetDataListAsync(PageInput input)
        {
            return await base.GetDataListAsync(input);
        }

        public override async Task<Quartz_Task> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        [DataRepeatValidate(new string[] { "TaskName", "GroupName" }, new string[] { "任务名", "任务分组" }, false)]
        public override async Task AddDataAsync(Quartz_Task data)
        {
            var jobType = GetJobClass(data.ActionClass);
            if (jobType == null)
            {
                throw AjaxResultException.Status404NotFound("找不到任务类，添加失败");
            }

            JobExcuteResult result = null;
            if (data.IsEnabled)
            {
                result = await AddJob(jobType, data);
            }
            if (result == null || result.Success)
            {
                await InsertAsync(data);
            }
            else
            {
                throw AjaxResultException.Status409Conflict(result.Message);
            }
        }

        [DataRepeatValidate(new string[] { "TaskName", "GroupName" }, new string[] { "任务名", "任务分组" }, false)]
        public override async Task UpdateDataAsync(Quartz_Task data)
        {
            var jobType = GetJobClass(data.ActionClass);
            if (jobType == null)
            {
                throw AjaxResultException.Status404NotFound("找不到任务类，修改失败");
            }

            var oldjob = await GetTheDataAsync(data.Id);
            if (oldjob == null)
            {
                throw AjaxResultException.Status404NotFound("找不到任务，修改失败，请刷新后重试");
            }

            //如果修改了任务名和分组名
            if (data.TaskName != oldjob.TaskName || data.GroupName != oldjob.GroupName)
            {
                if (await _quartzManager.CheckExists(data.TaskName, data.GroupName))
                {
                    throw AjaxResultException.Status409Conflict($"修改的任务[{data.TaskName}-{data.GroupName}]已经存在");
                }
            }

            //删除旧的任务，启动新的任务
            await _quartzManager.DeleteJob(oldjob.TaskName, oldjob.GroupName);
            JobExcuteResult result = null;
            if (data.IsEnabled)
            {
                result = await AddJob(jobType, data);
            }
            if (result == null || result.Success)
            {
                await UpdateAsync(data);
            }
            else
            {
                throw AjaxResultException.Status409Conflict(result.Message);
            }
        }

        public override async Task DeleteDataAsync(List<string> ids)
        {
            var list = await GetIQueryable().In(ids).ToListAsync();

            foreach (var item in list)
            {
                await _quartzManager.DeleteJob(item.TaskName, item.GroupName);
            }
            await DeleteAsync(ids);
        }       

        public async Task StartAllAsync(bool withoutTestJob)
        {
            var list = await GetIQueryable().ToListAsync();
            foreach (var item in list)
            {
                var jobType = GetJobClass(item.ActionClass);
                if (jobType == null)
                {
                    continue;
                }

                if (withoutTestJob)
                {
                    if (jobType == typeof(TestJob))
                    {
                        continue;
                    }
                }

                if (item.IsEnabled)
                {
                    await AddJob(jobType, item);
                }
            }
        }

      
        public async Task StartDataAsync(List<string> ids)
        {
            var list = await GetIQueryable().In(ids).ToListAsync();

            foreach (var item in list)
            {
                if (await _quartzManager.CheckExists(item.TaskName, item.GroupName))
                {
                    // 恢复
                    var result = await _quartzManager.ResumeJob(item.TaskName, item.GroupName);
                    if (result.Success)
                    {
                        item.IsEnabled = true;
                        await UpdateAsync(item);
                    }
                    else
                    {
                        throw AjaxResultException.Status409Conflict(result.Message);
                    }
                }
                else
                {
                    var jobType = GetJobClass(item.ActionClass);
                    if (jobType == null)
                    {
                        throw AjaxResultException.Status404NotFound("找不到任务类，任务启动中断，请刷新");
                    }
                    // 添加任务
                    var result = await AddJob(jobType, item);
                    if (result.Success)
                    {
                        item.IsEnabled = true;
                        await UpdateAsync(item);
                    }
                    else
                    {
                        throw AjaxResultException.Status409Conflict("任务启动中断，请刷新，" + result.Message);
                    }
                }
            }
        }
        public async Task PauseDataAsync(List<string> ids)
        {
            var list = await GetIQueryable().In(ids).ToListAsync();

            foreach (var item in list)
            {
                var result = await _quartzManager.PauseJob(item.TaskName, item.GroupName);
                if (result.Success)
                {
                    item.IsEnabled = false;
                    await UpdateAsync(item);
                }
                else
                {
                    throw AjaxResultException.Status409Conflict("任务暂停中断，请刷新，" + result.Message);
                }
            }
        }
        public async Task TodoDataAsync(List<string> ids)
        {
            var list = await GetIQueryable().In(ids).ToListAsync();

            foreach (var item in list)
            {
                var result = await _quartzManager.DoJob(item.TaskName, item.GroupName);
                if (!result.Success)
                {
                    throw AjaxResultException.Status409Conflict("任务执行中断，" + result.Message);
                }
            }
        }
        #endregion

        public List<string> GetJobOptions()
        {
            return GlobalType.AllTypes.Where(p => typeof(IJob).IsAssignableFrom(p)).Select(p => p.Name).ToList();
        }

        #region 私有成员
        private Type GetJobClass(string name)
        {
            return GlobalType.AllTypes.Where(p => typeof(IJob).IsAssignableFrom(p)).FirstOrDefault(p => p.Name == name);
        }

        private async Task<JobExcuteResult> AddJob(Type jobType, Quartz_Task job)
        {
            JobDataMap JobDataMap = new JobDataMap();
            if (jobType == typeof(HttpResultfulJob))
            {
                JobDataMap.Add("ApiUrl", job.ApiUrl);
                JobDataMap.Add("AuthKey", job.AuthKey);
                JobDataMap.Add("AuthValue", job.AuthValue);
                JobDataMap.Add("RequestType", job.RequestType);      
            }

            JobDataMap.Add("TenantId", job.TenantId);
            JobDataMap.Add("CreatorId", job.CreatorId);
            JobDataMap.Add("CreatorName", job.CreatorName);

            var jobInfo = new JobInfo(job.TaskName, job.GroupName, JobDataMap);
            jobInfo.Triggers.Add(new TriggerInfo(job.TaskName, job.GroupName, job.Cron, job.Remark));
            return await _quartzManager.AddJob(jobType, jobInfo);
        }
        #endregion

        #region 数据模型

        #endregion
    }


}