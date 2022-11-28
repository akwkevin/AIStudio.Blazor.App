using AIStudio.Common.EventBus.Abstract;
using System.ComponentModel.DataAnnotations;

namespace AIStudio.Common.EventBus.Models;

/// <summary>
/// 
/// </summary>
/// <seealso cref="AIStudio.Common.EventBus.Abstract.EventModel" />
public class RequestEvent : EventModel
{
    /// <summary>
    /// 操作人
    /// </summary>
    /// <value>
    /// The creator identifier.
    /// </value>
    [MaxLength(64)]
    public string? CreatorId { get; set; }

    /// <summary>
    /// 操作人名称
    /// </summary>
    /// <value>
    /// The name of the creator.
    /// </value>
    [MaxLength(64)]
    public string? CreatorName { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    /// <value>
    /// The tenant identifier.
    /// </value>
    [MaxLength(64)]
    public string? TenantId { get; set; }

    /// <summary>
    /// 日志名称
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    [MaxLength(128)]
    public string Name { get; set; } = "";

    /// <summary>
    /// 是否执行成功
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is success; otherwise, <c>false</c>.
    /// </value>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// 具体消息
    /// </summary>
    /// <value>
    /// The message.
    /// </value>
    public string? Message { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    /// <value>
    /// The browser.
    /// </value>
    [MaxLength(512)]
    public string? Browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    /// <value>
    /// The operating system.
    /// </value>
    public string? OperatingSystem { get; set; }

    /// <summary>
    /// IP
    /// </summary>
    /// <value>
    /// The ip.
    /// </value>
    [MaxLength(32)]
    public string? Ip { get; set; }

    /// <summary>
    /// 完整请求地址
    /// </summary>
    /// <value>
    /// The URL.
    /// </value>
    [MaxLength(2048)]
    public string? Url { get; set; }

    /// <summary>
    /// 请求路径
    /// </summary>
    /// <value>
    /// The path.
    /// </value>
    [MaxLength(2048)]
    public string? Path { get; set; }

    /// <summary>
    /// 类名称
    /// </summary>
    /// <value>
    /// The name of the class.
    /// </value>
    [MaxLength(256)]
    public string? ClassName { get; set; }

    /// <summary>
    /// 方法名称
    /// </summary>
    /// <value>
    /// The name of the method.
    /// </value>
    [MaxLength(256)]
    public string? MethodName { get; set; }

    /// <summary>
    /// 请求方式
    /// </summary>
    /// <value>
    /// The request method.
    /// </value>
    [MaxLength(16)]
    public string? RequestMethod { get; set; }

    /// <summary>
    /// 请求Body
    /// </summary>
    /// <value>
    /// The body.
    /// </value>
    public string? Body { get; set; }

    /// <summary>
    /// 返回结果
    /// </summary>
    /// <value>
    /// The result.
    /// </value>
    public string? Result { get; set; }

    /// <summary>
    /// 耗时
    /// </summary>
    /// <value>
    /// The elapsed time.
    /// </value>
    public long ElapsedTime { get; set; }

    /// <summary>
    /// 操作时间
    /// </summary>
    /// <value>
    /// The operating time.
    /// </value>
    public DateTime OperatingTime { get; set; }
}
