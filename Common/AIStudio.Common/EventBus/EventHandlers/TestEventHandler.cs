using AIStudio.Common.EventBus.Abstract;

namespace AIStudio.Common.EventBus.EventHandlers
{
    public class TestEventModel : EventModel
    {
        public string Message { get; set; } = "测试消息";
    }

    public class TestEventHandler : IEventHandler<TestEventModel>
    {
        public Task Handle(TestEventModel @event)
        {
            Console.WriteLine($"收到信息：{@event.Message}");
            return Task.CompletedTask;
        }
    }
}
