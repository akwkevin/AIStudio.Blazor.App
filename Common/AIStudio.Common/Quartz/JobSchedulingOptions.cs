namespace AIStudio.Common.Quartz;

/// <summary>
/// 
/// </summary>
public class JobSchedulingOptions
{
    /// <summary>
    /// 定时任务启动时执行的程序
    /// </summary>
    /// <value>
    /// The start handle.
    /// </value>
    public Func<IServiceProvider, Task>? StartHandle { get; set; }

    /// <summary>
    /// 定时任务关闭时执行的程序
    /// </summary>
    /// <value>
    /// The shutdown handle.
    /// </value>
    public Func<IServiceProvider, Task>? ShutdownHandle { get; set; }
}
