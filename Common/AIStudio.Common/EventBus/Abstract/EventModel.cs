using AIStudio.Common.IdGenerator;
using AIStudio.Util.Helper;

namespace AIStudio.Common.EventBus.Abstract;

/// <summary>
/// 基础事件模型
/// </summary>
/// <seealso cref="AIStudio.Common.EventBus.Abstract.IEventModel" />
public abstract class EventModel : IEventModel
{
    /// <summary>
    /// 事件模型唯一Id。
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public virtual string Id { get; set; } = IdHelper.GetId();

    /// <summary>
    /// 创建事件对象时的 Utc 时间。
    /// </summary>
    /// <value>
    /// The UTC now.
    /// </value>
    public virtual DateTimeOffset UtcNow { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventModel"/> class.
    /// </summary>
    public EventModel()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventModel"/> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="utcNow">The UTC now.</param>
    public EventModel(string id, DateTimeOffset utcNow)
    {
        Id = id;
        UtcNow = utcNow;
    }
}
