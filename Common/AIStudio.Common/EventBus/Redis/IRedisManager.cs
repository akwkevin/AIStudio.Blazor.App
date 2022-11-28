using AIStudio.Common.EventBus.Abstract;
using StackExchange.Redis;

namespace AIStudio.Common.EventBus.Redis;

/// <summary>
/// 
/// </summary>
public interface IRedisManager
{
    /// <summary>
    /// Gets the connection.
    /// </summary>
    /// <value>
    /// The connection.
    /// </value>
    IConnectionMultiplexer Connection { get; }

    /// <summary>
    /// Gets the subscriber.
    /// </summary>
    /// <value>
    /// The subscriber.
    /// </value>
    ISubscriber Subscriber { get; }

    /// <summary>
    /// Starts the subscribe.
    /// </summary>
    void StartSubscribe();

    /// <summary>
    /// Publishes the asynchronous.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="event">The event.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken token = default)
        where TEvent : class, IEventModel;
}
