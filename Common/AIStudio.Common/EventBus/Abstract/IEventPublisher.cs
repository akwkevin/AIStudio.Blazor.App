namespace AIStudio.Common.EventBus.Abstract;

/// <summary>
/// 
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publishes the asynchronous.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="event">The event.</param>
    /// <returns></returns>
    Task PublishAsync<TEvent>(TEvent @event)
        where TEvent : class, IEventModel;
}
