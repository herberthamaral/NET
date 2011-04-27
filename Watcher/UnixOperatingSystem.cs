using System;
namespace DeskMetrics
{
	internal class UnixOperatingSystem:IOperatingSystem
	{
		public UnixOperatingSystem ()
		{
		}
	

		#region IOperatingSystem implementation
		
		public override string FrameworkVersion {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public override int Architecture {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
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
	}
}

