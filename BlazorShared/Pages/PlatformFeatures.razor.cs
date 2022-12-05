// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using BlazorShared.Services;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace BlazorShared.Pages;

public partial class PlatformFeatures
{
    [Inject, NotNull] protected ITools? Tools { get; set; }

    private string? Locations;
    private string? PhotoFilename;
    private string? 定位权限;
    private string? 摄像机权限;
    private string? 导航消息;
    private string? 截屏消息;
 
    async Task 获取定位() => Locations = await Tools.GetCurrentLocation();
    async Task TakePhoto() => PhotoFilename = await Tools.TakePhoto();
    async Task 检查定位权限() => 定位权限 = await Tools.CheckPermissionsLocation();
    async Task 检查摄像机权限() => 摄像机权限 = await Tools.CheckPermissionsCamera();
    void ShowSettingsUI() =>   Tools.ShowSettingsUI();

    async Task NavigateToMadrid() => 导航消息 = await Tools.NavigateToMadrid();
    async Task NavigateToPlazaDeEspana() => 导航消息 = await Tools.NavigateToPlazaDeEspana();
    async Task NavigateToPlazaDeEspanaByPlacemark() => 导航消息 = await Tools.NavigateToPlazaDeEspanaByPlacemark();
    async Task DriveToPlazaDeEspana() => 导航消息 = await Tools.DriveToPlazaDeEspana();
    async Task TakeScreenshotAsync() => 截屏消息 = await Tools.TakeScreenshotAsync();
}


