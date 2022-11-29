using AIStudio.Common.Quartz.Models;
using Quartz;
using Quartz.Impl.Triggers;
using Quartz.Spi;

namespace AIStudio.Common.Quartz.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class CronHelper
    {
        /// <summary>
        /// Dates the time2 cron.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static string DateTime2Cron(this DateTime date)
        {
            return date.ToString("ss mm HH dd MM ? yyyy");
        }

        /// <summary>
        /// Cron2s the date time.
        /// </summary>
        /// <param name="cron">The cron.</param>
        /// <returns></returns>
        public static DateTime Cron2DateTime(this string cron)
        {
            return DateTime.ParseExact(cron, "ss mm HH dd MM ? yyyy", System.Globalization.CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Dates the time2 date time offset.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns></returns>
        public static DateTimeOffset? DateTime2DateTimeOffset(this DateTime? datetime)
        {
            if (datetime == null)
                return null;

            return DateTime.SpecifyKind(datetime.Value, DateTimeKind.Unspecified);
        }

        /// <summary>
        /// Starts the time2 date time offset.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns></returns>
        public static DateTimeOffset? StartTime2DateTimeOffset(this DateTime? datetime)
        {
            if (datetime == null)
                return null;

            return DateTime.SpecifyKind(datetime.Value < DateTime.Now ? DateTime.Now : datetime.Value, DateTimeKind.Unspecified);
        }

        /// <summary>
        /// Dates the time offset2 date time.
        /// </summary>
        /// <param name="datetimeoffset">The datetimeoffset.</param>
        /// <returns></returns>
        public static DateTime? DateTimeOffset2DateTime(this DateTimeOffset? datetimeoffset)
        {
            if (datetimeoffset == null)
                return null;

            return datetimeoffset.Value.DateTime;
        }

        /// <summary>
        /// Determines whether [is valid expression].
        /// </summary>
        /// <param name="cronExpression">The cron expression.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the next run time.
        /// </summary>
        /// <param name="cronExpression">The cron expression.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns></returns>
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
