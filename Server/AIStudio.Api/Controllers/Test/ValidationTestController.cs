using AIStudio.Common.Swagger;
using Microsoft.AspNetCore.Mvc;
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

    [HttpPost("List")]
    public List<TestModel> List(List<TestModel> models)
    {
        return models;
    }
}

public class TestModel
{
    [Required(ErrorMessage = "���ֲ���Ϊ�գ�")]
    [StringLength(10, ErrorMessage = "���ֳ��Ȳ��ܳ���10���ַ���")]
    public string? Name { get; set; }

    [EmailAddress(ErrorMessage = "�����ʽ����")]
    public string? Email { get; set; }
}
