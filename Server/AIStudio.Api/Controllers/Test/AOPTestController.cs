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
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    /// <seealso cref="Controller" />
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.Test))]
    [Route("api/[controller]")]
    public class AOPTestController : Controller
    {
        /// <summary>
        /// The values service
        /// </summary>
        private readonly IValuesService _valuesService;
        /// <summary>
        /// AOPTestController
        /// </summary>
        /// <param name="valuesService">The values service.</param>
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
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return _valuesService.Find(id);
        }

        /// <summary>
        /// GET api/AOPTest/Get2?id=5
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("Get2")]
        public string Get2([FromQuery]string id)
        {
            return id;
        }

        /// <summary>
        /// GET api/AOPTest/Get3?id=5&amp;id2=6
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="id2">The id2.</param>
        /// <returns></returns>
        [HttpGet("Get3")]
        public string Get3([FromQuery] string id, string id2)
        {
            return id + id2;
        }

        /// <summary>
        /// GET api/AOPTest/Get4?Id=5&amp;Url=6&amp;PageId=7
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet("Get4")]
        public string Get4(ApiModel model)
        {
            return model.ToJson(); ;
        }

        /// <summary>
        /// GET api/AOPTest/Get5?Id=5&amp;Url=6&amp;PageId=7
        /// </summary>
        /// <param name="schoolName"></param>
        /// <param name="classNum"></param>
        /// <returns></returns>
        [HttpGet("Get5/{schoolName}/class/{classNo}")]
        public string Get5(string schoolName, [FromRoute(Name= "classNo")]string classNum)
        {
            return schoolName + " " + classNum;
        }

        /// <summary>
        /// GET api/AOPTest/Get6?Id=5&amp;Url=6&amp;PageId=7
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("Get6")]
        public string Get6([FromQuery]string pageNum, [FromQuery(Name = "pSize")] int pageSize)
        {
            return pageNum + " " + pageSize;
        }

        /// <summary>
        /// GET api/AOPTest/Get5?Id=5&amp;Url=6&amp;PageId=7
        /// </summary>
        /// <param name="schoolName"></param>
        /// <param name="classNum"></param>
        /// <returns></returns>
        [HttpGet("Get7/{schoolName}/class/{classNo}")]
        public string Get7(string schoolName, [FromRoute(Name = "classNo")] string classNum, [FromQuery] string pageNum, [FromQuery(Name = "pSize")] int pageSize)
        {
            return schoolName + " " + classNum + " " + pageNum + " " + pageSize;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ApiModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [FromRoute]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [FromQuery]
        public string Url { get; set; }
        /// <summary>
        /// Gets or sets the page identifier.
        /// </summary>
        /// <value>
        /// The page identifier.
        /// </value>
        [FromQuery]
        public int? PageId { get; set; }
    }

    /// <summary>
    /// IValuesService
    /// </summary>

    public interface IValuesService
    {
        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> FindAll();
        /// <summary>
        /// Finds the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        string Find(int id);
    }

    /// <summary>
    /// ValuesService
    /// </summary>
    /// <seealso cref="AIStudio.Api.Controllers.Test.IValuesService" />
    public class ValuesService : IValuesService
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<ValuesService> _logger;

        /// <summary>
        /// ValuesService
        /// </summary>
        /// <param name="logger">The logger.</param>
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
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string Find(int id)
        {
            _logger.LogDebug("{method} called with {id}", nameof(Find), id);

            return $"value{id}";
        }
    }
}
