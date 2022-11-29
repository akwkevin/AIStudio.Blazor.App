namespace AIStudio.Common.Quartz.Models;

/// <summary>
/// 
/// </summary>
public class TriggerInfo
{
    /// <summary>
    /// Gets the name of the group.
    /// </summary>
    /// <value>
    /// The name of the group.
    /// </value>
    public virtual string GroupName { get; private set; }
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public virtual string Name { get; private set; }

    /// <summary>
    /// Gets the cron.
    /// </summary>
    /// <value>
    /// The cron.
    /// </value>
    public virtual string Cron { get; private set; }
    /// <summary>
    /// Gets the describe.
    /// </summary>
    /// <value>
    /// The describe.
    /// </value>
    public virtual string Describe { get; private set; }
    /// <summary>
    /// Gets or sets the start time.
    /// </summary>
    /// <value>
    /// The start time.
    /// </value>
    public virtual DateTime? StartTime { get; set; }
    /// <summary>
    /// Gets or sets the end time.
    /// </summary>
    /// <value>
    /// The end time.
    /// </value>
    public virtual DateTime? EndTime { get; set; }
    /// <summary>
    /// Gets or sets the last run time.
    /// </summary>
    /// <value>
    /// The last run time.
    /// </value>
    public virtual DateTime? LastRunTime { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TriggerInfo" /> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="groupName">Name of the group.</param>
    /// <param name="cron">The cron.</param>
    /// <param name="describe">The describe.</param>
    public TriggerInfo(string name, string groupName, string cron, string describe = null)
    {
        Name = name;
        GroupName = groupName;
        Cron = cron;
        Describe = describe;
    }
}
