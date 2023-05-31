using Microsoft.Web.WebView2.Core;
using System.IO;
using System.Windows;

namespace WpfWithWebview2
{

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            webView.NavigationStarting += EnsureHttps;
            //使用本机功能将消息从 Web 内容传递到主机
            InitializeAsync();
        }

        //初始化 CoreWebView2 后，注册要响应WebMessageReceived的事件处理程序。 在 MainWindow.xaml.cs中，使用以下代码更新 InitializeAsync 和添加 UpdateAddressBar
        async void InitializeAsync()
        {
            //设置web用户文件夹 
            var browserExecutableFolder = Path.Combine(Path.GetFullPath(".."), "WebView2.FixedVersionRuntime");
#if DEBUG
            browserExecutableFolder = @"C:\WebView2.FixedVersionRuntime";
#endif
            if (!Directory.Exists(browserExecutableFolder))
            {
                MessageBox.Show($"请先下载并解压缩 WebView2.FixedVersionRuntime.rar 到 {browserExecutableFolder} 文件夹", "提示");
                return;
            }
            webView.CreationProperties = new Microsoft.Web.WebView2.Wpf.CoreWebView2CreationProperties()
            {
                BrowserExecutableFolder = browserExecutableFolder
            };

            await webView.EnsureCoreWebView2Async(null);
            webView.CoreWebView2.WebMessageReceived += UpdateAddressBar;

            await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.postMessage(window.document.URL);");
            await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.addEventListener(\'message\', event => alert(event.data));");
        }

        void UpdateAddressBar(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            var uri = args.TryGetWebMessageAsString();
            addressBar.Text = uri;
            webView.CoreWebView2.PostWebMessageAsString(uri);
        }

        void EnsureHttps(object sender, CoreWebView2NavigationStartingEventArgs args)
        {
            var uri = args.Uri;
            if (!uri.StartsWith("https://"))
            {
                args.Cancel = true;
            }
        }

        private void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
            if (webView != null && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.Navigate(addressBar.Text);
            }
        }
    }
}
