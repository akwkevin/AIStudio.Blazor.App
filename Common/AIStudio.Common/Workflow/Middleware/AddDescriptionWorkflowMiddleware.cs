using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Common.Workflow.Middleware
{
    /// <summary>
    /// AddDescriptionWorkflowMiddleware
    /// </summary>
    /// <seealso cref="WorkflowCore.Interface.IWorkflowMiddleware" />
    public class AddDescriptionWorkflowMiddleware : IWorkflowMiddleware
    {
        /// <summary>
        /// The phase in the workflow execution to run this middleware in
        /// </summary>
        public WorkflowMiddlewarePhase Phase => WorkflowMiddlewarePhase.PreWorkflow;
        /// <summary>
        /// Runs the middleware on the given <see cref="T:WorkflowCore.Models.WorkflowInstance" />.
        /// </summary>
        /// <param name="workflow">The <see cref="T:WorkflowCore.Models.WorkflowInstance" />.</param>
        /// <param name="next">The next middleware in the chain.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> that completes asynchronously once the
        /// middleware chain finishes running.
        /// </returns>
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
        /// <value>
        /// The description.
        /// </value>
        string Description { get; }
    }
}
