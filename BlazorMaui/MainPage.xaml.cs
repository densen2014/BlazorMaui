using LibraryShared;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using System;
using static Microsoft.Maui.ApplicationModel.Permissions;
#if ANDROID
using AndroidX.Activity;
#endif

namespace BlazorMaui
{
    public partial class MainPage : ContentPage
    {
        TestService TestService=new TestService();
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
#endif
        }

        private void BlazorWebViewInitializing(object? sender, BlazorWebViewInitializingEventArgs e)
        {
#if IOS || MACCATALYST                   
            e.Configuration.AllowsInlineMediaPlayback = true;
            e.Configuration.MediaTypesRequiringUserActionForPlayback = WebKit.WKAudiovisualMediaTypes.None;
#endif
        }
    }
}
