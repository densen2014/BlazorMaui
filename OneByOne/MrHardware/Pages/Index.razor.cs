// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using AME.CommonUtils;
using AME.Services;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace MrHardware.Pages;

public partial class Index
{
    [Inject,NotNull] protected WebClientInfoProvider? WebClientInfo { get; set; }
    [Inject,NotNull] protected ClipboardService? ClipboardService { get; set; } 

    private string? 文字;
    private string? version;

 
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        { 
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


