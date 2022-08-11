#if WINDOWS
using Windows.Storage;
#endif
#if ANDROID
using Android.Webkit;
#endif
using BlazorShared.Services;

using System.Security.Permissions;

namespace LibraryShared
{
    public class TestService : ITools
    {

        //        public async void BlazorWebView_BlazorWebViewInitialized(object sender, Microsoft.AspNetCore.Components.WebView.BlazorWebViewInitializedEventArgs e)
        //        {

        //#if ANDROID
        //            e.WebView.SetWebChromeClient(new MauiWebChromeClient());
        //            await CheckAndRequestLocationPermission();
        //#endif 


        //        }
        public void ShowSettingsUI()
        {
            //显示应用设置
            AppInfo.Current.ShowSettingsUI();
        }
        public string GetAppInfo()
        {
            //读取应用信息
            string name = AppInfo.Current.Name;
            string package = AppInfo.Current.PackageName;
            string version = AppInfo.Current.VersionString;
            string build = AppInfo.Current.BuildString;
            return $"{name},{version}.{build}";
        }

        //参考
        //Permissions
        //https://docs.microsoft.com/en-us/dotnet/maui/platform-integration/appmodel/permissions?tabs=android
        //Geolocation
        //https://docs.microsoft.com/en-us/dotnet/maui/platform-integration/device/geolocation?tabs=windows

#if ANDROID
        public async Task<PermissionStatus> CheckAndRequestLocationPermission()
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
#if ANDROID
#elif IOS || MACCATALYST
#else
//WINDOWS
#endif
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

#endif


#if ANDROID

        public class MauiWebChromeClient : WebChromeClient
        {
            public override void OnPermissionRequest(PermissionRequest request)
            {
                request.Grant(request.GetResources());
            }
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


#endif

        public async Task<string> CheckPermissionsCamera()
        {
            //检查权限的当前状态
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Camera>();

            //请求权限
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
            }

            return status.ToString();
        }
        public async Task<string> CheckPermissionsLocation()
        {
            //检查权限的当前状态
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            //请求权限
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            return status.ToString();
        }

        /// <summary>
        /// 拍照
        /// CapturePhotoAsync调用该方法以打开相机，让用户拍照。 如果用户拍照，该方法的返回值将是非 null 值。
        /// 以下代码示例使用媒体选取器拍摄照片并将其保存到缓存目录：
        /// </summary>
        public async Task<string> TakePhoto()
        {
            await CheckPermissionsCamera();

            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    // save the file into local storage
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    using Stream sourceStream = await photo.OpenReadAsync();
                    using FileStream localFileStream = File.OpenWrite(localFilePath);

                    await sourceStream.CopyToAsync(localFileStream);
                    return localFilePath;
                }
                else
                {
                    photo = await MediaPicker.Default.PickPhotoAsync();
                    if (photo != null)
                    {
                        return photo.FullPath;
                    }
                }
                return "photo null";

            }

