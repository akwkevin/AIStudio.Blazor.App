using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WorkflowCore.Interface;

namespace AIStudio.Common.Workflow
{
    public class WorkflowHostedService : IHostedService
    {
        private readonly IWorkflowHost _workflowHost;
        private readonly IServiceProvider _serviceProvider;
        private readonly WorkflowSetupOptions _options;

        public WorkflowHostedService(IWorkflowHost workflowHost, IServiceProvider serviceProvider, IOptions<WorkflowSetupOptions> options)
        {
            _workflowHost = workflowHost;
            _serviceProvider = serviceProvider;
            _options = options.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (_options.StartHandle != null) await _options.StartHandle(_serviceProvider);

            await _workflowHost.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _workflowHost.StopAsync(cancellationToken);
        }
    }

    public class WorkflowSetupOptions
    {
        /// <summary>
        /// 工作流启动时执行的程序
        /// </summary>
        public Func<IServiceProvider, Task>? StartHandle { get; set; }

        /// <summary>
        /// 工作流关闭时执行的程序
        /// </summary>
        public Func<IServiceProvider, Task>? ShutdownHandle { get; set; }
    }
}
