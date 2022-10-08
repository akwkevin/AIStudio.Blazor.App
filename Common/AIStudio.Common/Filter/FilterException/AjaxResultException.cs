using AIStudio.Util.Common;

namespace Simple.Common;

/// <summary>
/// 返回统一结果的异常
/// </summary>
public partial class AjaxResultException : Exception
{
    /// <summary>
    /// 结果信息
    /// </summary>
    public AjaxResult AjaxResult { get; private set; }

    /// <summary>
    /// 源异常
    /// </summary>
    public Exception? SourceException { get; private set; }

    public AjaxResultException()
        : this(new AjaxResult(), null)
    {
    }

    public AjaxResultException(AjaxResult result)
        : this(result, null)
    {
    }

    public AjaxResultException(Exception exception)
        : this(new AjaxResult(), exception)
    {
    }

    public AjaxResultException(AjaxResult result, Exception? exception)
    {
        AjaxResult = result;
        SourceException = exception;
    }
}

public partial class AjaxResultException
{
    public static AjaxResultException Status200OK(string? message = AjaxResultMessage.Status200OK, object? data = null)
    {
        var appResult = new AjaxResult(StatusCodes.Status200OK, message, data, true);
        return new AjaxResultException(appResult);
    }

    public static AjaxResultException Status400BadRequest(string? message = AjaxResultMessage.Status400BadRequest, object? data = null)
    {
        var appResult = new AjaxResult(StatusCodes.Status400BadRequest, message, data, false);
        return new AjaxResultException(appResult);
    }

    public static AjaxResultException Status401Unauthorized(string? message = AjaxResultMessage.Status401Unauthorized, object? data = null)
    {
        var appResult = new AjaxResult(StatusCodes.Status401Unauthorized, message, data, false);
        return new AjaxResultException(appResult);
    }

    public static AjaxResultException Status403Forbidden(string? message = AjaxResultMessage.Status403Forbidden, object? data = null)
    {
        var appResult = new AjaxResult(StatusCodes.Status403Forbidden, message, data, false);
        return new AjaxResultException(appResult);
    }

    public static AjaxResultException Status404NotFound(string? message = AjaxResultMessage.Status404NotFound, object? data = null)
    {
        var appResult = new AjaxResult(StatusCodes.Status404NotFound, message, data, false);
        return new AjaxResultException(appResult);
    }
    public static AjaxResultException Status409Conflict(string? message = AjaxResultMessage.Status409Conflict, object? data = null)
    {
        var appResult = new AjaxResult(StatusCodes.Status409Conflict, message, data, false);
        return new AjaxResultException(appResult);
    }

    public static AjaxResultException Status500InternalServerError(string? message = AjaxResultMessage.Status500InternalServerError, object? data = null)
    {
        var appResult = new AjaxResult(StatusCodes.Status500InternalServerError, message, data, false);
        return new AjaxResultException(appResult);
    }
}
