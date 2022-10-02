using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Simple.Common;

namespace AIStudio.Common.Result;

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
        IActionResult result;

        //（!Problem: 目前只能处理 Api 结果）

        // Mvc 结果

        //// Api 结果 弃用
        //result = new ContentResult()
        //{
        //    StatusCode = resultException.AjaxResult.Code,
        //    ContentType = "application/json",
        //    Content = JsonHelper.Serialize(resultException.AjaxResult)
        //};

        // Api 结果 统一处理为 ObjectResult 与控制器保持一致
        result = new ObjectResult(resultException.AjaxResult)
        {
            StatusCode = resultException.AjaxResult.Code
        };

        return result;
    }
}
