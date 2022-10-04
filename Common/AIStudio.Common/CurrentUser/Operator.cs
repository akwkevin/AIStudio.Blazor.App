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
            User = httpContextAccessor.HttpContext.User;
        }

        public readonly ClaimsPrincipal User;
        /// <summary>
        /// 当前操作者UserId
        /// </summary>
        public virtual string UserId => FindClaimValue(SimpleClaimTypes.UserId);

        public virtual string UserName => FindClaimValue(SimpleClaimTypes.UserName);

        public virtual bool IsSuperAdmin => FindClaimValue(SimpleClaimTypes.SuperAdmin) == SimpleClaimTypes.SuperAdmin;
        public virtual string TenantId => FindClaimValue(SimpleClaimTypes.TenantId);
        public virtual Claim? FindClaim(string claimType)
        {
            return User.Claims.FirstOrDefault(c => c.Type == claimType);
        }

        public virtual Claim[] FindClaims(string claimType)
        {
            return User.Claims.Where(c => c.Type == claimType).ToArray();
        }

        public virtual string? FindClaimValue(string claimType)
        {
            return FindClaim(claimType)?.Value;
        }

        //public void WriteUserLog(UserLogType userLogType, string msg)
        //{
        //    var log = new Base_UserLog
        //    {
        //        Id = IdHelper.GetId(),
        //        CreateTime = DateTime.Now,
        //        CreatorId = UserId,
        //        CreatorName = Property.UserName,
        //        LogContent = msg,
        //        LogType = userLogType.ToString()
        //    };

        //    Task.Factory.StartNew(async () =>
        //    {
        //        using (var scop = _serviceProvider.CreateScope())
        //        {
        //            var Su = scop.ServiceProvider.GetService<ISqlSugarClient>();
        //            await Su.Insertable(log).ExecuteCommandAsync();
        //        }
        //    }, TaskCreationOptions.LongRunning);
        //}
    }
}
