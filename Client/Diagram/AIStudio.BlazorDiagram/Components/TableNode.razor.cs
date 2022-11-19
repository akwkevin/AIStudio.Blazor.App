using AIStudio.BlazorDiagram.Models;
using Microsoft.AspNetCore.Components;

namespace AIStudio.BlazorDiagram.Components
{
    public partial class TableNode
    {
        [Parameter]
        public Table Node { get; set; }
    }
}
