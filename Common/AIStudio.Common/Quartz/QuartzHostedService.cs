using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace AIStudio.Common.Quartz;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Microsoft.Extensions.Hosting.IHostedService" />
public class QuartzHostedService : IHostedService
{
    /// <summary>
    /// The quartz manager
    /// </summary>
    private readonly IQuartzManager _quartzManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuartzHostedService"/> class.
    /// </summary>
    /// <param name="quartzManager">The quartz manager.</param>
    public QuartzHostedService(IQuartzManager quartzManager)
    {
        _quartzManager = quartzManager;
    }

    /// <summary>
    /// Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _quartzManager.Start();
    }

    /// <summary>
    /// Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _quartzManager.Shutdown();
    }
}
