using System.Security.Claims;

namespace AIStudio.Common.Authorization;

/// <summary>
/// 
/// </summary>
/// <seealso cref="AIStudio.Common.Authorization.IPermissionChecker" />
public class DefaultPermissionChecker : IPermissionChecker
{
    /// <summary>
    /// Determines whether [is granted asynchronous] [the specified claims principal].
    /// </summary>
    /// <param name="claimsPrincipal">The claims principal.</param>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public virtual Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name)
    {
        return Task.FromResult(true);
    }
}
