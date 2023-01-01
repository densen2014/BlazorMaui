// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using LibraryShared;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using System;
using static Microsoft.Maui.ApplicationModel.Permissions;
using System.IO;
#if ANDROID
using AndroidX.Activity;
#elif WINDOWS
using BlazorMaui.Platforms.Windows;
#endif

namespace BlazorMauiLite
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            blazorWebView.BlazorWebViewInitialized += BlazorWebViewInitialized;
            blazorWebView.BlazorWebViewInitializing += BlazorWebViewInitializing;
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
#elif WINDOWS
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
    }
}