using AIStudio.Common.Jwt;
using System.Security.Claims;

namespace AIStudio.Common.Authorization;

/// <summary>
/// 使用jwt信息鉴权
/// </summary>
public class TestPermissionChecker : IPermissionChecker
{
    /// <summary>
    /// 鉴权
    /// </summary>
    /// <param name="claimsPrincipal"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public virtual Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name)
    {
        var permissions = claimsPrincipal.Claims.Where(_ => _.Type == SimpleClaimTypes.Permission).Select(_ => _.Value).ToList();
        if (permissions.Any(p => p == name))
        {
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }
}
