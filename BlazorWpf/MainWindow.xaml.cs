using BlazorShared;
using Microsoft.AspNetCore.Components.WebView.Wpf;
using Microsoft.Web.WebView2.Core;
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
        protected string UploadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "uploads");

        public MainWindow()
        {
            Resources.Add("services", Startup.Services);
            InitializeComponent();
            blazorWebView.BlazorWebViewInitialized += BlazorWebViewInitialized;

        }

        void BlazorWebViewInitialized(object sender, EventArgs e)
        {
            //下载开始时引发 DownloadStarting，阻止默认下载
            blazorWebView.WebView.CoreWebView2.DownloadStarting += CoreWebView2_DownloadStarting;

            //指定下载保存位置
            blazorWebView.WebView.CoreWebView2.Profile.DefaultDownloadFolderPath = UploadPath;

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

        private void CoreWebView2_DownloadStarting(object sender, CoreWebView2DownloadStartingEventArgs e)
        {
            var downloadOperation = e.DownloadOperation;
            string fileName = Path.GetFileName(e.ResultFilePath);
            var filePath = Path.Combine(UploadPath, fileName);

            //指定下载保存位置
            e.ResultFilePath = filePath;
            MessageBox.Show( $"下载文件完成 {fileName}", "提示");
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
            blazorWebView.WebView.CoreWebView2.ExecuteScriptAsync("showAlert()");
        }
    }
}
