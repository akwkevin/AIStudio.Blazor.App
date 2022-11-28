using AIStudio.Entity.DTO.OA_Manage;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using AIStudio.Common.CurrentUser;
using Microsoft.Extensions.Logging;
using AIStudio.Common.DI;
using AIStudio.Util.DiagramEntity;

namespace AIStudio.Business.OA_Manage.Steps
{
    /// <summary>
    /// 开始节点
    /// </summary>
    public class OAStartStep : OABaseStep, IStepBody, ITransientDependency
    {
        /// <summary>Initializes a new instance of the <see cref="OAStartStep" /> class.</summary>
        /// <param name="userFormStepBusiness"></param>
        /// <param name="userFormBusiness"></param>
        /// <param name="registry"></param>
        /// <param name="operator"></param>
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
            OA_Data oAData = GetStep(context);            

            if (oAData.FirstStart)
            {
                oAData.FirstStart = false;
                OAStep.Status = 100;

                //改变流程图颜色
                var node = oAData.Nodes.FirstOrDefault(p => p.Id == OAStep.Id);
                if (node != null)
                {
                    node.Color = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.LightGreen); 
                }

                return ExecutionResult.Next();
            }

            return await base.RunAsync(context);
          
        }
    }
}
