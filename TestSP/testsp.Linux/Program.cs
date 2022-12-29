using System;
using SpiderEye.Linux;
using testsp.Core;

namespace testsp
{
    class Program : ProgramBase
    {
        public static void Main(string[] args)
        {
            LinuxApplication.Init();
            Run();
        }
    }
}
