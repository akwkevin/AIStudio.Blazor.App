using AIStudio.Common.Authentication.Jwt;
using AIStudio.Common.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIStudio.Api.Controllers.Test
{
    /// <summary>
    /// jwt验证，测试角色权限
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.Test))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtHelper _jwtHelper;

        public AccountController(JwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> GetToken()
        {
            return _jwtHelper.CreateToken();
        }

        /// <summary>
        /// Test
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult<string> GetTest()
        {
            return "Test Authorize";
        }

        /// <summary>
        /// Role TestAdmin
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "r_admin")]
        [HttpGet]
        public ActionResult<string> GetTestAdmin()
        {
            return "Test Authorize Admin";
        }

        /// <summary>
        /// Role TestUser
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "r_user")]
        [HttpGet]
        public ActionResult<string> GetTestUser()
        {
            return "Test Authorize User";
        }

        /// <summary>
        /// Role TestAdmin,TestUser
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "r_admin,r_user")] //user 或 admin 其一满足即可
        [HttpGet]
        public ActionResult<string> GetTestAdminOrUser()
        {
            return "Test Authorize Admin Or User";
        }
    }
}
