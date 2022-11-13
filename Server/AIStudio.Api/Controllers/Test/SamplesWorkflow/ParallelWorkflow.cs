using System;
using WorkflowCore.Interface;

namespace AIStudio.Api.Controllers.Test.SamplesWorkflow
{
    /// <summary>
    /// ParallelWorkflow
    /// </summary>
    public class ParallelWorkflow : IWorkflow<MyData>
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id => "parallel-sample";

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
                .Parallel()
                    .Do(then => 
                        then.StartWith<PrintMessage>()
                                .Input(step => step.Message, data => "Item 1.1")
                            .Then<PrintMessage>()
                                .Input(step => step.Message, data => "Item 1.2"))
                    .Do(then =>
                        then.StartWith<PrintMessage>()
                                .Input(step => step.Message, data => "Item 2.1")
                            .Then<PrintMessage>()
                                .Input(step => step.Message, data => "Item 2.2")
                            .Then<PrintMessage>()
                                .Input(step => step.Message, data => "Item 2.3"))
                    .Do(then =>
                        then.StartWith<PrintMessage>()
                                .Input(step => step.Message, data => "Item 3.1")
                            .Then<PrintMessage>()
                                .Input(step => step.Message, data => "Item 3.2"))
                .Join()
                .Then<SayGoodbye>();
        }        
    }
}
