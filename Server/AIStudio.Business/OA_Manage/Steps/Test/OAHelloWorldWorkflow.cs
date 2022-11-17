using WorkflowCore.Interface;

namespace AIStudio.Business.OA_Manage.Steps.Test
{
    public class OAHelloWorldWorkflow : IWorkflow
    {
        public string Id => nameof(OAHelloWorld);

        public int Version => 0;

        //测试类，等效于HelloWorld
        public void Build(IWorkflowBuilder<object> builder)
        {
            builder
                .StartWith<OAHelloWorld>()
                .Then<OAGoodbyeWorld>();
        }
    }
}
