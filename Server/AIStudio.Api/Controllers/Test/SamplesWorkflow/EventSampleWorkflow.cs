using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Api.Controllers.Test.SamplesWorkflow
{
    /// <summary>
    /// EventSampleWorkflow
    /// </summary>
    /// <seealso cref="WorkflowCore.Interface.IWorkflow&lt;AIStudio.Api.Controllers.Test.SamplesWorkflow.MyDataClass&gt;" />
    public class EventSampleWorkflow : IWorkflow<MyDataClass>
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<EventSampleWorkflow> _logger;
        /// <summary>
        /// EventSampleWorkflow
        /// </summary>
        /// <param name="logger">The logger.</param>
        public EventSampleWorkflow(ILogger<EventSampleWorkflow> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id => "EventSampleWorkflow";

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version => 1;

        /// <summary>
        /// Builds the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public void Build(IWorkflowBuilder<MyDataClass> builder)
        {
            builder
                .StartWith(context =>
                    ExecutionResult.Next()
                )
                .WaitFor("MyEvent", (data, context) =>
                        context.Workflow.Id, data => DateTime.Now)
                    .Output(data => data.Value, step => step.EventData)
                .Then<CustomMessage>()
                    .Input(step => 
                        step.Message, data => "The data from the event is " + data.Value)
                .Then(context =>
                    _logger.LogInformation("workflow complete")
                );
        }
    }
}
