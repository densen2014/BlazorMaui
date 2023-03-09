using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using BlazorShared;
using Microsoft.Web.WebView2.Core;

namespace BlazorWinForms
{
    public partial class Form1 : Form
    {
        protected string UploadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "uploads");
        BlazorWebView blazorWebView;

        public Form1()
        {
            InitializeComponent();

            //默认全屏
            //this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            //this.FormBorderStyle =FormBorderStyle.None;
            //this.TopMost = true;
            //this.KeyPreview = true;
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);

            blazorWebView = new BlazorWebView()
            {
                Dock = DockStyle.Fill,
                HostPage = "wwwroot/index.html",
                Services = Startup.Services
            }; 

            blazorWebView.RootComponents.Add<App>("#app");
            Controls.Add(blazorWebView);
            blazorWebView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);

            blazorWebView.BlazorWebViewInitialized += BlazorWebViewInitialized;

        }
        void BlazorWebViewInitialized(object sender, EventArgs e)
        {
            //下载开始时引发 DownloadStarting，阻止默认下载
            blazorWebView.WebView.CoreWebView2.DownloadStarting += CoreWebView2_DownloadStarting;

            //指定下载保存位置
            blazorWebView.WebView.CoreWebView2.Profile.DefaultDownloadFolderPath = UploadPath;

            ////[无依赖发布webview2程序] 固定版本运行时环境的方式来实现加载网页
            ////设置web用户文件夹 
            //var browserExecutableFolder = "c:\\wb2";
            //var userData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "BlazorWinFormsApp");
            //Directory.CreateDirectory(userData);
            //var creationProperties = new CoreWebView2CreationProperties()
            //{
            //    UserDataFolder = userData,
            //    BrowserExecutableFolder = browserExecutableFolder
            //};
            //mainBlazorWebView.WebView.CreationProperties = creationProperties;
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

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
             if (e.KeyCode == Keys.Escape)
            {
                if (this.WindowState == System.Windows.Forms.FormWindowState.Normal)
                {
                    this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                    this.FormBorderStyle = FormBorderStyle.None;
                }
                else
                {
                    this.WindowState = System.Windows.Forms.FormWindowState.Normal;
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                }
            }
        }
    }
}
