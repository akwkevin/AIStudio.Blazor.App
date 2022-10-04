namespace AIStudio.Util.Common
{
    /// <summary>
    /// Ajax请求结果
    /// </summary>
    public partial class AjaxResult<T> : AjaxResult
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        public new T Data { get; set; }

        public AjaxResult(int code = StatusCodes.Status200OK, string? message = AjaxResultMessage.Status200OK, T data = default(T), bool success = true)
        {
            Code = code;
            Msg = message;
            Data = data;
            Success = success;
        }
    }

    public partial class AjaxResult<T>
    {
        public static AjaxResult<T> Status200OK<T>(string? message = AjaxResultMessage.Status200OK, T data = default(T))
        {
            return new AjaxResult<T>(StatusCodes.Status200OK, message, data, true);
        }

        public static AjaxResult<T> Status400BadRequest(string? message = AjaxResultMessage.Status400BadRequest, T data = default(T))
        {
            return new AjaxResult<T>(StatusCodes.Status400BadRequest, message, data, false);
        }

        public static AjaxResult<T> Status401Unauthorized(string? message = AjaxResultMessage.Status401Unauthorized, T data = default(T))
        {
            return new AjaxResult<T>(StatusCodes.Status401Unauthorized, message, data, false);
        }

        public static AjaxResult<T> Status403Forbidden(string? message = AjaxResultMessage.Status403Forbidden, T data = default(T))
        {
            return new AjaxResult<T>(StatusCodes.Status403Forbidden, message, data, false);
        }

        public static AjaxResult<T> Status404NotFound(string? message = AjaxResultMessage.Status404NotFound, T data = default(T))
        {
            return new AjaxResult<T>(StatusCodes.Status404NotFound, message, data, false);
        }
        public static AjaxResult<T> Status409Conflict(string? message = AjaxResultMessage.Status409Conflict, T data = default(T))
        {
            return new AjaxResult<T>(StatusCodes.Status409Conflict, message, data, false);
        }

        public static AjaxResult<T> Status500InternalServerError(string? message = AjaxResultMessage.Status500InternalServerError, T data = default(T))
        {
            return new AjaxResult<T>(StatusCodes.Status500InternalServerError, message, data, false);
        }
    }
}
