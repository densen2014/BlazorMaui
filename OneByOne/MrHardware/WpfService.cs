using System.Diagnostics;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;

namespace MrHardware;

public class WpfService
{

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
         
        return Task.FromResult("模拟打印成功");
    }

    
}
