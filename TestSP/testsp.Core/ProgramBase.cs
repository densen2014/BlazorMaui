using SpiderEye;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace testsp.Core
{
    public abstract class ProgramBase
    {
        protected static void Run()
        {
            using (var window = new Window())
            {
                window.Title = "testsp";
                window.Icon = AppIcon.FromFile("icon", ".");
                window.UseBrowserTitle = true;
                window.EnableScriptInterface = true;
                window.CanResize = true;
                window.BackgroundColor = "#303030";
                window.Size = new Size(800, 600);
                window.MinSize = new Size(300, 200);
                window.MaxSize = new Size(3200, 2900);

                window.EnableDevTools = true;

                var menu = new Menu();
                var showItem = menu.MenuItems.AddLabelItem("Hello World");
                showItem.SetShortcut(ModifierKey.Primary, Key.O);
                showItem.Click += UiBridge.ShowItem_Click;

                var eventItem = menu.MenuItems.AddLabelItem("Send Event to Webview");
                eventItem.SetShortcut(ModifierKey.Primary, Key.E);
                eventItem.Click += async (s, e) => await window.Bridge.InvokeAsync("dateUpdated", DateTime.Now);

                var subMenuItem = menu.MenuItems.AddLabelItem("Open me!");
                subMenuItem.MenuItems.AddLabelItem("Boo!");

                var borderItem = menu.MenuItems.AddLabelItem("Window Border");
                var def = borderItem.MenuItems.AddLabelItem("Default");
                def.Click += (s, e) => { window.BorderStyle = WindowBorderStyle.Default; };
                var none = borderItem.MenuItems.AddLabelItem("None");
                none.Click += (s, e) => { window.BorderStyle = WindowBorderStyle.None; };

                var sizeItem = menu.MenuItems.AddLabelItem("Window Size");
                var max = sizeItem.MenuItems.AddLabelItem("Maximize");
                max.Click += (s, e) => { window.Maximize(); };
                var unmax = sizeItem.MenuItems.AddLabelItem("Unmaximize");
                unmax.Click += (s, e) => { window.Unmaximize(); };
                var min = sizeItem.MenuItems.AddLabelItem("Minimize");
                min.Click += (s, e) => { window.Minimize(); };
                var unmin = sizeItem.MenuItems.AddLabelItem("Unminimize");
                unmin.Click += (s, e) => { window.Unminimize(); };
                var full = sizeItem.MenuItems.AddLabelItem("Enter Fullscreen");
                full.Click += (s, e) => { window.EnterFullscreen(); };
                var unfull = sizeItem.MenuItems.AddLabelItem("Exit Fullscreen");
                unfull.SetShortcut(ModifierKey.Primary, Key.F11);
                unfull.Click += (s, e) => { window.ExitFullscreen(); };

                menu.MenuItems.AddSeparatorItem();

                var exitItem = menu.MenuItems.AddLabelItem("Exit");
                exitItem.Click += (s, e) => Application.Exit();


                var bridge = new UiBridge();
                Application.AddGlobalHandler(bridge);

                Application.ContentProvider = new EmbeddedContentProvider("App");
                Application.Run(window, "index.html");
            }
        }
    }

    public class UiBridge
    {
        private readonly Random random = new();

        public async Task RunLongProcedureOnTask()
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
        }

        public void RunLongProcedure()
        {
            Thread.Sleep(TimeSpan.FromSeconds(10));
        }

        public SomeDataModel GetSomeData()
        {
            return new SomeDataModel
            {
                Text = "Hello World",
                Number = random.Next(100),
            };
        }

        public double Power(PowerModel model)
        {
            return Math.Pow(model.Value, model.Power);
        }

        public void ProduceError()
        {
            throw new Exception("Intentional exception from .Net");
        }

        public class SomeDataModel
        {
            public string Text { get; set; }
            public int Number { get; set; }
        }
        public class PowerModel
        {
            public double Value { get; set; }
            public double Power { get; set; }
        }

        public static void ShowItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                $"Hello World from the SpiderEye Playground running on {Application.OS}",
                "Hello World",
                MessageBoxButtons.Ok);
        }
    }

}
