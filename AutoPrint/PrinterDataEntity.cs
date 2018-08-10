using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoPrint
{
    public class PrinterDataEntity
    {
        public PrinterDataEntity()
        {
            PrintId = 0;
            Z_PD = 2;
            JCLSH = string.Empty;
        }
        public int PrintId { get; set; }

        public int Z_PD { get; set; }

        public string JCLSH { get; set; }

        public string JYLBDH { get; set; }
    }
}
