// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using AME.CommonUtils;
using DemoShared.Services;
using AmeApi;
using AmeBlazor.Components;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DemoShared.Pages
{
    public partial class BarCode
    {

        [Inject] protected ToastService? ToastService { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
            }
        }


        private Task OnInit(IEnumerable<DeviceItem> devices)
        {
            var cams = string.Join("", devices.Select(i => i.Label));
            return Task.CompletedTask;
        }

        private Task OnImageResult(string barcode)
        {
            return Task.CompletedTask;
        }

        private Task OnImageError(string err)
        {
            return Task.CompletedTask;
        }

        private Task OnResult(string barcode)
        {
            ToastService!.Success("Scan result", barcode);
            return Task.CompletedTask;
        }

        private Task OnError(string error)
        {
            ToastService!.Error(error);
            return Task.CompletedTask;
        }

        private Task OnStart()
        {
            return Task.CompletedTask;
        }

        private Task OnClose()
        {
            return Task.CompletedTask;
        }

    }
}


