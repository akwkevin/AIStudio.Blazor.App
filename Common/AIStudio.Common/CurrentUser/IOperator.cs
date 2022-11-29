using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AIStudio.Common.CurrentUser
{
    /// <summary>
    /// 操作者
    /// </summary>
    public interface IOperator
    {
        /// <summary>
        /// 当前操作者UserId
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        string? UserId { get; }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        string? UserName { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is super admin.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is super admin; otherwise, <c>false</c>.
        /// </value>
        bool IsSuperAdmin { get; }
        /// <summary>
        /// Gets the tenant identifier.
        /// </summary>
        /// <value>
        /// The tenant identifier.
        /// </value>
        string? TenantId { get; }
        /// <summary>
        /// Gets or sets the login user identifier.
        /// </summary>
        /// <value>
        /// The login user identifier.
        /// </value>
        string? LoginUserId { get; set; }
        /// <summary>
        /// Gets or sets the name of the login user.
        /// </summary>
        /// <value>
        /// The name of the login user.
        /// </value>
        string? LoginUserName { get; set; }
        /// <summary>
        /// Gets or sets the login tenant identifier.
        /// </summary>
        /// <value>
        /// The login tenant identifier.
        /// </value>
        string? LoginTenantId { get; set; }
        #region 操作方法

        #endregion
    }
}
