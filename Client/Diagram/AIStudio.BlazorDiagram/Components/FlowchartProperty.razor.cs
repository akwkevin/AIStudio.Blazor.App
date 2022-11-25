using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using AIStudio.BlazorDiagram.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace AIStudio.BlazorDiagram.Components
{
    public partial class FlowchartProperty : IDisposable
    {
        private FlowchartNodelModel Model;

        [CascadingParameter]
        public Diagram Diagram { get; set; }

        public void Dispose()
        {
            Diagram.SelectionChanged -= Diagram_SelectionChanged;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Diagram.SelectionChanged += Diagram_SelectionChanged;
        }

        private void Diagram_SelectionChanged(SelectableModel model)
        {
            if (model is FlowchartNodelModel flowchartNodelModel)
            {
                Model = model.Selected ? flowchartNodelModel : null;
                StateHasChanged();
            }
        }

        private void OnTitleChanged(ChangeEventArgs e)
        {
            if (Model == null)
                return;

            Model.Title = e.Value.ToString();
            Model.Refresh();
        }
    }
}
