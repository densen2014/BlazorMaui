// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;

namespace BlazorMaui.Platforms.Windows;

internal class DialogPermissionRequestHandler : IPermissionRequestHandler
{
    private readonly UIElement _parentElement;
    private readonly Dictionary<CoreWebView2PermissionKind, CoreWebView2PermissionState> _cachedPermissions = new();

    public DialogPermissionRequestHandler(UIElement parentElement)
    {
        _parentElement = parentElement;
    }

    public async void OnPermissionRequested(CoreWebView2 sender, CoreWebView2PermissionRequestedEventArgs args)
    {
        args.Handled = true;

        if (_cachedPermissions.TryGetValue(args.PermissionKind, out var permissionState) && permissionState == CoreWebView2PermissionState.Allow)
        {
            args.State = CoreWebView2PermissionState.Allow;
            return;
        }

        var deferral = args.GetDeferral();

        var dialog = new ContentDialog
        {
            XamlRoot = _parentElement.XamlRoot,
            Title = "Permission request",
            Content = $"{args.Uri} is requesting access to {GetPermissionName(args.PermissionKind)}",
            PrimaryButtonText = "Allow",
            SecondaryButtonText = "Deny",
        };

        var result = await dialog.ShowAsync();

        args.State = result == ContentDialogResult.Primary ?
            CoreWebView2PermissionState.Allow :
            CoreWebView2PermissionState.Deny;

        _cachedPermissions[args.PermissionKind] = args.State;

        deferral.Complete();
    }

    private static string GetPermissionName(CoreWebView2PermissionKind permissionKind)
        => permissionKind switch
        {
            CoreWebView2PermissionKind.Microphone => "your microphone",
            CoreWebView2PermissionKind.Camera => "your camera",
            CoreWebView2PermissionKind.Geolocation => "your location",
            CoreWebView2PermissionKind.Notifications => "your notifications",
            CoreWebView2PermissionKind.OtherSensors => "this device's generic sensors",
            CoreWebView2PermissionKind.ClipboardRead => "your clipboard",
            _ => "unknown resources",
        };
}
