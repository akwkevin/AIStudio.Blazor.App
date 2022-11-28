using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Api.Controllers.Test.SamplesWorkflow
{
    /// <summary>
    /// DeferSampleWorkflow
    /// </summary>
    /// <seealso cref="WorkflowCore.Interface.IWorkflow" />
    public class DeferSampleWorkflow : IWorkflow
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<DeferSampleWorkflow> _logger;
        /// <summary>
        /// DeferSampleWorkflow
        /// </summary>
        /// <param name="logger">The logger.</param>
        public DeferSampleWorkflow(ILogger<DeferSampleWorkflow> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Id
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id => "DeferSampleWorkflow";

        /// <summary>
        /// Version
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version => 1;

        /// <summary>
        /// Build
        /// </summary>
        /// <param name="builder">The builder.</param>
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

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="WorkflowCore.Models.StepBody" />
    public class SleepStep : StepBody
    {

        /// <summary>
        /// Gets or sets the period.
        /// </summary>
        /// <value>
        /// The period.
        /// </value>
        public TimeSpan Period { get; set; }

        /// <summary>
        /// Runs the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            if (context.PersistenceData == null)
                return ExecutionResult.Sleep(Period, new object());
            else
                return ExecutionResult.Next();
        }
    }
}
