using AIStudio.Common.EventBus.Abstract;

namespace AIStudio.Common.EventBus.Local;

public interface IEventStore
{
    ValueTask WriteAsync<TEvent>(TEvent @event)
        where TEvent : class, IEventModel;

    ValueTask<object> ReadAsync(CancellationToken cancellationToken);
}
