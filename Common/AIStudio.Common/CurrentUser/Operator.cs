using AIStudio.Common.Autofac.Lifetime;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AIStudio.Common.CurrentUser
{
    /// <summary>
    /// 操作者
    /// </summary>
    public class Operator : IOperator, IInstancePerLifetimeScope
    {
        readonly IServiceProvider _serviceProvider;
        public Operator(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            UserId = httpContextAccessor?.HttpContext?.User.Claims
                .Where(x => x.Type == "userId").FirstOrDefault()?.Value;

            UserName = httpContextAccessor?.HttpContext?.User.Claims
               .Where(x => x.Type == "userName").FirstOrDefault()?.Value;
        }

        private object _lockObj = new object();

        /// <summary>
        /// 当前操作者UserId
        /// </summary>
        public string UserId { get; }

        public string UserName { get; }

        /// <summary>
        /// 判断是否为超级管理员
        /// </summary>
        /// <returns></returns>
        public bool IsAdmin()
        {
            //var role = Property.RoleType;
            //if (UserId == GlobalData.ADMINID || role.HasFlag(RoleTypes.超级管理员))
            //    return true;
            //else
                return false;
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
