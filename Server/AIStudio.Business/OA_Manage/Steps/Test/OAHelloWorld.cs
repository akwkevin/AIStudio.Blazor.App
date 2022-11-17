using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Business.OA_Manage.Steps.Test
{
    public class OAHelloWorld : OANormalStep
    {
        public string HelloName { get; set; }
        //在这中实现需要执行的方法
        //public override ExecutionResult Run(IStepExecutionContext context)
        //{
        //    GetStep(context);

        //    Console.WriteLine(HelloName + " Hello world");
        //    return ExecutionResult.Next();
        //}

        public override Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            GetStep(context);

            Console.WriteLine(HelloName + " Hello world");
            return Task.FromResult(ExecutionResult.Next());
        }
    }
}
