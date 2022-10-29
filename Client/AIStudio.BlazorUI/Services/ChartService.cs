using AIStudio.BlazorUI.Models.Charts;
using AIStudio.Util;
using System.Reflection;
namespace AIStudio.BlazorUI.Services
{
    public interface IChartService
    {
        Task<ChartDataItem[]> GetVisitDataAsync();
        Task<ChartDataItem[]> GetVisitData2Async();
        Task<ChartDataItem[]> GetSalesDataAsync();
        Task<RadarDataItem[]> GetRadarDataAsync();
    }

    public class ChartService : IChartService
    {
        private readonly HttpClient _httpClient;

        public ChartService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ChartDataItem[]> GetVisitDataAsync()
        {
            return (await GetChartDataAsync()).VisitData;
        }

        public async Task<ChartDataItem[]> GetVisitData2Async()
        {
            return (await GetChartDataAsync()).VisitData2;
        }

        public async Task<ChartDataItem[]> GetSalesDataAsync()
        {
            return (await GetChartDataAsync()).SalesData;
        }

        public async Task<RadarDataItem[]> GetRadarDataAsync()
        {
            return (await GetChartDataAsync()).RadarData;
        }

        private async Task<ChartData> GetChartDataAsync()
        {
            //return await _httpClient.GetFromJsonAsync<ChartData>("data/fake_chart_data.json");
            //return await _httpClient.GetFromJsonAsync<ChartData>("_content/AIStudio.BlazorUI/data/fake_chart_data.json");
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("AIStudio.BlazorUI.wwwroot.data.fake_chart_data.json");
            var str = await (new StreamReader(stream)).ReadToEndAsync();

            var data = str.ToObject<ChartData>();

            return data;
        }
    }
}
