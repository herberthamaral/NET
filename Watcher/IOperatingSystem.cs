using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeskMetrics
{
    internal interface IOperatingSystem
    {
        string FrameworkVersion { get; set; }
        int Architecture { get; set; }
        string Language { get; set; }
        int Lcid { get; set; }
        string JavaVersion { get; set; }
        string ServicePack { get; set; }
    }
}
