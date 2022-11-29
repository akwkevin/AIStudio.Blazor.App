using AIStudio.Common.Quartz.Extensions;
using AIStudio.Common.Quartz.Models;
using AIStudio.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Logging;

namespace AIStudio.Common.Quartz;

/// <summary>
/// 
/// </summary>
/// <seealso cref="AIStudio.Common.Quartz.IQuartzManager" />
public class QuartzManager : IQuartzManager
{
    /// <summary>
    /// The service provider
    /// </summary>
    private readonly IServiceProvider _serviceProvider;
    /// <summary>
    /// The scheduler factory
    /// </summary>
    private readonly ISchedulerFactory _schedulerFactory;
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<QuartzManager> _logger;
    /// <summary>
    /// The options
    /// </summary>
    private readonly JobSchedulingOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuartzManager"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="schedulerFactory">The scheduler factory.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="options">The options.</param>
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

    /// <summary>
    /// Gets the scheduler.
    /// </summary>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    public async Task<IScheduler> GetScheduler(CancellationToken token = default)
    {
        return await _schedulerFactory.GetScheduler(token);
    }


    /// <summary>
    /// Starts the specified token.
    /// </summary>
    /// <param name="token">The token.</param>
    public async Task Start(CancellationToken token = default)
    {
        var scheduler = await GetScheduler();

        _logger.LogTrace($"Job scheduling start.");

        await scheduler.Start(token);

        using var scope = _serviceProvider.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        if (_options.StartHandle != null) await _options.StartHandle(serviceProvider);
    }

    /// <summary>
    /// Shuts down this instance.
    /// </summary>
    /// <param name="token">The token.</param>
    public async Task Shutdown(CancellationToken token = default)
    {
        var scheduler = await GetScheduler();

        _logger.LogTrace($"Job scheduling shutdown.");

        await scheduler.Shutdown(token);

        using var scope = _serviceProvider.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        if (_options.ShutdownHandle != null) await _options.ShutdownHandle(serviceProvider);
    }

    /// <summary>
    /// Checks the exists.
    /// </summary>
    /// <param name="jobName">Name of the job.</param>
    /// <param name="groupName">Name of the group.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    public async Task<bool> CheckExists(string jobName, string groupName, CancellationToken token = default)
    {
        var scheduler = await GetScheduler();
        var jobKey = new JobKey(jobName, groupName);
        return await scheduler.CheckExists(jobKey, token);
    }

    /// <summary>
    /// Adds the job.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jobInfo">The job information.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    public Task<JobExcuteResult> AddJob<T>(JobInfo jobInfo, CancellationToken token = default) where T : IJob
    {
        return AddJob(typeof(T), jobInfo, token);
    }

    /// <summary>
    /// Adds the job.
    /// </summary>
    /// <param name="jobType">Type of the job.</param>
    /// <param name="jobInfo">The job information.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    public async Task<JobExcuteResult> AddJob(Type jobType, JobInfo jobInfo, CancellationToken token = default)
    {
        try
        {
            //先删除重复的任务
            await DeleteJob(jobInfo.Name, jobInfo.GroupName);

            var scheduler = await GetScheduler();

            var jobKey = new JobKey(jobInfo.Name, jobInfo.GroupName);
            var job = JobBuilder.Create(jobType)
                            .WithIdentity(jobKey)
                            .UsingJobData(jobInfo.JobDataMap)
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

    /// <summary>
    /// Deletes the job.
    /// </summary>
    /// <param name="jobName">Name of the job.</param>
    /// <param name="groupName">Name of the group.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
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
            //任务没有启动，直接返回成功
            return new JobExcuteResult(true, $"任务[{jobName}-{groupName}]任务不存在无需删除");
        }
    }

    /// <summary>
    /// Pauses the job.
    /// </summary>
    /// <param name="jobName">Name of the job.</param>
    /// <param name="groupName">Name of the group.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Resumes the job.
    /// </summary>
    /// <param name="jobName">Name of the job.</param>
    /// <param name="groupName">Name of the group.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
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
    /// <param name="jobName">Name of the job.</param>
    /// <param name="groupName">Name of the group.</param>
    /// <param name="token">The token.</param>
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
