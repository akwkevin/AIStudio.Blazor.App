﻿using AIStudio.Entity.DTO.OA_Manage;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Business.OA_Manage.Step
{
    /// <summary>
    /// 终止节点
    /// </summary>
    public class OAEndStep : OABaseStep, IEndStep
    {
        /// <summary>
        /// 
        /// </summary>
        public OAEndStep() 
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
            OAStep.Status = 100;

            //改变流程图颜色
            var node = oAData.nodes.FirstOrDefault(p => p.id == OAStep.Id);
            if (node != null)
            {
                node.color = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.LightGreen);
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