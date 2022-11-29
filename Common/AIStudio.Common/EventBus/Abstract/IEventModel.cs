namespace AIStudio.Common.EventBus.Abstract;

/// <summary>
/// 事件模型接口
/// </summary>
public interface IEventModel
{
    /// <summary>
    /// Gets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    string Id { get; }

    /// <summary>
    /// Gets the UTC now.
    /// </summary>
    /// <value>
    /// The UTC now.
    /// </value>
    DateTimeOffset UtcNow { get; }
}
