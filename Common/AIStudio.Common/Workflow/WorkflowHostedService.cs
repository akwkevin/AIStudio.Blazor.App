using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WorkflowCore.Interface;

namespace AIStudio.Common.Workflow
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Hosting.IHostedService" />
    public class WorkflowHostedService : IHostedService
    {
        /// <summary>
        /// The workflow host
        /// </summary>
        private readonly IWorkflowHost _workflowHost;
        /// <summary>
        /// The service provider
        /// </summary>
        private readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// The options
        /// </summary>
        private readonly WorkflowSetupOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowHostedService"/> class.
        /// </summary>
        /// <param name="workflowHost">The workflow host.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="options">The options.</param>
        public WorkflowHostedService(IWorkflowHost workflowHost, IServiceProvider serviceProvider, IOptions<WorkflowSetupOptions> options)
        {
            _workflowHost = workflowHost;
            _serviceProvider = serviceProvider;
            _options = options.Value;
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _workflowHost.StartAsync(cancellationToken);

            using var scope = _serviceProvider.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            if (_options.StartHandle != null) await _options.StartHandle(serviceProvider);  
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _workflowHost.StopAsync(cancellationToken);

            using var scope = _serviceProvider.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            if (_options.ShutdownHandle != null) await _options.ShutdownHandle(serviceProvider);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowSetupOptions
    {
        /// <summary>
        /// 工作流启动时执行的程序
        /// </summary>
        /// <value>
        /// The start handle.
        /// </value>
        public Func<IServiceProvider, Task>? StartHandle { get; set; }

        /// <summary>
        /// 工作流关闭时执行的程序
        /// </summary>
        /// <value>
        /// The shutdown handle.
        /// </value>
        public Func<IServiceProvider, Task>? ShutdownHandle { get; set; }
    }
}
