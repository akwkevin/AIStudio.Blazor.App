using WorkflowCore.Interface;

namespace AIStudio.Api.Controllers.Test.SamplesWorkflow
{
    /// <summary>
    /// ActivityWorkflow
    /// </summary>
    class ActivityWorkflow : IWorkflow<MyData>
    {
        public string Id => "activity-sample";
        public int Version => 1;

        public void Build(IWorkflowBuilder<MyData> builder)
        {
            builder
                .StartWith<HelloWorld>()
                .Activity("get-approval", (data) => data.Request)
                    .Output(data => data.ApprovedBy, step => step.Result)
                .Then<CustomMessage>()
                    .Input(step => step.Message, data => "Approved by " + data.ApprovedBy)
                .Then<GoodbyeWorld>();
        }
    }

}
