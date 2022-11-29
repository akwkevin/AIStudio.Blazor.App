using AIStudio.Common.EventBus.Abstract;
using AIStudio.Util;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace AIStudio.Common.EventBus.Redis;

/// <summary>
/// 
/// </summary>
/// <seealso cref="AIStudio.Common.EventBus.Redis.IRedisManager" />
/// <seealso cref="System.IDisposable" />
public class RedisManager : IRedisManager, IDisposable
{
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<RedisManager> _logger;
    /// <summary>
    /// The subscribe executer
    /// </summary>
    private readonly ISubscribeManager _subscribeExecuter;
    /// <summary>
    /// The options
    /// </summary>
    private readonly RedisEventBusOptions _options;

    /// <summary>
    /// The connection
    /// </summary>
    private volatile IConnectionMultiplexer? _connection;
    /// <summary>
    /// The subscriber
    /// </summary>
    private ISubscriber? _subscriber;
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
    /// Gets the connection.
    /// </summary>
    /// <value>
    /// The connection.
    /// </value>
    /// <exception cref="System.NullReferenceException">Connection</exception>
    public IConnectionMultiplexer Connection => _connection ?? throw new NullReferenceException(nameof(Connection));

    /// <summary>
    /// Gets the subscriber.
    /// </summary>
    /// <value>
    /// The subscriber.
    /// </value>
    /// <exception cref="System.NullReferenceException">Subscriber</exception>
    public ISubscriber Subscriber => _subscriber ?? throw new NullReferenceException(nameof(Subscriber));

    /// <summary>
    /// Initializes a new instance of the <see cref="RedisManager"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="subscribeExecuter">The subscribe executer.</param>
    /// <param name="options">The options.</param>
    public RedisManager(ILogger<RedisManager> logger,
                        ISubscribeManager subscribeExecuter,
                        IOptions<RedisEventBusOptions> options)
    {
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
        if (_subscriber != null)
        {
            return;
        }

        _connectionLock.Wait();
        try
        {
            if (_subscriber == null)
            {
                if (_options.ConnectionMultiplexerFactory == null)
                {
                    if (_options.ConfigurationOptions is not null)
                    {
                        _connection = ConnectionMultiplexer.Connect(_options.ConfigurationOptions);
                    }
                    else
                    {
                        _connection = ConnectionMultiplexer.Connect(_options.Configuration);
                    }
                }
                else
                {
                    _connection = _options.ConnectionMultiplexerFactory().GetAwaiter().GetResult();
                }

                _subscriber = _connection.GetSubscriber();
            }
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    /// <summary>
    /// Connects the asynchronous.
    /// </summary>
    /// <param name="token">The token.</param>
    private async Task ConnectAsync(CancellationToken token = default)
    {
        CheckDisposed();
        token.ThrowIfCancellationRequested();

        if (_subscriber != null)
        {
            return;
        }

        await _connectionLock.WaitAsync(token).ConfigureAwait(false);
        try
        {
            if (_subscriber == null)
            {
                if (_options.ConnectionMultiplexerFactory is null)
                {
                    if (_options.ConfigurationOptions is not null)
                    {
                        _connection = await ConnectionMultiplexer.ConnectAsync(_options.ConfigurationOptions).ConfigureAwait(false);
                    }
                    else
                    {
                        _connection = await ConnectionMultiplexer.ConnectAsync(_options.Configuration).ConfigureAwait(false);
                    }
                }
                else
                {
                    _connection = await _options.ConnectionMultiplexerFactory();
                }

                _subscriber = _connection.GetSubscriber();
            }
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    /// <summary>
    /// Starts the subscribe.
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

                    // 创建 Action<RedisChannel, RedisValue> 委托
                    var handler = CreateDelegateConsumerReceived(eventType);

                    // 订阅
                    Subscriber.Subscribe(eventName, handler);
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
    /// Publishes the asynchronous.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="event">The event.</param>
    /// <param name="token">The token.</param>
    public virtual async Task PublishAsync<TEvent>(TEvent @event, CancellationToken token = default)
        where TEvent : class, IEventModel
    {
        await ConnectAsync(token);

        var eventName = @event.GetType().Name;
        var message = @event.ToJson();

        await Subscriber.PublishAsync(eventName, message);
    }

    /// <summary>
    /// Creates the delegate consumer received.
    /// </summary>
    /// <param name="eventType">Type of the event.</param>
    /// <returns></returns>
    protected virtual Action<RedisChannel, RedisValue> CreateDelegateConsumerReceived(Type eventType)
    {
        Func<object, Task> processEvent = _subscribeExecuter.ProcessEvent;

        Action<RedisChannel, RedisValue> handler = async (channel, message) =>
        {
            // 获取消息
            var @event = JsonConvert.DeserializeObject(message.ToString(), eventType);

            // 处理消息
            if (@event != null) await processEvent.Invoke(@event).ConfigureAwait(false);
        };

        return handler;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        _disposed = true;
        _connection?.Dispose();
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
