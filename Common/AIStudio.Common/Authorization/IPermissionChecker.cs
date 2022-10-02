using System.Security.Claims;

namespace AIStudio.Common.Authorization;

public interface IPermissionChecker
{
    Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name);
}
