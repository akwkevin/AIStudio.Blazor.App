using AIStudio.Common.EventBus.Abstract;

namespace AIStudio.Common.EventBus.Local;

public class LocalEventPublisher : IEventPublisher
{
    private readonly IEventStore _eventStore;

    public LocalEventPublisher(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public virtual async Task PublishAsync<TEvent>(TEvent @event)
        where TEvent : class, IEventModel
    {
        await _eventStore.WriteAsync(@event);
    }
}
