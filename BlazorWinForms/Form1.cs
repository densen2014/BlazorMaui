using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using BlazorShared;

namespace BlazorWinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //默认全屏
            //this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            //this.FormBorderStyle =FormBorderStyle.None;
            //this.TopMost = true;
            //this.KeyPreview = true;
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);

            var blazor = new BlazorWebView()
            {
                Dock = DockStyle.Fill,
                HostPage = "wwwroot/index.html",
                Services = Startup.Services
            };

            // win7运行失败! [无依赖发布webview2程序] 固定版本运行时环境的方式来实现加载网页
            ////设置web用户文件夹 
            //var browserExecutableFolder = "c:\\wb2";
            //blazor.WebView.CreationProperties = new Microsoft.Web.WebView2.WinForms.CoreWebView2CreationProperties()
            //{
            //    BrowserExecutableFolder = browserExecutableFolder
            //};

            blazor.RootComponents.Add<App>("#app");
            Controls.Add(blazor);
            blazor.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);

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
