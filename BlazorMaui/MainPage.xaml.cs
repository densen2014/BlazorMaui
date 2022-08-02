using Android.Webkit;
using Microsoft.Maui.Controls;
using System;

namespace BlazorMaui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private  void BlazorWebView_BlazorWebViewInitialized(object sender, Microsoft.AspNetCore.Components.WebView.BlazorWebViewInitializedEventArgs e)
        {

#if ANDROID
            e.WebView.SetWebChromeClient(new MauiWebChromeClient());
#endif
        }

        public class MauiWebChromeClient : WebChromeClient
        {
            public override async void OnPermissionRequest(PermissionRequest request)
            {
                request.Grant(request.GetResources());
                await CheckAndRequestLocationPermission();
                var location = await GetCurrentLocation();
            }
        }

        //参考
        //Permissions
        //https://docs.microsoft.com/en-us/dotnet/maui/platform-integration/appmodel/permissions?tabs=android
        //Geolocation
        //https://docs.microsoft.com/en-us/dotnet/maui/platform-integration/device/geolocation?tabs=windows

        public static async Task<PermissionStatus> CheckAndRequestLocationPermission()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<CameraAndLocationPerms>();
 
            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<CameraAndLocationPerms>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<CameraAndLocationPerms>(); 

            return status;
        }

        /// <summary>
        /// 请求摄像机和位置
        /// </summary>
        public class CameraAndLocationPerms : Permissions.BasePlatformPermission
        {
            public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
                new List<(string androidPermission, bool isRuntime)>
                {
                    (global::Android.Manifest.Permission.Camera, true),
                    (global::Android.Manifest.Permission.CaptureAudioOutput, true),
                    (global::Android.Manifest.Permission.CaptureSecureVideoOutput, true),
                    (global::Android.Manifest.Permission.CaptureVideoOutput, true),
                    (global::Android.Manifest.Permission.LocationHardware, true),
                    (global::Android.Manifest.Permission.AccessFineLocation, true),
                    (global::Android.Manifest.Permission.AccessLocationExtraCommands, true),
                    (global::Android.Manifest.Permission.AccessNetworkState, true),
                    (global::Android.Manifest.Permission.CallPhone, true),
                    (global::Android.Manifest.Permission.Flashlight, true),
                    (global::Android.Manifest.Permission.RecordAudio, true),
                    (global::Android.Manifest.Permission.Vibrate , true),
                    (global::Android.Manifest.Permission.WriteSettings , true),
                }.ToArray();
        }

        /// <summary>
        /// 请求读取和写入存储访问
        /// </summary>
        public class ReadWriteStoragePerms : Permissions.BasePlatformPermission
        {
            public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
                new List<(string androidPermission, bool isRuntime)>
                {
                    (global::Android.Manifest.Permission.ReadExternalStorage, true),
                    (global::Android.Manifest.Permission.WriteExternalStorage, true)
                }.ToArray();
        }

        public async Task<string> GetCachedLocation()
        {
            try
            {
                Location location = await Geolocation.Default.GetLastKnownLocationAsync();

                if (location != null)
                    return $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }

            return "None";
        }

        private static CancellationTokenSource _cancelTokenSource;
        private static bool _isCheckingLocation;

        public static async Task<string> GetCurrentLocation()
        {
            try
            {
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

#if IOS
request.RequestFullAccuracy = true;
#endif

                Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                if (location != null) { 
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    return $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
                }
            }
            // Catch one of the following exceptions:
            //   FeatureNotSupportedException
            //   FeatureNotEnabledException
            //   PermissionException
            catch (Exception ex)
            {
                // Unable to get location
            }
            finally
            {
                _isCheckingLocation = false;
            }
            return "None";
        }

        public void CancelRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
                _cancelTokenSource.Cancel();
        }

        Location boston = new Location(42.358056, -71.063611);
        Location sanFrancisco = new Location(37.783333, -122.416667);

        public void DistanceBetweenTwoLocations()
        {
            double miles = Location.CalculateDistance(boston, sanFrancisco, DistanceUnits.Miles);
        }
    }
}
