using AIStudio.Common.DI;
using AIStudio.Common.Jwt;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AIStudio.Common.CurrentUser
{
    /// <summary>
    /// 操作者
    /// </summary>
    public class Operator : IOperator, IScopedDependency
    {
        readonly IServiceProvider _serviceProvider;
        public Operator(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            HttpContextAccessor = httpContextAccessor;
        }

        public IHttpContextAccessor HttpContextAccessor { get; }
        /// <summary>
        /// 当前操作者UserId
        /// </summary>
        public virtual string? UserId => FindClaimValue(SimpleClaimTypes.UserId);

        public virtual string? UserName => FindClaimValue(SimpleClaimTypes.Name);

        public virtual bool IsSuperAdmin => FindClaimValue(SimpleClaimTypes.SuperAdmin) == SimpleClaimTypes.SuperAdmin;
        public virtual string? TenantId => FindClaimValue(SimpleClaimTypes.TenantId);

        public string? LoginUserId { get; set;}
        public string? LoginUserName { get; set; }
        public string? LoginTenantId { get; set; }

        public virtual Claim? FindClaim(string claimType)
        {
            return HttpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == claimType);
        }

        public virtual Claim[] FindClaims(string claimType)
        {
            return HttpContextAccessor?.HttpContext?.User?.Claims.Where(c => c.Type == claimType).ToArray();
        }

        public virtual string? FindClaimValue(string claimType)
        {
            return FindClaim(claimType)?.Value;
        }
    }
}
