using AIStudio.Common.CurrentUser;
using AIStudio.Common.Swagger;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.IBusiness.Base_Manage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIStudio.Api.Controllers.Base_Manage
{
    /// <summary>
    /// 首页控制器
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Base_Manage/[controller]/[action]")]
    [Authorize]

    public class HomeController : ApiControllerBase
    {
        readonly IHomeBusiness _homeBus;
        readonly IPermissionBusiness _permissionBus;
        readonly IBase_UserBusiness _userBus;
        readonly IOperator _operator;

        public HomeController(IHomeBusiness homeBus,
            IPermissionBusiness permissionBus,
            IBase_UserBusiness userBus,
            IOperator @operator)
        {
            _homeBus = homeBus;
            _permissionBus = permissionBus;
            _userBus = userBus;
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

        [HttpPost]
        public async Task<object> GetOperatorInfo()
        {
            var theInfo = await _userBus.GetTheDataAsync(_operator.UserId);
            var permissions = await _permissionBus.GetUserPermissionValuesAsync(_operator.UserId);
            var resObj = new
            {
                UserInfo = theInfo,
                Permissions = permissions
            };

            return resObj;
        }
        [HttpPost]
        public async Task<object> GetOperatorClientInfo()
        {
            var theInfo = await _userBus.GetTheDataAsync(_operator.UserId);
            var permissions = await _permissionBus.GetUserPermissionValuesAsync(_operator.UserId);
            var resObj = new
            {
                UserInfo = theInfo,
                Permissions = permissions
            };

            return resObj;
        }

        [HttpPost]
        public async Task<List<Base_ActionTree>> GetOperatorMenuList()
        {
            var xx = await _permissionBus.GetUserMenuListAsync(_operator.UserId);
            return xx;
        }
    }
}