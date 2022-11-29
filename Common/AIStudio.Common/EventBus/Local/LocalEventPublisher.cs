using AIStudio.Common.EventBus.Abstract;

namespace AIStudio.Common.EventBus.Local;

/// <summary>
/// 
/// </summary>
/// <seealso cref="AIStudio.Common.EventBus.Abstract.IEventPublisher" />
public class LocalEventPublisher : IEventPublisher
{
    /// <summary>
    /// The event store
    /// </summary>
    private readonly IEventStore _eventStore;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalEventPublisher"/> class.
    /// </summary>
    /// <param name="eventStore">The event store.</param>
    public LocalEventPublisher(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    /// <summary>
    /// Publishes the asynchronous.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="event">The event.</param>
    public virtual async Task PublishAsync<TEvent>(TEvent @event)
        where TEvent : class, IEventModel
    {
        await _eventStore.WriteAsync(@event);
    }
}
