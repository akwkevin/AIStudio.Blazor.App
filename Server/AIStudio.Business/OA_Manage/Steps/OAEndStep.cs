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
    /// 终止节点
    /// </summary>
    public class OAEndStep : OABaseStep, IEndStep, ITransientDependency
    {
        public OAEndStep(IOA_UserFormStepBusiness userFormStepBusiness, IOA_UserFormBusiness userFormBusiness, IWorkflowRegistry registry, IOperator @operator) : base(userFormStepBusiness, userFormBusiness, registry, @operator)
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
            OAStep.Status = 100;

            //改变流程图颜色
            var node = oAData.Nodes?.FirstOrDefault(p => p.Id == OAStep.Id);
            if (node != null)
            {
                node.Color = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.LightGreen);
            }

            var form = await _userFormBusiness.GetEntityAsync(context.Workflow.Id);
            if (form == null)
                throw new ArgumentException();
           
            form.Status = 100;
            form.ModifyTime = DateTime.Now;

            await _userFormBusiness.UpdateAsync(form);

            return ExecutionResult.Next();
        }
    }
}
