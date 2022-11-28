using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.Jwt
{
    /// <summary>
    /// 
    /// </summary>
    public static class SimpleClaimTypes
    {
        /// <summary>
        /// The role
        /// </summary>
        public const string Role = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        /// <summary>
        /// The sid
        /// </summary>
        public const string Sid = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid";

        /// <summary>
        /// The version
        /// </summary>
        public const string Version = "http://schemas.microsoft.com/ws/2008/06/identity/claims/version";

        /// <summary>
        /// The name
        /// </summary>
        public const string Name = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

        /// <summary>
        /// The email
        /// </summary>
        public const string Email = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";

        /// <summary>
        /// The expiration
        /// </summary>
        public const string Expiration = "http://schemas.microsoft.com/ws/2008/06/identity/claims/expiration";

        /// <summary>
        /// The expired
        /// </summary>
        public const string Expired = "http://schemas.microsoft.com/ws/2008/06/identity/claims/expired";

        //public const string UserName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/username";

        /// <summary>
        /// The user identifier
        /// </summary>
        public const string UserId = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uid";

        /// <summary>
        /// The actor
        /// </summary>
        public const string Actor = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor";

        /// <summary>
        /// 是否超级管理
        /// </summary>
        public const string SuperAdmin = "SuperAdmin";

        /// <summary>
        /// 是否超级管理
        /// </summary>
        public const string Admin = "Admin";

        /// <summary>
        /// 租户Id
        /// </summary>
        public const string TenantId = "TenantId";

        /// <summary>
        /// 租户类型
        /// </summary>
        public const string TenantType = "TenantType";
        /// <summary>
        /// 租户名称
        /// </summary>
        public const string TenantName = "TenantName";

        /// <summary>
        /// The permission
        /// </summary>
        public const string Permission = "Permission";
    }
}
