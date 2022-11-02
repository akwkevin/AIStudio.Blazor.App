using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Business.OA_Manage.Step
{
    /// <summary>
    /// 中间节点
    /// </summary>
    public class OAMiddleStep : OABaseStep
    {
        /// <summary>
        /// 
        /// </summary>
        public OAMiddleStep()
        {

        }


        /// <summary>
        /// 节点触发
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            return await base.RunAsync(context);
        }
    }
}
