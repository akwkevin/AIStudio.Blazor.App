using AIStudio.Common.Autofac;
using AIStudio.Common.Swagger;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Mvc;

namespace AIStudio.Api.Controllers.Test
{
    /// <summary>
    /// 测试Autofac注入.
    /// </summary>
    /// <seealso cref="Controller" />
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.Test))]
    [Route("api/[controller]")]
    public class AutofacTestController : Controller
    {
        private readonly IValuesService _valuesService;

        public AutofacTestController(IValuesService valuesService)
        {
            _valuesService = valuesService;
        }

        /// <summary>
        /// GET api/values
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _valuesService.FindAll();
        }

        /// <summary>
        /// GET api/values/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return _valuesService.Find(id);
        }
    }

    [Intercept(typeof(AutofacAOP))]
    public interface IValuesService
    {
        IEnumerable<string> FindAll();

        string Find(int id);
    }

    public class ValuesService : IValuesService
    {
        private readonly ILogger<ValuesService> _logger;

        public ValuesService(ILogger<ValuesService> logger)
        {
            _logger = logger;
        }

        public IEnumerable<string> FindAll()
        {
            _logger.LogDebug("{method} called", nameof(FindAll));

            return new[] { "value1", "value2" };
        }

        public string Find(int id)
        {
            _logger.LogDebug("{method} called with {id}", nameof(Find), id);

            return $"value{id}";
        }
    }
}
