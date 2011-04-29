// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - OperatingSystem/MacOSXOperatingSystem.cs        //
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
	internal class MacOSXOperatingSystem: UnixOperatingSystem
	{
		public MacOSXOperatingSystem ()
		{
		}
		
		#region IOperatingSystem implementation

		public override int Architecture {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		
		public override string Version {
			get {
				throw new System.NotImplementedException();
			}
			set {
				throw new System.NotImplementedException();
			}
		}
		
		#endregion
	}
}

