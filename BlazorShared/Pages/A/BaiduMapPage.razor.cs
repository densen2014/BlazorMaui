using BootstrapBlazor.Components;

namespace BlazorShared.Pages;

/// <summary>
/// 百度地图 BaiduMap
/// </summary>
public sealed partial class BaiduMapPage
{

    private string message;
    private BaiduItem baiduItem;

    private Task OnResult(BaiduItem geolocations)
    {
        baiduItem = geolocations;
        this.message = baiduItem.Status;
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
            new AttributeItem("BaiduKey",@"为空则在 IConfiguration 服务获取 ""BaiduKey"" , 默认在 appsettings.json 文件配置",  ""),
            new AttributeItem("Style","地图大小",  "height:700px;width:100%;","string"),
            new AttributeItem("Init","初始化",  "-","Task<bool>"),
            new AttributeItem("ResetMaps","复位",  "-","Task"),
            new AttributeItem("GetLocation","获取定位",  "-","void"),
            new AttributeItem("OnResult","获取定位回调",  "-","Func<BaiduItem, Task>"),
            new AttributeItem("OnError","错误信息回调",  "-","Func<string, Task>"),
    };
}
