using AIStudio.Util.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Simple.Common;

namespace AIStudio.Common.Filter.FilterException;

internal class AjaxResultOptionsSetup : IConfigureOptions<AjaxResultOptions>
{
    public void Configure(AjaxResultOptions options)
    {
        // 默认结果工厂
        options.ResultFactory = resultException =>
        {
            return GetResult(resultException);
        };
    }

    internal static IActionResult GetResult(AjaxResultException resultException)
    {
        // AppResultException 都返回 200 状态码
        var objectResult = new ObjectResult(resultException.AjaxResult);
        objectResult.StatusCode = StatusCodes.Status200OK;
        return objectResult;
    }
}
