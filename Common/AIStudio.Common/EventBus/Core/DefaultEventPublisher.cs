using AIStudio.Common.EventBus.Abstract;

namespace AIStudio.Common.EventBus.Core;

/// <summary>
/// 事件总线发布者默认实现
/// </summary>
/// <seealso cref="AIStudio.Common.EventBus.Abstract.IEventPublisher" />
public class DefaultEventPublisher : IEventPublisher
{
    /// <summary>
    /// Publishes the asynchronous.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="event">The event.</param>
    /// <returns></returns>
    public virtual Task PublishAsync<TEvent>(TEvent @event)
        where TEvent : class, IEventModel
    {
        return Task.CompletedTask;
    }
}
