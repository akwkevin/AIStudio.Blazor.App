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
        string UserId { get; }

        string UserName { get; }

        bool IsSuperAdmin { get; }
        string TenantId { get; }

        string LoginName { get; set; }
        #region 操作方法



        /// <summary>
        /// 记录操作日志
        /// </summary>
        /// <param name="userLogType">用户日志类型</param>
        /// <param name="msg">内容</param>
        //void WriteUserLog(UserLogType userLogType, string msg);

        #endregion
    }
}
