using AIStudio.Common.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using NLog.Fluent;
using System.ComponentModel.DataAnnotations;

namespace AIStudio.Api.Controllers.Test;

/// <summary>
/// 测试数据校验
/// </summary>
[ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.Test))]
[ApiController]
[Route("[controller]/[action]")]
public class ValidationTestController : ControllerBase
{
    private readonly ILogger _logger;
    /// <summary>
    /// ValidationTestController
    /// </summary>
    /// <param name="logger"></param>
    public ValidationTestController(ILogger<ValidationTestController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 1.使用代码校验
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public string TraditionValidation(TestModel model)
    {
        if (string.IsNullOrEmpty(model.Name))
        {   
            return "名字不能为空！";
        }
        if (model.Name.Length > 10)
        {
            return "名字长度不能超过10个字符！";
        }
        _logger.LogError("验证通过！");
        return "验证通过！";
    }

    /// <summary>
    /// 2.使用模型校验
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public TestModel ModelValidation(TestModel model)
    {
        return model;
    }

    /// <summary>
    /// 使用模型校验
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    [HttpPost("List")]
    public List<TestModel> List(List<TestModel> models)
    {
        return models;
    }

    /// <summary>
    /// 异常测试
    /// </summary>
    [HttpPost("Exception")]
    public void Exception()
    {
       throw new System.Exception("异常测试");
    }
}

/// <summary>
/// 测试模型
/// </summary>
public class TestModel
{
    /// <summary>
    /// 名字
    /// </summary>
    [Required(ErrorMessage = "名字不能为空！")]
    [StringLength(10, ErrorMessage = "名字长度不能超过10个字符！")]
    public string? Name { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [EmailAddress(ErrorMessage = "邮箱格式错误！")]
    public string? Email { get; set; }

    /// <summary>
    /// 其它
    /// </summary>
    public string? Other { get; set; }
}
