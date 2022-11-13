using AIStudio.Common.DI;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Api.Controllers.Test.SamplesWorkflow
{
    /// <summary>
    /// HelloWorldWorkflow
    /// </summary>
    public class HelloWorldWorkflow : IWorkflow
    {
        /// <summary>
        /// Build
        /// </summary>
        /// <param name="builder"></param>
        public void Build(IWorkflowBuilder<object> builder)
        {
            builder
                .UseDefaultErrorBehavior(WorkflowErrorHandling.Suspend)
                .StartWith<HelloWorld>()
                .Then<GoodbyeWorld>();
        }

        /// <summary>
        /// Id
        /// </summary>
        public string Id => "HelloWorld";

        /// <summary>
        /// Version
        /// </summary>
        public int Version => 1;

    }

    /// <summary>
    /// GoodbyeWorld
    /// </summary>
    public class GoodbyeWorld : StepBody, ITransientDependency
    {
        private readonly ILogger<GoodbyeWorld> _logger;
        /// <summary>
        /// GoodbyeWorld
        /// </summary>
        /// <param name="logger"></param>
        public GoodbyeWorld(ILogger<GoodbyeWorld> logger)
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
            _logger.LogInformation("Goodbye world");
            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// HelloWorld
    /// </summary>
    public class HelloWorld : StepBody, ITransientDependency
    {
        private readonly ILogger<HelloWorld> _logger;
        /// <summary>
        /// HelloWorld
        /// </summary>
        /// <param name="logger"></param>
        public HelloWorld(ILogger<HelloWorld> logger)
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
            _logger.LogInformation("Hello world");
            return ExecutionResult.Next();
        }
    }
}
