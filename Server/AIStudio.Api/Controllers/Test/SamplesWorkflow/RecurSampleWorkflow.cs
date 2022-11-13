using System;
using WorkflowCore.Interface;

namespace AIStudio.Api.Controllers.Test.SamplesWorkflow
{
    /// <summary>
    /// RecurSampleWorkflow
    /// </summary>
    class RecurSampleWorkflow : IWorkflow<MyData>
    {
        private readonly ILogger<RecurSampleWorkflow> _logger;
        /// <summary>
        /// RecurSampleWorkflow
        /// </summary>
        /// <param name="logger"></param>
        public RecurSampleWorkflow(ILogger<RecurSampleWorkflow> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Id
        /// </summary>
        public string Id => "recur-sample";
       
        /// <summary>
        /// Verison
        /// </summary>
        public int Version => 1;

        /// <summary>
        /// Build
        /// </summary>
        /// <param name="builder"></param>
        public void Build(IWorkflowBuilder<MyData> builder)
        {
            builder
                .StartWith(context => _logger.LogInformation("Hello"))
                //每隔1s执行一次，data.Counter > 5为执行条件
                .Recur(data => TimeSpan.FromSeconds(1), data => data.Counter > 5).Do(recur =>
                {
                    recur.StartWith(context => _logger.LogInformation("Doing recurring task"))
                    .Then<IncrementStep>()
                            .Input(step => step.Value1, data => data.Counter)
                            .Output(data => data.Counter, step => step.Value2);
                })
                .Then(context => _logger.LogInformation("Carry on"));
        }
    }

}
