using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Api.Controllers.Test.SamplesWorkflow
{
    /// <summary>
    /// WhileWorkflow
    /// </summary>
    public class WhileWorkflow : IWorkflow<MyData>
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id => "While";

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
                .While(data => data.Counter < 3)
                    .Do(x => x
                        .StartWith<DoSomething>()
                        .Then<IncrementStep>()
                            .Input(step => step.Value1, data => data.Counter)
                            .Output(data => data.Counter, step => step.Value2))
                .Then<SayGoodbye>();
        }        
    }

    /// <summary>
    /// MyData
    /// </summary>
    public class MyData
    {
        /// <summary>
        /// Counter
        /// </summary>
        public int Counter { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Request
        /// </summary>
        public string Request { get; set; }

        /// <summary>
        /// ApprovedBy
        /// </summary>
        public string ApprovedBy { get; set; }
    }

    /// <summary>
    /// IncrementStep
    /// </summary>
    public class IncrementStep : StepBody
    {
        /// <summary>
        /// Value1
        /// </summary>
        public int Value1 { get; set; }

        /// <summary>
        /// Value2
        /// </summary>
        public int Value2 { get; set; }

        /// <summary>
        /// Run
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Value2 = Value1 + 1;
            return ExecutionResult.Next();
        }
    }
}
