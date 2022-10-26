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

        /// <summary>
        /// 首页控制器
        /// </summary>
        /// <param name="homeBus"></param>
        public HomeController(IHomeBusiness homeBus)
        {
            _homeBus = homeBus;
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
        /// 刷新Token(Headers返回新的Token和刷新Token)
        /// </summary>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]
        [AllowAnonymous]
        public async Task<string> RefreshToken(RefreshTokenInputDTO input)
        {
            var token = await _homeBus.RefreshTokenAsync(input);
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
        public async Task<Base_UserDTO> GetOperatorInfo()
        {
            return await _homeBus.GetOperatorInfoAsync();
        }
     
        /// <summary>
        /// 获取操作员菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<Base_ActionTree>> GetOperatorMenuList()
        {
            return await _homeBus.GetOperatorMenuListAsync();
        }
    }
}