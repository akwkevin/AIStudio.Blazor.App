using StackExchange.Redis;

namespace AIStudio.Common.EventBus.Redis;

/// <summary>
/// 
/// </summary>
public class RedisEventBusOptions
{
    /// <summary>
    /// 用于连接 Redis 的配置。
    /// </summary>
    /// <value>
    /// The configuration.
    /// </value>
    public string Configuration { get; set; } = "127.0.0.1:6379";

    /// <summary>
    /// 用于连接 Redis 的配置。
    /// 优先级高于 Configuration .
    /// </summary>
    /// <value>
    /// The configuration options.
    /// </value>
    public ConfigurationOptions? ConfigurationOptions { get; set; }

    /// <summary>
    /// 获取或设置一个创建 ConnectionMultiplexer 实例的委托。
    /// </summary>
    /// <value>
    /// The connection multiplexer factory.
    /// </value>
    public Func<Task<IConnectionMultiplexer>>? ConnectionMultiplexerFactory { get; set; }
}
