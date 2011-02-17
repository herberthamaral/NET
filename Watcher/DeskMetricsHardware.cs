// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - DeskMetricsHardware.cs                          //
//     Copyright (c) 2010-2011 DeskMetrics Limited                       //
//                                                                       //
//     http://deskmetrics.com                                            //
//     http://support.deskmetrics.com                                    //
//                                                                       //
//     support@deskmetrics.com                                           //
//                                                                       //
//     This code is provided under the DeskMetrics Modified BSD License  //
//     A copy of this license has been distributed in a file called      //
//     LICENSE with this source code.                                    //
//                                                                       //
// **********************************************************************//

using System;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Threading;
using System.Text;
using System.Management;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DeskMetrics
{
    internal class Hardware
    {
        /// <summary>
        /// Field Processor Name
        /// </summary>
        private string _processorName;
        /// <summary>
        /// Field Processor Archicteture
        /// </summary>
        private int _processorArchicteture;
        /// <summary>
        /// Field Processor Cores
        /// </summary>
        private int _processorCore;
        /// <summary>
        /// Field Memory total
        /// </summary>
        private double _memoryTotal;
        /// <summary>
        /// Field Memory free
        /// </summary>
        private double _memoryFree;
        /// <summary>
        /// Field Disk space total
        /// </summary>
        private long _diskTotal;
        /// <summary>
        /// Field Disk free
        /// </summary>
        private long _diskFree;
        /// <summary>
        /// Field Screen Resolution
        /// </summary>
        private string _screenResolution;
        /// <summary>
        /// Field Processor Brand
        /// </summary>
        private string _processorBrand;
        /// <summary>
        /// Field Processor Frequency
        /// </summary>
        private int _processorFrequency;

        /// <summary>
        /// GetProcessorFrequency and Set Processor Name
        /// </summary>
        public string ProcessorName
        {
            get
            {
                return _processorName;
            }
            set
            {
                _processorName = value;
            }
        }

        /// <summary>
        /// GetProcessorFrequency and Set Processor Archicteture
        /// </summary>
        public int ProcessorArchicteture
        {
            get
            {
                return _processorArchicteture;
            }
            set
            {
                _processorArchicteture = value;
            }
        }

        /// <summary>
        /// GetProcessorFrequency and Set Processor Cores
        /// </summary>
        public int ProcessorCores
        {
            get
            {
                return _processorCore;
            }
            set
            {
                _processorCore = value;
            }
        }

        /// <summary>
        /// GetProcessorFrequency and Set Memory total
        /// </summary>
        public double MemoryTotal
        {
            get
            {
                return _memoryTotal;
            }
            set
            {
                _memoryTotal = value;
            }
        }

        /// <summary>
        /// GetProcessorFrequency and Set Memory free
        /// </summary>
        public double MemoryFree
        {
            get
            {
                return _memoryFree;
            }
            set
            {
                _memoryFree = value;
            }
        }

        /// <summary>
        /// GetProcessorFrequency and Set Disk space total
        /// </summary>
        public long DiskTotal
        {
            get
            {
                return _diskTotal;
            }
            set
            {
                _diskTotal = value;
            }
        }

        /// <summary>
        /// GetProcessorFrequency and Set Disk space free
        /// </summary>
        public long DiskFree
        {
            get
            {
                return _diskFree;
            }
            set
            {
                _diskFree = value;
            }
        }

        /// <summary>
        /// GetProcessorFrequency and Set Screen Resolution
        /// </summary>
        public String ScreenResolution
        {
            get
            {
                return _screenResolution;
            }
            set
            {
                _screenResolution = value;
            }
        }

        /// <summary>
        /// GetProcessorFrequency and Set Processor Brand
        /// </summary>
        public string ProcessorBrand
        {
            get
            {
                return _processorBrand;
            }
            set
            {
                _processorBrand = value;
            }
        }

        /// <summary>
        /// GetProcessorFrequency and Set Processor Frequency
        /// </summary>
        public int ProcessorFrequency
        {
            get
            {
                return _processorFrequency;
            }
            set
            {
                _processorFrequency = value;
            }
        }

        /// <summary>
        /// GetProcessorFrequency Processor Archicteture GetComponentName
        /// </summary>
        public void GetProcessorData()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_Processor");
                foreach (ManagementObject sysItem in searcher.Get())
                {

                    try
                    {
                        string valuename = sysItem["Name"].ToString();

                        if (valuename != "")
                        {
                            valuename = valuename.Replace("(TM)", "");
                            valuename = valuename.Replace("(R)", "");
                            valuename = valuename.Replace("  ", "");

                            ProcessorName = valuename;
                        }
                    }
                    catch (Exception)
                    {
                        ProcessorName = "null";
                    }

                    try
                    {
                        string valuename = ProcessorName.ToLower();
                        if ((valuename.IndexOf("intel") != 0) || (valuename.IndexOf("pentium") != 0) || (valuename.IndexOf("celeron") != 0) || (valuename.IndexOf("genuineintel") != 0))
                        {
                            ProcessorBrand = "Intel";
                        }
                        else
                        {
                            if ((valuename.IndexOf("amd") != 0) || (valuename.IndexOf("athlon") != 0) || (valuename.IndexOf("sempron") != 0))
                            {
                                ProcessorBrand = "AMD";
                            }
                        }
                    }
                    catch 
                    {
                        ProcessorBrand = "null";
                    }

                    try
                    {
                        string valuearch = sysItem["AddressWidth"].ToString();
                        ProcessorArchicteture = int.Parse(valuearch);
                    }
                    catch 
                    {
                        ProcessorArchicteture = -1;
                    }

                    try
                    {
                        string valuecores = sysItem["NumberOfLogicalProcessors"].ToString();
                        ProcessorCores = int.Parse(valuecores);

                        if ((ProcessorCores <= 0) || (ProcessorCores == 1))
                        {
                            ProcessorCores = Environment.ProcessorCount;
                        }
                    }
                    catch
                    {
                        ProcessorCores = -1;
                    }

                    try
                    {
                        string valuefreq = sysItem["CurrentClockSpeed"].ToString();
                        ProcessorFrequency = int.Parse(valuefreq);
                    }
                    catch
                    {
                        ProcessorFrequency = -1;
                    }
                }

            }
            catch 
            {
            }
        }

        /// <summary>
        /// GetProcessorFrequency  Physical Memory  GetComponentName
        /// </summary>
        public void GetMemoryData()
        {

            try
            {

                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from CIM_OperatingSystem");
                foreach (ManagementObject sysItem in searcher.Get())
                {
                    string value  = sysItem["FreePhysicalMemory"].ToString();
                    string value2 = sysItem["TotalVisibleMemorySize"].ToString();
                    MemoryFree    = Convert.ToDouble(value) * 1024;
                    MemoryTotal   = Convert.ToDouble(value2) * 1024;
                }

            }
            catch
            {
                MemoryFree = -1;
                MemoryTotal = -1;
            }

        }

        /// <summary>
        /// GetProcessorFrequency Disk Size  GetComponentName
        /// </summary>

        public void GetDiskData()
        {
            try
            {
                string[] diretorios = Directory.GetLogicalDrives();
                foreach (string item in diretorios)
                {
                    if (Directory.Exists(item + "Windows"))
                    {
                        DriveInfo _drive = new DriveInfo(item);
                        DiskTotal = _drive.TotalSize;
                        DiskFree  = _drive.TotalFreeSpace;
                    }
                }

            }
            catch
            {
                DiskTotal = -1;
                DiskFree = -1;
            }
        }
        /// <summary>
        /// GetProcessorFrequency Screen resolution GetComponentName
        /// </summary>
        public void GetScreenResolution()
        {
            try
            {
                int deskHeight = Screen.PrimaryScreen.Bounds.Height;
                int deskWidth = Screen.PrimaryScreen.Bounds.Width;
                ScreenResolution = deskWidth + "x" + deskHeight;
            }
            catch
            {
                ScreenResolution = "null";
            }
        }    

    }
}
