using AIStudio.Blazor.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AIStudio.BlazorWpf.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //var serviceCollection = new ServiceCollection();
            //serviceCollection.AddWpfBlazorWebView();
            //Resources.Add("services", serviceCollection.BuildServiceProvider());

            //serviceCollection.AddHttpClient();

            //var config = new ConfigurationBuilder().AddJsonFile("wwwroot/appsettings.json").Build();

            ////builder.Configuration.AddConfiguration(config);
            //serviceCollection.AddServices(null);    // 第2外：添加扩展方法引入服务
        }
    }
}
