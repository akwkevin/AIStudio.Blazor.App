using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Api.Controllers.Test.SamplesWorkflow
{
    /// <summary>
    /// DeferSampleWorkflow
    /// </summary>
    public class DeferSampleWorkflow : IWorkflow
    {
        private readonly ILogger<DeferSampleWorkflow> _logger;
        /// <summary>
        /// DeferSampleWorkflow
        /// </summary>
        /// <param name="logger"></param>
        public DeferSampleWorkflow(ILogger<DeferSampleWorkflow> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Id
        /// </summary>
        public string Id => "DeferSampleWorkflow";

        /// <summary>
        /// Version
        /// </summary>
        public int Version => 1;

        /// <summary>
        /// Build
        /// </summary>
        /// <param name="builder"></param>
        public void Build(IWorkflowBuilder<object> builder)
        {
            builder
                .StartWith(context =>
                {
                    _logger.LogInformation("Workflow started");
                    return ExecutionResult.Next();
                })
                .Then<SleepStep>()
                    .Input(step => step.Period, data => TimeSpan.FromSeconds(3))
                .Then(context =>
                {
                    _logger.LogInformation("workflow complete");
                    return ExecutionResult.Next();
                });
        }
    }

    public class SleepStep : StepBody
    {

        public TimeSpan Period { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            if (context.PersistenceData == null)
                return ExecutionResult.Sleep(Period, new object());
            else
                return ExecutionResult.Next();
        }
    }
}
