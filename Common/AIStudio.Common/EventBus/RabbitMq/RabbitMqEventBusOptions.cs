namespace AIStudio.Common.EventBus.RabbitMq;

/// <summary>
/// 
/// </summary>
public class RabbitMqEventBusOptions
{
    /// <summary>
    /// RabbitMQ 有效。交换机名称。
    /// </summary>
    /// <value>
    /// The name of the ex change.
    /// </value>
    public string ExChangeName { get; set; } = "event_bus_exchange";

    /// <summary>
    /// RabbitMQ 有效。交换机类型。
    /// </summary>
    /// <value>
    /// The type of the ex change.
    /// </value>
    public string ExChangeType { get; set; } = "direct";

    /// <summary>
    /// RabbitMQ 有效。队列名称。
    /// 如果是分布式部署，若节点队列名称相同，则只会有一个节点消费消息。
    /// </summary>
    /// <value>
    /// The name of the queue.
    /// </value>
    public string QueueName { get; set; } = "event_bus_queue";

    /// <summary>
    /// Gets or sets the name of the host.
    /// </summary>
    /// <value>
    /// The name of the host.
    /// </value>
    public string HostName { get; set; } = "localhost";

    /// <summary>
    /// Gets or sets the port.
    /// </summary>
    /// <value>
    /// The port.
    /// </value>
    public int Port { get; set; } = 5672;

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    /// <value>
    /// The name of the user.
    /// </value>
    public string UserName { get; set; } = "guest";

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <value>
    /// The password.
    /// </value>
    public string Password { get; set; } = "guest";

    /// <summary>
    /// Gets or sets the virtual host.
    /// </summary>
    /// <value>
    /// The virtual host.
    /// </value>
    public string VirtualHost { get; set; } = "";
}
