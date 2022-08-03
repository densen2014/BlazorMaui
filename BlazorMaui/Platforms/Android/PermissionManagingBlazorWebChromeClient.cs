using Android;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Webkit;
using AndroidX.Activity;
using AndroidX.Activity.Result;
using AndroidX.Activity.Result.Contract;
using AndroidX.Core.Content;
using Java.Interop;
using System;
using System.Collections.Generic;
using View = Android.Views.View;
using WebView = Android.Webkit.WebView;

namespace BlazorMaui;

internal class PermissionManagingBlazorWebChromeClient : WebChromeClient, IActivityResultCallback
{
    // This class implements a permission requesting workflow that matches workflow recommended
    // by the official Android developer documentation.
    // See: https://developer.android.com/training/permissions/requesting#workflow_for_requesting_permissions
    // The current implementation supports location, camera, and microphone permissions. To add your own,
    // update the s_rationalesByPermission dictionary to include your rationale for requiring the permission.
    // If necessary, you may need to also update s_requiredPermissionsByWebkitResource to define how a specific
    // Webkit resource maps to an Android permission.

    // In a real app, you would probably use more convincing rationales tailored toward what your app does.
    private const string CameraAccessRationale = "This app requires access to your camera. Please grant access to your camera when requested.";
    private const string LocationAccessRationale = "This app requires access to your location. Please grant access to your precise location when requested.";
    private const string MicrophoneAccessRationale = "This app requires access to your microphone. Please grant access to your microphone when requested.";

    private static readonly Dictionary<string, string> s_rationalesByPermission = new()
    {
        [Manifest.Permission.Camera] = CameraAccessRationale,
        [Manifest.Permission.AccessFineLocation] = LocationAccessRationale,
        [Manifest.Permission.RecordAudio] = MicrophoneAccessRationale,
        // Add more rationales as you add more supported permissions.
    };

    private static readonly Dictionary<string, string[]> s_requiredPermissionsByWebkitResource = new()
    {
        [PermissionRequest.ResourceVideoCapture] = new[] { Manifest.Permission.Camera },
        [PermissionRequest.ResourceAudioCapture] = new[] { Manifest.Permission.ModifyAudioSettings, Manifest.Permission.RecordAudio },
        // Add more Webkit resource -> Android permission mappings as needed.
    };

    private readonly WebChromeClient _blazorWebChromeClient;
    private readonly ComponentActivity _activity;
    private readonly ActivityResultLauncher _requestPermissionLauncher;

    private Action<bool>? _pendingPermissionRequestCallback;

    public PermissionManagingBlazorWebChromeClient(WebChromeClient blazorWebChromeClient, ComponentActivity activity)
    {
        _blazorWebChromeClient = blazorWebChromeClient;
        _activity = activity;
        _requestPermissionLauncher = _activity.RegisterForActivityResult(new ActivityResultContracts.RequestPermission(), this);
    }

    public override void OnCloseWindow(Android.Webkit.WebView? window)
    {
        _blazorWebChromeClient.OnCloseWindow(window);
        _requestPermissionLauncher.Unregister();
    }

    public override void OnGeolocationPermissionsShowPrompt(string? origin, GeolocationPermissions.ICallback? callback)
    {
        ArgumentNullException.ThrowIfNull(callback, nameof(callback));

        RequestPermission(Manifest.Permission.AccessFineLocation, isGranted => callback.Invoke(origin, isGranted, false));
    }

    public override void OnPermissionRequest(PermissionRequest? request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        if (request.GetResources() is not { } requestedResources)
        {
            request.Deny();
            return;
        }

        RequestAllResources(requestedResources, grantedResources =>
        {
            if (grantedResources.Count == 0)
            {
                request.Deny();
            }
            else
            {
                request.Grant(grantedResources.ToArray());
            }
        });
    }

    private void RequestAllResources(Memory<string> requestedResources, Action<List<string>> callback)
    {
        if (requestedResources.Length == 0)
        {
            // No resources to request - invoke the callback with an empty list.
            callback(new());
            return;
        }

        var currentResource = requestedResources.Span[0];
        var requiredPermissions = s_requiredPermissionsByWebkitResource.GetValueOrDefault(currentResource, Array.Empty<string>());

        RequestAllPermissions(requiredPermissions, isGranted =>
        {
            // Recurse with the remaining resources. If the first resource was granted, use a modified callback
            // that adds the first resource to the granted resources list.
            RequestAllResources(requestedResources[1..], !isGranted ? callback : grantedResources =>
            {
                grantedResources.Add(currentResource);
                callback(grantedResources);
            });
        });
    }

    private void RequestAllPermissions(Memory<string> requiredPermissions, Action<bool> callback)
    {
        if (requiredPermissions.Length == 0)
        {
            // No permissions left to request - success!
            callback(true);
            return;
        }

        RequestPermission(requiredPermissions.Span[0], isGranted =>
        {
            if (isGranted)
            {
                // Recurse with the remaining permissions.
                RequestAllPermissions(requiredPermissions[1..], callback);
            }
            else
            {
                // The first required permission was not granted. Fail now and don't attempt to grant
                // the remaining permissions.
                callback(false);
            }
        });
    }

