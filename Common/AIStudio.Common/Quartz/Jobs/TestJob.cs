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
    public class TestJob : IJob
    {
        private readonly ILogger<TestJob> _logger;
        private readonly IEventPublisher _publisher;
        public TestJob(ILogger<TestJob> logger, IEventPublisher publisher)
        {
            _logger = logger;
            _publisher = publisher;
        }
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
