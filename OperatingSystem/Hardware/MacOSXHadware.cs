using System;
namespace DeskMetrics.OperatingSystem.Hardware
{
	public class MacOSXHadware:UnixHardware
	{
		public MacOSXHadware ()
		{
		}
	

		#region IHardware implementation
		public override string ProcessorName {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public override int ProcessorArchicteture {
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

		public override  string ScreenResolution {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public override  string ProcessorBrand {
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
	}
}

