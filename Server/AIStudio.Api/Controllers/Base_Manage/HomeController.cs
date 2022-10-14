using AIStudio.Common.CurrentUser;
using AIStudio.Common.Filter.FilterAttribute;
using AIStudio.Common.Swagger;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
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

    public class HomeController : ApiControllerBase
    {
        readonly IHomeBusiness _homeBus;
        readonly IPermissionBusiness _permissionBus;
        readonly IBase_UserBusiness _userBus;
        readonly IOperator _operator;

        /// <summary>
        /// HomeController
        /// </summary>
        /// <param name="homeBus"></param>
        /// <param name="permissionBus"></param>
        /// <param name="userBus"></param>
        /// <param name="operator"></param>
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
        [RequestRecord]
        [HttpPost]
        [AllowAnonymous]
        public async Task<string> SubmitLogin(LoginInputDTO input)
        {
            var token = await _homeBus.SubmitLoginAsync(input);    

            return token;
        }

        /// <summary>
        /// 用户退出
        /// </summary>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]
        [AllowAnonymous]
        public async Task SubmitLogout()
        {
            await _homeBus.SubmitLogoutAsync();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task ChangePwd(ChangePwdInputDTO input)
        {
            await _homeBus.ChangePwdAsync(input);
        }

        /// <summary>
        /// 获取操作员信息
        /// </summary>
        /// <returns></returns>
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
     
        /// <summary>
        /// 获取操作员菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<Base_ActionTree>> GetOperatorMenuList()
        {
            var xx = await _permissionBus.GetUserMenuListAsync(_operator.UserId);
            return xx;
        }
    }
}