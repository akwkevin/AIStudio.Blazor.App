using AIStudio.Common.DI;
using WorkflowCore.Interface;
using WorkflowCore.Models;


namespace AIStudio.Api.Controllers.Test.SamplesWorkflow
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="WorkflowCore.Interface.IWorkflow" />
    public class MultipleOutcomeWorkflow : IWorkflow
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id => "MultipleOutcomeWorkflow";

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
        public void Build(IWorkflowBuilder<object> builder)
        {
            builder
                .StartWith<RandomOutput>(x => x.Name("Random Step"))
                    .When(0)
                        .Then<TaskA>()
                        .Then<TaskB>()                        
                        .End<RandomOutput>("Random Step")
                    .When(1)
                        .Then<TaskC>()
                        .Then<TaskD>()
                        .End<RandomOutput>("Random Step");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="WorkflowCore.Models.StepBody" />
    /// <seealso cref="AIStudio.Common.DI.ITransientDependency" />
    public class TaskA : StepBody, ITransientDependency
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<TaskA> _logger;
        /// <summary>
        /// TaskA
        /// </summary>
        /// <param name="logger">The logger.</param>
        public TaskA(ILogger<TaskA> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Runs the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _logger.LogInformation("Doing Task A");
            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="WorkflowCore.Models.StepBody" />
    /// <seealso cref="AIStudio.Common.DI.ITransientDependency" />
    public class TaskB : StepBody, ITransientDependency
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<TaskB> _logger;
        /// <summary>
        /// TaskB
        /// </summary>
        /// <param name="logger">The logger.</param>
        public TaskB(ILogger<TaskB> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Runs the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _logger.LogInformation("Doing Task B");
            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="WorkflowCore.Models.StepBody" />
    /// <seealso cref="AIStudio.Common.DI.ITransientDependency" />
    public class TaskC : StepBody, ITransientDependency
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<TaskC> _logger;
        /// <summary>
        /// TaskC
        /// </summary>
        /// <param name="logger">The logger.</param>
        public TaskC(ILogger<TaskC> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Runs the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _logger.LogInformation("Doing Task C");
            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="WorkflowCore.Models.StepBody" />
    /// <seealso cref="AIStudio.Common.DI.ITransientDependency" />
    public class TaskD : StepBody, ITransientDependency
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<TaskD> _logger;
        /// <summary>
        /// TaskD
        /// </summary>
        /// <param name="logger">The logger.</param>
        public TaskD(ILogger<TaskD> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Runs the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _logger.LogInformation("Doing Task D");
            return ExecutionResult.Next();
        }
    }
}
