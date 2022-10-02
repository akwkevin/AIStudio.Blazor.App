using System.Security.Claims;

namespace AIStudio.Common.Authorization;

public class TestPermissionChecker : IPermissionChecker
{
    public virtual Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name)
    {
        var permissions = claimsPrincipal.Claims.Where(_ => _.Type == "Permission").Select(_ => _.Value).ToList();
        if (permissions.Any(_ => _.StartsWith(name)))
        {
            return Task.FromResult(true);
        }

        return Task.FromResult(false); 
    }
}
