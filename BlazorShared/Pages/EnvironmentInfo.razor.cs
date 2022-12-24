// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using AME.CommonUtils;
using DemoShared.Services;
using AmeApi;
using AmeBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using DemoShared.Models;
using Newtonsoft.Json;
using AME.Services;
using System.Diagnostics.CodeAnalysis;

namespace DemoShared.Pages;

public partial class EnvironmentInfo
{
    [Inject,NotNull] protected ITools? Tools { get; set; } 

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        { 
        }
    }
     
}


