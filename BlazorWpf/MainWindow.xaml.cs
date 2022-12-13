using BlazorShared;
using Microsoft.AspNetCore.Components.WebView.Wpf;
using Microsoft.Web.WebView2.Wpf;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows;

namespace BlazorWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Resources.Add("services", Startup.Services);
            InitializeComponent();

            ////[无依赖发布webview2程序] 固定版本运行时环境的方式来实现加载网页
            ////设置web用户文件夹 
            //var browserExecutableFolder = "c:\\wb2";
            //var userData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "BlazorWinFormsApp");
            //Directory.CreateDirectory(userData);
            //var creationProperties = new CoreWebView2CreationProperties()
            //{
            //    UserDataFolder = userData,
            //    BrowserExecutableFolder = browserExecutableFolder
            //};
            //mainBlazorWebView.WebView.CreationProperties = creationProperties;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
              owner: this,
              messageBoxText: $"Current counter value is: {Startup._appState.Counter}",
              caption: "Counter");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainBlazorWebView.WebView.CoreWebView2.ExecuteScriptAsync("showAlert()");
        }
    }
}
