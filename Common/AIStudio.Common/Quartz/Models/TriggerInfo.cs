namespace AIStudio.Common.Quartz.Models;

public class TriggerInfo
{
    public virtual string GroupName { get; private set; }
    public virtual string Name { get; private set; }

    public virtual string Cron { get; private set; }
    public virtual string Describe { get; private set; }
    public virtual DateTime? StartTime { get; set; }
    public virtual DateTime? EndTime { get; set; }
    public virtual DateTime? LastRunTime { get; set; }

    public TriggerInfo(string name, string groupName, string cron, string describe = null)
    {
        Name = name;
        GroupName = groupName;
        Cron = cron;
        Describe = describe;
    }
}
