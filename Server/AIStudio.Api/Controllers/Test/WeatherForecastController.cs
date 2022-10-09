using AIStudio.Common.Swagger;
using Microsoft.AspNetCore.Mvc;

namespace AIStudio.Api.Controllers.Test
{
    /// <summary>
    /// 工程自带Api
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.Test))]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 获取WeatherForecast
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }

    /// <summary>
    /// 天气预报
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 温度C
        /// </summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// 温度F
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>
        /// 汇总
        /// </summary>
        public string? Summary { get; set; }
    }
}