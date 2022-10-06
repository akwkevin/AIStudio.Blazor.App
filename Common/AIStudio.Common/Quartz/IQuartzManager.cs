using AIStudio.Common.Quartz.Models;
using Quartz;

namespace AIStudio.Common.Quartz;

public interface IQuartzManager
{
    Task<IScheduler> GetScheduler(CancellationToken token = default);

    Task Start(CancellationToken token = default);

    Task Shutdown(CancellationToken token = default);

    Task<bool> CheckExists(string jobName, string groupName, CancellationToken token = default);

    Task<JobExcuteResult> AddJob<T>(JobInfo jobInfo, CancellationToken token = default)  where T : IJob;

    Task<JobExcuteResult> AddJob(Type jobType, JobInfo jobInfo, CancellationToken token = default);

    Task<JobExcuteResult> DeleteJob(string jobName, string groupName, CancellationToken token = default);

    Task<JobExcuteResult> PauseJob(string jobName, string groupName, CancellationToken token = default);

    Task<JobExcuteResult> ResumeJob(string jobName, string groupName, CancellationToken token = default);

    Task<JobExcuteResult> DoJob(string jobName, string groupName, CancellationToken token = default);
}
