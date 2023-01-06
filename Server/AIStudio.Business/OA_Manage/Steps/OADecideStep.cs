using AIStudio.Common.DI;
using AIStudio.Entity.DTO.OA_Manage;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Primitives;

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
            if (!(context.Workflow.Data is OA_Data))
                throw new ArgumentException();

            OA_Data oAData = context.Workflow.Data as OA_Data;
            var OAStep = oAData.Steps.FirstOrDefault(p => p.Id == context.Step.ExternalId);
            OAStep.Status = 100;

            //改变流程图颜色
            var node = oAData.Nodes.FirstOrDefault(p => p.Id == context.Step.ExternalId);
            if (node != null)
            {
                node.Color = "#90EE90";
            }     

            return base.Run(context);
        }
    }
}
