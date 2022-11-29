using AIStudio.Common.EventBus.Abstract;
using AIStudio.Common.Quartz;
using Microsoft.Extensions.Logging;

namespace AIStudio.Common.EventBus.EventHandlers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AIStudio.Common.EventBus.Abstract.EventModel" />
    public class TestEvent : EventModel
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; } = "测试消息";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AIStudio.Common.EventBus.Abstract.IEventHandler&lt;AIStudio.Common.EventBus.EventHandlers.TestEvent&gt;" />
    public class TestEventHandler : IEventHandler<TestEvent>
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<TestEventHandler> _logger;
        /// <summary>
        /// Initializes a new instance of the <see cref="TestEventHandler"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public TestEventHandler(ILogger<TestEventHandler> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 处理程序
        /// </summary>
        /// <param name="event">事件模型</param>
        /// <returns></returns>
        public Task Handle(TestEvent @event)
        {
            _logger.LogInformation(@event.Message);
            return Task.CompletedTask;
        }
    }
}