    private void RequestPermission(string permission, Action<bool> callback)
    {
        // This method implements the workflow described here:
        // https://developer.android.com/training/permissions/requesting#workflow_for_requesting_permissions

        if (ContextCompat.CheckSelfPermission(_activity, permission) == Permission.Granted)
        {
            callback.Invoke(true);
        }
        else if (_activity.ShouldShowRequestPermissionRationale(permission) && s_rationalesByPermission.TryGetValue(permission, out var rationale))
        {
            new AlertDialog.Builder(_activity)
                .SetTitle("Enable app permissions")!
                .SetMessage(rationale)!
                .SetNegativeButton("No thanks", (_, _) => callback(false))!
                .SetPositiveButton("Continue", (_, _) => LaunchPermissionRequestActivity(permission, callback))!
                .Show();
        }
        else
        {
            LaunchPermissionRequestActivity(permission, callback);
        }
    }

    private void LaunchPermissionRequestActivity(string permission, Action<bool> callback)
    {
        if (_pendingPermissionRequestCallback is not null)
        {
            throw new InvalidOperationException("Cannot perform multiple permission requests simultaneously.");
        }

        _pendingPermissionRequestCallback = callback;
        _requestPermissionLauncher.Launch(permission);
    }

    void IActivityResultCallback.OnActivityResult(Java.Lang.Object isGranted)
    {
        var callback = _pendingPermissionRequestCallback;
        _pendingPermissionRequestCallback = null;
        callback?.Invoke((bool)isGranted);
    }

    #region Unremarkable overrides
    // See: https://github.com/dotnet/maui/issues/6565
    public override JniPeerMembers JniPeerMembers => _blazorWebChromeClient.JniPeerMembers;
    public override Bitmap? DefaultVideoPoster => _blazorWebChromeClient.DefaultVideoPoster;
    public override Android.Views.View? VideoLoadingProgressView => _blazorWebChromeClient.VideoLoadingProgressView;
    public override void GetVisitedHistory(IValueCallback? callback)
        => _blazorWebChromeClient.GetVisitedHistory(callback);
    public override bool OnConsoleMessage(ConsoleMessage? consoleMessage)
        => _blazorWebChromeClient.OnConsoleMessage(consoleMessage);
    public override bool OnCreateWindow(WebView? view, bool isDialog, bool isUserGesture, Message? resultMsg)
        => _blazorWebChromeClient.OnCreateWindow(view, isDialog, isUserGesture, resultMsg);
    public override void OnGeolocationPermissionsHidePrompt()
        => _blazorWebChromeClient.OnGeolocationPermissionsHidePrompt();
    public override void OnHideCustomView()
        => _blazorWebChromeClient.OnHideCustomView();
    public override bool OnJsAlert(WebView? view, string? url, string? message, JsResult? result)
        => _blazorWebChromeClient.OnJsAlert(view, url, message, result);
    public override bool OnJsBeforeUnload(WebView? view, string? url, string? message, JsResult? result)
        => _blazorWebChromeClient.OnJsBeforeUnload(view, url, message, result);
    public override bool OnJsConfirm(WebView? view, string? url, string? message, JsResult? result)
        => _blazorWebChromeClient.OnJsConfirm(view, url, message, result);
    public override bool OnJsPrompt(WebView? view, string? url, string? message, string? defaultValue, JsPromptResult? result)
        => _blazorWebChromeClient.OnJsPrompt(view, url, message, defaultValue, result);
    public override void OnPermissionRequestCanceled(PermissionRequest? request)
        => _blazorWebChromeClient.OnPermissionRequestCanceled(request);
    public override void OnProgressChanged(WebView? view, int newProgress)
        => _blazorWebChromeClient.OnProgressChanged(view, newProgress);
    public override void OnReceivedIcon(WebView? view, Bitmap? icon)
        => _blazorWebChromeClient.OnReceivedIcon(view, icon);
    public override void OnReceivedTitle(WebView? view, string? title)
        => _blazorWebChromeClient.OnReceivedTitle(view, title);
    public override void OnReceivedTouchIconUrl(WebView? view, string? url, bool precomposed)
        => _blazorWebChromeClient.OnReceivedTouchIconUrl(view, url, precomposed);
    public override void OnRequestFocus(WebView? view)
        => _blazorWebChromeClient.OnRequestFocus(view);
    public override void OnShowCustomView(View? view, ICustomViewCallback? callback)
        => _blazorWebChromeClient.OnShowCustomView(view, callback);
    public override bool OnShowFileChooser(WebView? webView, IValueCallback? filePathCallback, FileChooserParams? fileChooserParams)
        => _blazorWebChromeClient.OnShowFileChooser(webView, filePathCallback, fileChooserParams);
    #endregion
}
