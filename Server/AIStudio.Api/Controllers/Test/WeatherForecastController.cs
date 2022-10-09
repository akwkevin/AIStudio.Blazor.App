using AIStudio.Common.Swagger;
using Microsoft.AspNetCore.Mvc;

namespace AIStudio.Api.Controllers.Test
{
    /// <summary>
    /// �����Դ�Api
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
        /// ��ȡWeatherForecast
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
    /// ����Ԥ��
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// ����
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// �¶�C
        /// </summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// �¶�F
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>
        /// ����
        /// </summary>
        public string? Summary { get; set; }
    }
}