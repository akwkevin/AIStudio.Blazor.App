using WorkflowCore.Interface;
using WorkflowCore.Models;
using AIStudio.Common.CurrentUser;
using Microsoft.Extensions.Logging;

namespace AIStudio.Business.OA_Manage.Steps
{
    /// <summary>
    /// 中间节点
    /// </summary>
    public class OAMiddleStep : OABaseStep
    {

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
