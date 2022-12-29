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
using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;

namespace BlazorShared.Pages
{
    public partial class Serials
    {
        [Inject,NotNull] protected ITools? Tools { get; set; }

        private string? 打印消息 { get; set; }

        private string? 外屏显示消息 { get; set; }

        private string? NFC消息 { get; set; }

        List<string>? PortList { get; set; }  

        Task GetPortlist()
        {
            PortList = SerialPort.GetPortNames().ToList();
            return Task.CompletedTask;
        }

        async Task 打印() => 打印消息 = await Tools.Print();

        async Task 外屏显示() => 外屏显示消息 = await Tools.ExtDSP();

        async Task 读NFC() => NFC消息 = await Tools.ReadNFC();

    }
}


