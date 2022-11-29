using AIStudio.Common.EventBus.Abstract;

namespace AIStudio.Common.EventBus.Local;

/// <summary>
/// 
/// </summary>
public interface IEventStore
{
    /// <summary>
    /// Writes the asynchronous.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="event">The event.</param>
    /// <returns></returns>
    ValueTask WriteAsync<TEvent>(TEvent @event)
        where TEvent : class, IEventModel;

    /// <summary>
    /// Reads the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    ValueTask<object> ReadAsync(CancellationToken cancellationToken);
}
