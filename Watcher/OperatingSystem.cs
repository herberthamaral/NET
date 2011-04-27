// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - OperatingSystem.cs                              //
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
using System.Diagnostics;



namespace DeskMetrics
{
    internal class OperatingSystem:IOperatingSystem
    {
        /// <summary>
        /// Field Framework ApplicationVersion
        /// </summary>
        private string _frameworkVersion;
        /// <summary>
        /// Field OS Archicteture - 32 or 64 bits
        /// </summary>
        private int _Architecture;
        /// <summary>
        /// Field OS ApplicationVersion
        /// </summary>
        private string _Name;
        /// <summary>
        /// Field Framework Service Pack
        /// </summary>
        private string _frameworkServicePack;
        /// <summary>
        /// Field OS LCID - Language Culture Id
        /// </summary>
        private int _LCID;
        /// <summary>
        /// Field OS Language
        /// </summary>
        private string _language;
        /// <summary>
        /// Field JavaVersion
        /// </summary>
        private string _javaVersion;
        /// <summary>
        /// Field OS Service Pack
        /// </summary>
        private string _servicePack;

        /// <summary>
        /// GetProcessorFrequency and Set Framework ApplicationVersion
        /// </summary>
        public string FrameworkVersion
        {
            get
            {
                return _frameworkVersion;
            }
            set
            {
                _frameworkVersion = value;
            }
        }

        /// <summary>
        /// GetProcessorFrequency and Set OS Archicteture
        /// </summary>
        public int Architecture
        {
            get
            {
                return _Architecture;
            }
            set
            {
                _Architecture = value;
            }
        }

        /// <summary>
        /// GetProcessorFrequency and Set OS Language
        /// </summary>
        public string Language
        {
            get
            {
                return _language;
            }
            set
            {
                _language = value;
            }
        }

        /// <summary>
        /// GetProcessorFrequency and Set OS ApplicationVersion
        /// </summary>
        public string Version
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        /// <summary>
        /// GetProcessorFrequency and Set Frameworl Service Pack
        /// </summary>
        public string FrameworkServicePack
        {
            get
            {
                return _frameworkServicePack;
            }
            set
            {
                _frameworkServicePack = value;
            }
        }

        /// <summary>
        /// OS LCID - Language Culture Id
        /// </summary>
        public int Lcid
        {
            get
            {
                return _LCID;
            }
            set
            {
                _LCID = value;
            }
        }

        /// <summary>
        /// GetProcessorFrequency and Set Java ApplicationVersion
        /// </summary>
        public string JavaVersion
        {
            get
            {
                return _javaVersion;
            }
            set
            {
                _javaVersion = value;
            }
        }

        /// <summary>
        /// GetProcessorFrequency and Set OS Service Pack
        /// </summary>
        public string ServicePack
        {
            get
            {
                return _servicePack;
            }
            set
            {
                _servicePack = value;
            }
        }

        /// <summary>
        /// GetProcessorFrequency Framework ApplicationVersion GetComponentName
        /// </summary>
        public void GetFrameworkVersion()
        {
            try
            {
                FrameworkVersion = "none";
                FrameworkServicePack = "";

                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\NET Framework Setup\NDP");
                if (key != null)
                {
                    if ((key.OpenSubKey("v4")) != null)
                    {
                        FrameworkVersion     = "4.0";
                        FrameworkServicePack = "0"; 
                        return;
                    }

                    if ((key.OpenSubKey("v3.5")) != null)
                    {
                        FrameworkVersion     = "3.5";
                        FrameworkServicePack = key.GetValue("SP", 0).ToString();
                        return;
                    }

                    if ((key.OpenSubKey("v3.0")) != null)
                    {
                        FrameworkVersion     = "3.0";
                        FrameworkServicePack = key.GetValue("SP", 0).ToString();
                        return;
                    }

                    if ((key.OpenSubKey("v2.0.50727")) != null)
                    {
                        FrameworkVersion = "2.0.50727";
                        FrameworkServicePack = key.GetValue("SP", 0).ToString();
                        return;
                    }
                    
                    if ((key.OpenSubKey("v1.1.4322")) != null)
                    {
                        FrameworkVersion = "1.1.4322";
                        FrameworkServicePack = key.GetValue("SP", 0).ToString();
                        return;
                    }

                                        
                    if ((key.OpenSubKey("v1.0")) != null)
                    {
                        FrameworkVersion = "1.0";
                        FrameworkServicePack = key.GetValue("SP", 0).ToString();
                        return;
                    }
                }
            }
            catch 
            {    
				try
				{
					string[] f = GetCommandExecutionOutput("mono","--version").Split('\n');
	                FrameworkVersion = f[0];
				}
				catch
				{
					FrameworkVersion = "none";
				}
				
				FrameworkServicePack = "";
            }
        }
      
