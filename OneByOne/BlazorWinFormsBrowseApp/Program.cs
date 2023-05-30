// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

namespace BlazorWinFormsBrowseApp;

internal static class Program
{
    public static Form1 form1=new Form1();
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Startup.Init();
        ApplicationConfiguration.Initialize();
        Application.Run(form1);
    }
}
