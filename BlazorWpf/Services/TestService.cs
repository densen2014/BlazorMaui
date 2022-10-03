﻿using BlazorShared.Services;
using System.Reflection;

namespace LibraryShared
{
    public class TestService : ITools
    {
        public Task<string> CheckPermissionsCamera() => Task.FromResult("未实现");
        public Task<string> CheckPermissionsLocation() => Task.FromResult("未实现");
        public Task<string> CheckMock() => Task.FromResult("未实现");

        public double DistanceBetweenTwoLocations() => 0;

        public Task<string> GetCachedLocation() => Task.FromResult("未实现");

        public Task<string> GetCurrentLocation() => Task.FromResult("未实现");
        public Task<string> TakePhoto() => Task.FromResult("未实现");
        public void ShowSettingsUI() { }
        public string GetAppInfo() => $"{Assembly.GetExecutingAssembly().GetName().Name}-{Assembly.GetExecutingAssembly().GetName().Version}";
        public Task<string> NavigateToBuilding25() => Task.FromResult("未实现");
        public Task<string> NavigateToBuilding() => Task.FromResult("未实现");
        public Task<string> NavigateToBuildingByPlacemark() => Task.FromResult("未实现");
        public Task<string> DriveToBuilding25() => Task.FromResult("未实现");
        public Task<string> TakeScreenshotAsync() => Task.FromResult("未实现");

       
    }


}