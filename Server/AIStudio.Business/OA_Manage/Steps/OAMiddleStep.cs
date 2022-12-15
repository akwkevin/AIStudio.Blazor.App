using WorkflowCore.Interface;
using WorkflowCore.Models;
using AIStudio.Common.CurrentUser;
using Microsoft.Extensions.Logging;
using AIStudio.Common.DI;

namespace AIStudio.Business.OA_Manage.Steps
{
    /// <summary>
    /// 中间节点
    /// </summary>
    public class OAMiddleStep : OABaseStep, ITransientDependency
    {
        public OAMiddleStep(IOA_UserFormStepBusiness userFormStepBusiness, IOA_UserFormBusiness userFormBusiness, IWorkflowRegistry registry) : base(userFormStepBusiness, userFormBusiness, registry)
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
