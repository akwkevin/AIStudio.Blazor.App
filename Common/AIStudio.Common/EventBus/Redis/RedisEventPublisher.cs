using AIStudio.Common.EventBus.Abstract;

namespace AIStudio.Common.EventBus.Redis;

/// <summary>
/// 
/// </summary>
/// <seealso cref="AIStudio.Common.EventBus.Abstract.IEventPublisher" />
public class RedisEventPublisher : IEventPublisher
{
    /// <summary>
    /// The redis manager
    /// </summary>
    private readonly IRedisManager _redisManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="RedisEventPublisher"/> class.
    /// </summary>
    /// <param name="redisManager">The redis manager.</param>
    public RedisEventPublisher(IRedisManager redisManager)
    {
        _redisManager = redisManager;
    }

    /// <summary>
    /// Publishes the asynchronous.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="event">The event.</param>
    /// <returns></returns>
    public Task PublishAsync<TEvent>(TEvent @event)
        where TEvent : class, IEventModel
    {
        return _redisManager.PublishAsync(@event);
    }
}
