using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Common.Workflow.Middleware
{
    /// <summary>
    /// AddDescriptionWorkflowMiddleware
    /// </summary>
    public class AddDescriptionWorkflowMiddleware : IWorkflowMiddleware
    {
        public WorkflowMiddlewarePhase Phase => WorkflowMiddlewarePhase.PreWorkflow;
        public Task HandleAsync(WorkflowInstance workflow, WorkflowDelegate next)
        {
            if (workflow.Data is IDescriptiveWorkflowParams descriptiveParams)
            {
                workflow.Description = descriptiveParams.Description;
            }

            return next();
        }
    }

    /// <summary>
    /// IDescriptiveWorkflowParams
    /// </summary>
    public interface IDescriptiveWorkflowParams
    {
        /// <summary>
        /// Description
        /// </summary>
        string Description { get; }
    }
}
