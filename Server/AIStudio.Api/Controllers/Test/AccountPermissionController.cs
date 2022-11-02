using AIStudio.Common.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AIStudio.Common.Authentication.Jwt.JwtHelper;

namespace AIStudio.Api.Controllers.Test
{
    /// <summary>
    /// jwt验证，测试Permissions权限
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.Test))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountPermissionController : ControllerBase
    {
        /// <summary>
        /// Create 权限,Test已赋权
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Permissions.Add)]
        public ActionResult<string> UserCreate() => "UserCreate";

        /// <summary>
        /// Update 权限,Test已赋权
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Permissions.Edit)]
        public ActionResult<string> UserUpdate() => "UserUpdate";

        /// <summary>
        /// Delete 权限,Test没有赋权
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Permissions.Delete)]
        public ActionResult<string> UserDelete() => "UserDelete";
    }
}
