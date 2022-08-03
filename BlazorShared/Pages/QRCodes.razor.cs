// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using AME.CommonUtils;
using BlazorShared.Services;
using AmeApi;
using AmeBlazor.Components;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BlazorShared.Pages
{
    public partial class QRCodes
    {
        [Inject] protected ToastService ToastService { get; set; }

        private Task OnGenerated()
        {
            ToastService.Information("Info","Generated");
            return Task.CompletedTask;
        }
    }
}


