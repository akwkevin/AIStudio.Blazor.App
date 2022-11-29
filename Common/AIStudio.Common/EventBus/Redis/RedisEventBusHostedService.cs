using AIStudio.Common.EventBus.RabbitMq;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AIStudio.Common.EventBus.Redis;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Microsoft.Extensions.Hosting.IHostedService" />
public class RedisEventBusHostedService : IHostedService
{
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<RedisEventBusHostedService> _logger;
    /// <summary>
    /// The redis manager
    /// </summary>
    private readonly IRedisManager _redisManager;
    /// <summary>
    /// Initializes a new instance of the <see cref="RedisEventBusHostedService"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="redisManager">The redis manager.</param>
    public RedisEventBusHostedService(ILogger<RedisEventBusHostedService> logger,
                                      IRedisManager redisManager)
    {
        _logger = logger;
        _redisManager = redisManager;
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
        _redisManager.StartSubscribe();

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
