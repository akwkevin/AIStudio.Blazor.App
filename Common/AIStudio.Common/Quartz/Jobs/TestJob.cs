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
        public TestJob(ILogger<TestJob> logger)
        {
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var trigger = context.Trigger;           
            _logger.LogInformation($"{trigger.JobKey.Name}-{trigger.JobKey.Group} Execute TestJob");
        }
    }
}
