using AIStudio.Common.EventBus.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AIStudio.Common.EventBus.Core;

/// <summary>
/// 
/// </summary>
/// <seealso cref="AIStudio.Common.EventBus.Abstract.ISubscribeManager" />
public class SubscribeManager : ISubscribeManager
{
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<SubscribeManager> _logger;
    /// <summary>
    /// The service provider
    /// </summary>
    private readonly IServiceProvider _serviceProvider;
    /// <summary>
    /// The subscriber dictionary
    /// </summary>
    private readonly SubscriberDictionary _subscriberDictionary;

    /// <summary>
    /// 订阅者列表
    /// </summary>
    public IDictionary<Type, List<Type>> Subscribers => _subscriberDictionary.ToDictionary();

    /// <summary>
    /// Initializes a new instance of the <see cref="SubscribeManager"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="subscriberDictionary">The subscriber dictionary.</param>
    public SubscribeManager(ILogger<SubscribeManager> logger,
                            IServiceProvider serviceProvider,
                            SubscriberDictionary subscriberDictionary)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _subscriberDictionary = subscriberDictionary;
    }

    /// <summary>
    /// 处理事件
    /// </summary>
    /// <param name="event"></param>
    public virtual async Task ProcessEvent(object @event)
    {
        Type eventType = @event.GetType();
        if (Subscribers.TryGetValue(eventType, out var handlerTypes))
        {
            using var scope = _serviceProvider.CreateScope();
            foreach (var handlerType in handlerTypes)
            {
                try
                {
                    var handler = scope.ServiceProvider.GetService(handlerType);
                    var handle = handlerType.GetMethod("Handle");

                    //var handlerGenericType = typeof(IEventHandler<>).MakeGenericType(eventType);
                    //var handle = handlerGenericType.GetMethod("Handle");

                    if (handle != null)
                    {
                        // 调用 Handle 方法
                        var result = handle.Invoke(handler, new object[] { @event });

                        // 如果返回 Task 则等待
                        if (result is Task task)
                        {
                            await task;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"事件 {eventType.Name} 发生错误，错误信息：{ex.Message}");
                }
            }
        }
    }
}
