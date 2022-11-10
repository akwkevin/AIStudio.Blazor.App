using AIStudio.Common.DI;
using AIStudio.Common.DI.AOP;
using AIStudio.Common.Swagger;
using AIStudio.Util;
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
        /// GET api/AOPTest/Get/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return _valuesService.Find(id);
        }

        /// <summary>
        /// GET api/AOPTest/Get2?id=5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Get2")]
        public string Get2([FromQuery]string id)
        {
            return id;
        }

        /// <summary>
        /// GET api/AOPTest/Get3?id=5&amp;id2=6
        /// </summary>
        /// <param name="id"></param>
        /// <param name="id2"></param>
        /// <returns></returns>
        [HttpGet("Get3")]
        public string Get3([FromQuery] string id, string id2)
        {
            return id + id2;
        }

        /// <summary>
        /// GET api/AOPTest/Get4?Id=5&amp;Url=6&amp;PageId=7
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("Get4")]
        public string Get4(ApiModel model)
        {
            return model.ToJson(); ;
        }
    }

    public class ApiModel
    {
        [FromRoute]
        public int Id { get; set; }
        [FromQuery]
        public string Url { get; set; }
        [FromQuery]
        public int? PageId { get; set; }
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
