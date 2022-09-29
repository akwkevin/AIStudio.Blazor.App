using AIStudio.Blazor.UI;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AIStudio.BlazorWinform.Client
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            BlazorWebView blazorWebView1 = new BlazorWebView();
            blazorWebView1.Dock = DockStyle.Fill;
            this.Controls.Add(blazorWebView1);

            blazorWebView1.HostPage = "wwwroot\\index.html";
            blazorWebView1.Services = Program.ServiceCollection.BuildServiceProvider();
            blazorWebView1.RootComponents.Add<Main>("#app");
        }
    }
}