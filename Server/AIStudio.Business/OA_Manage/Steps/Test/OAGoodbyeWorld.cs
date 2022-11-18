using AIStudio.Common.CurrentUser;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using AIStudio.Common.DI;

namespace AIStudio.Business.OA_Manage.Steps.Test
{
    public class OAGoodbyeWorld: OAEndStep, ITransientDependency
    {
        public OAGoodbyeWorld(IOA_UserFormStepBusiness userFormStepBusiness, IOA_UserFormBusiness userFormBusiness, IWorkflowRegistry registry, IOperator @operator) : base(userFormStepBusiness, userFormBusiness, registry, @operator)
        {
        }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            Console.WriteLine("Goodbye world");
            return await base.RunAsync(context);
        }
    }
}
