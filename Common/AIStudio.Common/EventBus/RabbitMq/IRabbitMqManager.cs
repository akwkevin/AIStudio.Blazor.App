using AIStudio.Common.EventBus.Abstract;
using RabbitMQ.Client;

namespace AIStudio.Common.EventBus.RabbitMq;

/// <summary>
/// 
/// </summary>
public interface IRabbitMqManager
{
    /// <summary>
    /// RabbitMQ 连接实例
    /// </summary>
    /// <value>
    /// The connection.
    /// </value>
    IConnection Connection { get; }

    /// <summary>
    /// 启用的交换机名称
    /// </summary>
    /// <value>
    /// The name of the ex change.
    /// </value>
    public string ExChangeName { get; }

    /// <summary>
    /// 启用的交换机类型
    /// </summary>
    /// <value>
    /// The type of the ex change.
    /// </value>
    public string ExChangeType { get; }

    /// <summary>
    /// 启用的队列名称
    /// </summary>
    /// <value>
    /// The name of the queue.
    /// </value>
    public string QueueName { get; }

    /// <summary>
    /// 启动订阅。
    /// </summary>
    void StartSubscribe();

    /// <summary>
    /// 发布消息。
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="event">The event.</param>
    /// <returns></returns>
    Task PublishAsync<TEvent>(TEvent @event)
        where TEvent : class, IEventModel;
}
