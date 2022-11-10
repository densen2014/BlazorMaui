namespace BlazorShared.Services
{
    public interface ITools
    {
        Task<string> CheckPermissionsCamera();
        Task<string> TakePhoto();

        Task<string> CheckPermissionsLocation();
        Task<string> GetCachedLocation();

        Task<string> GetCurrentLocation();

        Task<string> CheckMock();

        double DistanceBetweenTwoLocations();

        void ShowSettingsUI();
        string GetAppInfo();
        Task<string> NavigateToMadrid();
        Task<string> NavigateToPlazaDeEspana();
        Task<string> NavigateToPlazaDeEspanaByPlacemark();
        Task<string> DriveToPlazaDeEspana();
        Task<string> TakeScreenshotAsync();
        List<string> GetPortlist();
    }

    public class NullService : ITools
    {
        public Task<string> CheckPermissionsCamera() => Task.FromResult("未实现");
        public Task<string> CheckPermissionsLocation() => Task.FromResult("未实现");
        public Task<string> CheckMock() => Task.FromResult("未实现");

        public double DistanceBetweenTwoLocations() => 0;

        public Task<string> GetCachedLocation() => Task.FromResult("未实现");

        public Task<string> GetCurrentLocation() => Task.FromResult("未实现");
        public Task<string> TakePhoto() => Task.FromResult("未实现");
        public void ShowSettingsUI() { }
        public string GetAppInfo() => "未实现";
        public Task<string> NavigateToMadrid() => Task.FromResult("未实现");
        public Task<string> NavigateToPlazaDeEspana() => Task.FromResult("未实现");
        public Task<string> NavigateToPlazaDeEspanaByPlacemark() => Task.FromResult("未实现");
        public Task<string> DriveToPlazaDeEspana() => Task.FromResult("未实现");
        public Task<string> TakeScreenshotAsync() => Task.FromResult("未实现");

#if WINDOWS
        public List<string> GetPortlist()
        {
            return System.IO.Ports.SerialPort.GetPortNames().ToList();
        }
#elif ANDROID || IOS || MACCATALYST
        public List<string> GetPortlist()
        {
            if (OperatingSystem.IsIOS() || OperatingSystem.IsMacCatalyst())
            {
                return null;
            }
            else if (OperatingSystem.IsAndroid())
            {
                return null;
            }
            else
            {
                return null;
            } 
            
        }
#else
        public List<string> GetPortlist()
        {
            if (OperatingSystem.IsWindows())
            {
                return System.IO.Ports.SerialPort.GetPortNames().ToList();
            }
            else
            {
                return null;
            }
        }
#endif

    }
}
