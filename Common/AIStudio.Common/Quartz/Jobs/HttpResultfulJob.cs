using AIStudio.Common.Quartz.Extensions;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System.ComponentModel.DataAnnotations;

namespace AIStudio.Common.Quartz
{
    public class HttpResultfulJob : IJob
    {
        private readonly ILogger<HttpResultfulJob> _logger;
        public HttpResultfulJob(ILogger<HttpResultfulJob> logger)
        {
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap jobdatamap = context.MergedJobDataMap;
            string httpMessage = "";
            AbstractTrigger trigger = (context as JobExecutionContextImpl).Trigger as AbstractTrigger;

            var requestType = jobdatamap.Get("RequestType") as string;
            var apiurl = jobdatamap.Get("ApiUrl") as string;
            var authKey = jobdatamap.Get("AuthKey") as string;
            var authValue = jobdatamap.Get("AuthValue") as string;
            if (string.IsNullOrEmpty(apiurl) || apiurl == "/")
            {
                _logger.LogWarning($"{trigger.FullName}未配置url");
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

            _logger.LogInformation($"{trigger.FullName} Execute:{httpMessage}");
        }
    }
}
