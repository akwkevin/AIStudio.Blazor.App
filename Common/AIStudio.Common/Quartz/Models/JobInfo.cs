using Quartz;

namespace AIStudio.Common.Quartz.Models;

/// <summary>
/// 
/// </summary>
public class JobInfo
{
    /// <summary>
    /// 分组
    /// </summary>
    /// <value>
    /// The name of the group.
    /// </value>
    public virtual string GroupName { get; private set; }
    /// <summary>
    /// 名称
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public virtual string Name { get; private set; }
    /// <summary>
    /// 触发器
    /// </summary>
    /// <value>
    /// The triggers.
    /// </value>
    public virtual IList<TriggerInfo> Triggers { get; private set; } = new List<TriggerInfo>();//目前仅有一个
    /// <summary>
    /// 任务执行入参数据
    /// </summary>
    /// <value>
    /// The job data map.
    /// </value>
    public virtual JobDataMap JobDataMap { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="JobInfo"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="groupName">Name of the group.</param>
    /// <param name="jobDataMap">The job data map.</param>
    public JobInfo(string name, string groupName, JobDataMap jobDataMap)
    {
        Name = name;
        GroupName = groupName;
        JobDataMap = jobDataMap ?? new JobDataMap();
    }
}
