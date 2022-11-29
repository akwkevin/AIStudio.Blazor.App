using AIStudio.Common.EventBus.Abstract;
using AIStudio.Util;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace AIStudio.Common.EventBus.RabbitMq;

/// <summary>
/// 
/// </summary>
/// <seealso cref="AIStudio.Common.EventBus.RabbitMq.IRabbitMqManager" />
/// <seealso cref="System.IDisposable" />
public class RabbitMqManager : IRabbitMqManager, IDisposable
{
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<RabbitMqManager> _logger;
    /// <summary>
    /// The connection factory
    /// </summary>
    private readonly IConnectionFactory _connectionFactory;
    /// <summary>
    /// The subscribe executer
    /// </summary>
    private readonly ISubscribeManager _subscribeExecuter;
    /// <summary>
    /// The options
    /// </summary>
    private readonly RabbitMqEventBusOptions _options;

    /// <summary>
    /// The connection
    /// </summary>
    private IConnection? _connection;
    /// <summary>
    /// The consumer channel
    /// </summary>
    private IModel? _consumerChannel;
    /// <summary>
    /// The disposed
    /// </summary>
    private bool _disposed;
    /// <summary>
    /// The started
    /// </summary>
    private bool _started;
    /// <summary>
    /// The connection lock
    /// </summary>
    private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);
    /// <summary>
    /// The start lock
    /// </summary>
    private readonly SemaphoreSlim _startLock = new SemaphoreSlim(1, 1);

    /// <summary>
    /// RabbitMQ 连接实例
    /// </summary>
    /// <exception cref="System.NullReferenceException">Connection</exception>
    public IConnection Connection => _connection ?? throw new NullReferenceException(nameof(Connection));

    /// <summary>
    /// 消费者 Channel 实例
    /// </summary>
    /// <value>
    /// The consumer channel.
    /// </value>
    /// <exception cref="System.NullReferenceException">ConsumerChannel</exception>
    public IModel ConsumerChannel => _consumerChannel ?? throw new NullReferenceException(nameof(ConsumerChannel));

    /// <summary>
    /// 启用的交换机名称
    /// </summary>
    public string ExChangeName => _options.ExChangeName;

    /// <summary>
    /// 启用的交换机类型
    /// </summary>
    public string ExChangeType => _options.ExChangeType;

    /// <summary>
    /// 启用的队列名称
    /// </summary>
    public string QueueName => _options.QueueName;

    /// <summary>
    /// Initializes a new instance of the <see cref="RabbitMqManager"/> class.
    /// </summary>
    /// <param name="connectionFactory">The connection factory.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="subscribeExecuter">The subscribe executer.</param>
    /// <param name="options">The options.</param>
    public RabbitMqManager(IConnectionFactory connectionFactory,
                           ILogger<RabbitMqManager> logger,
                           ISubscribeManager subscribeExecuter,
                           IOptions<RabbitMqEventBusOptions> options)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        _subscribeExecuter = subscribeExecuter;
        _options = options.Value;
    }

    /// <summary>
    /// Connects this instance.
    /// </summary>
    private void Connect()
    {
        CheckDisposed();
        if (_connection != null)
        {
            return;
        }

        _connectionLock.Wait();
        try
        {
            if (_connection == null)
            {
                // 创建连接
                _connection = _connectionFactory.CreateConnection();

                // 创建 消费者 channel
                _consumerChannel = _connection.CreateModel();

                // 声明交换机和队列
                _consumerChannel.ExchangeDeclare(exchange: ExChangeName, type: ExChangeType);
                _consumerChannel.QueueDeclare(queue: QueueName, true, false, false, null);
            }
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    /// <summary>
    /// 启动订阅。
    /// </summary>
    public virtual void StartSubscribe()
    {
        Connect();

        if (_started)
        {
            return;
        }

        _startLock.Wait();

        try
        {
            if (!_started)
            {
                foreach (var subscriber in _subscribeExecuter.Subscribers)
                {
                    var eventType = subscriber.Key;
                    var eventName = subscriber.Key.Name;

                    // 绑定交换机，队列，路由
                    ConsumerChannel.QueueBind(queue: QueueName, exchange: ExChangeName, routingKey: eventName);

                    // 消费者
                    EventingBasicConsumer consumer = new EventingBasicConsumer(ConsumerChannel);

                    // 添加接收事件
                    consumer.Received += CreateDelegateConsumerReceived(eventType);

                    // 启动消费者
                    ConsumerChannel.BasicConsume(queue: QueueName, autoAck: false, consumer);
                }

                _started = true;
            }
        }
        finally
        {
            _startLock.Release();
        }
    }

    /// <summary>
    /// 发布消息。
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="event"></param>
    /// <returns></returns>
    public virtual Task PublishAsync<TEvent>(TEvent @event)
        where TEvent : class, IEventModel
    {
        Connect();

        var eventName = @event.GetType().Name;

        using var channel = Connection.CreateModel();

        var message = @event.ToJson();
        var body = Encoding.UTF8.GetBytes(message);

        var properties = channel.CreateBasicProperties();
        properties.DeliveryMode = 2; // persistent

        channel.BasicPublish(
            exchange: ExChangeName,
            routingKey: eventName,
            mandatory: true,
            basicProperties: properties,
            body: body);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Creates the delegate consumer received.
    /// </summary>
    /// <param name="eventType">Type of the event.</param>
    /// <returns></returns>
    protected virtual EventHandler<BasicDeliverEventArgs> CreateDelegateConsumerReceived(Type eventType)
    {
        Func<object, Task> processEvent = _subscribeExecuter.ProcessEvent;

        EventHandler<BasicDeliverEventArgs> eventHandler = async (sender, e) =>
        {
            // 获取消息
            //var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.Span);
            var @event = JsonConvert.DeserializeObject(message, eventType);

            // 处理消息
            if (@event != null) await processEvent.Invoke(@event);

            // 标记消费
            ConsumerChannel.BasicAck(e.DeliveryTag, multiple: false);
        };

        return eventHandler;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public virtual void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        _disposed = true;
        _consumerChannel?.Dispose();
        _connection?.Close();
    }

    /// <summary>
    /// Checks the disposed.
    /// </summary>
    /// <exception cref="System.ObjectDisposedException"></exception>
    private void CheckDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }
}
