using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Common.Workflow.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="WorkflowCore.Interface.IWorkflowMiddleware" />
    public class PrintWorkflowSummaryMiddleware : IWorkflowMiddleware
    {
        /// <summary>
        /// The log
        /// </summary>
        private readonly ILogger<PrintWorkflowSummaryMiddleware> _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintWorkflowSummaryMiddleware"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public PrintWorkflowSummaryMiddleware(ILogger<PrintWorkflowSummaryMiddleware> log)
        {
            _log = log;
        }

        /// <summary>
        /// The phase in the workflow execution to run this middleware in
        /// </summary>
        public WorkflowMiddlewarePhase Phase => WorkflowMiddlewarePhase.PostWorkflow;

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
            if (!workflow.CompleteTime.HasValue)
            {
                return next();
            }

            var duration = workflow.CompleteTime.Value - workflow.CreateTime;
            _log.LogInformation($@"Workflow {workflow.Description} completed in {duration:g}");

            foreach (var step in workflow.ExecutionPointers)
            {
                var stepName = step.StepName;
                var stepDuration = (step.EndTime - step.StartTime) ?? TimeSpan.Zero;
                _log.LogInformation($"  - Step {stepName} completed in {stepDuration:g}");
            }

            return next();
        }
    }
}
