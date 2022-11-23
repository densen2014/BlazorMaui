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
    public partial class Circles
    {
        [Inject] protected ToastService? ToastService { get; set; }

        private int CircleValue = 0;

        private void Add(int interval)
        {
            CircleValue += interval;
            CircleValue = Math.Min(100, Math.Max(0, CircleValue));
        }
    }
}


