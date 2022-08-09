// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using Microsoft.Web.WebView2.Core;

namespace BlazorMaui.Platforms.Windows;

internal interface IPermissionRequestHandler
{
    void OnPermissionRequested(CoreWebView2 sender, CoreWebView2PermissionRequestedEventArgs args);
}
