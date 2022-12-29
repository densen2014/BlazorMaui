using System;
using SpiderEye.Windows;
using testsp.Core;

namespace testsp
{
    class Program : ProgramBase
    {
        [STAThread]
        public static void Main(string[] args)
        {
            WindowsApplication.Init();
            Run();
        }
    }
}
