using AIStudio.Common.DI;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Api.Controllers.Test.SamplesWorkflow
{
    /// <summary>
    /// CompensatingWorkflow
    /// </summary>
    class CompensatingWorkflow : IWorkflow
    {
        private readonly ILogger<CompensatingWorkflow> _logger;
        /// <summary>
        /// CompensatingWorkflow
        /// </summary>
        /// <param name="logger"></param>
        public CompensatingWorkflow(ILogger<CompensatingWorkflow> logger)
        {
            _logger = logger;
        }

        public string Id => "compensate-sample";
        public int Version => 1;

        public void Build(IWorkflowBuilder<object> builder)
        {
           
            builder
                .StartWith(context => _logger.LogInformation("Begin"))
                .Saga(saga => saga
                    .StartWith<Task1>()
                        .CompensateWith<UndoTask1>()
                    //Task2 一直在抛异常，导致任务一直重试
                    .Then<Task2>()
                        .CompensateWith<UndoTask2>()
                    .Then<Task3>()
                        .CompensateWith<UndoTask3>()
                )
                    .OnError(WorkflowCore.Models.WorkflowErrorHandling.Retry, TimeSpan.FromSeconds(5))
                .Then(context => _logger.LogInformation("End"));
        }
    }

    /// <summary>
    /// Task1
    /// </summary>
    public class Task1 : StepBody, ITransientDependency
    {
        private readonly ILogger<Task1> _logger;
        /// <summary>
        /// Task1
        /// </summary>
        /// <param name="logger"></param>
        public Task1(ILogger<Task1> logger)
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
            _logger.LogInformation("Doing Task 1");
            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// Task2
    /// </summary>
    public class Task2 : StepBody, ITransientDependency
    {
        private readonly ILogger<Task2> _logger;
        /// <summary>
        /// Task2
        /// </summary>
        /// <param name="logger"></param>
        public Task2(ILogger<Task2> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Run
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _logger.LogInformation("Doing Task 2");
            throw new Exception();
        }
    }

    /// <summary>
    /// Task3
    /// </summary>
    public class Task3 : StepBody, ITransientDependency
    {
        private readonly ILogger<Task3> _logger;
        /// <summary>
        /// Task3
        /// </summary>
        /// <param name="logger"></param>
        public Task3(ILogger<Task3> logger)
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
            _logger.LogInformation("Doing Task 3");
            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// UndoTask1
    /// </summary>
    public class UndoTask1 : StepBody, ITransientDependency
    {
        private readonly ILogger<UndoTask1> _logger;
        /// <summary>
        /// UndoTask1
        /// </summary>
        /// <param name="logger"></param>
        public UndoTask1(ILogger<UndoTask1> logger)
        {
            _logger = logger;
        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _logger.LogInformation("Undoing Task 1");
            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// UndoTask2
    /// </summary>
    public class UndoTask2 : StepBody, ITransientDependency
    {
        private readonly ILogger<UndoTask2> _logger;
        /// <summary>
        /// UndoTask2
        /// </summary>
        /// <param name="logger"></param>
        public UndoTask2(ILogger<UndoTask2> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            _logger.LogInformation("Undoing Task 2");
            return ExecutionResult.Next();
        }
    }

    /// <summary>
    /// UndoTask3
    /// </summary>
    public class UndoTask3 : StepBody, ITransientDependency
    {
        private readonly ILogger<UndoTask3> _logger;
        /// <summary>
        /// UndoTask3
        /// </summary>
        /// <param name="logger"></param>
        public UndoTask3(ILogger<UndoTask3> logger)
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
            _logger.LogInformation("Undoing Task 3");
            return ExecutionResult.Next();
        }
    }
}