using AIStudio.Common.Authentication.Jwt;
using AIStudio.Common.CurrentUser;
using AIStudio.Common.Jwt;
using AIStudio.Common.Swagger;
using Coldairarrow.Business.Base_Manage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AIStudio.Api.Controllers.Base_Manage
{
    /// <summary>
    /// 首页控制器
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Base_Manage/[controller]/[action]")]

    public class HomeController : ControllerBase
    {
        readonly IHomeBusiness _homeBus;
        readonly IOperator _operator;

        public HomeController(IHomeBusiness homeBus, IOperator @operator)
        {
            _homeBus = homeBus;
            _operator = @operator;
        }

        /// <summary>
        /// 用户登录(获取token)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<string> SubmitLogin(LoginInputDTO input)
        {
            var token = await _homeBus.SubmitLoginAsync(input);    

            return token;
        }
        [HttpPost]
        public async Task ChangePwd(ChangePwdInputDTO input)
        {
            await _homeBus.ChangePwdAsync(input);
        }

        //[HttpPost]
        //public async Task<object> GetOperatorInfo()
        //{
        //    var theInfo = await _userBus.GetTheDataAsync(_operator.UserId);
        //    var permissions = await _permissionBus.GetUserPermissionValuesAsync(_operator.UserId);
        //    var resObj = new
        //    {
        //        UserInfo = theInfo,
        //        Permissions = permissions
        //    };

        //    return resObj;
        //}
        //[HttpPost]
        //public async Task<object> GetOperatorClientInfo()
        //{
        //    var theInfo = await _userBus.GetTheDataAsync(_operator.UserId);
        //    var permissions = await _permissionBus.GetUserPermissionValuesAsync(_operator.UserId);
        //    var resObj = new
        //    {
        //        UserInfo = theInfo,
        //        Permissions = permissions
        //    };

        //    return resObj;
        //}

        //[HttpPost]
        //public async Task<List<Base_ActionDTO>> GetOperatorMenuList()
        //{
        //    return await _permissionBus.GetUserMenuListAsync(_operator.UserId);
        //}
    }
}