using AIStudio.Common.EventBus.Abstract;

namespace AIStudio.Common.EventBus.Core;

/// <summary>
/// 订阅者字典列表
/// </summary>
public class SubscriberDictionary
{
    /// <summary>
    /// The subscribers
    /// </summary>
    private readonly IDictionary<Type, List<Type>> _subscribers = new Dictionary<Type, List<Type>>();

    /// <summary>
    /// Initializes a new instance of the <see cref="SubscriberDictionary"/> class.
    /// </summary>
    internal SubscriberDictionary()
    {
    }

    /// <summary>
    /// Adds this instance.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <typeparam name="THandler">The type of the handler.</typeparam>
    public void Add<TEvent, THandler>()
        where TEvent : class, IEventModel
        where THandler : IEventHandler<TEvent>
    {
        var eventType = typeof(TEvent);
        var handlerType = typeof(THandler);

        if (!_subscribers.ContainsKey(eventType))
        {
            _subscribers[eventType] = new List<Type>();
        }

        _subscribers[eventType].Add(handlerType);
    }

    /// <summary>
    /// Adds the specified subscribers.
    /// </summary>
    /// <param name="subscribers">The subscribers.</param>
    internal void Add(SubscriberDictionary subscribers)
    {
        foreach (var subscriber in subscribers.ToDictionary())
        {
            _subscribers.Add(subscriber);
        }
    }

    /// <summary>
    /// Converts to dictionary.
    /// </summary>
    /// <returns></returns>
    internal IDictionary<Type, List<Type>> ToDictionary()
    {
        return _subscribers;
    }
}
