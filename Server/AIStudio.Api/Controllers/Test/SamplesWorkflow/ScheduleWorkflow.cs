using System;
using WorkflowCore.Interface;

namespace AIStudio.Api.Controllers.Test.SamplesWorkflow
{
    /// <summary>
    /// ScheduleWorkflow
    /// </summary>
    class ScheduleWorkflow : IWorkflow
    {
        private readonly ILogger<ScheduleWorkflow> _logger;
        /// <summary>
        /// ScheduleWorkflow
        /// </summary>
        /// <param name="logger"></param>
        public ScheduleWorkflow(ILogger<ScheduleWorkflow> logger)
        {
            _logger = logger;
        }

        public string Id => "schedule-sample";
        public int Version => 1;

        public void Build(IWorkflowBuilder<object> builder)
        {
            builder
                .StartWith(context => _logger.LogInformation("Hello"))
                ///5s后执行
                .Schedule(data => TimeSpan.FromSeconds(5)).Do(schedule => schedule
                    .StartWith(context => _logger.LogInformation("Doing scheduled tasks"))
                )
                .Then(context => _logger.LogInformation("Doing normal tasks"));
        }
    }
}
