using AIStudio.Common.Quartz.Models;
using Quartz;
using Quartz.Impl.Triggers;
using Quartz.Spi;

namespace AIStudio.Common.Quartz.Extensions
{
    public static class CronHelper
    {
        public static string DateTime2Cron(this DateTime date)
        {
            return date.ToString("ss mm HH dd MM ? yyyy");
        }

        public static DateTime Cron2DateTime(this string cron)
        {
            return DateTime.ParseExact(cron, "ss mm HH dd MM ? yyyy", System.Globalization.CultureInfo.CurrentCulture);
        }

        public static DateTimeOffset? DateTime2DateTimeOffset(this DateTime? datetime)
        {
            if (datetime == null)
                return null;

            return DateTime.SpecifyKind(datetime.Value, DateTimeKind.Unspecified);
        }

        public static DateTimeOffset? StartTime2DateTimeOffset(this DateTime? datetime)
        {
            if (datetime == null)
                return null;

            return DateTime.SpecifyKind(datetime.Value < DateTime.Now ? DateTime.Now : datetime.Value, DateTimeKind.Unspecified);
        }

        public static DateTime? DateTimeOffset2DateTime(this DateTimeOffset? datetimeoffset)
        {
            if (datetimeoffset == null)
                return null;

            return datetimeoffset.Value.DateTime;
        }

        public static JobExcuteResult IsValidExpression(this string cronExpression)
        {
            try
            {
                CronTriggerImpl trigger = new CronTriggerImpl();
                trigger.CronExpressionString = cronExpression;
                DateTimeOffset? date = trigger.ComputeFirstFireTimeUtc(null);
                return new JobExcuteResult(date != null, date == null ? $"请确认表达式{cronExpression}是否正确!" : "");
            }
            catch (Exception e)
            {
                return new JobExcuteResult(false, $"请确认表达式{cronExpression}是否正确!{e.Message}");
            }
        }

        public static JobExcuteResult GetNextRunTime(this string cronExpression, DateTime? startTime, DateTime? endTime)
        {
            try
            {
                ITrigger trigger = TriggerBuilder.Create()
                .StartAt(CronHelper.StartTime2DateTimeOffset(startTime) ?? SystemTime.UtcNow())
                .EndAt(CronHelper.DateTime2DateTimeOffset(endTime))
                .WithCronSchedule(cronExpression)
                .Build();

                IList<DateTimeOffset> dates = TriggerUtils.ComputeFireTimes(trigger as IOperableTrigger, null, 5).ToList();
                return new JobExcuteResult(true, String.Join(",", dates.Select(p => TimeZoneInfo.ConvertTimeFromUtc(p.DateTime, TimeZoneInfo.Local))));
            }
            catch (Exception ex)
            {
                return new JobExcuteResult(false, $"请确认表达式{cronExpression}是否正确！{ex.Message}");
            }
        }
    }
}
