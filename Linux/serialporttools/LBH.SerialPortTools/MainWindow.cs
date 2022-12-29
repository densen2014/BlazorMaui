using Gtk;
using System.IO.Ports;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LBH.SerialPortTools
{
    public class MainWindow:Window
    {
        //操作面板
        private VBox layout = new VBox();
        private HBox mainBox=new HBox();
        private SerialPort mSerialPort;
        private Thread receiveThread;
        private Statusbar statusbar;
        private int sendByteCount,recevieByteCount=0;
        private string cacheFile=System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"sp.cache");

        JsonSerializerOptions options = new JsonSerializerOptions { 
            ReferenceHandler= ReferenceHandler.IgnoreCycles
        };
        public MainWindow():base(WindowType.Toplevel)
        {
            this.Build();
            InitHistoryConfig();
        }


        #region 状态栏
        void UpdateStatus()
        {
            string comState = "COM Closed";
            if (mSerialPort != null && mSerialPort.IsOpen)
            {
                comState = $"{mSerialPort.PortName} Opened";
            }
            string status = string.Format(" STATUS: : {0}      S: {1}      R:{2}", comState, sendByteCount, recevieByteCount);
            statusbar.Pop (0); 
            statusbar.Push (0, status);
        }
        #endregion

        #region 变量声明
        //串口设置
        private ComboBox comboCom;
        //波特率
        private ComboBox comboRate;
        //停止位
        private ComboBox comboStop;
        //数据位
        private ComboBox comboData;
        //奇偶校验
        private ComboBox comboJo;
        //16进制显示
        private CheckButton ck16;
        //RTX设置
        private CheckButton ckRtx;
        //DTR设置
        private CheckButton ckDtr;
        //16进制发送
        private CheckButton ckS16;
        //新行发送
        private CheckButton ckNewline;
        //定时发送
        private CheckButton ckTime;
        //定时毫秒
        private Entry txtTime;
        //打开串口按钮
        private Button btnSp;
        //清除接受按钮
        private Button btnClear;
        //数据接收区
        private TextView textResult;
        //数据发送区域
        private TextView textSend;
        //发送数据
        private Button btnSend;
        //清除数据
        private Button btnSendClear;
        //定时器ID
        private uint timerSendId = 0;

        private Gdk.Pixbuf img;
        private Gtk.Image imgRender;
        #endregion

        #region 界面布局
        private void Build()
        {
            this.WindowPosition = WindowPosition.Center;
            this.SetDefaultSize(600, 600);
            this.Title = "串口调试助手v1.0.00";
            this.Resizable = false;
            this.DeleteEvent += MainWindow_DeleteEvent;
            this.Add(layout);

            //统计数据
            statusbar = new Statusbar();
            statusbar.Visible = true;
            UpdateStatus();

            layout.PackStart(mainBox, false, false, 0);
            layout.PackEnd(statusbar, false, false, 0);
            this.BuildLayout();
            this.ShowAll();
        }
        private void BuildLayout()
        {
            #region 两列布局
            VBox leftBox = new VBox();
            leftBox.Margin = 5;
            leftBox.SetSizeRequest(200, 400);
            this.mainBox.Add(leftBox);

            VBox rightBox = new VBox();
            rightBox.SetSizeRequest(400, 600);
            rightBox.Margin = 5;
            this.mainBox.Add(rightBox);
            #endregion

            #region 串口操作
            Frame frame1 = new Frame("<b>串口设置</b>");
            ((Gtk.Label)frame1.LabelWidget).UseMarkup = true;
            leftBox.PackStart(frame1, true, true, 5);

            VBox vfram = new VBox();
            frame1.Add(vfram);
            //串口设置
            HBox hCom = new HBox();
            var lblCom = new Label { Text = "串口号:" };
            lblCom.UseMarkup = true;
            lblCom.Halign = Align.Start;
            comboCom = new ComboBox();

            comboCom.SetSizeRequest(150, 30);
            hCom.PackStart(lblCom, true, true, 5);
            hCom.PackStart(comboCom, true, true, 5);

            vfram.PackStart(hCom, false, false, 2);
            //波特率
            HBox hRate = new HBox();
            var lblRate = new Label { Text = "波特率:" };
            lblRate.Halign = Align.Start;
            comboRate = new ComboBox();
            comboRate.SetSizeRequest(150, 30);
            hRate.PackStart(lblRate, true, true, 5);
            hRate.PackStart(comboRate, true, true, 5);

            vfram.PackStart(hRate, false, false, 2);


            //停止位
            HBox hStop = new HBox();
            var lblStop = new Label { Text = "停止位:" };
            lblStop.Halign = Align.Start;
            comboStop = new ComboBox();
            comboStop.SetSizeRequest(150, 30);
            hStop.PackStart(lblStop, true, true, 5);
            hStop.PackStart(comboStop, true, true, 5);

            vfram.PackStart(hStop, false, false, 2);


            //数据位
            HBox hData = new HBox();
            var lblData = new Label { Text = "数据位:" };
            lblData.Halign = Align.Start;
            comboData = new ComboBox();
            comboData.SetSizeRequest(150, 30);
            hData.PackStart(lblData, true, true, 5);
            hData.PackStart(comboData, true, true, 5);

            vfram.PackStart(hData, false, false, 2);


            //奇偶校验
            HBox hJo = new HBox();
            var lblJo = new Label { Text = "校验位:" };
            lblJo.Halign = Align.Start;
            comboJo = new ComboBox();
            comboJo.SetSizeRequest(150, 30);
            hJo.PackStart(lblJo, true, true, 5);
            hJo.PackStart(comboJo, true, true, 5);

            vfram.PackStart(hJo, false, false, 2);

            //打开/关闭串口
            HBox co = new HBox();
            btnSp = new Button { Label = "打开串口" };
            btnSp.Name = "btnSp";
            btnSp.AlwaysShowImage = true;
            btnSp.Image = Image.NewFromIconName("window-new", IconSize.Button);
            btnSp.Clicked += BtnSp_Clicked;
            btnSp.SetSizeRequest(110, 30);
            co.PackStart(btnSp, true, true, 5);

            vfram.PackStart(co, false, false, 2);
            //开关表示图
            HBox imgSwitch = new HBox();
            img = new Gdk.Pixbuf("2.png",30,30);
            imgRender =new Image(img);
            imgRender.Margin = 5;
            imgSwitch.PackStart(imgRender, true, true, 5);

            vfram.PackStart(imgSwitch, false, false, 2);
            #endregion

            #region 接受设置
            Frame frame2 = new Frame("<b>接收设置</b>");
            ((Gtk.Label)frame2.LabelWidget).UseMarkup = true;
            leftBox.PackStart(frame2, true, true, 5);

            VBox f2vbox=new VBox();
            f2vbox.Margin = 5;

            btnClear = new Button { Label = "清空接收区" };
            btnClear.Clicked += BtnClear_Clicked;
            f2vbox.PackStart(btnClear, false, false, 5);

            ck16 = new CheckButton { Label = "16进制显示" };
            f2vbox.PackStart(ck16, false, false, 5);
            ck16.Toggled += new EventHandler(ck16Toggled);

            ckRtx = new CheckButton { Label = "RTX设置" };
            f2vbox.PackStart(ckRtx, false, false, 5);
            ckRtx.Toggled += new EventHandler(ckRtxToggled);


            ckDtr = new CheckButton { Label = "DTR设置" };
            f2vbox.PackStart(ckDtr, false, false, 5);
            ckDtr.Toggled += new EventHandler(ckDtrToggled);

            frame2.Add(f2vbox);
            #endregion

            #region 发送设置
            Frame frame3 = new Frame("<b>发送设置</b>");
            ((Gtk.Label)frame3.LabelWidget).UseMarkup = true;
            leftBox.PackStart(frame3, true, true, 5);

            VBox f3vbox = new VBox();
            f3vbox.Margin = 5;

            ckS16 = new CheckButton { Label = "16进制发送" };
            f3vbox.PackStart(ckS16, false, false, 5);            

            ckNewline = new CheckButton { Label = "发送新行" };
            f3vbox.PackStart(ckNewline, false, false, 5);

            HBox timeBox = new HBox();
            ckTime = new CheckButton { Label = "定时发送" };
            timeBox.PackStart(ckTime, false, false,0);
            ckTime.Toggled += new EventHandler(ckTimeToggled);

            txtTime =new Entry();
            txtTime.SetSizeRequest(100, 20);
            txtTime.Buffer.Text = "1000";
            timeBox.PackStart(txtTime, false, false, 5);

            Label lblTime=new Label { Text="ms"};
            timeBox.PackStart(lblTime, false, false, 5);
            f3vbox.PackStart(timeBox, false, false, 5);

            frame3.Add(f3vbox);

            #endregion

            #region 数据接收区            
            Frame frame4 = new Frame("<b>数据显示</b>");
            ((Gtk.Label)frame4.LabelWidget).UseMarkup = true;
            rightBox.PackStart(frame4, true, true, 5);
            var scrollResult = new ScrolledWindow();
            scrollResult.SetSizeRequest(360, 500);
            //数据接收区
            textResult =new TextView();
            textResult.Name = "textResult";
            textResult.Margin = 2;
            textResult.Editable = false;
            textResult.WrapMode = WrapMode.WordChar;    
            textResult.SizeAllocated += TextView_SizeAllocated;
            scrollResult.Child = textResult;
            frame4.Add(scrollResult);
            #endregion

            #region 数据发送区
            Frame frame5 = new Frame("<b>数据发送</b>");
            ((Gtk.Label)frame5.LabelWidget).UseMarkup = true;
            rightBox.PackStart(frame5, true, true, 5);

            var scrollSend = new ScrolledWindow();
            scrollSend.SetSizeRequest(360, 100);
            textSend = new TextView();
            textSend.Margin = 2;
            textSend.WrapMode = WrapMode.WordChar;
            scrollSend.Child = textSend;
            frame5.Add(scrollSend);

            HBox hSendOpt=new HBox();
            hSendOpt.Margin = 2;
            btnSend = new Button { Label = "发送数据" };
            btnSend.Clicked += BtnSend_Clicked;
            btnSendClear = new Button { Label = "清除发送" };
            btnSendClear.Clicked += BtnSendClear_Clicked;
            btnSend.SetSizeRequest(100, 30);
            btnSendClear.SetSizeRequest(100, 30);
            hSendOpt.PackStart(btnSend, false, false, 5);
            hSendOpt.PackStart(btnSendClear, false, false, 5);
            rightBox.PackStart(hSendOpt, false, false, 5);

            #endregion            
        }
        #endregion

        #region 加载上次配置
        private void InitHistoryConfig()
        {
            var config = LoadCache();
            FillCombo(config,comboCom, "com", SerialPort.GetPortNames());
            FillCombo(config, comboRate, "rate", Utils.baudRateList);
            FillCombo(config, comboStop, "stop", Utils.stopBitList);
            FillCombo(config, comboData, "data", Utils.dataBitList);
            FillCombo(config, comboJo, "pty", Utils.parityList);
        }
        private void InitCache(ComConfig config)
        {
            try
            {
                if (File.Exists(cacheFile)) File.Delete(cacheFile);
                var cacheString = JsonSerializer.Serialize(config, options);
                byte[] bs = Encoding.Default.GetBytes(cacheString);
                using (Stream fileStream = new FileStream(cacheFile, FileMode.OpenOrCreate))
                {
                    fileStream.Write(bs, 0, bs.Length);
                }
            }
            catch (Exception)
            {

            }
        }
        private ComConfig LoadCache()
        {
            ComConfig config = null;
            if (File.Exists(cacheFile))
            {
                try
                {
                    using (Stream fileStream = new FileStream(cacheFile, FileMode.Open))
                    {
                        var bs = new byte[fileStream.Length];
                        fileStream.Read(bs, 0, bs.Length);
                        var tmp = Encoding.Default.GetString(bs);
                        config = JsonSerializer.Deserialize<ComConfig>(tmp);
                    }
                }
                catch (Exception)
                {

                }
            }
            return config;
        }
        private void FillCombo(ComConfig config, Gtk.ComboBox cb, string type, string[] ctextList)
        {
            cb.Clear();
            CellRendererText cell = new CellRendererText();
            cb.PackStart(cell, false);
            cb.AddAttribute(cell, "text", 0);
            ListStore store = new ListStore(typeof(string));
            cb.Model = store;

            int index = 0;
            for (int i = 0; i < ctextList.Length; i++)
            {
                store.AppendValues(ctextList[i]);
                if (config != null && type == "com" && ctextList[i] == config.PortName)
                {
                    index = i;
                }
                if (config != null && type == "rate" && ctextList[i] == config.BaudRate)
                {
                    index = i;
                }
                if (config != null && type == "data" && ctextList[i] == config.DataBit)
                {
                    index = i;
                }
                if (config != null && type == "stop" && ctextList[i] == config.StopBit)
                {
                    index = i;
                }
                if (config != null && type == "pty" && ctextList[i] == config.Parity)
                {
                    index = i;
                }
            }
            cb.Active = index;
        }

        #endregion

        #region 事件处理

        private void MainWindow_DeleteEvent(object o, DeleteEventArgs args)
        {
            Application.Quit();
        }

        private void TextView_SizeAllocated(object o, SizeAllocatedArgs args)
        {
            textResult.ScrollToIter(textResult.Buffer.EndIter, 0, false, 0, 0);
        }

        private void ckTimeToggled(object? sender, EventArgs e)
        {
            if (ckTime.Active)
            {
                if (mSerialPort == null || !mSerialPort.IsOpen)
                {
                    MessageBox.Show("尚未打开串口，请打开后再发送", "提示",parent:this);
                    ckTime.Active = false;
                    return;
                }

                if (!uint.TryParse(txtTime.Buffer.Text, out uint sp))
                {
                    MessageBox.Show("周期格式不正确", "错误", parent: this);
                    ckTime.Active = false;
                    return;
                }
                txtTime.SetProperty("editable",new GLib.Value(false));
             
                timerSendId = GLib.Timeout.Add(sp, () =>
                {
                    sendData();
                    return true;
                });
            }
            else
            {
                if (timerSendId == 0)
                    return;
                txtTime.SetProperty("editable", new GLib.Value(true));
                GLib.Timeout.Remove(timerSendId);
            }
        }

        private void BtnSendClear_Clicked(object? sender, EventArgs e)
        {
            textSend.Buffer.Text = "";
            sendByteCount = 0;
            UpdateStatus();
        }

        private void BtnSend_Clicked(object? sender, EventArgs e)
        {
            sendData();
        }

        private void sendData()
        {
            if (mSerialPort != null && mSerialPort.IsOpen)
            {
                if ((ckNewline.Active) && (ckS16.Active))
                {
                    //发送16进制新行  
                    byte[] bytes = Utils.convertHexStringToBytes(textSend.Buffer.Text);
                    if (bytes != null)
                    {
                        mSerialPort.Write(bytes, 0, bytes.Length);
                        byte[] newLine = { 0xd, 0xa };
                        mSerialPort.Write(newLine, 0, newLine.Length);
                        sendByteCount += (bytes.Length + newLine.Length);
                    }
                }
                else if ((ckNewline.Active) && (!ckS16.Active))
                {
                    //发送普通字符串新行
                    mSerialPort.Write(textSend.Buffer.Text + "\r\n");
                    sendByteCount += (textSend.Buffer.Text.Length + 2);
                }
                else if ((!ckNewline.Active) && (!ckS16.Active))
                {
                    //发送普通字符串
                    mSerialPort.Write(textSend.Buffer.Text);
                    sendByteCount += textSend.Buffer.Text.Length;
                }
                else if ((!ckNewline.Active) && (ckS16.Active))
                {
                    //发送16进制
                    byte[] bytes = Utils.convertHexStringToBytes(textSend.Buffer.Text);
                    if (bytes != null)
                    {
                        mSerialPort.Write(bytes, 0, bytes.Length);
                        sendByteCount += bytes.Length;
                    }

                }
                else
                {
                    MessageBox.Show("不支持的操作！","错误", parent: this);
                }
                UpdateStatus();
            }
            else
            {
                MessageBox.Show("尚未打开串口，请打开后再发送","提示", parent: this);
            }
        }

        private void BtnClear_Clicked(object? sender, EventArgs e)
        {
            this.textResult.Buffer.Clear();
            recevieByteCount = 0;
            UpdateStatus();
        }

        private void BtnSp_Clicked(object? sender, EventArgs e)
        {
            if (mSerialPort != null)
            {
                if (mSerialPort.IsOpen)
                {
                    try
                    {
                        mSerialPort.Close();
                        btnSp.Label = "打开串口";
                        //定时发送取消勾选
                        ckTime.Active = false;
                        //更改按钮样式
                        btnSp.StyleContext.RemoveClass("btnRed");
                        btnSp.Image = Image.NewFromIconName("window-new", IconSize.Button);
                        img = new Gdk.Pixbuf("2.png", 30, 30);
                        this.imgRender.Pixbuf = img;
                        //改为可用状态
                        ChangeComConfigStatus(true);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("串口关闭失败", "错误", parent: this);
                    }
                }
                else
                {
                    OpenSerialPort();
                }
            }
            else
            {
                OpenSerialPort();
            }
            UpdateStatus();
        }

        private void OpenSerialPort()
        {
            if (comboCom.Active == -1)
            {
                MessageBox.Show("请先选择串口", "提示", parent: this); return;
            }
            if (comboRate.Active == -1)
            {
                MessageBox.Show("请设置波特率", "提示", parent: this); return;
            }
            if (comboData.Active == -1)
            {
                MessageBox.Show("请设置数据位", "提示", parent: this); return;
            }
            if (comboJo.Active == -1)
            {
                MessageBox.Show("请设置奇偶校验位", "提示", parent: this); return;
            }
            if (comboStop.Active == -1)
            {
                MessageBox.Show("请设置停止位", "提示", parent: this); return;
            }
            string com = GetComboxText(comboCom);
            int rate = int.Parse(GetComboxText(comboRate));
            int dataBits = int.Parse(GetComboxText(comboData));
            Parity parity = (Parity)Enum.Parse(typeof(Parity), GetComboxText(comboJo));
            StopBits stopBits = (StopBits)Enum.Parse(typeof(StopBits), GetComboxText(comboStop), true);

            var config = new ComConfig
            {
                PortName = com,
                BaudRate = GetComboxText(comboRate),
                DataBit = GetComboxText(comboData),
                Parity = GetComboxText(comboJo),
                StopBit = GetComboxText(comboStop)
            };

            InitCache(config);

            if (stopBits != StopBits.None)
            {
                mSerialPort = new SerialPort(com, rate, parity, dataBits, stopBits);
            }
            else
            {
                mSerialPort = new SerialPort(com, rate, parity, dataBits);
            }

            if (!mSerialPort.IsOpen)
            {
                try
                {
                    mSerialPort.Open();
                    btnSp.Label = "关闭串口";
                    //更改按钮样式
                    btnSp.StyleContext.AddClass("btnRed");
                    btnSp.Image = Image.NewFromIconName("window-close", IconSize.Button);
                    img = new Gdk.Pixbuf("1.png", 30, 30);
                    this.imgRender.Pixbuf = img;
                    ChangeComConfigStatus(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"打开串口失败:{ex.ToString()}", "提示", parent: this);
                    return;
                }

            }
            mSerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        }

        private void ChangeComConfigStatus(bool state)
        {
            comboCom.Sensitive = state;
            comboData.Sensitive = state;
            comboJo.Sensitive = state;
            comboRate.Sensitive = state;
            comboStop.Sensitive = state;
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            receiveThread = new Thread(new ParameterizedThreadStart(displayReceiveData));
            receiveThread.Start(indata);
        }

        private void displayReceiveData(object? obj)
        {
            if (ck16.Active)
            {
                AppendText(textResult, Utils.convertToHexString((string)obj));
                recevieByteCount = (textResult.Buffer.Text.Length + 1) / 3;
            }
            else
            {
                AppendText(textResult, (string)obj);
                recevieByteCount = textResult.Buffer.Text.Length;
            }
            UpdateStatus();
        }

        #endregion

        #region 接收设置
        private void ckDtrToggled(object? sender, EventArgs e)
        {
            if (mSerialPort != null)
            {
                if (ckDtr.Active)
                {
                    mSerialPort.DtrEnable = true;
                }
                else
                {
                    mSerialPort.DtrEnable = false;
                }
            }
        }

        private void ckRtxToggled(object? sender, EventArgs e)
        {
            if (mSerialPort != null)
            {
                if (ckRtx.Active)
                {
                    mSerialPort.RtsEnable = true;
                }
                else
                {
                    mSerialPort.RtsEnable = false;
                }
            }
        }

        private void ck16Toggled(object? sender, EventArgs e)
        {
            if (ck16.Active)
            {
                textResult.Buffer.Text = Utils.convertToHexString(textResult.Buffer.Text);
            }
            else
            {
                textResult.Buffer.Text = Utils.convertHexStringToCommonString(textResult.Buffer.Text);
            }
        }
        #endregion

        #region 辅助方法
        private string GetComboxText(ComboBox comboBox)
        {
            TreeIter tree;
            comboBox.GetActiveIter(out tree);
            String selectedText = (String)comboBox.Model.GetValue(tree, 0);
            return selectedText;
        }
        private void AppendText(TextView textView, string text)
        {
            var buffer = textView.Buffer;
            TextIter insertIter = buffer.EndIter;
            buffer.Insert(ref insertIter, text);
        }
        #endregion
    }
}
