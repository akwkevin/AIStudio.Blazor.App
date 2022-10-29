using AIStudio.BlazorFlowchart.Components;
using AIStudio.BlazorFlowchart.Models;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Reflection;

namespace AIStudio.BlazorFlowchart.Pages
{

    public partial class Index : IDisposable
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        public Diagram Diagram { get; } = new Diagram(new DiagramOptions
        {
            GridSize = 40,
            AllowMultiSelection = false,
            Links = new DiagramLinkOptions
            {
                Factory = (diagram, sourcePort) =>
                {
                    return new LinkModel(sourcePort, null)
                    {
                        Router = Routers.Orthogonal,
                        PathGenerator = PathGenerators.Straight,
                    };
                }
            }
        });


        public void Dispose()
        {
            Diagram.Links.Added -= OnLinkAdded;
            Diagram.Links.Removed -= Diagram_LinkRemoved;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();            

            Diagram.RegisterModelComponent<Table, TableNode>();
            Diagram.Nodes.Add(new Table());

            Diagram.Links.Added += OnLinkAdded;
            Diagram.Links.Removed += Diagram_LinkRemoved;
        }

        ///// <summary>
        ///// 生命周期事件-渲染后
        ///// </summary>
        ///// <param name="firstRender"></param>
        ///// <returns></returns>
        //protected override async void OnAfterRender(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        try
        //        {
        //            var assembly = Assembly.GetExecutingAssembly();
        //            var stream = assembly.GetManifestResourceStream("AIStudio.BlazorFlowchart.wwwroot.script.min.js");
        //            string scriptContent = "";
        //            using (var reader = new System.IO.StreamReader(stream))
        //            {
        //                scriptContent = reader.ReadToEnd();
        //            }

        //            var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "data:text/javascript;base64," + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(scriptContent)));
        //            await module.InvokeVoidAsync("Initialize");
        //            //var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./script.min.js");
        //        }
        //        catch (Exception ex)
        //        {

        //        }

        //    }
        //    base.OnAfterRender(firstRender);
        //}

        private void OnLinkAdded(BaseLinkModel link)
        {
            link.TargetPortChanged += OnLinkTargetPortChanged;
        }

        private void OnLinkTargetPortChanged(BaseLinkModel link, PortModel oldPort, PortModel newPort)
        {
            link.Labels.Add(new LinkLabelModel(link, "1..*", -40, new Point(0, -30)));
            link.Refresh();

            ((newPort ?? oldPort) as ColumnPort).Column.Refresh();
        }

        private void Diagram_LinkRemoved(BaseLinkModel link)
        {
            link.TargetPortChanged -= OnLinkTargetPortChanged;

            if (!link.IsAttached)
                return;

            var sourceCol = (link.SourcePort as ColumnPort).Column;
            var targetCol = (link.TargetPort as ColumnPort).Column;
            (sourceCol.Primary ? targetCol : sourceCol).Refresh();
        }

        private void NewTable()
        {
            Diagram.Nodes.Add(new Table());
        }

        private async Task ShowJson()
        {
            var json = JsonConvert.SerializeObject(new
            {
                Nodes = Diagram.Nodes.Cast<object>(),
                Links = Diagram.Links.Cast<object>()
            }, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            await JSRuntime.InvokeVoidAsync("console.log", json);
        }

        private void Debug()
        {
            Console.WriteLine(Diagram.Container);
            foreach (var port in Diagram.Nodes.ToList()[0].Ports)
                Console.WriteLine(port.Position);
        }
    }
}
