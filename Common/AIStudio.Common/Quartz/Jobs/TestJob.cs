using AIStudio.Common.EventBus.Abstract;
using AIStudio.Common.EventBus.EventHandlers;
using AIStudio.Common.EventBus.Models;
using AIStudio.Common.Quartz.Extensions;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System.ComponentModel.DataAnnotations;

namespace AIStudio.Common.Quartz
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Quartz.IJob" />
    public class TestJob : IJob
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<TestJob> _logger;
        /// <summary>
        /// The publisher
        /// </summary>
        private readonly IEventPublisher _publisher;
        /// <summary>
        /// Initializes a new instance of the <see cref="TestJob"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="publisher">The publisher.</param>
        public TestJob(ILogger<TestJob> logger, IEventPublisher publisher)
        {
            _logger = logger;
            _publisher = publisher;
        }
        /// <summary>
        /// Called by the <see cref="T:Quartz.IScheduler" /> when a <see cref="T:Quartz.ITrigger" />
        /// fires that is associated with the <see cref="T:Quartz.IJob" />.
        /// </summary>
        /// <param name="context">The execution context.</param>
        /// <remarks>
        /// The implementation may wish to set a  result object on the
        /// JobExecutionContext before this method exits.  The result itself
        /// is meaningless to Quartz, but may be informative to
        /// <see cref="T:Quartz.IJobListener" />s or
        /// <see cref="T:Quartz.ITriggerListener" />s that are watching the job's
        /// execution.
        /// </remarks>
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap jobdatamap = context.MergedJobDataMap;
            string httpMessage = "";
            var trigger = context.Trigger;

            var tenantId = jobdatamap.Get("TenantId") as string;
            var creatorId = jobdatamap.Get("CreatorId") as string;
            var creatorName = jobdatamap.Get("CreatorName") as string;

            var message = $"{trigger.JobKey.Name}-{trigger.JobKey.Group} Execute TestJob";
            _logger.LogDebug(message);

            var eventModel = new SystemEvent();
            eventModel.LogType = "系统任务执行";
            eventModel.Message = message;
            eventModel.TenantId = tenantId;
            eventModel.CreatorId = creatorId;
            eventModel.CreatorName = creatorName;

            await _publisher.PublishAsync(eventModel);
        }
    }
}
