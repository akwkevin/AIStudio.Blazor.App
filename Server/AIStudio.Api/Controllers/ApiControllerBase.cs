using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIStudio.Api.Controllers
{
    /// <summary>
    /// 对外接口基控制器,会把json转成参数
    /// </summary>
    [Authorize]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
    }
}
