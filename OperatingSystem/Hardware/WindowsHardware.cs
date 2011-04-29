// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - OperatingSystem/Hardware/WindowsHardware.cs     //
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
using System.Windows.Forms;
using System.Management;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace DeskMetrics.OperatingSystem.Hardware
{
	public class WindowsHardware:IHardware
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
		
		public WindowsHardware ()
		{
		}
		
		#region IHardware implementation
		public override  string ProcessorName {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public override  int ProcessorArchicteture {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public override  int ProcessorCores {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public override double MemoryTotal {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public override double MemoryFree {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public override long DiskTotal {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public override long DiskFree {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public override string ScreenResolution {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public override string ProcessorBrand {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public override int ProcessorFrequency {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		#endregion
		
		Dictionary<int, int> arch = new Dictionary<int, int>
        {
            {0,32}, //x86
            {1,32}, //MIPS
            {2,32}, //Alpha
            {3,32}, //PowerPC
            {6,32}, //Itanium
            {9,64} //x64
        };

        private Dictionary<int, int> Arch
        {
            get { return arch; }
        }
		
		
		void GetProcessorData()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_Processor");
                foreach (ManagementObject sysItem in searcher.Get())
                    GetProcessorDataFromManagementObject(sysItem);
            }
            catch 
            {
                //Probably Unix
            }
        }

        void GetProcessorDataFromManagementObject(ManagementObject sysItem)
        {
            GetProcessorName(sysItem);
            GetProcessorBrand(sysItem);
            GetProcessorArchitecture(sysItem);
            GetNumberOfProcessorCores(sysItem);
            GetProcessorFrequency(sysItem);
        }

        void GetProcessorFrequency(ManagementObject sysItem)
        {
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

        void GetNumberOfProcessorCores(ManagementObject sysItem)
        {
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
        }

        void GetProcessorArchitecture(ManagementObject sysItem)
        {
            try
            {

                // Relying on Architecture because AddressWidth is based on operating system
                // not on real processor architecture
                int valuearch = Arch[int.Parse(sysItem["Architecture"].ToString())];
                ProcessorArchicteture = valuearch;
            }
            catch
            {
                ProcessorArchicteture = -1;
            }
        }

        void GetProcessorBrand(ManagementObject sysItem)
        {
            try
            {
                ProcessorBrand = sysItem["Manufacturer"].ToString();
            }
            catch
            {
                ProcessorBrand = "null";
            }
        }

        void GetProcessorName(ManagementObject sysItem)
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
        }

        /// <summary>
        /// GetProcessorFrequency  Physical Memory  GetComponentName
        /// </summary>
        void GetMemoryData()
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
				try
				{
					string[] free = IOperatingSystem.GetCommandExecutionOutput("free","-m").Split('\n');
					string memoryinfo = free[1];
					Regex regex = new Regex(@"\d+");
					MatchCollection matches = regex.Matches(memoryinfo);
					double mega = 1024*1024;
	                MemoryFree = Int32.Parse(matches[2].ToString())*mega;
	                MemoryTotal = Int32.Parse(matches[0].ToString())*mega;
				}
				catch
				{
					MemoryFree = 0;
					MemoryTotal = 0;
				}
            }

        }

        /// <summary>
        /// GetProcessorFrequency Disk Size  GetComponentName
        /// </summary>

        void GetDiskData()
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
        void GetScreenResolution()
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

