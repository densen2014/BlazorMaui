// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MauiApp7test.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        public static object CurrentWindow;
        public static AppWindow AppWindow;
        
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            CurrentWindow = Application.Windows[0].Handler?.PlatformView;
            IntPtr _windowHandle = WindowNative.GetWindowHandle(CurrentWindow);
            var windowId = Win32Interop.GetWindowIdFromWindow(_windowHandle);
            AppWindow = AppWindow.GetFromWindowId(windowId);
            
            SetTitle("MauiApp7test");
        }
        public static void SetTitle(string title) => AppWindow.Title = title;


        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
