using BlazorShared.Services;
using Extensions;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace LibraryShared
{
    public class WpfService : ITools
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
            return SerialPort.GetPortNames().ToList();
        }
        public string CacheDirectory() => AppDomain.CurrentDomain.BaseDirectory;
        public string AppDataDirectory() => AppDomain.CurrentDomain.BaseDirectory;

        public Task<string> Print()
        {
            if (MessageBox.Show("列出打印机点[是], 模拟打印点[否]", "WPF", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                string printerList = string.Join(Environment.NewLine, System.Drawing.Printing.PrinterSettings.InstalledPrinters.Cast<string>());
                return Task.FromResult($"InstalledPrinters{Environment.NewLine}{printerList}");

            }
            string Filepath = AppDomain.CurrentDomain.BaseDirectory + "\\test.txt";
            System.IO.File.WriteAllText(Filepath, "test");

            PrintDialog Dialog = new PrintDialog();

            Dialog.ShowDialog();

            ProcessStartInfo printProcessInfo = new ProcessStartInfo()
            {
                Verb = "print",
                CreateNoWindow = true,
                FileName = Filepath,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            //Process printProcess = new Process();
            //printProcess.StartInfo = printProcessInfo;
            //printProcess.Start();

            //printProcess.WaitForInputIdle();

            //Thread.Sleep(3000);

            //if (false == printProcess.CloseMainWindow())
            //{
            //    printProcess.Kill();
            //} 
            return Task.FromResult("模拟打印成功");
        }

        public Task<string> ReadNFC()
        {
            return Task.FromResult("未实现");
        }

        public Task<string> ExtDSP()
        {
            return Task.FromResult("未实现");
        }
        
    }


}
