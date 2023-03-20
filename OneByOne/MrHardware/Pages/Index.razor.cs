// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using AME.CommonUtils;
using AME.Services;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace MrHardware.Pages;

public partial class Index
{
    [Inject,NotNull] protected WebClientInfoProvider? WebClientInfo { get; set; }
    [Inject,NotNull] protected AME.Services.ClipboardService? ClipboardService { get; set; } 

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


    private List<TreeItem> DensenTools_Load()
    {
        var funs = new List<string>() { "NetworkAdapter", "PhysicalMedia", "DiskDrive" };
        var funNode = new List<TreeItem>();
        funs.ForEach(x => funNode.Add(new TreeItem() { Text = x }));

        var items = AME.Util.HardwareUtil.GetMacAddress("", "", "");
        items.ForEach(x => funNode[0].Items.Add(new TreeItem() { Key = x.MACAddress, Text = (x.PhysicalAdapter ? "" : "(虚拟) ") + x.Manufacturer + " " + x.MACAddress }));

        var itemsHDD1 = AME.Util.HardwareUtil.GetPhysicalMediaID();
        itemsHDD1.ForEach(x => funNode[1].Items.Add(new TreeItem() { Text = x }));

        var itemsHDD = AME.Util.HardwareUtil.GetDiskDriveIDS(true, false);
        itemsHDD.ForEach(x => funNode[2].Items.Add(new TreeItem() { Key = x.SerialNumber, Text = x.SerialNumber + x.Size + " " + x.InterfaceType }));

        return funNode;
    }

}
