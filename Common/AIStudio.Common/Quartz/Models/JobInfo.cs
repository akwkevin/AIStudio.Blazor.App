using Quartz;

namespace AIStudio.Common.Quartz.Models;

public class JobInfo
{
    /// <summary>
    /// 分组
    /// </summary>
    public virtual string GroupName { get; private set; }
    /// <summary>
    /// 名称
    /// </summary>
    public virtual string Name { get; private set; }
    /// <summary>
    /// 触发器
    /// </summary>
    public virtual IList<TriggerInfo> Triggers { get; private set; } = new List<TriggerInfo>();//目前仅有一个
    /// <summary>
    /// 任务执行入参数据
    /// </summary>
    public virtual JobDataMap JobDataMap { get; private set; }

    public JobInfo(string name, string groupName, JobDataMap jobDataMap)
    {
        Name = name;
        GroupName = groupName;
        JobDataMap = jobDataMap ?? new JobDataMap();
    }
}
