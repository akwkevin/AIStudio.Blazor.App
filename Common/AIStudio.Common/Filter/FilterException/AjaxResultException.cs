using AIStudio.Util.Common;

namespace Simple.Common;

/// <summary>
/// 返回统一结果的异常
/// </summary>
/// <seealso cref="System.Exception" />
/// <seealso cref="System.Exception" />
public partial class AjaxResultException : Exception
{
    /// <summary>
    /// 结果信息
    /// </summary>
    /// <value>
    /// The ajax result.
    /// </value>
    public AjaxResult AjaxResult { get; private set; }

    /// <summary>
    /// 源异常
    /// </summary>
    /// <value>
    /// The source exception.
    /// </value>
    public Exception? SourceException { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AjaxResultException"/> class.
    /// </summary>
    public AjaxResultException()
        : this(new AjaxResult(), null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AjaxResultException"/> class.
    /// </summary>
    /// <param name="result">The result.</param>
    public AjaxResultException(AjaxResult result)
        : this(result, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AjaxResultException"/> class.
    /// </summary>
    /// <param name="exception">The exception.</param>
    public AjaxResultException(Exception exception)
        : this(new AjaxResult(), exception)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AjaxResultException"/> class.
    /// </summary>
    /// <param name="result">The result.</param>
    /// <param name="exception">The exception.</param>
    public AjaxResultException(AjaxResult result, Exception? exception)
    {
        AjaxResult = result;
        SourceException = exception;
    }
}

public partial class AjaxResultException
{
    /// <summary>
    /// Status200s the ok.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    public static AjaxResultException Status200OK(string? message = AjaxResultMessage.Status200OK, object? data = null)
    {
        var appResult = new AjaxResult(StatusCodes.Status200OK, message, data, true);
        return new AjaxResultException(appResult);
    }

    /// <summary>
    /// Status400s the bad request.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    public static AjaxResultException Status400BadRequest(string? message = AjaxResultMessage.Status400BadRequest, object? data = null)
    {
        var appResult = new AjaxResult(StatusCodes.Status400BadRequest, message, data, false);
        return new AjaxResultException(appResult);
    }

    /// <summary>
    /// Status401s the unauthorized.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    public static AjaxResultException Status401Unauthorized(string? message = AjaxResultMessage.Status401Unauthorized, object? data = null)
    {
        var appResult = new AjaxResult(StatusCodes.Status401Unauthorized, message, data, false);
        return new AjaxResultException(appResult);
    }

    /// <summary>
    /// Status403s the forbidden.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    public static AjaxResultException Status403Forbidden(string? message = AjaxResultMessage.Status403Forbidden, object? data = null)
    {
        var appResult = new AjaxResult(StatusCodes.Status403Forbidden, message, data, false);
        return new AjaxResultException(appResult);
    }

    /// <summary>
    /// Status404s the not found.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    public static AjaxResultException Status404NotFound(string? message = AjaxResultMessage.Status404NotFound, object? data = null)
    {
        var appResult = new AjaxResult(StatusCodes.Status404NotFound, message, data, false);
        return new AjaxResultException(appResult);
    }
    /// <summary>
    /// Status409s the conflict.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    public static AjaxResultException Status409Conflict(string? message = AjaxResultMessage.Status409Conflict, object? data = null)
    {
        var appResult = new AjaxResult(StatusCodes.Status409Conflict, message, data, false);
        return new AjaxResultException(appResult);
    }

    /// <summary>
    /// Status500s the internal server error.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    public static AjaxResultException Status500InternalServerError(string? message = AjaxResultMessage.Status500InternalServerError, object? data = null)
    {
        var appResult = new AjaxResult(StatusCodes.Status500InternalServerError, message, data, false);
        return new AjaxResultException(appResult);
    }
}
