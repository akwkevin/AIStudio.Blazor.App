using AIStudio.Common.EventBus.Abstract;

namespace AIStudio.Common.EventBus.RabbitMq;

/// <summary>
/// 
/// </summary>
/// <seealso cref="AIStudio.Common.EventBus.Abstract.IEventPublisher" />
public class RabbitMqEventPublisher : IEventPublisher
{
    /// <summary>
    /// The rabbit mq manager
    /// </summary>
    private readonly IRabbitMqManager _rabbitMqManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="RabbitMqEventPublisher"/> class.
    /// </summary>
    /// <param name="rabbitMqManager">The rabbit mq manager.</param>
    public RabbitMqEventPublisher(IRabbitMqManager rabbitMqManager)
    {
        _rabbitMqManager = rabbitMqManager;
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
        return _rabbitMqManager.PublishAsync(@event);
    }
}
