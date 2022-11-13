using System;
using System.Collections.Generic;
using WorkflowCore.Interface;

namespace AIStudio.Api.Controllers.Test.SamplesWorkflow
{
    /// <summary>
    /// ForEachSyncWorkflow
    /// </summary>
    public class ForEachSyncWorkflow : IWorkflow
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id => "ForeachSync";

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
                .ForEach(data => new List<int> { 1, 2, 3, 4 }, data => false)
                    .Do(x => x
                        .StartWith<DisplayContext>()
                            .Input(step => step.Item, (data, context) => context.Item)
                        .Then<DoSomething>())
                .Then<SayGoodbye>();
        }        
    }    
}
