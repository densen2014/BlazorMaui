// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Maui.Platform;
using System.Text.RegularExpressions;
#if ANDROID
using Android.Webkit;
using AndroidX.Activity;
#elif WINDOWS
using Microsoft.Web.WebView2.Core;
#elif IOS || MACCATALYST
using Foundation;
using WebKit;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
#endif

namespace MauiApp_PdfReader;

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

        //Window secondWindow = new Window(new MainPage());
        //Application.Current.OpenWindow(secondWindow);
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
        var file = e.ContentDisposition;
        Uri uri = new Uri(e.Url);
        await DownloadAsync(uri, e.Mimetype);
    }
#endif

    private async Task DownloadAsync(string url,string? mimeType=null)
    {
        Uri uri = new Uri(url);
        await DownloadAsync(uri, mimeType);
    }

    private async Task DownloadAsync(Uri uri,string? mimeType=null)
    {
        string fileName = Path.GetFileName(uri.LocalPath);
        var httpClient = new HttpClient();
        var filePath = Path.Combine(UploadPath, fileName);
#if ANDROID
        if (uri.Scheme== "data")
        {
            //"data:text/html;base64,77u/PCFET0NUWVBFIGh0bWw+DQo8aHRtbCBsYW5nPSJlbiI+DQo8aGVhZD4NCiAgICA8bWV0YSBjaGFyc2V0PSJ1dGYtOCIgLz4NCiAgICA8bWV0YSBuYW1lPSJ2aWV3cG9ydCIgY29udGVudD0id2lkdGg9ZGV2aWNlLXdpZHRoLCBpbml0aWFsLXNjYWxlPTEuMCwgbWF4aW11bS1zY2FsZT0xLjAsIHVzZXItc2NhbGFibGU9bm8sIHZpZXdwb3J0LWZpdD1jb3ZlciIgLz4NCiAgICA8dGl0bGU+UGRmUmVhZGVyPC90aXRsZT4NCiAgICA8YmFzZSBocmVmPSIvIiAvPg0KICAgIDxsaW5rIHJlbD0ic3R5bGVzaGVldCIgaHJlZj0iY3NzL2Jvb3RzdHJhcC9ib290c3RyYXAubWluLmNzcyIgLz4NCiAgICA8bGluayBocmVmPSJjc3MvYXBwLmNzcyIgcmVsPSJzdHlsZXNoZWV0IiAvPg0KICAgIDxsaW5rIGhyZWY9Ik1hdWlBcHBfUGRmUmVhZGVyLnN0eWxlcy5jc3MiIHJlbD0ic3R5bGVzaGVldCIgLz4NCjwvaGVhZD4NCg0KPGJvZHk+DQoNCiAgICA8ZGl2IGNsYXNzPSJzdGF0dXMtYmFyLXNhZmUtYXJlYSI+PC9kaXY+DQoNCiAgICA8ZGl2IGlkPSJhcHAiPkxvYWRpbmcuLi48L2Rpdj4NCg0KICAgIDxkaXYgaWQ9ImJsYXpvci1lcnJvci11aSI+DQogICAgICAgIEFuIHVuaGFuZGxlZCBlcnJvciBoYXMgb2NjdXJyZWQuDQogICAgICAgIDxhIGhyZWY9IiIgY2xhc3M9InJlbG9hZCI+UmVsb2FkPC9hPg0KICAgICAgICA8YSBjbGFzcz0iZGlzbWlzcyI+8J+XmTwvYT4NCiAgICA8L2Rpdj4NCg0KICAgIDxzY3JpcHQgc3JjPSJfZnJhbWV3b3JrL2JsYXpvci53ZWJ2aWV3LmpzIiBhdXRvc3RhcnQ9ImZhbHNlIj48L3NjcmlwdD4NCg0KPC9ib2R5Pg0KDQo8L2h0bWw+"

            fileName =DataUrl2Filename( uri.OriginalString);
            filePath = Path.Combine(UploadPath,$"{DateTime.Now.ToString("yyyy-MM-dd-hhmmss")}-{fileName}");
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

    /// <summary>
    /// 从 DataUrl 转换为 Stream
    /// <para>Convert from a DataUrl to an Stream</para>
    /// </summary>
    /// <param name="base64encodedstring"></param>
    /// <returns></returns>
    public static Stream DataUrl2Stream(string base64encodedstring)
    {
        var base64Data = Regex.Match(base64encodedstring, @"data:text/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
        var bytes = Convert.FromBase64String(base64Data);
        var stream = new MemoryStream(bytes);
        return stream;
    }


    /// <summary>
    /// 从 base64 转换为 Stream
    /// <para>Convert from a base64 to an Stream</para>
    /// </summary>
    /// <param name="base64encodedstring"></param>
    /// <returns></returns>
    public static Stream Base642Stream(string base64encodedstring)
    {
        var bytes = Convert.FromBase64String(base64encodedstring);
        var stream = new MemoryStream(bytes);
        return stream;
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
