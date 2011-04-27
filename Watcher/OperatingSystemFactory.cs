using System;
namespace DeskMetrics
{
	internal class OperatingSystemFactory
	{
		private OperatingSystemFactory ()
		{
			
		}
		
		public static IOperatingSystem GetOperatingSystem()
		{
			System.OperatingSystem _osInfo = Environment.OSVersion;
			switch (_osInfo.Platform)
			{
            	case PlatformID.MacOSX:
					return new MacOSXOperatingSystem();
                case PlatformID.Unix:
					return new UnixOperatingSystem();
			}
			return new WindowsOperatingSystem();
		}
	}
}

