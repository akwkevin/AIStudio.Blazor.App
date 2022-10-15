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
        private readonly ILogger<TestEventHandler> _logger;
        public TestEventHandler(ILogger<TestEventHandler> logger)
        {
            _logger = logger;
        }
        public Task Handle(TestEventModel @event)
        {
            _logger.LogInformation(@event.Message);
            return Task.CompletedTask;
        }
    }
}
