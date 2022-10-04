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
        public bool Success { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }

        public int Total { get; set; }

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
        public static AjaxResult Status200OK(string? message = AjaxResultMessage.Status200OK, object? data = null)
        {
            return new AjaxResult(StatusCodes.Status200OK, message, data, true);
        }

        public static AjaxResult Status400BadRequest(string? message = AjaxResultMessage.Status400BadRequest, object? data = null)
        {
            return new AjaxResult(StatusCodes.Status400BadRequest, message, data, false);
        }

        public static AjaxResult Status401Unauthorized(string? message = AjaxResultMessage.Status401Unauthorized, object? data = null)
        {
            return new AjaxResult(StatusCodes.Status401Unauthorized, message, data, false);
        }

        public static AjaxResult Status403Forbidden(string? message = AjaxResultMessage.Status403Forbidden, object? data = null)
        {
            return new AjaxResult(StatusCodes.Status403Forbidden, message, data, false);
        }

        public static AjaxResult Status404NotFound(string? message = AjaxResultMessage.Status404NotFound, object? data = null)
        {
            return new AjaxResult(StatusCodes.Status404NotFound, message, data, false);
        }
        public static AjaxResult Status409Conflict(string? message = AjaxResultMessage.Status409Conflict, object? data = null)
        {
            return new AjaxResult(StatusCodes.Status409Conflict, message, data, false);
        }

        public static AjaxResult Status500InternalServerError(string? message = AjaxResultMessage.Status500InternalServerError, object? data = null)
        {
            return new AjaxResult(StatusCodes.Status500InternalServerError, message, data, false);
        }
    }

    public class UploadResult
    {
        public string name { get; set; }
        public string status { get; set; }
        public string thumbUrl { get; set; }
        public string url { get; set; }
        public int uploaded { get; set; }
    }
}
