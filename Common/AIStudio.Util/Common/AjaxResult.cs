namespace AIStudio.Util.Common
{
    /// <summary>
    /// Ajax请求结果
    /// </summary>
    public partial class AjaxResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public int Code { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        /// <value>
        /// The MSG.
        /// </value>
        public string Msg { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public object Data { get; set; }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public int Total { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AjaxResult"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <param name="data">The data.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        public AjaxResult(int code = StatusCodes.Status200OK, string? message = AjaxResultMessage.Status200OK, object? data = null, bool success = true)
        {
            Code = code;
            Msg = message;
            Data = data;
            Success = success;
        }
    }

    public partial class AjaxResult
    {
        /// <summary>
        /// Status200s the ok.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static AjaxResult Status200OK(string? message = AjaxResultMessage.Status200OK, object? data = null)
        {
            return new AjaxResult(StatusCodes.Status200OK, message, data, true);
        }

        /// <summary>
        /// Status400s the bad request.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static AjaxResult Status400BadRequest(string? message = AjaxResultMessage.Status400BadRequest, object? data = null)
        {
            return new AjaxResult(StatusCodes.Status400BadRequest, message, data, false);
        }

        /// <summary>
        /// Status401s the unauthorized.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static AjaxResult Status401Unauthorized(string? message = AjaxResultMessage.Status401Unauthorized, object? data = null)
        {
            return new AjaxResult(StatusCodes.Status401Unauthorized, message, data, false);
        }

        /// <summary>
        /// Status403s the forbidden.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static AjaxResult Status403Forbidden(string? message = AjaxResultMessage.Status403Forbidden, object? data = null)
        {
            return new AjaxResult(StatusCodes.Status403Forbidden, message, data, false);
        }

        /// <summary>
        /// Status404s the not found.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static AjaxResult Status404NotFound(string? message = AjaxResultMessage.Status404NotFound, object? data = null)
        {
            return new AjaxResult(StatusCodes.Status404NotFound, message, data, false);
        }
        /// <summary>
        /// Status409s the conflict.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static AjaxResult Status409Conflict(string? message = AjaxResultMessage.Status409Conflict, object? data = null)
        {
            return new AjaxResult(StatusCodes.Status409Conflict, message, data, false);
        }

        /// <summary>
        /// Status500s the internal server error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static AjaxResult Status500InternalServerError(string? message = AjaxResultMessage.Status500InternalServerError, object? data = null)
        {
            return new AjaxResult(StatusCodes.Status500InternalServerError, message, data, false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UploadResult
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string name { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string status { get; set; }
        /// <summary>
        /// Gets or sets the thumb URL.
        /// </summary>
        /// <value>
        /// The thumb URL.
        /// </value>
        public string thumbUrl { get; set; }
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string url { get; set; }
        /// <summary>
        /// Gets or sets the uploaded.
        /// </summary>
        /// <value>
        /// The uploaded.
        /// </value>
        public int uploaded { get; set; }
    }
}
