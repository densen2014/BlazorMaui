using BlazorShared.Services;
using System.Reflection;

namespace LibraryShared
{
    public class TestService : ITools
    {
        public Task<string> CheckPermissionsCamera() => Task.FromResult("未实现");
        public Task<string> CheckPermissionsLocation() => Task.FromResult("未实现");
        public Task<string> CheckMock() => Task.FromResult("未实现");

        public double? DistanceBetweenTwoLocations() => 0;

        public Task<string> GetCachedLocation() => Task.FromResult("未实现");

        public Task<string> GetCurrentLocation() => Task.FromResult("未实现");
        public Task<string> TakePhoto() => Task.FromResult("未实现");
        public void ShowSettingsUI() { }
        public string GetAppInfo() => $"{Assembly.GetExecutingAssembly().GetName().Name}-{Assembly.GetExecutingAssembly().GetName().Version}";
        public Task<string> NavigateToMadrid() => Task.FromResult("未实现");
        public Task<string> NavigateToPlazaDeEspana() => Task.FromResult("未实现");
        public Task<string> NavigateToPlazaDeEspanaByPlacemark() => Task.FromResult("未实现");
        public Task<string> DriveToPlazaDeEspana() => Task.FromResult("未实现");
        public Task<string> TakeScreenshotAsync() => Task.FromResult("未实现");

        public List<string> GetPortlist()
        {
            return System.IO.Ports.SerialPort.GetPortNames().ToList();
        }
        public string CacheDirectory() => AppDomain.CurrentDomain.BaseDirectory;
        public string AppDataDirectory() => AppDomain.CurrentDomain.BaseDirectory;
    }


}
