using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Api.Controllers.Test.SamplesWorkflow
{
    /// <summary>
    /// EventSampleWorkflow
    /// </summary>
    public class EventSampleWorkflow : IWorkflow<MyDataClass>
    {
        private readonly ILogger<EventSampleWorkflow> _logger;
        /// <summary>
        /// EventSampleWorkflow
        /// </summary>
        /// <param name="logger"></param>
        public EventSampleWorkflow(ILogger<EventSampleWorkflow> logger)
        {
            _logger = logger;
        }

        public string Id => "EventSampleWorkflow";
            
        public int Version => 1;
            
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
