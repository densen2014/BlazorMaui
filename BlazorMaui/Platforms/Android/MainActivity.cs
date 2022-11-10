using Android.App;
using Android.Content.PM;
using Android.OS;

namespace BlazorMaui
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        //protected override void OnCreate(Bundle savedInstanceState)
        //{
        //    base.OnCreate(savedInstanceState);

        //    ActivityCompat.RequestPermissions(this, new[] {
        //        Android.Manifest.Permission.ModifyAudioSettings,
        //        Android.Manifest.Permission.Camera,
        //        Android.Manifest.Permission.CaptureAudioOutput,
        //        Android.Manifest.Permission.CaptureSecureVideoOutput,
        //        Android.Manifest.Permission.CaptureVideoOutput,
        //        Android.Manifest.Permission.LocationHardware,
        //        Android.Manifest.Permission.AccessFineLocation,
        //        Android.Manifest.Permission.AccessLocationExtraCommands,
        //        Android.Manifest.Permission.AccessNetworkState,
        //        Android.Manifest.Permission.CallPhone,
        //        Android.Manifest.Permission.Flashlight,
        //        Android.Manifest.Permission.RecordAudio,
        //        Android.Manifest.Permission.Vibrate ,
        //        Android.Manifest.Permission.WriteSettings ,
        //    }, 0);
        //}
    }
}
