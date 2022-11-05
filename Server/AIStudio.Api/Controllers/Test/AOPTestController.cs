using AIStudio.Common.DI;
using AIStudio.Common.DI.AOP;
using AIStudio.Common.Swagger;
using Microsoft.AspNetCore.Mvc;

namespace AIStudio.Api.Controllers.Test
{
    /// <summary>
    /// 测试TestAOP注入.
    /// </summary>
    /// <seealso cref="Controller" />
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.Test))]
    [Route("api/[controller]")]
    public class AOPTestController : Controller
    {
        private readonly IValuesService _valuesService;
        /// <summary>
        /// AOPTestController
        /// </summary>
        /// <param name="valuesService"></param>
        public AOPTestController(IValuesService valuesService)
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

    /// <summary>
    /// IValuesService
    /// </summary>
   
    public interface IValuesService
    {
        IEnumerable<string> FindAll();
        string Find(int id);
    }

    /// <summary>
    /// ValuesService
    /// </summary>
    public class ValuesService : IValuesService
    {
        private readonly ILogger<ValuesService> _logger;

        /// <summary>
        /// ValuesService
        /// </summary>
        /// <param name="logger"></param>
        public ValuesService(ILogger<ValuesService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// FindAll
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> FindAll()
        {
            _logger.LogDebug("{method} called", nameof(FindAll));

            return new[] { "value1", "value2" };
        }

        /// <summary>
        /// Find
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Find(int id)
        {
            _logger.LogDebug("{method} called with {id}", nameof(Find), id);

            return $"value{id}";
        }
    }
}
