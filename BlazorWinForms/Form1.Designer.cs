namespace BlazorWinForms
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
            buttonShowCounter = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            buttonWebviewAlert = new Button();
            buttonHome = new Button();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // buttonShowCounter
            // 
            buttonShowCounter.Location = new Point(3, 3);
            buttonShowCounter.Name = "buttonShowCounter";
            buttonShowCounter.Size = new Size(183, 40);
            buttonShowCounter.TabIndex = 0;
            buttonShowCounter.Text = "Show counter";
            buttonShowCounter.UseVisualStyleBackColor = true;
            buttonShowCounter.Click += ButtonShowCounter_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(buttonShowCounter);
            flowLayoutPanel1.Controls.Add(buttonWebviewAlert);
            flowLayoutPanel1.Controls.Add(buttonHome);
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(2100, 50);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // buttonWebviewAlert
            // 
            buttonWebviewAlert.Location = new Point(192, 3);
            buttonWebviewAlert.Name = "buttonWebviewAlert";
            buttonWebviewAlert.Size = new Size(190, 40);
            buttonWebviewAlert.TabIndex = 1;
            buttonWebviewAlert.Text = "Webview alert";
            buttonWebviewAlert.UseVisualStyleBackColor = true;
            buttonWebviewAlert.Click += ButtonWebviewAlert_Click;
            // 
            // buttonHome
            // 
            buttonHome.Location = new Point(388, 3);
            buttonHome.Name = "buttonHome";
            buttonHome.Size = new Size(164, 40);
            buttonHome.TabIndex = 2;
            buttonHome.Text = "Home";
            buttonHome.UseVisualStyleBackColor = true;
            buttonHome.Click += ButtonHome_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2100, 1200);
            Controls.Add(flowLayoutPanel1);
            Name = "Form1";
            Text = "BlazorWinForms";
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button buttonShowCounter;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button buttonWebviewAlert;
        private Button buttonHome;
    }
}
