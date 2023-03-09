using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Maui.Platform;
#if ANDROID
using Android.Webkit;
using AndroidX.Activity;
#elif WINDOWS
using BlazorMaui.Platforms.Windows;
using Microsoft.Web.WebView2.Core;
#endif

namespace BlazorMaui
{
    public partial class MainPage : ContentPage
    {
        protected string UploadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "uploads");

        public MainPage()
        {
            InitializeComponent();
             
            _blazorWebView.BlazorWebViewInitialized += BlazorWebViewInitialized;
            _blazorWebView.BlazorWebViewInitializing += BlazorWebViewInitializing;
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
            //attachment; filename=ndp48-web.exe; filename*=UTF-8''ndp48-web.exe
            //var file = e.ContentDisposition;
            Uri uri = new Uri(e.Url);
            string fileName = Path.GetFileName(uri.LocalPath);
            var httpClient = new HttpClient();
            var filePath = Path.Combine(UploadPath, fileName);
            byte[] fileBytes = await httpClient.GetByteArrayAsync(e.Url);
            File.WriteAllBytes(filePath, fileBytes);
            await DisplayAlert("提示", $"下载文件完成 {fileName}", "OK");
        }
#endif

    }
}
