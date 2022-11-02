using WorkflowCore.Interface;

namespace AIStudio.Business.OA_Manage.HelloWorldStep
{
    public class HelloWorldWorkflow : IWorkflow
    {
        public string Id => nameof(HelloWorld);

        public int Version => 1;

        //测试类，等效于HelloWorld
        public void Build(IWorkflowBuilder<object> builder)
        {
            builder
                .StartWith<HelloWorld>()
                .Then<GoodbyeWorld>(); 
        }
    }
}
