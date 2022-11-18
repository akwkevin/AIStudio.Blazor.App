using AIStudio.Entity.DTO.OA_Manage;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using AIStudio.Common.CurrentUser;
using Microsoft.Extensions.Logging;
using AIStudio.Common.DI;

namespace AIStudio.Business.OA_Manage.Steps
{
    /// <summary>
    /// 开始节点
    /// </summary>
    public class OAStartStep : OABaseStep, IStepBody, ITransientDependency
    {
        public OAStartStep(IOA_UserFormStepBusiness userFormStepBusiness, IOA_UserFormBusiness userFormBusiness, IWorkflowRegistry registry, IOperator @operator) : base(userFormStepBusiness, userFormBusiness, registry, @operator)
        {
        }

        /// <summary>
        /// 节点触发
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            OAData oAData = GetStep(context);            

            if (oAData.FirstStart)
            {
                oAData.FirstStart = false;
                OAStep.Status = 100;

                //改变流程图颜色
                var node = oAData.nodes.FirstOrDefault(p => p.id == OAStep.Id);
                if (node != null)
                {
                    node.color = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.LightGreen); 
                }

                return ExecutionResult.Next();
            }

            return await base.RunAsync(context);
          
        }
    }
}
