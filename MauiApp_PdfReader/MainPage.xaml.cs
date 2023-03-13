// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using Foundation;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
#if ANDROID
using Android.Webkit;
using AndroidX.Activity;
#elif WINDOWS
using Microsoft.Web.WebView2.Core;
#elif IOS || MACCATALYST
using WebKit;
using static System.Runtime.InteropServices.JavaScript.JSType;
#endif

namespace MauiApp_PdfReader
{
    public partial class MainPage : ContentPage
    {
        protected string UploadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "uploads");

        public MainPage()
        {
            InitializeComponent();

            blazorWebView.BlazorWebViewInitialized += BlazorWebViewInitialized;
            blazorWebView.BlazorWebViewInitializing += BlazorWebViewInitializing;

            blazorWebView.UrlLoading +=
            (sender, urlLoadingEventArgs) =>
            {
                if (urlLoadingEventArgs.Url.Host != "0.0.0.0")
                {
                    //外部链接WebView内打开,例如pdf浏览器
                    Console.WriteLine(urlLoadingEventArgs.Url);
                    urlLoadingEventArgs.UrlLoadingStrategy =
                        UrlLoadingStrategy.OpenInWebView;

                    //拦截可处理 IOS || MACCATALYST 下载文件, 简单测试一下
                    if (urlLoadingEventArgs.Url.ToString().EndsWith(".exe"))
                    {
                        Task.Run(async () => await DownloadAsync(urlLoadingEventArgs.Url));
                    }
                }
            };

        }

        private void BlazorWebViewInitialized(object? sender, BlazorWebViewInitializedEventArgs e)
        {

#if ANDROID

            if (e.WebView.Context?.GetActivity() is not ComponentActivity activity)
            {
                throw new InvalidOperationException($"The permission-managing WebChromeClient requires that the current activity be a '{nameof(ComponentActivity)}'.");
            }

            e.WebView.Settings.JavaScriptEnabled = true;
            e.WebView.Settings.AllowFileAccess = true;
            e.WebView.Settings.MediaPlaybackRequiresUserGesture = false;
            e.WebView.Settings.SetGeolocationEnabled(true);
            e.WebView.Settings.SetGeolocationDatabasePath(e.WebView.Context?.FilesDir?.Path);
            e.WebView.Download +=(async (s,e)=> await WebView_DownloadAsync(s,e));
            //e.WebView.SetWebChromeClient(new PermissionManagingBlazorWebChromeClient(e.WebView.WebChromeClient!, activity));
#elif WINDOWS
            e.WebView.CoreWebView2.DownloadStarting += (async (s, e) => await CoreWebView2_DownloadStartingAsync(s, e));
#elif IOS || MACCATALYST
            e.WebView.NavigationDelegate = new NavigationDelegate();
#endif

        }


#if WINDOWS
        private async Task CoreWebView2_DownloadStartingAsync(object sender, CoreWebView2DownloadStartingEventArgs e)
        {
            var downloadOperation = e.DownloadOperation;
            string fileName = Path.GetFileName(e.ResultFilePath);
            var filePath = Path.Combine(UploadPath, fileName);

            //指定下载保存位置
            e.ResultFilePath = filePath;
            await DisplayAlert("提示", $"下载文件完成 {fileName}", "OK");
        }
#endif


#if ANDROID
        private async Task WebView_DownloadAsync(object sender, DownloadEventArgs e)
        {
            //attachment; filename=ndp48-web.exe; filename*=UTF-8''ndp48-web.exe
            //var file = e.ContentDisposition;
            Uri uri = new Uri(e.Url);
            await DownloadAsync(e.Url);
        }
#endif

        private async Task DownloadAsync(string url)
        {
            Uri uri = new Uri(url);
            await DownloadAsync(uri);
        }

        private async Task DownloadAsync(Uri uri)
        {
            string fileName = Path.GetFileName(uri.LocalPath);
            var httpClient = new HttpClient();
            var filePath = Path.Combine(UploadPath, fileName);
            byte[] fileBytes = await httpClient.GetByteArrayAsync(uri);
            File.WriteAllBytes(filePath, fileBytes);
            await DisplayAlert("提示", $"下载文件完成 {fileName}", "OK");
        }

        private void BlazorWebViewInitializing(object? sender, BlazorWebViewInitializingEventArgs e)
        {
#if IOS || MACCATALYST
            e.Configuration.AllowsInlineMediaPlayback = true;
            e.Configuration.MediaTypesRequiringUserActionForPlayback = WebKit.WKAudiovisualMediaTypes.None;
#endif
        }


#if IOS || MACCATALYST


        public class NavigationDelegate : NSObject, IWKNavigationDelegate
        {

            [Export("webView:didFinishNavigation:")]

            public async void DidFinishNavigation(WKWebView webView, WKNavigation navigation)

            {

                var content = await webView.EvaluateJavaScriptAsync("(function() { return (''+document.getElementsByTagName('html')[0].innerHTML+''); })();");

                var html = FromObject(content);

                Console.WriteLine((html.ToString()).Substring(0, 40));

            }

        }

#endif
    }
}