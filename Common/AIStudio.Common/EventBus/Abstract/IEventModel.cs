namespace AIStudio.Common.EventBus.Abstract;

/// <summary>
/// 事件模型接口
/// </summary>
public interface IEventModel
{
    string Id { get; }

    DateTimeOffset UtcNow { get; }
}
