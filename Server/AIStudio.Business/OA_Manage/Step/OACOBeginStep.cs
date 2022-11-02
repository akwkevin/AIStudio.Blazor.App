using AIStudio.Entity.DTO.OA_Manage;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Business.OA_Manage.Step
{
    /// <summary>
    /// 并行开始
    /// </summary>
    public class OACOBeginStep : StepBody
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

            return ExecutionResult.Next();
        }
    }
}
