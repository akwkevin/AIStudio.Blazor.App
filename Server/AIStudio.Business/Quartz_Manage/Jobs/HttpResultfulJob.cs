using AIStudio.Common.EventBus.Abstract;
using AIStudio.Common.EventBus.Models;
using AIStudio.Common.Quartz.Extensions;
using AIStudio.Entity.Enum;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using System.ServiceModel.Channels;

namespace AIStudio.Business.Quartz_Manage.Jobs
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Quartz.IJob" />
    public class HttpResultfulJob : IJob
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<HttpResultfulJob> _logger;
        /// <summary>
        /// The publisher
        /// </summary>
        private readonly IEventPublisher _publisher;
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResultfulJob"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="publisher">The publisher.</param>
        public HttpResultfulJob(ILogger<HttpResultfulJob> logger, IEventPublisher publisher)
        {
            _logger = logger;
            _publisher = publisher;
        }

        /// <summary>
        /// Called by the <see cref="T:Quartz.IScheduler" /> when a <see cref="T:Quartz.ITrigger" />
        /// fires that is associated with the <see cref="T:Quartz.IJob" />.
        /// </summary>
        /// <param name="context">The execution context.</param>
        /// <remarks>
        /// The implementation may wish to set a  result object on the
        /// JobExecutionContext before this method exits.  The result itself
        /// is meaningless to Quartz, but may be informative to
        /// <see cref="T:Quartz.IJobListener" />s or
        /// <see cref="T:Quartz.ITriggerListener" />s that are watching the job's
        /// execution.
        /// </remarks>
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

            var message = $"{trigger.JobKey.Group}.{trigger.JobKey.Name} Execute:{httpMessage}";
            _logger.LogInformation(message);

            var eventModel = new SystemEvent();
            eventModel.LogType = UserLogType.系统任务.ToString();
            eventModel.Name = trigger.JobKey.Group + "." + trigger.JobKey.Name;
            eventModel.Message = message;
            eventModel.TenantId = tenantId;
            eventModel.CreatorId = creatorId;
            eventModel.CreatorName = creatorName;

            await _publisher.PublishAsync(eventModel);
        }
    }
}
