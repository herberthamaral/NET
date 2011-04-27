using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeskMetrics
{
    internal interface IHardware
    {
        string ProcessorName { get; set; }
        int ProcessorArchicteture { get; set; }
        int ProcessorCores { get; set; }
        double MemoryTotal { get; set; }
        double MemoryFree { get; set; }
        long DiskTotal { get; set; }
        long DiskFree { get; set; }
        String ScreenResolution { get; set; }
        string ProcessorBrand { get; set; }
        int ProcessorFrequency { get; set; }

    }
}