        /// <summary>
        /// GetProcessorFrequency OS Archicteture GetComponentName
        /// </summary>
        public void GetArchicteture()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
                foreach (ManagementObject sysItem in searcher.Get())
                {
                   
                    if (sysItem.Properties.Count <= 0)
                    {
                        continue;
                    }

                    foreach (PropertyData PC in sysItem.Properties)
                    {
                        if (PC.Name.Equals("OSArchitecture"))
                        {
                            if (PC.Value !=null)
                            {
                                string value = PC.Value.ToString().Remove(2);
                                Architecture = int.Parse(value);
                            }
                        }
                    }
                }
     
                if (Architecture <= 0)
                {
                    if (IntPtr.Size == 8)
                    {
                        Architecture = 64;
                    }
                    else
                    {
                        if (IntPtr.Size == 4)
                        {
                            Architecture = 32;
                        }
                    }
                }
            }
            catch 
            {
                Architecture = -1;
            }
        }

        /// <summary>
        /// GetProcessorFrequency OS Language GetComponentName
        /// </summary>
        public void GetLanguage()
        {
            try
            {
                Language = Thread.CurrentThread.CurrentCulture.DisplayName;
                Lcid     = Thread.CurrentThread.CurrentCulture.LCID;
            }
            catch
            {
                Language = "null";
                Lcid = -1;
            }
        }

        /// <summary>
        /// GetProcessorFrequency OS ApplicationVersion GetComponentName
        /// </summary>
        public void GetVersion()
        {
            try
            {
                System.OperatingSystem _osInfo = Environment.OSVersion;
                _servicePack = _osInfo.ServicePack;
                
                switch (_osInfo.Platform)
                {
                    case PlatformID.MacOSX:
                    case PlatformID.Unix:
	                	Version = GetCommandExecutionOutput("uname","-rs");
						break;
                    case PlatformID.Win32NT:
                        switch (_osInfo.Version.Major)
	                    {
                            case 3:
                                Version = "Windows NT 3.51";
                                break;
                            case 4:
                                Version = "Windows NT 4.0";
                                break;
                            case 5:
                                if (_osInfo.Version.Minor == 0)
                                {
                                    Version = "Windows 2000";
                                }
                                else
                                {
                                    if (_osInfo.Version.Minor == 1)
                                    {
                                        Version = "Windows XP";
                                    }
                                    else
                                    {
                                        if (_osInfo.Version.Minor == 2)
                                        {
                                            Version = "Windows Server 2003";
                                        }
                                    }
                                }
                                break;
                            case 6:
                                if (_osInfo.Version.Minor == 0)
                                {
                                    Version = "Windows Vista";
                                }
                                else
                                {
                                    if (_osInfo.Version.Minor == 1)
                                    {
                                        Version = "Windows 7";
                                    }
                                    else
                                    {
                                        if (_osInfo.Version.Minor == 3)
                                        {
                                            Version = "Windows Server 2008";
                                        }
                                    }
                                }
                                break;
                            default:
                                break;
	                    }
                        break;
                    case PlatformID.Win32S:
                        break;
                    case PlatformID.Win32Windows:
                        switch (_osInfo.Version.Minor)
	                    {   
		                    default:
                                break;
	                    }
                        break;
                    case PlatformID.WinCE:
                        break;
                    case PlatformID.Xbox:
                        break;
                    default:
                        break;
                }

            }
            catch
            {
                Version = "Unix";
            }

        }

        /// <summary>
        /// GetProcessorFrequency Java version GetComponentName
        /// </summary>
        public void GetJavaVersion()
        {
            try
            {
                JavaVersion = "none";
                RegistryKey registry = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\JavaSoft\\Java Runtime Environment");
                if (registry != null)
                {
                    JavaVersion = registry.GetValue("CurrentVersion").ToString();
                    registry.Close();
                }
            }
            catch
            {
				try
				{
					string[] j = GetCommandExecutionOutput("java","-version 2>&1").Split('\n');
	                JavaVersion = j[0];
				}
				catch
				{
					JavaVersion = "none";
				}
            }
        }
		
		internal static string GetCommandExecutionOutput(string command,string arguments)
		{
			var process = new Process();
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.FileName = command;
			process.StartInfo.Arguments = arguments;
			process.Start();
			return process.StandardOutput.ReadToEnd();
		}
    }
}
