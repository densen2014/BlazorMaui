using Foundation;
//using SQLitePCL;

namespace BlazorMaui
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp()
        {
            //对于 iOS/MacCatalyst，我们需要设置 SQLite 提供程序。我们可以做到AppDelegate
            //raw.SetProvider(new SQLite3Provider_sqlite3());
            return MauiProgram.CreateMauiApp();
        }
    }
}
