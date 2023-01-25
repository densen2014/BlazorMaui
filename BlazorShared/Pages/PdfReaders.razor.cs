using BootstrapBlazor.Components;
using System.ComponentModel;

namespace BlazorShared.Pages;

public sealed partial class PdfReaders
{
    private EnumZoomMode Zoom { get; set; } = EnumZoomMode.PageHeight;

    private EnumPageMode Pagemode { get; set; } = EnumPageMode.None;

    [DisplayName("搜索")]
    private string? Search { get; set; } = "Demos";

    private int Page { get; set; } = 3;

    PdfReader? pdfReader { get; set; }

    PdfReader? pdfReader2 { get; set; }

    [DisplayName("文件相对路径或者URL")]
    private string FileName { get; set; } = "/_content/BlazorShared/samples/sample.pdf";

    private string FileNameStream { get; set; } = "https://blazor.app1.es/_content/DemoShared/samples/sample.pdf";

    [DisplayName("流模式")]
    private bool StreamMode { get; set; }

    [DisplayName("禁用复制/打印/下载")]
    public bool ReadOnly { get; set; }

    [DisplayName("水印内容")]
    public string Watermark { get; set; } = "www.blazor.zone";


    private bool Debug { get; set; }

    private async Task Apply()
    {
        if (pdfReader!=null) await pdfReader.Refresh();
    }

    private async Task ApplyZoom()
    {
        Zoom = Zoom switch
        {
            EnumZoomMode.Auto => EnumZoomMode.PageActual,
            EnumZoomMode.PageActual => EnumZoomMode.PageFit,
            EnumZoomMode.PageFit => EnumZoomMode.PageWidth,
            EnumZoomMode.PageWidth => EnumZoomMode.PageHeight,
            EnumZoomMode.PageHeight => EnumZoomMode.Zoom75,
            EnumZoomMode.Zoom75 => EnumZoomMode.Zoom50,
            EnumZoomMode.Zoom50 => EnumZoomMode.Zoom25,
            EnumZoomMode.Zoom25 => EnumZoomMode.Zoom200,
            _ => EnumZoomMode.Auto
        };
        await Refresh();
    }

    async Task Refresh()
    {
        if (pdfReader2 != null)
            await pdfReader2.Refresh(Search, Page, Pagemode, Zoom, ReadOnly, Watermark);
    }

    private async Task ApplyPagemode()
    {
        Pagemode = Pagemode switch
        {
            EnumPageMode.Thumbs => EnumPageMode.Outline,
            EnumPageMode.Outline => EnumPageMode.Attachments,
            EnumPageMode.Attachments => EnumPageMode.Layers,
            EnumPageMode.Layers => EnumPageMode.None,
            _ => EnumPageMode.Thumbs
        };
        await Refresh();
    }

    private async Task ApplyPage()
    {
        Search = null;
        await Refresh();
    }

    private async Task ApplyPagePrevious()
    {
        Page--;
        Search = null;
        await Refresh();
    }

    private async Task ApplyPageNext()
    {
        Page++;
        Search = null;
        await Refresh();
    }

    private async Task ApplySearch()=> await Refresh();
}
