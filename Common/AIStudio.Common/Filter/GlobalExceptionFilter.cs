using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.Filter
{
    public class GlobalExceptionFilter : BaseActionFilterAsync, IAsyncExceptionFilter
    {
        readonly ILogger _logger;
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            Exception ex = context.Exception;

            //if (ex is BusException busEx)
            //{
            //    _logger.LogInformation(busEx.Message);
            //    context.Result = Error(busEx.Message, busEx.ErrorCode);
            //}
            //else
            {
                _logger.LogError(ex, "");
                context.Result = Error(ex.Message);
            }

            await Task.CompletedTask;
        }
    }
}
