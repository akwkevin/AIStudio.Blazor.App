using AIStudio.Common.DI;
using AIStudio.Common.Workflow.Middleware;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Api.Controllers.Test.SamplesWorkflow
{
    /// <summary>
    /// FlakyConnectionWorkflow
    /// </summary>
    public class FlakyConnectionWorkflow : IWorkflow<FlakyConnectionParams>
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id => "flaky-sample";

        /// <summary>
        /// Version
        /// </summary>
        public int Version => 1;

        /// <summary>
        /// Build
        /// </summary>
        /// <param name="builder"></param>
        public void Build(IWorkflowBuilder<FlakyConnectionParams> builder)
        {
            builder
                .StartWith<LogMessage>()
                .Input(x => x.Message, _ => "Starting workflow")

                .Then<FlakyConnection>()
                .Input(x => x.SucceedAfterAttempts, _ => 3)

                .Then<LogMessage>()
                .Input(x => x.Message, _ => "Finishing workflow");
        }
    }


    /// <summary>
    /// FlakyConnectionParams
    /// </summary>
    public class FlakyConnectionParams : IDescriptiveWorkflowParams
    {
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// FlakyConnection
    /// </summary>
    public class FlakyConnection : StepBodyAsync, ITransientDependency
    {
        private static readonly TimeSpan Delay = TimeSpan.FromSeconds(1);
        private int _currentCallCount = 0;

        /// <summary>
        /// SucceedAfterAttempts
        /// </summary>
        public int? SucceedAfterAttempts { get; set; } = 3;

        /// <summary>
        /// RunAsync
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="TimeoutException"></exception>
        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            if (SucceedAfterAttempts.HasValue && _currentCallCount >= SucceedAfterAttempts.Value)
            {
                return ExecutionResult.Next();
            }

            _currentCallCount++;
            await Task.Delay(Delay);
            throw new TimeoutException("A call has timed out");
        }
    }

    /// <summary>
    /// LogMessage
    /// </summary>
    public class LogMessage : StepBodyAsync, ITransientDependency
    {
        private readonly ILogger<LogMessage> _log;
        /// <summary>
        /// LogMessage
        /// </summary>
        /// <param name="log"></param>
        public LogMessage(ILogger<LogMessage> log)
        {
            _log = log;
        }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// RunAsync
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            if (Message != null)
            {
                _log.LogInformation(Message);
            }

            return Task.FromResult(ExecutionResult.Next());
        }
    }
}
