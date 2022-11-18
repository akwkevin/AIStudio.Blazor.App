using AIStudio.Entity.DTO.OA_Manage;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Primitives;
using AIStudio.Common.CurrentUser;
using AIStudio.Common.DI;

namespace AIStudio.Business.OA_Manage.Steps
{
    /// <summary>
    /// 条件分支
    /// </summary>
    public class OADecideStep: Decide, ITransientDependency
    {
        /// <summary>
        /// 节点触发
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            if (!(context.Workflow.Data is OAData))
                throw new ArgumentException();

            OAData oAData = context.Workflow.Data as OAData;
            var OAStep = oAData.Steps.FirstOrDefault(p => p.Id == context.Step.ExternalId);
            OAStep.Status = 100;

            //改变流程图颜色
            var node = oAData.nodes.FirstOrDefault(p => p.id == context.Step.ExternalId);
            if (node != null)
            {
                node.color = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.LightGreen);
            }     

            return base.Run(context);
        }
    }
}
