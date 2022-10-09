using AIStudio.Common.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using NLog.Fluent;
using System.ComponentModel.DataAnnotations;

namespace AIStudio.Api.Controllers.Test;

/// <summary>
/// ��������У��
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
    /// 1.ʹ�ô���У��
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public string TraditionValidation(TestModel model)
    {
        if (string.IsNullOrEmpty(model.Name))
        {   
            return "���ֲ���Ϊ�գ�";
        }
        if (model.Name.Length > 10)
        {
            return "���ֳ��Ȳ��ܳ���10���ַ���";
        }
        _logger.LogError("��֤ͨ����");
        return "��֤ͨ����";
    }

    /// <summary>
    /// 2.ʹ��ģ��У��
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public TestModel ModelValidation(TestModel model)
    {
        return model;
    }

    /// <summary>
    /// ʹ��ģ��У��
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    [HttpPost("List")]
    public List<TestModel> List(List<TestModel> models)
    {
        return models;
    }

    /// <summary>
    /// �쳣����
    /// </summary>
    [HttpPost("Exception")]
    public void Exception()
    {
       throw new System.Exception("�쳣����");
    }
}

/// <summary>
/// ����ģ��
/// </summary>
public class TestModel
{
    /// <summary>
    /// ����
    /// </summary>
    [Required(ErrorMessage = "���ֲ���Ϊ�գ�")]
    [StringLength(10, ErrorMessage = "���ֳ��Ȳ��ܳ���10���ַ���")]
    public string? Name { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [EmailAddress(ErrorMessage = "�����ʽ����")]
    public string? Email { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public string? Other { get; set; }
}
