using Microsoft.AspNetCore.Components;

namespace AIStudio.BlazorUI.Pages.Home.Analysis
{
    public partial class Field
    {
        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public string Value { get; set; }
    }
}