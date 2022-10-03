using AIStudio.Common.Authentication.Jwt;
using AIStudio.Common.CurrentUser;
using AIStudio.Common.DI;
using AIStudio.Common.Helpers;
using AIStudio.Common.Jwt;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Util;
using AutoMapper;
using Coldairarrow.Business.Base_Manage;
using Simple.Common;
using SqlSugar;
using StackExchange.Redis;
using System.Security.Claims;

namespace AIStudio.Business.Base_Manage
{
    public class HomeBusiness : IHomeBusiness, ITransientDependency
    {
        private readonly IOperator _operator;
        private readonly IMapper _mapper;
        private readonly ISqlSugarClient Su;
        public HomeBusiness(ISqlSugarClient su, IOperator @operator, IMapper mapper)
        {
            _operator = @operator;
            _mapper = mapper;
            Su = su;
        }

        public async Task<string> SubmitLoginAsync(LoginInputDTO input)
        {
            var password = input.password;
            input.password = HashHelper.Md5(input.password);
            var theUser = await Su.Queryable<Base_User>().Select<Base_UserDTO>()
                .Where(x => x.UserName == input.userName && (x.Password == input.password || x.Password == password))
                .FirstAsync();

            if (theUser.IsNullOrEmpty())
                throw AjaxResultException.Status401Unauthorized("账号或密码不正确！");

            var userRoles = await Su.Queryable<Base_UserRole, Base_Role>((a, b) => new object[] { JoinType.Left, a.RoleId == b.Id})
                .Where(a => a.UserId == theUser.Id)
                .Select((a, b) => new
                {
                    a.UserId,
                    RoleId = b.Id,
                    b.RoleName
                }).ToListAsync();

            theUser.RoleIdList = userRoles.Select(x => x.RoleId).ToList();
            theUser.RoleNameList = userRoles.Select(x => x.RoleName).ToList();

            List<Claim> claims = new List<Claim>
            {
                new Claim(SimpleClaimTypes.UserId,theUser.Id),
                new Claim(SimpleClaimTypes.Name, theUser.UserName)               
            };
            if (!string.IsNullOrEmpty(theUser.TenantId))
            {
                claims.Add(new Claim(SimpleClaimTypes.TenantId, theUser.TenantId));
            }
            foreach (var role in theUser.RoleIdList)
            {
                if (string.IsNullOrEmpty(role)) continue;
                claims.Add(new Claim(SimpleClaimTypes.Role, role));
            }
            foreach (var rolename in theUser.RoleNameList)
            {
                if (string.IsNullOrEmpty(rolename)) continue;
                claims.Add(new Claim(SimpleClaimTypes.Actor, rolename));
            }

            var jwtToken = JwtHelper.CreateToken(claims);

            return jwtToken;
        }

        public async Task ChangePwdAsync(ChangePwdInputDTO input)
        {
            //var theUser = _operator.UserId;
            //if (theUser.Password != input.oldPwd?.ToMD5String())
            //    throw AjaxResultException.Status401Unauthorized("原密码错误!");

            //theUser.Password = input.newPwd.ToMD5String();
            //await Su.Updateable<Base_User>(_mapper.Map<Base_User>(theUser)).ExecuteCommandAsync();

            ////更新缓存
            //await _base_UserCache.UpdateCacheAsync(theUser.Id);
            throw new NotImplementedException();
        }
    }
}
