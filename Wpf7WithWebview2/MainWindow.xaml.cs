// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using Microsoft.Web.WebView2.Core;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Wpf7WithWebview2;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
 
        var FullScreenKeyBinding = new KeyBinding(ApplicationCommands.Help, new KeyGesture(Key.F12));
        var CloseKeyBinding = new KeyBinding(ApplicationCommands.Close, new KeyGesture(Key.F11));
        var LoginKeyBinding = new KeyBinding(ApplicationCommands.Open, new KeyGesture(Key.Enter));

        this.InputBindings.Add(FullScreenKeyBinding);
        this.InputBindings.Add(CloseKeyBinding);
        this.InputBindings.Add(LoginKeyBinding);
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

        webView.NavigationStarting += EnsureHttps;
        await webView.EnsureCoreWebView2Async(null);
        webView.CoreWebView2.WebMessageReceived += UpdateAddressBar;

        await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.postMessage(window.document.URL);");
        await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.addEventListener(\'message\', event => alert(event.data));");

        webView.CoreWebView2.DownloadStarting += CoreWebView2_DownloadStarting; 

        //指定下载保存位置
        webView.CoreWebView2.Profile.DefaultDownloadFolderPath = @"C:\mytargetdowloadpath";
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

    private void CoreWebView2_DownloadStarting(object sender, CoreWebView2DownloadStartingEventArgs e)
    {
        var downloadOperation = e.DownloadOperation;

        //指定下载后保存位置
        //e.ResultFilePath = @"C:\mytargetdowloadpath\mydownloadedfile.zip";
    }


    private void ButtonGo_Click(object sender, RoutedEventArgs e)
    {
        if (webView != null && webView.CoreWebView2 != null)
        {
            webView.CoreWebView2.Navigate(addressBar.Text);
        }
    }

    private void Open_Executed(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Login executed");
    }

    private void Close_Executed(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Close executed");
        this.Close();
    }

    private void Help_Executed(object sender, RoutedEventArgs e)
    {
        //MessageBox.Show("Full screen executed");
        this.WindowStyle = this.WindowStyle == WindowStyle.None? WindowStyle.ThreeDBorderWindow: WindowStyle.None;
        this.WindowState = this.WindowState == WindowState.Maximized?WindowState.Normal: WindowState.Maximized;

    }

}
