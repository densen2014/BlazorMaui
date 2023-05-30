// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using Microsoft.AspNetCore.Components.WebView;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Web.WebView2.Core;

namespace BlazorWinFormsBrowseApp;

#nullable disable

public partial class Form1 : Form
{
    protected string UploadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "uploads");
    BlazorWebView blazorWebView;

    public Form1()
    {
        InitializeComponent();

        Text = "BlazorWinFormsBrowseApp";

        blazorWebView = new BlazorWebView()
        {
            Dock = DockStyle.Fill,
            HostPage = "wwwroot/index.html",
            Services = Startup.Services
        };

        blazorWebView.RootComponents.Add<App>("#app");
        Controls.Add(blazorWebView);
        blazorWebView.BringToFront();

        blazorWebView.BlazorWebViewInitialized += BlazorWebViewInitialized;

        blazorWebView.UrlLoading +=
            (sender, urlLoadingEventArgs) =>
            {
                if (urlLoadingEventArgs.Url.Host != "0.0.0.0")
                {
                    //外部链接WebView内打开,例如pdf浏览器
                    Console.WriteLine(urlLoadingEventArgs.Url);
                    urlLoadingEventArgs.UrlLoadingStrategy =
                        UrlLoadingStrategy.OpenInWebView;
                }
            };

        comboBoxUrl.Items.AddRange(new object[]
        {
            "https://www.blazor.zone",
            "https://blazor.app1.es",
            "https://baidu.com",
            "https://bing.com",
            "https://github.com",
            "https://google.com",
            "https://blazor.net",
        });
    }

    void BlazorWebViewInitialized(object sender, EventArgs e)
    {
        //下载开始时引发 DownloadStarting，阻止默认下载
        blazorWebView.WebView.CoreWebView2.DownloadStarting += CoreWebView2_DownloadStarting;

        //指定下载保存位置
        blazorWebView.WebView.CoreWebView2.Profile.DefaultDownloadFolderPath = UploadPath;
    }

    private void CoreWebView2_DownloadStarting(object sender, CoreWebView2DownloadStartingEventArgs e)
    {
        var downloadOperation = e.DownloadOperation;
        string fileName = Path.GetFileName(e.ResultFilePath);
        var filePath = Path.Combine(UploadPath, fileName);

        //指定下载保存位置
        e.ResultFilePath = filePath;
        MessageBox.Show($"下载文件完成 {fileName}", "提示");
    }

    private void ButtonWebviewAlert_Click(object sender, EventArgs e)
    {
        blazorWebView.WebView.CoreWebView2.ExecuteScriptAsync("alert('hello from native UI')");
    }

    private void ButtonHome_Click(object sender, EventArgs e)
    {
        blazorWebView.WebView.CoreWebView2.Navigate("https://0.0.0.0/");
    }

    private void ButtonGo_Click(object sender, EventArgs e)
    {
        blazorWebView.WebView.CoreWebView2.Navigate(comboBoxUrl.Text);
        comboBoxUrl.Items.Add(comboBoxUrl.Text);
    }

    private async void ButtonNewTab_Click(object sender, EventArgs e)
    {
        browserComponent1.Visible = true;
        await browserComponent1.NavigateTo(comboBoxUrl.Text);
    }

    public async Task<bool> NavigateTo(string url)
    {
        browserComponent1.Visible = true;
        return await browserComponent1.NavigateTo(url);
    }
}
