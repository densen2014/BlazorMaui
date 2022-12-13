// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using AME.CommonUtils;
using AME.Services;
using AmeApi;
using AmeBlazor.Components;
using BlazorShared.Models;
using BlazorShared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace BlazorShared.Pages;

public partial class Index
{
    [Inject,NotNull] protected WebClientInfoProvider? WebClientInfo { get; set; }
    [Inject,NotNull] protected ClipboardService? ClipboardService { get; set; }
    [Inject,NotNull] protected IIPAddressManager? IPAddressManager { get; set; }
    [Inject, NotNull] protected ITools? Tools { get; set; }

    private string? 文字;
    private string? version;

 
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        { 
            version= Tools.GetAppInfo();
            StateHasChanged();
        }
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

}


