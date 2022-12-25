using BootstrapBlazor.Components;

namespace BlazorShared.Pages;

/// <summary>
/// Geolocation 地理定位/移动距离追踪
/// <para></para>
/// 扩展阅读:Chrome中模拟定位信息，清除定位信息<para></para>
/// https://blog.csdn.net/u010844189/article/details/81163438
/// </summary>
public sealed partial class GeolocationPage
{

    private string? status { get; set; }
    private Geolocationitem? geolocations { get; set; }
    private List<Geolocationitem> Items { get; set; } = new List<Geolocationitem>() { new Geolocationitem() };
    private string? message;

    private Task OnResult(Geolocationitem geolocations)
    {
        this.geolocations = geolocations;
        Items[0] = geolocations;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private Task OnUpdateStatus(string status)
    {
        this.status = status;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private Task OnError(string message)
    {
        this.message = message;
        StateHasChanged();
        return Task.CompletedTask;
    }


    /// <summary>
    /// 获得属性方法
    /// </summary>
    /// <returns></returns>
    private IEnumerable<AttributeItem> GetAttributes() => new AttributeItem[]
    {
            new AttributeItem("GetLocationButtonText","获取位置按钮文字",  "获取位置"),
            new AttributeItem("WatchPositionButtonText","移动距离追踪按钮文字","移动距离追踪"),
            new AttributeItem("ClearWatchPositionButtonText","停止追踪按钮文字","停止追踪"),
            new AttributeItem("ShowButtons","是否显示默认按钮界面","true","bool","true|false"),
            new AttributeItem("OnResult","定位完成回调方法", "-","Func<Geolocationitem, Task>"),
            new AttributeItem("OnUpdateStatus","状态更新回调方法", "-","Func<string, Task>"),
            new AttributeItem("OnError","错误信息回调",  "-","Func<string, Task>"),
    };
}
