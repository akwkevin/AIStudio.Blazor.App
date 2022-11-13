using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Api.Controllers.Test.SamplesWorkflow
{
    /// <summary>
    /// PassingDataWorkflow2
    /// </summary>
    public class PassingDataWorkflow2 : IWorkflow<Dictionary<string, int>>
    {
        private readonly ILogger<PassingDataWorkflow2> _logger;
        /// <summary>
        /// PassingDataWorkflow2
        /// </summary>
        /// <param name="logger"></param>
        public PassingDataWorkflow2(ILogger<PassingDataWorkflow2> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Build
        /// </summary>
        /// <param name="builder"></param>
        public void Build(IWorkflowBuilder<Dictionary<string, int>> builder)
        {
            builder
                .StartWith(context =>
                {
                    _logger.LogInformation("Starting PassingDataWorkflow2...");
                    return ExecutionResult.Next();
                })
                .Then<AddNumbers>()
                    .Input(step => step.Input1, data => data["Value1"])
                    .Input(step => step.Input2, data => data["Value2"])
                    .Output((step, data) => data["Value3"] = step.Output)
                .Then<CustomMessage>()
                    .Name("Print custom message")
                    .Input(step => step.Message, data => "The answer is " + data["Value3"].ToString())
                .Then(context =>
                    {
                        _logger.LogInformation("PassingDataWorkflow2 complete");
                        return ExecutionResult.Next();
                    });
        }

        /// <summary>
        /// Id
        /// </summary>
        public string Id => "PassingDataWorkflow2";

        /// <summary>
        /// Version
        /// </summary>
        public int Version => 1;

    }
}
