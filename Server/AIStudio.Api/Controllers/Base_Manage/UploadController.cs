using AIStudio.Api.Controllers;
using AIStudio.Common.Result;
using AIStudio.Common.Swagger;
using AIStudio.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coldairarrow.Api.Controllers.Base_Manage
{
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Base_Manage/[controller]/[action]")]
    [Authorize]
    public class UploadController : ApiControllerBase
    {
        readonly IConfiguration _configuration;
        public UploadController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult UploadFileByForm()
        {
            var file = Request.Form.Files.FirstOrDefault();
            if (file == null)
                return AjaxResultActionFilter.JsonContent(new { status = "error" }.ToJson());

            string path = $"/Upload/{Guid.NewGuid().ToString("N")}/{file.FileName}";
            string physicPath = GetAbsolutePath($"~{path}");
            string dir = Path.GetDirectoryName(physicPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            using (FileStream fs = new FileStream(physicPath, FileMode.Create))
            {
                file.CopyTo(fs);
            }

            string url = $"{_configuration["WebRootUrl"]}{path}";
            var res = new
            {
                name = file.FileName,
                status = "done",
                thumbUrl = url,
                url = url
            };

            return AjaxResultActionFilter.JsonContent(res.ToJson());
        }

        protected string GetAbsolutePath(string virtualPath)
        {
            string path = virtualPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (path[0] == '~')
                path = path.Remove(0, 2);
            string rootPath = HttpContext.RequestServices.GetService<IWebHostEnvironment>().WebRootPath;

            return Path.Combine(rootPath, path);
        }
    }
}
