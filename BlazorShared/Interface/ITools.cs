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

        double? DistanceBetweenTwoLocations();

        void ShowSettingsUI();
        string? GetAppInfo();
        Task<string> NavigateToMadrid();
        Task<string> NavigateToPlazaDeEspana();
        Task<string> NavigateToPlazaDeEspanaByPlacemark();
        Task<string> DriveToPlazaDeEspana();
        Task<string> TakeScreenshotAsync();
        
        string? CacheDirectory();
        string? AppDataDirectory();

        /// <summary>
        /// 获取串口列表
        /// </summary>
        /// <returns></returns>
        List<string>? GetPortlist();

        /// <summary>
        /// 打印
        /// </summary>
        /// <returns></returns>
        Task<string> Print();

        /// <summary>
        /// 读NFC
        /// </summary>
        /// <returns></returns>
        Task<string> ReadNFC();

        /// <summary>
        /// 客户显示屏
        /// </summary>
        /// <returns></returns>
        Task<string> ExtDSP();

    }

 }
