using AIStudio.Common.EventBus.Abstract;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AIStudio.Common.EventBus.Local;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Microsoft.Extensions.Hosting.BackgroundService" />
public class LocalEventBusHostedService : BackgroundService
{
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<LocalEventBusHostedService> _logger;
    /// <summary>
    /// The subscribe executer
    /// </summary>
    private readonly ISubscribeManager _subscribeExecuter;
    /// <summary>
    /// The queue
    /// </summary>
    private readonly IEventStore _queue;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalEventBusHostedService"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="subscribeExecuter">The subscribe executer.</param>
    /// <param name="queue">The queue.</param>
    public LocalEventBusHostedService(ILogger<LocalEventBusHostedService> logger,
                                      ISubscribeManager subscribeExecuter,
                                      IEventStore queue)
    {
        _logger = logger;
        _subscribeExecuter = subscribeExecuter;
        _queue = queue;
    }

    /// <summary>
    /// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The implementation should return a task that represents
    /// the lifetime of the long running operation(s) being performed.
    /// </summary>
    /// <param name="stoppingToken">Triggered when <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is called.</param>
    /// <returns>
    /// A <see cref="T:System.Threading.Tasks.Task" /> that represents the long running operations.
    /// </returns>
    /// <remarks>
    /// See <see href="https://docs.microsoft.com/dotnet/core/extensions/workers">Worker Services in .NET</see> for implementation guidelines.
    /// </remarks>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogTrace($"{nameof(LocalEventBusHostedService)} is running.");

        return ProcessTaskQueueAsync(stoppingToken);
    }

    /// <summary>
    /// Processes the task queue asynchronous.
    /// </summary>
    /// <param name="stoppingToken">The stopping token.</param>
    private async Task ProcessTaskQueueAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // 从队列中读取事件消息
            object @event = await _queue.ReadAsync(stoppingToken);

            // 处理事件
            await _subscribeExecuter.ProcessEvent(@event);
        }
    }

    /// <summary>
    /// Stops the asynchronous.
    /// </summary>
    /// <param name="stoppingToken">The stopping token.</param>
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogTrace($"{nameof(LocalEventBusHostedService)} is stopping.");

        await base.StopAsync(stoppingToken);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    public override void Dispose()
    {
        _logger.LogTrace($"{nameof(LocalEventBusHostedService)} is dispose.");

        base.Dispose();
    }
}
