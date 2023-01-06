using AIStudio.Common.CurrentUser;
using AIStudio.Common.DI;
using AIStudio.Entity.DTO.OA_Manage;
using AIStudio.Util.DiagramEntity;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Business.OA_Manage.Steps
{
    /// <summary>
    /// 并行结束
    /// </summary>
    public class OACOEndStep : OABaseStep , IEndStep, ITransientDependency
    {
        public OACOEndStep(IOA_UserFormStepBusiness userFormStepBusiness, IOA_UserFormBusiness userFormBusiness, IWorkflowRegistry registry, IOperator @operator) : base(userFormStepBusiness, userFormBusiness, registry)
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

            //改变流程图颜色
            var node = oAData.Nodes?.FirstOrDefault(p => p.Id == OAStep.Id);
            if (node != null)
            {
                node.Color = "#FFA500";
            }

            if (OAStep.PreStepId != null)
            {
                var def = _registry.GetDefinition(context.Workflow.WorkflowDefinitionId, context.Workflow.Version);
                foreach (var id in OAStep.PreStepId)
                {                   
                    var pre = def.Steps.Find(p => p.ExternalId == id);
                    if (!context.Workflow.ExecutionPointers.Any(p => p.StepId == pre.Id && p.Status == PointerStatus.Complete))
                    {
                        var result = ExecutionResult.Persist(context.PersistenceData);
                        result.SleepFor = TimeSpan.FromSeconds(5);
                        return result;
                    }
                }
            }

            OAStep.Status = 100;

            //改变流程图颜色
            if (node != null)
            {
                node.Color = "#90EE90";
            }           

            return ExecutionResult.Next();
        }
    }
}
