using AIStudio.Common.DI;
using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Api.Controllers.Test.SamplesWorkflow
{
    /// <summary>
    /// IfWorkflow
    /// </summary>
    public class IfWorkflow : IWorkflow<MyData>
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id => "if-sample";

        /// <summary>
        /// Version
        /// </summary>
        public int Version => 1;

        /// <summary>
        /// Build
        /// </summary>
        /// <param name="builder"></param>
        public void Build(IWorkflowBuilder<MyData> builder)
        {
            builder
                .StartWith<SayHello>()
                .If(data => data.Counter < 3).Do(then => then
                    .StartWith<PrintMessage>()
                        .Input(step => step.Message, data => "Value is less than 3")
                )
                .If(data => data.Counter < 5).Do(then => then
                    .StartWith<PrintMessage>()
                        .Input(step => step.Message, data => "Value is less than 5")
                )
                .Then<SayGoodbye>();
        }        
    }

    /// <summary>
    /// PrintMessage
    /// </summary>
    public class PrintMessage : StepBody, ITransientDependency
    {
        private readonly ILogger<PrintMessage> _logger;
        /// <summary>
        /// PrintMessage
        /// </summary>
        /// <param name="logger"></param>
        public PrintMessage(ILogger<PrintMessage> logger)
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
