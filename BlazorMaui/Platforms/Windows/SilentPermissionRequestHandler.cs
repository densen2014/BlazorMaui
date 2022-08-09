// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using BlazorMaui.Platforms.Windows;
using Microsoft.Web.WebView2.Core;
using System;

namespace BlazorMaui.Platforms.Windows;

internal class SilentPermissionRequestHandler : IPermissionRequestHandler
{
    private static readonly Uri BaseUri = new("https://0.0.0.0");

    public void OnPermissionRequested(CoreWebView2 sender, CoreWebView2PermissionRequestedEventArgs args)
    {
        args.State = Uri.TryCreate(args.Uri, UriKind.RelativeOrAbsolute, out var uri) && BaseUri.IsBaseOf(uri) ?
            CoreWebView2PermissionState.Allow :
            CoreWebView2PermissionState.Deny;
        args.Handled = true;
    }
}
