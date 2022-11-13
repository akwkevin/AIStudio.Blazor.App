using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Api.Controllers.Test.SamplesWorkflow
{
    /// <summary>
    /// PassingDataWorkflow
    /// </summary>
    public class PassingDataWorkflow : IWorkflow<MyDataClass>
    {
        private readonly ILogger<PassingDataWorkflow> _logger;
        /// <summary>
        /// PassingDataWorkflow
        /// </summary>
        /// <param name="logger"></param>
        public PassingDataWorkflow(ILogger<PassingDataWorkflow> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Build
        /// </summary>
        /// <param name="builder"></param>
        public void Build(IWorkflowBuilder<MyDataClass> builder)
        {
            builder
                .StartWith(context =>
                {
                    _logger.LogInformation("Starting PassingDataWorkflow...");
                    return ExecutionResult.Next();
                })
                .Then<AddNumbers>()
                    .Input(step => step.Input1, data => data.Value1)
                    .Input(step => step.Input2, data => data.Value2)
                    .Output(data => data.Value3, step => step.Output)
                .Then<CustomMessage>()
                    .Name("Print custom message")
                    .Input(step => step.Message, data => "The answer is " + data.Value3.ToString())
                .Then(context =>
                    {
                        _logger.LogInformation("PassingDataWorkflow complete");
                        return ExecutionResult.Next();
                    });
        }

        /// <summary>
        /// Id
        /// </summary>
        public string Id => "PassingDataWorkflow";

        /// <summary>
        /// Version
        /// </summary>
        public int Version => 1;

    }

    /// <summary>
    /// MyDataClass
    /// </summary>
    public class MyDataClass
    {
        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Value1
        /// </summary>
        public int Value1 { get; set; }

        /// <summary>
        /// Value2
        /// </summary>
        public int Value2 { get; set; }

        /// <summary>
        /// Value3
        /// </summary>
        public int Value3 { get; set; }

    }

    /// <summary>
    /// AddNumbers
    /// </summary>
    public class AddNumbers : StepBodyAsync
    {
        /// <summary>
        /// Input1
        /// </summary>
        public int Input1 { get; set; }

        /// <summary>
        /// Input2
        /// </summary>
        public int Input2 { get; set; }

        /// <summary>
        /// Output
        /// </summary>
        public int Output { get; set; }

        /// <summary>
        /// RunAsync
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            Output = (Input1 + Input2);
            return ExecutionResult.Next();
        }
    }

  

}
