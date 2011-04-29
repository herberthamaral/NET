// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - OperatingSystem/OperatingSystemFactory.cs       //
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
namespace DeskMetrics.OperatingSystem
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

