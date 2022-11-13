using AIStudio.Common.DI;
using System;
using System.Collections.Generic;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Api.Controllers.Test.SamplesWorkflow
{
    /// <summary>
    /// ForEachWorkflow
    /// </summary>
    public class ForEachWorkflow : IWorkflow
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id => "Foreach";
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
                .StartWith<SayHello>()
                .ForEach(data => new List<int> { 1, 2, 3, 4 })
                    .Do(x => x
                        .StartWith<DisplayContext>()
                            .Input(step => step.Item, (data, context) => context.Item)
                        .Then<DoSomething>())
                .Then<SayGoodbye>();
        }        
    }

    /// <summary>
    /// DisplayContext
    /// </summary>
    public class DisplayContext : StepBody, ITransientDependency
    {
        private readonly ILogger<DisplayContext> _logger;
        /// <summary>
        /// DisplayContext
        /// </summary>
        /// <param name="logger"></param>
        public DisplayContext(ILogger<DisplayContext> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Item
        /// </summary>
        public object Item { get; set; }

        /// <summary>
        /// Run
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _logger.LogInformation($"Working on item {Item}");
            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// DoSomething
    /// </summary>
    public class DoSomething : StepBody, ITransientDependency
    {
        private readonly ILogger<DoSomething> _logger;
        /// <summary>
        /// DoSomething
        /// </summary>
        /// <param name="logger"></param>
        public DoSomething(ILogger<DoSomething> logger)
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
            _logger.LogInformation("Doing something...");
            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// SayGoodbye
    /// </summary>
    public class SayGoodbye : StepBody, ITransientDependency
    {
        private readonly ILogger<SayGoodbye> _logger;
        /// <summary>
        /// SayGoodbye
        /// </summary>
        /// <param name="logger"></param>
        public SayGoodbye(ILogger<SayGoodbye> logger)
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
            _logger.LogInformation("Goodbye");
            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// SayHello
    /// </summary>
    public class SayHello : StepBody, ITransientDependency
    {
        private readonly ILogger<SayHello> _logger;
        /// <summary>
        /// SayHello
        /// </summary>
        /// <param name="logger"></param>
        public SayHello(ILogger<SayHello> logger)
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
            _logger.LogInformation("Hello");
            return ExecutionResult.Next();
        }
    }
}
