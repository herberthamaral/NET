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

