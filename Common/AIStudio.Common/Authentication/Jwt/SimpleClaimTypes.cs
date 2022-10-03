using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.Jwt
{
    public static class SimpleClaimTypes
    {
        public const string Role = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        public const string Sid = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid";

        public const string Version = "http://schemas.microsoft.com/ws/2008/06/identity/claims/version";

        public const string Name = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

        public const string Email = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";

        public const string Expiration = "http://schemas.microsoft.com/ws/2008/06/identity/claims/expiration";

        public const string Expired = "http://schemas.microsoft.com/ws/2008/06/identity/claims/expired";

        public const string UserName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/username";

        public const string UserId = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uid";

        public const string Actor = "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor";

        /// <summary>
        /// 是否超级管理
        /// </summary>
        public const string SuperAdmin = "SuperAdmin";

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



     
    }
}
