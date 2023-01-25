using BootstrapBlazor.Components;

namespace BlazorShared.Pages;

/// <summary>
/// Screen Capture 截屏/录像
/// </summary>
public sealed partial class ScreenCapturePage
{

    private CaptureOptions Options { get; set; } = new CaptureOptions();
    
    private string? message;

    private async Task OnCaptureResult(Stream item)
    {
        if (OCR!=null) await OCR.OCRFromStream(item);
        StateHasChanged();
    }

    private Task OnError(string message)
    {
        this.message = message;
        StateHasChanged();
        return Task.CompletedTask;
    }

    #region 附加OCR演示
    private OCR? OCR { get; set; } 
    
    private Task OnResult(List<string> res)
    {
        this.message = "识别完成";
        StateHasChanged();
        return Task.CompletedTask;
    }

    #endregion 

    /// <summary>
    /// 获得属性方法
    /// </summary>
    /// <returns></returns>
    private IEnumerable<AttributeItem> GetAttributes() => new AttributeItem[]
    {
        new AttributeItem("Start","截屏",  "-","Task"),
        new AttributeItem("OnCaptureResult","截屏回调",  "-","Func<Stream, Task>"),
        new AttributeItem("OnError","错误信息回调",  "-","Func<string, Task>"),
        new AttributeItem("ShowUI","显示内置UI",  "true","bool","true|false"),
        new AttributeItem("Debug","显示log",  "false","bool","true|false"),
    };
}
