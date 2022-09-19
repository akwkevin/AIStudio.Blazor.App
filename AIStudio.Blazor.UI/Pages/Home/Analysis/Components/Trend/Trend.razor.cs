using Microsoft.AspNetCore.Components;

namespace AIStudio.Blazor.UI.Pages.Home.Analysis
{
    public partial class Trend
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Flag { get; set; }
    }
}