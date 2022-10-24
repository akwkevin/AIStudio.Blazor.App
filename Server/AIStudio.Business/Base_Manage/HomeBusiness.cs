using AIStudio.Common.AppSettings;
using AIStudio.Common.Authentication.Jwt;
using AIStudio.Common.CurrentUser;
using AIStudio.Common.DI;
using AIStudio.Common.Jwt;
using AIStudio.Entity;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Simple.Common;
using SqlSugar;
using System.Data;
using System.Security.Claims;

namespace AIStudio.Business.Base_Manage
{
    public class HomeBusiness : IHomeBusiness, ITransientDependency
    {
        private readonly IOperator _operator;
        private readonly IMapper _mapper;
        private readonly ISqlSugarClient Db;
        private readonly IBase_UserBusiness _userBusiness;
        private readonly IPermissionBusiness _permissionBus;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HomeBusiness(ISqlSugarClient db, IOperator @operator, IMapper mapper, IBase_UserBusiness userBusiness, IPermissionBusiness permissionBus, IHttpContextAccessor httpContextAccessor)
        {
            _operator = @operator;
            _mapper = mapper;
            _userBusiness = userBusiness;
            _permissionBus = permissionBus;
            _httpContextAccessor = httpContextAccessor;
            Db = db;
        }

        public async Task<string> SubmitLoginAsync(LoginInputDTO input)
        {
            if (!input.Password.IsNullOrEmpty() && !input.Password.IsMd5())
            {
                input.Password = input.Password.ToMD5String();
            }
            var theUser = await Db.Queryable<Base_User>().Select<Base_UserDTO>()
                .Where(x => x.UserName == input.UserName && x.Password == input.Password)
                .FirstAsync();

            _operator.LoginUserId = theUser?.Id;
            _operator.LoginUserName = theUser?.UserName;
            _operator.LoginTenantId = theUser?.TenantId;

            if (theUser.IsNullOrEmpty())
                throw AjaxResultException.Status401Unauthorized("账号或密码不正确！");

            var userRoles = await Db.Queryable<Base_UserRole, Base_Role>((a, b) => new object[] { JoinType.Left, a.RoleId == b.Id })
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

                if (rolename == RoleTypes.超级管理员.ToString())
                {
                    claims.Add(new Claim(SimpleClaimTypes.SuperAdmin, rolename));
                }
            }

            var jwtToken = JwtHelper.CreateToken(claims);
            _httpContextAccessor.HttpContext.Response.Headers["access-token"] = jwtToken;
            _httpContextAccessor.HttpContext.Response.Headers["x-access-token"] = JwtHelper.CreateToken(claims, isRefresh: true);
            return jwtToken;
        }

   

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="tokeninput"></param>
        /// <returns></returns>
        public async Task<string> RefreshTokenAsync(RefreshTokenInputDTO tokeninput)
        {
            var (_isValid, _, _) = JwtHelper.Validate(tokeninput.RefreshToken, AppSettingsConfig.JwtOptions.RefreshSecretKey);
            if (_isValid)
            {
                var token = JwtHelper.Exchange(tokeninput.Token, tokeninput.RefreshToken, AppSettingsConfig.JwtOptions.SecretKey, AppSettingsConfig.JwtOptions.RefreshSecretKey, _httpContextAccessor.HttpContext, AppSettingsConfig.JwtOptions.AccessExpireHours, AppSettingsConfig.JwtOptions.RefreshClockSkew);
                if (!string.IsNullOrWhiteSpace(token))
                {
                    _httpContextAccessor.HttpContext.Response.Headers["access-token"] = token;
                    _httpContextAccessor.HttpContext.Response.Headers["x-access-token"] = JwtHelper.GenerateRefreshToken(tokeninput.Token, AppSettingsConfig.JwtOptions.RefreshSecretKey, (long)AppSettingsConfig.JwtOptions.RefreshExpireHours);
                }
            }
            else
            {
                throw AjaxResultException.Status401Unauthorized("RefreshToklen无效,请重新登录!");
            }
            return "";
        }

        public Task SubmitLogoutAsync()
        {
            _httpContextAccessor.HttpContext.Response.Headers["access-token"] = "invalid_token";
            _httpContextAccessor.HttpContext.Response.Headers["x-access-token"] = "invalid_token";
            return Task.CompletedTask;
        }

        public async Task ChangePwdAsync(ChangePwdInputDTO input)
        {
            var theUser = await _userBusiness.GetTheDataAsync(_operator.UserId);
            if (theUser?.Password != input.OldPassword?.ToMD5String())
                throw AjaxResultException.Status401Unauthorized("原密码错误!");

            theUser.Password = input.NewPassword.ToMD5String();
            await Db.Updateable<Base_User>(_mapper.Map<Base_User>(theUser)).ExecuteCommandAsync();
        }

        public async Task<UserInfoPermissionsDTO> GetOperatorInfoAsync()
        {
            var theInfo = await _userBusiness.GetTheDataAsync(_operator.UserId);
            var permissions = await _permissionBus.GetUserPermissionValuesAsync(_operator.UserId);
            var resObj = new UserInfoPermissionsDTO
            {
                UserInfo = theInfo,
                Permissions = permissions
            };

            return resObj;
        }

        public async Task<List<Base_ActionTree>> GetOperatorMenuListAsync()
        {
            return await _permissionBus.GetUserMenuListAsync(_operator.UserId);
        }
    }
}
