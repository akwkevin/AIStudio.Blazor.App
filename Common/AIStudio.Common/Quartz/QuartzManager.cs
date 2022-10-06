using AIStudio.Common.Quartz.Const;
using AIStudio.Common.Quartz.Extensions;
using AIStudio.Common.Quartz.Models;
using AIStudio.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl.Matchers;
using System.Runtime;

namespace AIStudio.Common.Quartz;

public class QuartzManager : IQuartzManager
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly ILogger<QuartzManager> _logger;
    private readonly JobSchedulingOptions _options;

    public QuartzManager(IServiceProvider serviceProvider,
                         ISchedulerFactory schedulerFactory,
                         ILogger<QuartzManager> logger,
                         IOptions<JobSchedulingOptions> options)
    {
        _serviceProvider = serviceProvider;
        _schedulerFactory = schedulerFactory;
        _logger = logger;
        _options = options.Value;
    }

    public async Task<IScheduler> GetScheduler(CancellationToken token = default)
    {
        return await _schedulerFactory.GetScheduler(token);
    }

    ///// <summary>
    ///// 获取所有的作业
    ///// </summary>
    ///// <param name="schedulerFactory"></param>
    ///// <returns></returns>
    //public async Task<List<JobInfo>> GetAllJobs()
    //{
    //    List<JobInfo> list = new List<JobInfo>();
    //    try
    //    {
    //        IScheduler scheduler = await GetScheduler();
    //        var groups = await scheduler.GetJobGroupNames();
    //        foreach (var groupName in groups)
    //        {
    //            foreach (var jobKey in await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName)))
    //            {
    //                JobInfo jobInfo = new JobInfo(jobKey.Name, jobKey.Group);

    //                var triggers = await scheduler.GetTriggersOfJob(jobKey);
    //                foreach (ITrigger trigger in triggers)
    //                {
    //                    TriggerInfo triggerInfo = new TriggerInfo(jobKey.Name, jobKey.Group, null);
    //                    DateTimeOffset? dateTimeOffset = trigger.GetPreviousFireTimeUtc();
    //                    if (dateTimeOffset != null)
    //                    {
    //                        triggerInfo.LastRunTime = Convert.ToDateTime(dateTimeOffset.ToString());
    //                    }
    //                    jobInfo.Triggers.Add(triggerInfo);
    //                }
    //                list.Add(jobInfo);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError("获取作业异常：" + ex.Message + ex.StackTrace);
    //    }
    //    return list;
    //}

    public async Task Start(CancellationToken token = default)
    {
        var scheduler = await GetScheduler();

        _logger.LogInformation($"Job scheduling start.");

        await scheduler.Start(token);

        using var scope = _serviceProvider.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        if (_options.StartHandle != null) await _options.StartHandle(serviceProvider);
    }

    public async Task Shutdown(CancellationToken token = default)
    {
        var scheduler = await GetScheduler();

        _logger.LogInformation($"Job scheduling shutdown.");

        await scheduler.Shutdown(token);

        using var scope = _serviceProvider.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        if (_options.ShutdownHandle != null) await _options.ShutdownHandle(serviceProvider);
    }

    public async Task<bool> CheckExists(string jobName, string groupName, CancellationToken token = default)
    {
        var scheduler = await GetScheduler();
        var jobKey = new JobKey(jobName, groupName);
        return await scheduler.CheckExists(jobKey, token);
    }

    public Task<JobExcuteResult> AddJob<T>(JobInfo jobInfo, CancellationToken token = default) where T : IJob
    {
        return AddJob(typeof(T), jobInfo, token);
    }

    public async Task<JobExcuteResult> AddJob(Type jobType, JobInfo jobInfo, CancellationToken token = default)
    {

        try
        {
            //if (await CheckExists(jobInfo.Name, jobInfo.GroupName))
            //{
            //    return new JobExcuteResult(false, $"任务[{jobInfo.Name}-{jobInfo.GroupName}]已经存在");
            //}

            //先删除重复的任务
            await DeleteJob(jobInfo.Name, jobInfo.GroupName);

            var scheduler = await GetScheduler();

            var jobKey = new JobKey(jobInfo.Name, jobInfo.GroupName);
            var job = JobBuilder.Create(jobType)
                            .WithIdentity(jobKey)
                            .UsingJobData(JobSchedulingConst.JobNameKey, jobInfo.Name)
                            .Build();

            var triggers = new List<ITrigger>();
            foreach (var triggerInfo in jobInfo.Triggers)
            {
                var validExpression = triggerInfo.Cron.IsValidExpression();
                if (!validExpression.Success)
                    return validExpression;

                var trigger = TriggerBuilder.Create()
                    .ForJob(jobKey)
                    .WithIdentity(triggerInfo.Name, triggerInfo.GroupName)
                     .StartAt(CronHelper.StartTime2DateTimeOffset(triggerInfo.StartTime) ?? SystemTime.UtcNow())
                       .EndAt(CronHelper.DateTime2DateTimeOffset(triggerInfo.EndTime))
                    .WithDescription(triggerInfo.Describe)
                    .WithCronSchedule(triggerInfo.Cron)
                    .Build();
                triggers.Add(trigger);
            }

            await scheduler.ScheduleJob(job, triggers, true, token);

            _logger.LogInformation($"AddJob: {jobInfo.ToJson()}");
            return new JobExcuteResult(true, $"任务[{jobInfo.Name}-{jobInfo.GroupName}]添加成功");
        }
        catch (Exception ex)
        {
            return new JobExcuteResult(false, $"任务[{jobInfo.Name}-{jobInfo.GroupName}]添加失败:{ex.Message}");
        }
    }

    public async Task<JobExcuteResult> DeleteJob(string jobName, string groupName, CancellationToken token = default)
    {
        if (await CheckExists(jobName, groupName))
        {
            try
            {
                var scheduler = await GetScheduler();

                var jobKey = new JobKey(jobName, groupName);
                var triggers = await scheduler.GetTriggersOfJob(jobKey);
                foreach (var trigger in triggers)
                {
                    await scheduler.PauseTrigger(trigger.Key);
                    await scheduler.UnscheduleJob(trigger.Key);// 移除触发器
                }
                if (await scheduler.DeleteJob(jobKey, token))
                {
                    _logger.LogInformation($"DeleteJob {jobName}-{groupName}");
                    return new JobExcuteResult(true, $"任务[{jobName}-{groupName}]删除成功");
                }
                else
                {
                    return new JobExcuteResult(false, $"任务[{jobName}-{groupName}]删除失败");
                }
            }
            catch (Exception ex)
            {
                return new JobExcuteResult(false, $"任务[{jobName}-{groupName}]删除失败:{ex.Message}");
            }
        }
        else
        {
            return new JobExcuteResult(false, $"任务[{jobName}-{groupName}]任务不存在");
        }
    }

    public async Task<JobExcuteResult> PauseJob(string jobName, string groupName, CancellationToken token = default)
    {
        if (await CheckExists(jobName, groupName))
        {
            try
            {
                var scheduler = await GetScheduler();
                var jobKey = new JobKey(jobName, groupName);
                await scheduler.PauseJob(jobKey, token);

                _logger.LogInformation($"PauseJob {jobName}-{groupName}");
                return new JobExcuteResult(true, $"任务[{jobName}-{groupName}]暂停成功");
            }
            catch (Exception ex)
            {
                return new JobExcuteResult(false, $"任务[{jobName}-{groupName}]暂停失败:{ex.Message}");
            }
        }
        else
        {
            return new JobExcuteResult(false, $"任务[{jobName}-{groupName}]任务不存在");
        }
    }

    public async Task<JobExcuteResult> ResumeJob(string jobName, string groupName, CancellationToken token = default)
    {
        if (await CheckExists(jobName, groupName))
        {
            try
            {
                var scheduler = await GetScheduler();
                var jobKey = new JobKey(jobName, groupName);
                await scheduler.ResumeJob(jobKey, token);

                _logger.LogInformation($"ResumeJob {jobName}-{groupName}");
                return new JobExcuteResult(true, $"任务[{jobName}-{groupName}]恢复成功");
            }
            catch (Exception ex)
            {
                return new JobExcuteResult(false, $"任务[{jobName}-{groupName}]恢复失败:{ex.Message}");
            }
        }
        else
        {
            return new JobExcuteResult(false, $"任务[{jobName}-{groupName}]任务不存在");
        }
    }

    /// <summary>
    /// 立即执行一次
    /// </summary>
    /// <param name="jobName"></param>
    /// <param name=""></param>
    /// <param name="groupName"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<JobExcuteResult> DoJob(string jobName, string groupName, CancellationToken token = default)
    {
        if (await CheckExists(jobName, groupName))
        {
            try
            {
                var scheduler = await GetScheduler();
                var jobKey = new JobKey(jobName, groupName);
                await scheduler.TriggerJob(jobKey, token);

                _logger.LogInformation($"ResumeJob {jobName}-{groupName}");
                return new JobExcuteResult(true, $"任务[{jobName}-{groupName}]执行成功");
            }
            catch (Exception ex)
            {
                return new JobExcuteResult(false, $"任务[{jobName}-{groupName}]执行失败:{ex.Message}");
            }
        }
        else
        {
            return new JobExcuteResult(false, $"任务[{jobName}-{groupName}]任务不存在");
        }
    }

}
