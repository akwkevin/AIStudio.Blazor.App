using System.Security.Claims;

namespace AIStudio.Common.Authorization;

/// <summary>
/// 
/// </summary>
public interface IPermissionChecker
{
    /// <summary>
    /// Determines whether [is granted asynchronous] [the specified claims principal].
    /// </summary>
    /// <param name="claimsPrincipal">The claims principal.</param>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name);
}
