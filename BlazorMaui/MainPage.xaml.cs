using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Maui.Platform;
using Microsoft.AspNetCore.Components.WebView.Maui;
using System.Text.RegularExpressions;
#if ANDROID
using Android.Webkit;
using AndroidX.Activity;
#elif WINDOWS
using BlazorMaui.Platforms.Windows;
using Microsoft.Web.WebView2.Core;
#elif IOS || MACCATALYST
using Foundation;
using WebKit;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
#endif

namespace BlazorMaui
{

#if IOS || MACCATALYST
//在 iPadOS 和 Mac Catalyst 上使用多窗口支持
[Register("SceneDelegate")]
#endif
    public partial class MainPage : ContentPage
    {
        protected string UploadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "uploads");

        public MainPage()
        {
            InitializeComponent();
             
            _blazorWebView.BlazorWebViewInitialized += BlazorWebViewInitialized;
            _blazorWebView.BlazorWebViewInitializing += BlazorWebViewInitializing;

            _blazorWebView.UrlLoading +=
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
            e.WebView.SetWebChromeClient(new PermissionManagingBlazorWebChromeClient(e.WebView.WebChromeClient!, activity));
            e.WebView.Download += (async (s, e) => await WebView_DownloadAsync(s, e));
#elif WINDOWS
             e.WebView.CoreWebView2.DownloadStarting += (async (s, e) => await CoreWebView2_DownloadStartingAsync(s, e)); 
           var permissionHandler =
#if HANDLE_WEBVIEW2_PERMISSIONS_SILENTLY
            new SilentPermissionRequestHandler();
#else
                new DialogPermissionRequestHandler(e.WebView);
#endif

            e.WebView.CoreWebView2.PermissionRequested += permissionHandler.OnPermissionRequested;
#elif IOS
        //e.Configuration.AllowsInlineMediaPlayback = true;
        //e.Configuration.MediaTypesRequiringUserActionForPlayback = WebKit.WKAudiovisualMediaTypes.None;
#endif
        }

        private void BlazorWebViewInitializing(object? sender, BlazorWebViewInitializingEventArgs e)
        {
#if IOS || MACCATALYST                   
            e.Configuration.AllowsInlineMediaPlayback = true;
            e.Configuration.MediaTypesRequiringUserActionForPlayback = WebKit.WKAudiovisualMediaTypes.None;
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
            Uri uri = new Uri(e.Url);
            await DownloadAsync(uri, e.Mimetype);
        }
#endif

        private async Task DownloadAsync(string url, string? mimeType = null)
        {
            Uri uri = new Uri(url);
            await DownloadAsync(uri, mimeType);
        }

        private async Task DownloadAsync(Uri uri, string? mimeType = null)
        {
            string fileName = Path.GetFileName(uri.LocalPath);
            var httpClient = new HttpClient();
            var filePath = Path.Combine(UploadPath, fileName);
#if ANDROID
            if (uri.Scheme == "data")
            {
                fileName = DataUrl2Filename(uri.OriginalString);
                filePath = Path.Combine(UploadPath, $"{DateTime.Now.ToString("yyyy-MM-dd-hhmmss")}-{fileName}");
                var bytes = DataUrl2Bytes(uri.OriginalString);
                File.WriteAllBytes(filePath, bytes);
                await DisplayAlert("提示", $"下载文件完成 {fileName}", "OK");
                return;
            }
#endif
            byte[] fileBytes = await httpClient.GetByteArrayAsync(uri);
            File.WriteAllBytes(filePath, fileBytes);
            await DisplayAlert("提示", $"下载文件完成 {fileName}", "OK");
        }

        public static string DataUrl2Filename(string base64encodedstring)
        {
            var filename = Regex.Match(base64encodedstring, @"data:text/(?<filename>.+?);(?<type2>.+?),(?<data>.+)").Groups["filename"].Value;
            return filename;
        }

        /// <summary>
        /// 从 DataUrl 转换为 Stream
        /// <para>Convert from a DataUrl to an Stream</para>
        /// </summary>
        /// <param name="base64encodedstring"></param>
        /// <returns></returns>
        public static byte[] DataUrl2Bytes(string base64encodedstring)
        {
            var base64Data = Regex.Match(base64encodedstring, @"data:text/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
            var bytes = Convert.FromBase64String(base64Data);
            return bytes;
        }
    }
}
