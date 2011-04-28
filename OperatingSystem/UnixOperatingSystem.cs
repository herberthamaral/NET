using System;
namespace DeskMetrics.OperatingSystem
{
	internal class UnixOperatingSystem:IOperatingSystem
	{
		public UnixOperatingSystem ()
		{
		}
	

		#region IOperatingSystem implementation
		
		string _frameworkVersion;
		public override string FrameworkVersion {
			get {
				if (_frameworkVersion == null)
					_frameworkVersion = GetFrameworkVersion();
				return _frameworkVersion;
			}
			set {}
		}

		int _architecture = 0;
		public override int Architecture {
			get {
				if (_architecture == 0)
					_architecture = GetArchitecture();
				return _architecture;
			}
			set {}
		}

		string _javaVersion;
		public override string JavaVersion {
			get {
				if (_javaVersion == null)
					_javaVersion = GetUnixJavaVersion();
				return _javaVersion;
			}
			set {}
		}

		
		public override string ServicePack {
			get {
				return "none";
			}
			set {}
		}
		#endregion
		#region implemented abstract members of DeskMetrics.IOperatingSystem
		string _version;
		public override string Version {
			get {
				if (_version == null)
					_version = GetOperatingSystemVersion();
				return _version;
			}
			set{}
		}
		
		
		public override string FrameworkServicePack {
			get {
				return "none";
			}
			set{}
		}
		
		#endregion
		
		string GetOperatingSystemVersion()
		{
			return GetCommandExecutionOutput("uname","-rs");
		}
		
		string GetFrameworkVersion()
		{
			try
			{
				string[] f = GetCommandExecutionOutput("mono","--version").Split('\n');
                return f[0];
			}
			catch
			{
				return "none";
			}
		}
		
		string GetUnixJavaVersion()
		{
			try
			{
				string[] j = GetCommandExecutionOutput("java","-version 2>&1").Split('\n');
				j = j[0].Split('"');
                return  j[1];
			}
			catch
			{
				return "none";
			}
		}
		
		int GetArchitecture()
		{
			try{
				string arch = GetCommandExecutionOutput("uname","-m");
				if (arch.Contains("64"))
					return 64;
			}
			catch{}
			
			return 32;
		}
	}
}

