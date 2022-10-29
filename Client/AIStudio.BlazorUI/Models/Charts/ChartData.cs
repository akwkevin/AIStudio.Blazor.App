using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIStudio.BlazorUI.Pages.Home.Analysis.Components.Charts;

namespace AIStudio.BlazorUI.Models.Charts
{
    public class ChartData
    {
        public ChartDataItem[] VisitData { get; set; }
        public ChartDataItem[] VisitData2 { get; set; }
        public ChartDataItem[] SalesData { get; set; }
        public SearchDataItem[] SearchData { get; set; }
        public OfflineDataItem[] OfflineData { get; set; }
        public OfflineChartDataItem[] OfflineChartData { get; set; }
        public ChartDataItem[] SalesTypeData { get; set; }
        public ChartDataItem[] SalesTypeDataOnline { get; set; }
        public ChartDataItem[] SalesTypeDataOffline { get; set; }
        public RadarDataItem[] RadarData { get; set; }
    }
}
