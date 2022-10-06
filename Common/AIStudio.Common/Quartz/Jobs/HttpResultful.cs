using Coldairarrow.Business.Quartz_Manage;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AIStudio.Common.Quartz
{
    public class HttpResultful : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            DateTime dateTime = DateTime.Now;
            Quartz_TaskDTO taskOptions = context.GetTaskOptions();
            string httpMessage = "";
            AbstractTrigger trigger = (context as JobExecutionContextImpl).Trigger as AbstractTrigger;
            if (taskOptions == null)
            {               
                FileQuartz.WriteJobExecute(LogLevel.Warning, trigger.FullName , "未到找作业或可能被移除"); 
                return;
            }

            if (string.IsNullOrEmpty(taskOptions.ApiUrl) || taskOptions.ApiUrl == "/")
            {                
                FileQuartz.WriteJobExecute(LogLevel.Warning, trigger.FullName , "未配置url");
                return;
            }

            LogLevel logLevel = LogLevel.Information;
            try
            {
                Dictionary<string, string> header = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(taskOptions.AuthKey)
                    && !string.IsNullOrEmpty(taskOptions.AuthValue))
                {
                    header.Add(taskOptions.AuthKey.Trim(), taskOptions.AuthValue.Trim());
                }

                if (taskOptions.RequestType?.ToLower() == "get")
                {
                    httpMessage = await HttpManager.HttpGetAsync(taskOptions.ApiUrl, header);
                }
                else
                {
                    httpMessage = await HttpManager.HttpPostAsync(taskOptions.ApiUrl, null, null, 60, header);
                }
            }
            catch (Exception ex)
            {
                logLevel = LogLevel.Error;
                httpMessage = ex.Message;
            }

            FileQuartz.WriteJobExecute(logLevel, trigger.FullName, httpMessage);
            return;
        }
    }
}
