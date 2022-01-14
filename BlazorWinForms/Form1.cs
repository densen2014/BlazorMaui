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
