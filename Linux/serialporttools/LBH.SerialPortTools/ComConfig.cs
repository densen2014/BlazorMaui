using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBH.SerialPortTools
{
    internal class ComConfig
    {
        public string PortName { get; set; }
        public string BaudRate { get; set; }
        public string StopBit { get; set; }
        public string DataBit { get; set; }
        public string  Parity { get; set; }
    }
}
