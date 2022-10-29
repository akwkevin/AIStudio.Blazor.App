using AIStudio.BlazorFlowchart.Models;
using Microsoft.AspNetCore.Components;

namespace AIStudio.BlazorFlowchart.Components
{
    public partial class TableNode
    {
        [Parameter]
        public Table Node { get; set; }
    }
}
