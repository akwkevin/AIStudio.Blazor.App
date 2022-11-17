using AIStudio.Common.CurrentUser;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Business.OA_Manage.Steps.Test
{
    public class OAGoodbyeWorld: OACOEndStep
    { 

        public override Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            GetStep(context);

            Console.WriteLine("Goodbye world");
            return Task.FromResult(ExecutionResult.Next());
        }
    }
}
