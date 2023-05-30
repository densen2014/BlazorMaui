// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using Microsoft.AspNetCore.Components.WebView;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Web.WebView2.Core;
#nullable disable

namespace BlazorWinFormsBrowseApp;

internal class BrowserComponent : UserControl
{
    protected string UploadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "uploads");
    private FlowLayoutPanel flowLayoutPanel1;
    private Button buttonWebviewAlert;
    private Button buttonHome;
    private ComboBox comboBoxUrl;
    private Button buttonGo;
    private Button buttonHide;
    BlazorWebView blazorWebView;

    public string Url { get; set; } = "wwwroot/index.html";

    public string[] UrlHistory { get; set; } =
        {
            "https://www.blazor.zone",
            "https://blazor.app1.es",
            "https://baidu.com",
            "https://bing.com",
            "https://github.com",
            "https://google.com",
            "https://blazor.net",
        };


    private void InitializeComponent()
    {
        flowLayoutPanel1 = new FlowLayoutPanel();
        buttonWebviewAlert = new Button();
        buttonHome = new Button();
        comboBoxUrl = new ComboBox();
        buttonGo = new Button();
        buttonHide = new Button();
        flowLayoutPanel1.SuspendLayout();
        SuspendLayout();
        // 
        // flowLayoutPanel1
        // 
        flowLayoutPanel1.Controls.Add(buttonWebviewAlert);
        flowLayoutPanel1.Controls.Add(buttonHome);
        flowLayoutPanel1.Controls.Add(comboBoxUrl);
        flowLayoutPanel1.Controls.Add(buttonGo);
        flowLayoutPanel1.Controls.Add(buttonHide);
        flowLayoutPanel1.Dock = DockStyle.Top;
        flowLayoutPanel1.Location = new Point(0, 0);
        flowLayoutPanel1.Name = "flowLayoutPanel1";
        flowLayoutPanel1.Size = new Size(1595, 50);
        flowLayoutPanel1.TabIndex = 3;
        // 
        // buttonWebviewAlert
        // 
        buttonWebviewAlert.Location = new Point(3, 3);
        buttonWebviewAlert.Name = "buttonWebviewAlert";
        buttonWebviewAlert.Size = new Size(190, 40);
        buttonWebviewAlert.TabIndex = 1;
        buttonWebviewAlert.Text = "Webview alert";
        buttonWebviewAlert.UseVisualStyleBackColor = true;
        buttonWebviewAlert.Click += ButtonWebviewAlert_Click;
        // 
        // buttonHome
        // 
        buttonHome.Location = new Point(199, 3);
        buttonHome.Name = "buttonHome";
        buttonHome.Size = new Size(164, 40);
        buttonHome.TabIndex = 2;
        buttonHome.Text = "Home";
        buttonHome.UseVisualStyleBackColor = true;
        buttonHome.Click += ButtonHome_Click;
        // 
        // comboBoxUrl
        // 
        comboBoxUrl.FormattingEnabled = true;
        comboBoxUrl.Location = new Point(369, 3);
        comboBoxUrl.Name = "comboBoxUrl";
        comboBoxUrl.Size = new Size(858, 36);
        comboBoxUrl.TabIndex = 5;
        // 
        // buttonGo
        // 
        buttonGo.Location = new Point(1233, 3);
        buttonGo.Name = "buttonGo";
        buttonGo.Size = new Size(164, 40);
        buttonGo.TabIndex = 4;
        buttonGo.Text = "Go";
        buttonGo.UseVisualStyleBackColor = true;
        buttonGo.Click += ButtonGo_Click;
        // 
        // buttonHide
        // 
        buttonHide.Location = new Point(1403, 3);
        buttonHide.Name = "buttonHide";
        buttonHide.Size = new Size(46, 40);
        buttonHide.TabIndex = 6;
        buttonHide.Text = "X";
        buttonHide.UseVisualStyleBackColor = true;
        buttonHide.Click += ButtonHide_Click;
        // 
        // BrowserComponent
        // 
        Controls.Add(flowLayoutPanel1);
        Name = "BrowserComponent";
        Size = new Size(1595, 890);
        flowLayoutPanel1.ResumeLayout(false);
        ResumeLayout(false);
    }

    public BrowserComponent()
    {
        InitializeComponent();

        Text = "BrowserComponent";

        blazorWebView = new BlazorWebView()
        {
            Dock = DockStyle.Fill,
            HostPage = Url,
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


        comboBoxUrl.Items.AddRange(UrlHistory);

        comboBoxUrl.KeyDown += (sender, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonGo.PerformClick();
            }
        };
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

    private async void ButtonGo_Click(object sender, EventArgs e)
    {
        await NavigateTo(comboBoxUrl.Text);
    }

    public async void Init(string url, string[] urlHistory = null)
    {
        if (urlHistory != null) UrlHistory = urlHistory;
        await NavigateTo(url);
    }

    public async Task<bool> NavigateTo(string url)
    {
        if (string.IsNullOrEmpty(url)) return false;
        Url = url;
        comboBoxUrl.Text = url;
        while (blazorWebView.WebView.CoreWebView2 == null)
        {
            await Task.Delay(100);
        }
        blazorWebView.WebView.CoreWebView2.Navigate(comboBoxUrl.Text);
        if (!comboBoxUrl.Items.Contains(comboBoxUrl.Text))
        {
            comboBoxUrl.Items.Add(comboBoxUrl.Text);
        }
        return true;
    }

    private void ButtonHide_Click(object sender, EventArgs e)
    {
        this.Hide();
    }

}
