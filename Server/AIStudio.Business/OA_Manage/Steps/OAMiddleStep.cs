using WorkflowCore.Interface;
using WorkflowCore.Models;
using AIStudio.Common.CurrentUser;

namespace AIStudio.Business.OA_Manage.Steps
{
    /// <summary>
    /// 中间节点
    /// </summary>
    public class OAMiddleStep : OABaseStep
    {
        /// <summary>
        /// 中间节点
        /// </summary>
        /// <param name="userFormStepBusiness"></param>
        /// <param name="userFormBusiness"></param>
        /// <param name="registry"></param>
        /// <param name="operator"></param>
        public OAMiddleStep(IOA_UserFormStepBusiness userFormStepBusiness, IOA_UserFormBusiness userFormBusiness, IWorkflowRegistry registry, IOperator @operator) 
            : base(userFormStepBusiness, userFormBusiness, registry, @operator)
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
