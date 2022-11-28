using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Common.Workflow.Middleware
{
    /// <summary>
    /// Loosely based off this article:
    /// https://www.frakkingsweet.com/net-core-log-correlation-easy-access-to-headers/
    /// </summary>
    /// <seealso cref="WorkflowCore.Interface.IWorkflowStepMiddleware" />
    public class AddMetadataToLogsMiddleware: IWorkflowStepMiddleware
    {
        /// <summary>
        /// The log
        /// </summary>
        private readonly ILogger<AddMetadataToLogsMiddleware> _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddMetadataToLogsMiddleware"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public AddMetadataToLogsMiddleware(ILogger<AddMetadataToLogsMiddleware> log)
        {
            _log = log;
        }

        /// <summary>
        /// Handle the workflow step and return an <see cref="T:WorkflowCore.Models.ExecutionResult" />
        /// asynchronously. It is important to invoke <see cref="!:next" /> at some point
        /// in the middleware. Not doing so will prevent the workflow step from ever
        /// getting executed.
        /// </summary>
        /// <param name="context">The step's context.</param>
        /// <param name="body">An instance of the step body that is going to be run.</param>
        /// <param name="next">The next middleware in the chain.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task`1" /> of the workflow result.
        /// </returns>
        public async Task<ExecutionResult> HandleAsync(
            IStepExecutionContext context,
            IStepBody body,
            WorkflowStepDelegate next)
        {
            var workflowId = context.Workflow.Id;
            var stepId = context.Step.Id;

            using (_log.BeginScope("WorkflowId => {@WorkflowId}", workflowId))
            using (_log.BeginScope("StepId => {@StepId}", stepId))
            {
                return await next();
            }
        }
    }
}
