using System;
namespace DeskMetrics
{
	internal class MacOSXOperatingSystem:IOperatingSystem
	{
		public MacOSXOperatingSystem ()
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

		public override string Language {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public override int Lcid {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public override string JavaVersion {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public override string ServicePack {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		#endregion
		#region implemented abstract members of DeskMetrics.IOperatingSystem
		public override string Version {
			get {
				throw new System.NotImplementedException();
			}
			set {
				throw new System.NotImplementedException();
			}
		}
		
		
		public override string FrameworkServicePack {
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

