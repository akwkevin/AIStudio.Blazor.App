using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace AIStudio.Common.Workflow.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="WorkflowCore.Interface.IWorkflowStepMiddleware" />
    public class PollyRetryMiddleware : IWorkflowStepMiddleware
    {
        /// <summary>
        /// The step context key
        /// </summary>
        private const string StepContextKey = "WorkflowStepContext";
        /// <summary>
        /// The maximum retries
        /// </summary>
        private const int MaxRetries = 3;
        /// <summary>
        /// The log
        /// </summary>
        private readonly ILogger<PollyRetryMiddleware> _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="PollyRetryMiddleware"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public PollyRetryMiddleware(ILogger<PollyRetryMiddleware> log)
        {
            _log = log;
        }

        /// <summary>
        /// Gets the retry policy.
        /// </summary>
        /// <returns></returns>
        public IAsyncPolicy<ExecutionResult> GetRetryPolicy() =>
            Policy<ExecutionResult>
                .Handle<TimeoutException>()
                .RetryAsync(
                    MaxRetries,
                    (result, retryCount, context) =>
                        UpdateRetryCount(result.Exception, retryCount, context[StepContextKey] as IStepExecutionContext)
                );

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
            WorkflowStepDelegate next
        )
        {
            return await GetRetryPolicy().ExecuteAsync(ctx => next(), new Dictionary<string, object>
            {
                { StepContextKey, context }
            });
        }

        /// <summary>
        /// Updates the retry count.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="stepContext">The step context.</param>
        /// <returns></returns>
        private Task UpdateRetryCount(
            Exception exception,
            int retryCount,
            IStepExecutionContext stepContext)
        {
            var stepInstance = stepContext.ExecutionPointer;
            stepInstance.RetryCount = retryCount;

            _log.LogWarning(
                exception,
                "Exception occurred in step {StepId}. Retrying [{RetryCount}/{MaxCount}]",
                stepInstance.Id,
                retryCount,
                MaxRetries
            );

            // TODO: Come up with way to persist workflow
            return Task.CompletedTask;
        }
    }
}
