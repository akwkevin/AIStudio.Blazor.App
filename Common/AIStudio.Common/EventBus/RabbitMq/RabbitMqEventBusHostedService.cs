using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AIStudio.Common.EventBus.RabbitMq;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Microsoft.Extensions.Hosting.IHostedService" />
public class RabbitMqEventBusHostedService : IHostedService
{
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<RabbitMqEventBusHostedService> _logger;
    /// <summary>
    /// The rabbit mq manager
    /// </summary>
    private readonly IRabbitMqManager _rabbitMqManager;
    /// <summary>
    /// Initializes a new instance of the <see cref="RabbitMqEventBusHostedService"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="rabbitMqManager">The rabbit mq manager.</param>
    public RabbitMqEventBusHostedService(ILogger<RabbitMqEventBusHostedService> logger,
                                         IRabbitMqManager rabbitMqManager)
    {
        _logger = logger;
        _rabbitMqManager = rabbitMqManager;
    }

    /// <summary>
    /// Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns></returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(RabbitMqEventBusHostedService)} is running.");

        // 启动订阅
        _rabbitMqManager.StartSubscribe();

        return Task.CompletedTask;
    }

    /// <summary>
    /// Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    /// <returns></returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(RabbitMqEventBusHostedService)} is stopping.");

        return Task.CompletedTask;
    }
}
