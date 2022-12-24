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

namespace DemoShared.Pages
{
    public partial class Serials
    {
        [Inject,NotNull] protected ITools? Tools { get; set; }

        private string? Status;
        private string? 打印消息;
        private string? 外屏显示消息 { get; set; }
        private string? NFC消息 { get; set; }

        private bool isBusy;
        List<string>? PortList { get; set; }

        private CancellationTokenSource? AutoRefreshCancelTokenSource { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
            }
        }

        /// <summary>
        /// Dispose 方法
        /// </summary>
        public void Dispose()
        {
            AutoRefreshCancelTokenSource = null;
        }

        Task GetPortlist()
        {
            PortList = Tools.GetPortlist();
            return Task.CompletedTask;
        }

        async Task 打印() => 打印消息 = await Tools.Print();
        async Task 外屏显示() => 外屏显示消息 = await Tools.ExtDSP();
        async Task 读NFC() => NFC消息 = await Tools.ReadNFC();

    }
}


