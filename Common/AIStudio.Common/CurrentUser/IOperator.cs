using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AIStudio.Common.CurrentUser
{
    /// <summary>
    /// 操作者
    /// </summary>
    public interface IOperator
    {
        IHttpContextAccessor HttpContextAccessor { get; }
        /// <summary>
        /// 当前操作者UserId
        /// </summary>
        string? UserId { get; }

        string? UserName { get; }

        bool IsSuperAdmin { get; }
        string? TenantId { get; }
        string? LoginUserId { get; set; }
        string? LoginUserName { get; set; }
        string? LoginTenantId { get; set; }
        #region 操作方法

        #endregion
    }
}
