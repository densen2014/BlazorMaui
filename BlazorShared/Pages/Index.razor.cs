// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using AME.CommonUtils;
using BlazorShared.Services;
using AmeApi;
using AmeBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using BlazorShared.Models;
using Newtonsoft.Json;
using AME.Services;

namespace BlazorShared.Pages;

public partial class Index
{
    [Inject] protected HttpClient? Http { get; set; }
    [Inject] protected ILogger<Index>? Logger { get; set; }
    [Inject] protected WebClientInfoProvider? WebClientInfo { get; set; }
    [Inject] protected ClipboardService? ClipboardService { get; set; }
    [Inject] protected IIPAddressManager? IPAddressManager { get; set; }
    [Inject] protected ITools? Tools { get; set; }
    //[Inject] protected IFreeSql fsql { get; set; }

    private List<PcStatus> pcStatuss;
    private List<PC> pcs;
    private List<Item> records;
    private string 文字;
    private string Locations;
    private string PhotoFilename;
    private string version;
    private string 定位权限;
    private string 摄像机权限;
    private string 导航消息;
    private string 截屏消息;

    private bool isBusy;
    TableLazyHero<PC> list1 { get; set; }
    private List<string> ComponentItems { get; set; } = new List<string>();

    private CancellationTokenSource AutoRefreshCancelTokenSource { get; set; }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            //worker();
            //records = fsql.Select<Item>().ToList();
            version= Tools.GetAppInfo();
        }
    }

    /// <summary>
    /// Dispose 方法
    /// </summary>
    public void Dispose()
    {
        AutoRefreshCancelTokenSource = null;
    }

    public void worker()
    {
        AutoRefreshCancelTokenSource = new CancellationTokenSource();
        _ = Task.Run(async () =>
        {
            try
            {
                while (!(AutoRefreshCancelTokenSource?.IsCancellationRequested ?? true))
                {
                    await 刷新();
                    await InvokeAsync(StateHasChanged);
                    if (list1 != null)
                    {
                        await InvokeAsync(() => list1?.Load(pcs));
                    }
                    await Task.Delay(TimeSpan.FromMinutes(5), AutoRefreshCancelTokenSource?.Token ?? new CancellationToken(true));
                }
            }
            catch
            {
            }
        });

    }

    private Task 刷新()
    {
        if (isBusy) return Task.CompletedTask;
        isBusy = true;
        Logger.LogInformation($"{DateTime.Now.ToString("HH:mm:ss")} 读API");
        //var res = await Http?.GetStringAsync("https://xxxxx");
        var res = @"
                        {
                        ""count"": 13,
                        ""working"": 13,
                        ""unWorking"": 0,
                        ""online"": 13,
                        ""offline"": 0,
                        ""pcs"": [
                        {
                        ""id"": 1044,
                        ""status"": ""运行"",
                        ""name"": ""01KV999"",
                        ""statusDate"": ""2022-01-04 17:41:50"",
                        ""profileDate"": ""2021-12-29 22:23:07"",
                        ""guid"": ""0-RFj2JLDHf0"",
                        ""description"": ""CPU: 30% PC启动: 16天08:53:33 ""
                        },
                        {
                        ""id"": 1050,
                        ""status"": ""运行"",
                        ""name"": ""02KV999"",
                        ""statusDate"": ""2022-01-04 17:41:59"",
                        ""profileDate"": ""2022-01-02 12:59:04"",
                        ""guid"": ""0-0kSscEMoS"",
                        ""description"": ""CPU: 15% PC启动: 16天08:58:24 ""
                        },
                        {
                        ""id"": 1039,
                        ""status"": ""运行"",
                        ""name"": ""03KV999"",
                        ""statusDate"": ""2022-01-04 17:41:51"",
                        ""profileDate"": ""2022-01-02 00:40:19"",
                        ""guid"": ""0-kUC6gQbyl"",
                        ""description"": ""CPU: 55% PC启动: 16天09:03:23 ""
                        }
                        ]
                        }";
        var pcStatus = JsonConvert.DeserializeObject<PcStatus>(res);
        pcStatuss = new List<PcStatus> { pcStatus };
        pcs = pcStatus.PCs;
        isBusy = false;
        return Task.CompletedTask;
    }

    private async Task 刷新数据()
    {
        if (isBusy) return;
        await 刷新();
        await list1.Load(pcs);
    }

    async Task 粘贴()
    {
        try
        {
            文字 = await ClipboardService.ReadTextAsync();
        }
        catch
        {
        }
    }
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


