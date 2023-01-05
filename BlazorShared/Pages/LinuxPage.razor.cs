// **********************************
// Densen Informatica 中讯科技
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com
// **********************************

using BlazorShared.Data;
using BlazorShared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorShared.Pages;

/// <summary>
///
/// </summary>
public partial class LinuxPage : IAsyncDisposable
{
    [Inject] IJSRuntime? JS { get; set; }

    private IJSObjectReference? module;

    private DotNetObjectReference<LinuxPage>? instance { get; set; }
    private string? LongRunningTaskState { get; set; }

    private string? DataState { get; set; }

    private string? RawData { get; set; }

    private string? Error { get; set; }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            module = await JS!.InvokeAsync<IJSObjectReference>("import", "./app.js");
            instance = DotNetObjectReference.Create(this);
        }
    }

    /// <summary>
    /// 后台任务
    /// </summary>
    public virtual async Task StartLongRunningTask()
    {
        //CrossBridgeService.runLongProcedureOnTask
        var dirs = await module!.InvokeAsync<object>("invokeApi", nameof(CrossBridgeService) + "." + nameof(CrossBridgeService.RunLongProcedureOnTask).ToCamelCase(), nameof(GetResult), instance);
        LongRunningTaskState = dirs.ToString();
    }

    /// <summary>
    /// 读取平台服务数据
    /// </summary>
    public virtual async Task GetData()
    {
        //CrossBridgeService.getSomeData
        var dirs = await module!.InvokeAsync<object>("invokeApi", nameof(CrossBridgeService) + "."+ nameof(CrossBridgeService.GetSomeData).ToCamelCase(), nameof(GetResult2), instance);
        DataState = dirs.ToString();
    }

    /// <summary>
    /// 打开新窗口
    /// </summary>
    public virtual async Task ShowWindow()
    {
        await module!.InvokeVoidAsync("showWindow", instance);
    } 

    /// <summary>
    /// 获取状态回调方法
    /// </summary>
    /// <returns></returns>
    [JSInvokable]
    public void GetStatus(string apiResult)
    {
        try
        {
            Console.WriteLine(apiResult);
            RawData = apiResult;
            StateHasChanged();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 获取结果回调方法
    /// </summary>
    /// <returns></returns>
    [JSInvokable]
    public void GetResult(ApiResult e)
    {
        try
        {
            Console.WriteLine(e);
            RawData = $"{e.success},{e.value},{e.error}";
            if (e.success)
            {
                LongRunningTaskState = e.value?.ToString() ?? "Done!";
            }
            else
            {
                Error = e?.error?.ToString();
            }
            StateHasChanged();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 获取结果2回调方法
    /// </summary>
    /// <returns></returns>
    [JSInvokable]
    public void GetResult2(ApiResult2 e)
    {
        try
        {
            Console.WriteLine(e);
            RawData = $"{e.success},{e.value},{e.error}";
            if (e.success)
            {
                DataState = $"{e.value?.Number},{e.value?.Text}";
            }
            else
            {
                Error = e.error?.ToString();
            }
            StateHasChanged();
        }
        catch
        {
        }
    }

    public class ApiResult
    {
        public bool success { get; set; }
        public object? error { get; set; }
        public virtual object? value { get; set; }
    }

    public class ApiResult2 : ApiResult
    {
        public new SomeDataModel? value { get; set; }
    }


    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (module is not null)
        {
            await module.DisposeAsync();
        }
    }

}
