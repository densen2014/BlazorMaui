using Android;
using Android.App;
using Android.Runtime;

#region Geolocation 
//using Application = Microsoft.Maui.Controls.Application;
//[assembly: UsesPermission(Android.Manifest.Permission.AccessCoarseLocation)]
//[assembly: UsesPermission(Android.Manifest.Permission.AccessFineLocation)]
//[assembly: UsesFeature("android.hardware.location", Required = false)]
//[assembly: UsesFeature("android.hardware.location.gps", Required = false)]
//[assembly: UsesFeature("android.hardware.location.network", Required = false)]

////Android 10 - Q (API Level 29 or higher)
//[assembly: UsesPermission(Manifest.Permission.AccessBackgroundLocation)]
#endregion 

namespace BlazorMaui
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
