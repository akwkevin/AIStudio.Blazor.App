using AIStudio.Common.DI;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using AIStudio.Util.DiagramEntity;

namespace AIStudio.Business.OA_Manage.Steps
{
    /// <summary>
    /// 并行开始
    /// </summary>
    public class OACOBeginStep : StepBody, ITransientDependency
    {
        /// <summary>
        /// 节点触发
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            if (!(context.Workflow.Data is OA_Data))
                throw new ArgumentException();

            OA_Data oAData = context.Workflow.Data as OA_Data;
            var OAStep = oAData.Steps.FirstOrDefault(p => p.Id == context.Step.ExternalId);
            OAStep.Status = 100;

            //改变流程图颜色
            var node = oAData.Nodes.FirstOrDefault(p => p.Id == context.Step.ExternalId);
            if (node != null)
            {
                node.Color = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.LightGreen);
            }

            return ExecutionResult.Next();
        }
    }
}
