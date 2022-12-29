using System;
using SpiderEye.Mac;
using testsp.Core;

namespace testsp
{
    class Program : ProgramBase
    {
        public static void Main(string[] args)
        {
            MacApplication.Init();
            Run();
        }
    }
}
