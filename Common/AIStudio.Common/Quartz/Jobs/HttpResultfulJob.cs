using AIStudio.Common.EventBus.Abstract;
using AIStudio.Common.EventBus.Models;
using AIStudio.Common.Quartz.Extensions;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using System.ServiceModel.Channels;

namespace AIStudio.Common.Quartz
{
    public class HttpResultfulJob : IJob
    {
        private readonly ILogger<HttpResultfulJob> _logger;
        private readonly IEventPublisher _publisher;
        public HttpResultfulJob(ILogger<HttpResultfulJob> logger, IEventPublisher publisher)
        {
            _logger = logger;
            _publisher = publisher;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap jobdatamap = context.MergedJobDataMap;
            string httpMessage = "";
            var trigger = context.Trigger;

            var requestType = jobdatamap.Get("RequestType") as string;
            var apiurl = jobdatamap.Get("ApiUrl") as string;
            var authKey = jobdatamap.Get("AuthKey") as string;
            var authValue = jobdatamap.Get("AuthValue") as string;
            var tenantId = jobdatamap.Get("TenantId") as string;
            var creatorId = jobdatamap.Get("CreatorId") as string;
            var creatorName = jobdatamap.Get("CreatorName") as string;

            if (string.IsNullOrEmpty(apiurl) || apiurl == "/")
            {
                _logger.LogWarning($"{trigger.JobKey.Name}-{trigger.JobKey.Group}未配置url");
                return;
            }

            LogLevel logLevel = LogLevel.Information;
            try
            {
                Dictionary<string, string> header = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(authKey) && !string.IsNullOrEmpty(authValue))
                {
                    header.Add(authKey.Trim(), authValue.Trim());
                }

                if (requestType?.ToLower() == "get")
                {
                    httpMessage = await HttpManager.HttpGetAsync(apiurl, header);
                }
                else
                {
                    httpMessage = await HttpManager.HttpPostAsync(apiurl, null, null, 60, header);
                }
            }
            catch (Exception ex)
            {
                logLevel = LogLevel.Error;
                httpMessage = ex.Message;
            }

            var message = $"{trigger.JobKey.Name}-{trigger.JobKey.Group} Execute:{httpMessage}";
            _logger.LogInformation(message);

            var eventModel = new SystemEvent();
            eventModel.LogType = "系统任务执行";
            eventModel.Message = message;
            eventModel.TenantId = tenantId;
            eventModel.CreatorId = creatorId;
            eventModel.CreatorName = creatorName;

            await _publisher.PublishAsync(eventModel);
        }
    }
}