            return null;
        }

        /// <summary>
        /// 获取最后一个已知位置, 设备可能已缓存设备的最新位置。
        /// 使用此方法 GetLastKnownLocationAsync 访问缓存的位置（如果可用）。
        /// 这通常比执行完整位置查询更快，但可能不太准确。
        /// 如果不存在缓存位置，此方法将 null返回 。
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetCachedLocation()
        {
            await CheckPermissionsLocation();
            string result = null;
            try
            {
                Location location = await Geolocation.Default.GetLastKnownLocationAsync();

                if (location != null)
                {
                    result = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
                    Console.WriteLine(result);
                    return result;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                result = $"not supported on device, {fnsEx.Message}";
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                result = $"not enabled on device, {fneEx.Message}";
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                result = $"permission, {pEx.Message}";
            }
            catch (Exception ex)
            {
                // Unable to get location
                result = $"Unable to get location, {ex.Message}";
            }

            return result ?? "None";
        }

        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;


        /// <summary>
        /// 获取当前位置
        /// 虽然检查设备 的最后已知位置 可能更快，但它可能不准确。
        /// 使用该方法 GetLocationAsync 查询设备的当前位置。
        /// 可以配置查询的准确性和超时。
        /// 最好是使用 GeolocationRequest 和 CancellationToken 参数的方法重载，
        /// 因为可能需要一些时间才能获取设备的位置。
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetCurrentLocation()
        {
            await CheckPermissionsLocation();
            string result = null;
            try
            {
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

#if IOS
                //从 iOS 14 开始，用户可能会限制应用检测完全准确的位置。
                //该 Location.ReducedAccuracy 属性指示位置是否使用降低的准确性。
                //若要请求完全准确性，请将 GeolocationRequest.RequestFullAccuracy 属性设置为 true
                request.RequestFullAccuracy = true;
#endif

                Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                if (location != null)
                {
                    result = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
                    Console.WriteLine(result);
                    return result;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                result = $"not supported on device, {fnsEx.Message}";
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                result = $"not enabled on device, {fneEx.Message}";
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                result = $"permission, {pEx.Message}";
            }
            catch (Exception ex)
            {
                // Unable to get location
                result = $"Unable to get location, {ex.Message}";
            }
            finally
            {
                _isCheckingLocation = false;
            }
            return result ?? "None";
        }

        public void CancelRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
                _cancelTokenSource.Cancel();
        }

        /// <summary>
        ///检测模拟位置
        ///一些设备可能会从提供程序或通过提供模拟位置的应用程序返回模拟位置。
        ///可以使用任意Location值来检测此情况IsFromMockProvider：
        /// </summary>
        /// <returns></returns>
        public async Task<string> CheckMock()
        {
            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium);
            Location location = await Geolocation.Default.GetLocationAsync(request);

            if (location != null && location.IsFromMockProvider)
            {
                return "location is from a mock provider";
            }
            return "null";
        }

        //美国波士顿和旧金山美国之间的距离
        Location boston = new Location(42.358056, -71.063611);
        Location sanFrancisco = new Location(37.783333, -122.416667);

        /// <summary>
        /// 两个位置之间的距离
        /// 该方法 Location.CalculateDistance 计算两个地理位置之间的距离。
        /// 此计算距离不考虑道路或其他路径，只是地球表面两点之间的最短距离。
        /// 此计算称为 大圆距离 计算
        /// </summary>
        public double DistanceBetweenTwoLocations()
        {
            return Location.CalculateDistance(boston, sanFrancisco, DistanceUnits.Miles);
        }

        /// <summary>
        /// 使用地图
        /// </summary>
        /// <returns></returns>
        public async Task<string> NavigateToBuilding25()
        {
            var location = new Location(47.645160, -122.1306032);
            var options = new MapLaunchOptions { Name = "Microsoft Building 25" };

            try
            {
                if (await Map.Default.TryOpenAsync(location, options) == false)
                {
                    return "Map failed to open";
                }
                return "ok";
            }
            catch (Exception ex)
            {
                return $"No map application available to open, Message: {ex.Message}";
            }
        }

        /// <summary>
        /// 使用 a Placemark 打开地图时，需要更多信息。 此信息可帮助地图应用搜索要查找的位置。
        /// </summary>
        /// <returns></returns>
        public async Task<string> NavigateToBuilding()
        {
            var placemark = new Placemark
            {
                CountryName = "United States",
                AdminArea = "WA",
                Thoroughfare = "Microsoft Building 25",
                Locality = "Redmond"
            };
            var options = new MapLaunchOptions { Name = "Microsoft Building 25" };

            try
            {
                await Map.Default.OpenAsync(placemark, options);
                return "ok";
            }
            catch (Exception ex)
            {
                return $"No map application available to open or placemark can not be located, Message: {ex.Message}";
            }
        }

        /// <summary>
        /// 扩展方法
        /// </summary>
        /// <returns></returns>
        public async Task<string> NavigateToBuildingByPlacemark()
        {
            var placemark = new Placemark
            {
                CountryName = "United States",
                AdminArea = "WA",
                Thoroughfare = "Microsoft Building 25",
                Locality = "Redmond"
            };

            var options = new MapLaunchOptions { Name = "Microsoft Building 25" };

            try
            {
                await placemark.OpenMapsAsync(options);
                return "ok";
            }
            catch (Exception ex)
            {
                return $"No map application available to open or placemark can not be located, Message: {ex.Message}";
            }
        }



        /// <summary>
        /// 添加导航
        /// <para></para>
        /// 打开地图时，可以计算从设备的当前位置到指定位置的路由。 将 MapLaunchOptions 类型传递给 Map.OpenAsync 方法，指定导航模式。 以下示例打开地图应用并指定驾驶导航模式：
        /// </summary>
        /// <returns></returns>
        public async Task<string> DriveToBuilding25()
        {
            var location = new Location(47.645160, -122.1306032);
            var options = new MapLaunchOptions
            {
                Name = "Microsoft Building 25",
                NavigationMode = NavigationMode.Driving
            };

            try
            {
                await Map.Default.OpenAsync(location, options);
                return "ok";
            }
            catch (Exception ex)
            {
                return $"No map application available to open, Message: {ex.Message}";
            }
        }

        /// <summary>
        /// 捕获屏幕快照
        /// </summary>
        /// <returns></returns>
        public async Task<string> TakeScreenshotAsync()
        {
            if (Screenshot.Default.IsCaptureSupported)
            {
                IScreenshotResult screen = await Screenshot.Default.CaptureAsync();

                Stream stream = await screen.OpenReadAsync();

                //var imageSource= ImageSource.FromStream(() => stream);
                string localFilePath = Path.Combine(FileSystem.CacheDirectory, $"{Guid.NewGuid()}.png");
                using FileStream localFileStream = File.OpenWrite(localFilePath);

                await stream.CopyToAsync(localFileStream);
                return localFilePath; 
            }

            return null;
        }

    }


}
