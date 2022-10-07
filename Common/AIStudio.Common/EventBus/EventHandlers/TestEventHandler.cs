using AIStudio.Common.EventBus.Abstract;
using AIStudio.Common.Quartz;
using Microsoft.Extensions.Logging;

namespace AIStudio.Common.EventBus.EventHandlers
{
    public class TestEventModel : EventModel
    {
        public string Message { get; set; } = "测试消息";
    }

    public class TestEventHandler : IEventHandler<TestEventModel>
    {
        public TestEventHandler(ILogger<TestEventHandler> logger)
        {

        }
        public Task Handle(TestEventModel @event)
        {
            Console.WriteLine($"收到信息：{@event.Message}");
            return Task.CompletedTask;
        }
    }
}
