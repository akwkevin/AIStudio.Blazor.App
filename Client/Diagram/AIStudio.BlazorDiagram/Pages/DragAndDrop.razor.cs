using AIStudio.BlazorDiagram.Components;
using AIStudio.BlazorDiagram.Models;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace AIStudio.BlazorDiagram.Pages
{
    public partial class DragAndDrop
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        private readonly Diagram Diagram = new Diagram();
        private string? _draggedType;
        private string JsonString { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();


            Diagram.RegisterModelComponent<BotAnswerNodeModel, BotAnswerWidget>();
        }

        private void OnDragStart(string key)
        {
            // Can also use transferData, but this is probably "faster"
            _draggedType = key;
        }

        private void OnDrop(DragEventArgs e)
        {
            if (_draggedType == null) // Unkown item
                return;

            var position = Diagram.GetRelativeMousePoint(e.ClientX, e.ClientY);
            var node = _draggedType == "0" ? new NodeModel(position) : new BotAnswerNodeModel(position);
            node.AddPort(PortAlignment.Top);
            node.AddPort(PortAlignment.Bottom);
            Diagram.Nodes.Add(node);
            _draggedType = null;
        }

        private async Task ShowJson()
        {
            JsonString = DiagramHelper.ToJson(Diagram);
            await JSRuntime.InvokeVoidAsync("console.log", JsonString);
        }

        private void LoadJson()
        {
            DiagramHelper.ToObject(Diagram, JsonString);
        }
    }
}
