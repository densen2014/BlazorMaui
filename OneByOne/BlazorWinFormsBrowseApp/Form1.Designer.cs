// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

namespace BlazorWinFormsBrowseApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            flowLayoutPanel1 = new FlowLayoutPanel();
            buttonWebviewAlert = new Button();
            buttonHome = new Button();
            buttonGo = new Button();
            comboBoxUrl = new ComboBox();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(buttonWebviewAlert);
            flowLayoutPanel1.Controls.Add(buttonHome);
            flowLayoutPanel1.Controls.Add(comboBoxUrl);
            flowLayoutPanel1.Controls.Add(buttonGo);
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1831, 50);
            flowLayoutPanel1.TabIndex = 2;
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
            // comboBoxUrl
            // 
            comboBoxUrl.FormattingEnabled = true;
            comboBoxUrl.Location = new Point(369, 3);
            comboBoxUrl.Name = "comboBoxUrl";
            comboBoxUrl.Size = new Size(858, 36);
            comboBoxUrl.TabIndex = 5;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1831, 1105);
            Controls.Add(flowLayoutPanel1);
            Name = "Form1";
            Text = "Form1";
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private Button buttonWebviewAlert;
        private Button buttonHome;
        private Button buttonGo;
        private ComboBox comboBoxUrl;
    }
}