using AIStudio.Common.EventBus.Abstract;
using Microsoft.Extensions.Options;
using System.Threading.Channels;

namespace AIStudio.Common.EventBus.Local;

/// <summary>
/// 基于 Channel 的队列存储器
/// </summary>
/// <seealso cref="AIStudio.Common.EventBus.Local.IEventStore" />
public class ChannelEventStore : IEventStore
{
    /// <summary>
    /// The queue
    /// </summary>
    private readonly Channel<object> _queue;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChannelEventStore"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public ChannelEventStore(IOptions<LocalEventBusOptions> options)
    {
        // 通道容量
        int capacity = options.Value.Capacity;

        // 配置通道，设置超出默认容量后进入等待
        BoundedChannelOptions channelOptions = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };

        // 创建具有最大容量的通道
        _queue = Channel.CreateBounded<object>(channelOptions);
    }

    /// <summary>
    /// Writes the asynchronous.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="event">The event.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">event</exception>
    public async ValueTask WriteAsync<TEvent>(TEvent @event)
        where TEvent : class, IEventModel
    {
        if (@event is null)
        {
            throw new ArgumentNullException(nameof(@event));
        }

        await _queue.Writer.WriteAsync(@event);
    }

    /// <summary>
    /// Reads the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public async ValueTask<object> ReadAsync(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}
