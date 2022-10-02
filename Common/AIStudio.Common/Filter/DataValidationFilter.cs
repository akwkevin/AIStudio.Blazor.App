using AIStudio.Common.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;

namespace AIStudio.Common.Filter;

public class DataValidationFilter : BaseActionFilterAsync
{
    public override async Task OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var msgList = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);

            context.Result = Error(string.Join(",", msgList));
        }

        await Task.CompletedTask;
    }
}
