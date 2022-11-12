using AIStudio.Business.OA_Manage.Steps;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Business.OA_Manage.HelloWorldSteps
{
    public class GoodbyeWorld : OANormalStep
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            GetStep(context);

            Console.WriteLine("Goodbye world");
            return ExecutionResult.Next();
        }
    }
}
