using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeskMetrics.OperatingSystem.Hardware
{
    public abstract class IHardware
    {
        public abstract string ProcessorName { get; set; }
        public abstract int ProcessorArchicteture { get; set; }
        public abstract int ProcessorCores { get; set; }
        public abstract double MemoryTotal { get; set; }
        public abstract double MemoryFree { get; set; }
        public abstract long DiskTotal { get; set; }
        public abstract long DiskFree { get; set; }
        public abstract string ScreenResolution { get; set; }
        public abstract string ProcessorBrand { get; set; }
        public abstract int ProcessorFrequency { get; set; }

    }
}
