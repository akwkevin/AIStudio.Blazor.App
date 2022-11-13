using AIStudio.Common.DI;
using Microsoft.Extensions.Logging;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Api.Controllers.Test.SamplesWorkflow
{
    /// <summary>
    /// SimpleDecisionWorkflow
    /// </summary>
    public class SimpleDecisionWorkflow : IWorkflow
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id => "Simple Decision Workflow";

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
                .StartWith<HelloWorld>()
                .Then<RandomOutput>(randomOutput =>
                {
                    randomOutput
                        .When(0)
                            .Then<CustomMessage>(cm =>
                            {
                                cm.Name("Print custom message");
                                cm.Input(step => step.Message, data => "Looping back....");
                            })
                            .Then(randomOutput);  //loop back to randomOutput

                    randomOutput
                        .When(1)
                            .Then<GoodbyeWorld>();
                });
        }
    }

    /// <summary>
    /// RandomOutput
    /// </summary>
    public class RandomOutput : StepBody, ITransientDependency
    {
        private readonly ILogger<RandomOutput> _logger;
        /// <summary>
        /// RandomOutput
        /// </summary>
        /// <param name="logger"></param>
        public RandomOutput(ILogger<RandomOutput> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Run
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Random rnd = new Random();
            int value = rnd.Next(2);
            _logger.LogInformation("Generated random value {0}", value);
            return ExecutionResult.Outcome(value);
        }
    }

    /// <summary>
    /// CustomMessage
    /// </summary>
    public class CustomMessage : StepBody, ITransientDependency
    {
        private readonly ILogger<CustomMessage> _logger;
        /// <summary>
        /// CustomMessage
        /// </summary>
        /// <param name="logger"></param>
        public CustomMessage(ILogger<CustomMessage> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Run
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _logger.LogInformation(Message);
            return ExecutionResult.Next();
        }
    }


}
