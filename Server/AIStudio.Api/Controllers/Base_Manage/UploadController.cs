using AIStudio.Api.Controllers;
using AIStudio.Api.Controllers.Test;
using AIStudio.Common.AppSettings;
using AIStudio.Common.Filter;
using AIStudio.Common.Filter.FilterAttribute;
using AIStudio.Common.Swagger;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpYaml.Serialization.Logging;
using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;

namespace AIStudio.Api.Controllers.Base_Manage
{
    /// <summary>
    /// 文件上传
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Base_Manage/[controller]/[action]")]
    [Authorize]
    [IgnoreRequestRecord]
    public class UploadController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UploadController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// 文件上传控制器
        /// </summary>
        /// <param name="configuration"></param>
        public UploadController(ILogger<UploadController> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _configuration =configuration;        
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UploadFileByForm()
        {
            var file = Request.Form.Files.FirstOrDefault();
            if (file == null)
                return AjaxResultActionFilter.Error();

            string path = $"/Upload/{Guid.NewGuid().ToString("N")}/{file.FileName}";
            string physicPath = GetAbsolutePath($"~{path}");
            string dir = Path.GetDirectoryName(physicPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            using (FileStream fs = new FileStream(physicPath, FileMode.Create))
            {
                file.CopyTo(fs);
            }

            
            string url = $"{AppSettingsConfig.webUrl}{path}";
            var res = new
            {
                name = file.FileName,
                status = "done",
                thumbUrl = url,
                url = url
            };

            return AjaxResultActionFilter.Success(res);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UploadFilesByForm()
        {
            var files = Request.Form.Files;
            if (files == null || files.Count == 0)
                return AjaxResultActionFilter.Error();

            List<object> list = new List<object>();
            foreach (var file in files)
            {
                string path = $"/Upload/{Guid.NewGuid().ToString("N")}/{file.FileName}";
                string physicPath = GetAbsolutePath($"~{path}");
                string dir = Path.GetDirectoryName(physicPath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                using (FileStream fs = new FileStream(physicPath, FileMode.Create))
                {
                    file.CopyTo(fs);
                }

                string url = $"{AppSettingsConfig.webUrl}{path}";
                var res = new
                {
                    name = file.FileName,
                    status = "done",
                    thumbUrl = url,
                    url = url
                };
                list.Add(res);
            }
          
            return AjaxResultActionFilter.Success(list);
        }

        #region  大文件上传
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UploadFileChunck(IFormFile file, string tempDirectory, int index, int total)
        {
            if (file == null)
            {
                return BadRequest("请选择上传文件");
            }

            string fileUploadPath = GetUploadPath();
         
            string tmp = Path.Combine(fileUploadPath, tempDirectory) + "/";//临时保存分块的目录
            if (index == 0)
            {
                if (Directory.Exists(tmp))
                {
                    Directory.Delete(tmp);
                }
            }
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var strmd5 = GetMD5Value(stream);
                    //if (md5 == strmd5)//校验MD5值
                    //{
                    //}

                    if (await Save(stream, tmp, index.ToString()))
                    {
                      
                        bool mergeOk = false;
                        string path = "";
                        string physicPath = "";
                        if (total - 1 == index)
                        {
                            path = $"/Upload/{Guid.NewGuid().ToString("N")}/{file.FileName}";
                            physicPath = GetAbsolutePath($"~{path}");
                            string dir = Path.GetDirectoryName(physicPath);
                            if (!Directory.Exists(dir))
                                Directory.CreateDirectory(dir);

                            mergeOk = await FileMerge(tmp, physicPath);
                            if (mergeOk)
                            {
                                _logger.LogInformation($"文件上传成功：{physicPath}");
                            }
                        }

                        string url = $"{AppSettingsConfig.webUrl}{path}";
                        var res = new
                        {
                            index = index,
                            name = file.FileName,
                            status = mergeOk == true ? "done" :"part",
                            thumbUrl = url,
                            url = url
                        };

                        return AjaxResultActionFilter.Success(res);
                    }
                    else
                    {
                        return AjaxResultActionFilter.Error("上传失败");
                    }
                }
            }
            catch (Exception ex)
            {
                Directory.Delete(tmp);//删除文件夹
                _logger.LogError($"文件上传异常：{ex.Message}");
                return AjaxResultActionFilter.Error("上传失败");
            }

        }
        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="tmpDirectory">临时上传目录</param>        
        /// <param name="path">上传目录</param>
        /// <param name="saveFileName">保存之后新文件名</param>
        /// <returns></returns>
        private async Task<bool> FileMerge(string tmpDirectory, string saveName)
        {
            try
            {
                var files = Directory.GetFiles(tmpDirectory);

                using (var fs = new FileStream(saveName, FileMode.Create))
                {
                    foreach (var part in files.OrderBy(x => x.Length).ThenBy(x => x))
                    {
                        var bytes = System.IO.File.ReadAllBytes(part);
                        await fs.WriteAsync(bytes, 0, bytes.Length);
                        bytes = null;
                        System.IO.File.Delete(part);//删除分块
                    }
                    fs.Close();

                    Directory.Delete(tmpDirectory);//删除临时目录
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"文件合并异常：{ex.Message}");
                return false;
            }

        }

        #endregion       

        /// <summary>
        /// 文件保存到本地
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="path"></param>
        /// <param name="saveName"></param>
        /// <returns></returns>
        private async Task<bool> Save(Stream stream, string path, string saveName)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                await Task.Run(() =>
                {
                    FileStream fs = new FileStream(path + saveName, FileMode.Create);
                    stream.Position = 0;
                    stream.CopyTo(fs);
                    fs.Close();
                });
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError($"文件保存异常：{ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        /// <param name="obj">类型只能为string or stream，否则将会抛出错误</param>
        /// <returns>文件的MD5值</returns>
        private string GetMD5Value(object obj)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = null;
            switch (obj)
            {
                case string str:
                    data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(str));
                    break;
                case Stream stream:
                    data = md5Hash.ComputeHash(stream);
                    break;
                case null:
                    throw new ArgumentException("参数不能为空");
                default:
                    throw new ArgumentException("参数类型错误");
            }

            return BitConverter.ToString(data).Replace("-", "");
        }

        /// <summary>
        /// 获取路径
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        protected string GetAbsolutePath(string virtualPath)
        {
            string path = virtualPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (path[0] == '~')
                path = path.Remove(0, 2);
            string rootPath = HttpContext.RequestServices.GetService<IWebHostEnvironment>().WebRootPath;

            return Path.Combine(rootPath, path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected string GetUploadPath()
        {
            string rootPath = HttpContext.RequestServices.GetService<IWebHostEnvironment>().WebRootPath;

            return Path.Combine(rootPath, "Upload");
        }
    }
}
