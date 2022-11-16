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

public partial class EnvironmentInfo
{
    [Inject] protected ITools? Tools { get; set; } 

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        { 
        }
    }
     
}


