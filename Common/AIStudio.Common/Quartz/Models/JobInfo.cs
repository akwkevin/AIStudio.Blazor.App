namespace AIStudio.Common.Quartz.Models;

public class JobInfo
{
    public virtual string GroupName { get; private set; }
    public virtual string Name { get; private set; }
    public virtual IList<TriggerInfo> Triggers { get; private set; } = new List<TriggerInfo>();//目前仅有一个

    public JobInfo(string name, string groupName)
    {
        Name = name;
        GroupName = groupName;
    }
}
