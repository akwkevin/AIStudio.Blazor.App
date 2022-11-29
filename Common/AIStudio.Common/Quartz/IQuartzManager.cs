using AIStudio.Common.Quartz.Models;
using Quartz;

namespace AIStudio.Common.Quartz;

/// <summary>
/// 
/// </summary>
public interface IQuartzManager
{
    /// <summary>
    /// Gets the scheduler.
    /// </summary>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    Task<IScheduler> GetScheduler(CancellationToken token = default);

    /// <summary>
    /// Starts the specified token.
    /// </summary>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    Task Start(CancellationToken token = default);

    /// <summary>
    /// Shuts down this instance.
    /// </summary>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    Task Shutdown(CancellationToken token = default);

    /// <summary>
    /// Checks the exists.
    /// </summary>
    /// <param name="jobName">Name of the job.</param>
    /// <param name="groupName">Name of the group.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    Task<bool> CheckExists(string jobName, string groupName, CancellationToken token = default);

    /// <summary>
    /// Adds the job.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jobInfo">The job information.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    Task<JobExcuteResult> AddJob<T>(JobInfo jobInfo, CancellationToken token = default)  where T : IJob;

    /// <summary>
    /// Adds the job.
    /// </summary>
    /// <param name="jobType">Type of the job.</param>
    /// <param name="jobInfo">The job information.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    Task<JobExcuteResult> AddJob(Type jobType, JobInfo jobInfo, CancellationToken token = default);

    /// <summary>
    /// Deletes the job.
    /// </summary>
    /// <param name="jobName">Name of the job.</param>
    /// <param name="groupName">Name of the group.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    Task<JobExcuteResult> DeleteJob(string jobName, string groupName, CancellationToken token = default);

    /// <summary>
    /// Pauses the job.
    /// </summary>
    /// <param name="jobName">Name of the job.</param>
    /// <param name="groupName">Name of the group.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    Task<JobExcuteResult> PauseJob(string jobName, string groupName, CancellationToken token = default);

    /// <summary>
    /// Resumes the job.
    /// </summary>
    /// <param name="jobName">Name of the job.</param>
    /// <param name="groupName">Name of the group.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    Task<JobExcuteResult> ResumeJob(string jobName, string groupName, CancellationToken token = default);

    /// <summary>
    /// Does the job.
    /// </summary>
    /// <param name="jobName">Name of the job.</param>
    /// <param name="groupName">Name of the group.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    Task<JobExcuteResult> DoJob(string jobName, string groupName, CancellationToken token = default);
}
