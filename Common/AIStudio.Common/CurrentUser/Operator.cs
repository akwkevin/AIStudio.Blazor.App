using AIStudio.Common.DI;
using AIStudio.Common.Jwt;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AIStudio.Common.CurrentUser
{
    /// <summary>
    /// 操作者
    /// </summary>
    /// <seealso cref="AIStudio.Common.CurrentUser.IOperator" />
    /// <seealso cref="AIStudio.Common.DI.IScopedDependency" />
    public class Operator : IOperator, IScopedDependency
    {
        /// <summary>
        /// The service provider
        /// </summary>
        readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// Initializes a new instance of the <see cref="Operator"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public Operator(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets the HTTP context accessor.
        /// </summary>
        /// <value>
        /// The HTTP context accessor.
        /// </value>
        private IHttpContextAccessor _httpContextAccessor { get; }
        /// <summary>
        /// 当前操作者UserId
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public virtual string? UserId => FindClaimValue(SimpleClaimTypes.UserId);

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public virtual string? UserName => FindClaimValue(SimpleClaimTypes.Name);

        /// <summary>
        /// Gets a value indicating whether this instance is super admin.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is super admin; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsSuperAdmin => FindClaimValue(SimpleClaimTypes.SuperAdmin) == SimpleClaimTypes.SuperAdmin;
        /// <summary>
        /// Gets the tenant identifier.
        /// </summary>
        /// <value>
        /// The tenant identifier.
        /// </value>
        public virtual string? TenantId => FindClaimValue(SimpleClaimTypes.TenantId);

        /// <summary>
        /// Gets or sets the login user identifier.
        /// </summary>
        /// <value>
        /// The login user identifier.
        /// </value>
        public string? LoginUserId { get; set;}
        /// <summary>
        /// Gets or sets the name of the login user.
        /// </summary>
        /// <value>
        /// The name of the login user.
        /// </value>
        public string? LoginUserName { get; set; }
        /// <summary>
        /// Gets or sets the login tenant identifier.
        /// </summary>
        /// <value>
        /// The login tenant identifier.
        /// </value>
        public string? LoginTenantId { get; set; }

        /// <summary>
        /// Finds the claim.
        /// </summary>
        /// <param name="claimType">Type of the claim.</param>
        /// <returns></returns>
        public virtual Claim? FindClaim(string claimType)
        {
            var value = _httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == claimType);
            return value;
        }

        /// <summary>
        /// Finds the claims.
        /// </summary>
        /// <param name="claimType">Type of the claim.</param>
        /// <returns></returns>
        public virtual Claim[] FindClaims(string claimType)
        {
            return _httpContextAccessor?.HttpContext?.User?.Claims.Where(c => c.Type == claimType).ToArray();
        }

        /// <summary>
        /// Finds the claim value.
        /// </summary>
        /// <param name="claimType">Type of the claim.</param>
        /// <returns></returns>
        public virtual string? FindClaimValue(string claimType)
        {
            return FindClaim(claimType)?.Value;
        }
    }
}
