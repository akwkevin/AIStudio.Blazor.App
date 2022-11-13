using AIStudio.Common.DI;
using WorkflowCore.Interface;
using WorkflowCore.Models;


namespace AIStudio.Api.Controllers.Test.SamplesWorkflow
{
    public class MultipleOutcomeWorkflow : IWorkflow
    {
        public string Id => "MultipleOutcomeWorkflow";

        public int Version => 1;

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

    public class TaskA : StepBody, ITransientDependency
    {
        private readonly ILogger<TaskA> _logger;
        /// <summary>
        /// TaskA
        /// </summary>
        /// <param name="logger"></param>
        public TaskA(ILogger<TaskA> logger)
        {
            _logger = logger;
        }
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _logger.LogInformation("Doing Task A");
            return ExecutionResult.Next();
        }
    }

    public class TaskB : StepBody, ITransientDependency
    {
        private readonly ILogger<TaskB> _logger;
        /// <summary>
        /// TaskB
        /// </summary>
        /// <param name="logger"></param>
        public TaskB(ILogger<TaskB> logger)
        {
            _logger = logger;
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _logger.LogInformation("Doing Task B");
            return ExecutionResult.Next();
        }
    }

    public class TaskC : StepBody, ITransientDependency
    {
        private readonly ILogger<TaskC> _logger;
        /// <summary>
        /// TaskC
        /// </summary>
        /// <param name="logger"></param>
        public TaskC(ILogger<TaskC> logger)
        {
            _logger = logger;
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _logger.LogInformation("Doing Task C");
            return ExecutionResult.Next();
        }
    }

    public class TaskD : StepBody, ITransientDependency
    {
        private readonly ILogger<TaskD> _logger;
        /// <summary>
        /// TaskD
        /// </summary>
        /// <param name="logger"></param>
        public TaskD(ILogger<TaskD> logger)
        {
            _logger = logger;
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _logger.LogInformation("Doing Task D");
            return ExecutionResult.Next();
        }
    }
}
