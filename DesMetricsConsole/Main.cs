using System;
using DeskMetrics;

namespace DesMetricsConsole
{
	class MainClass
	{
		public static void Main (string[] args[5])
		{
			args[0] = "Install";
			args[1] = "4d47c012d9340b116a000000";
			args[2] = "0.1";
				
			if (args.Length != 3)
			{
				PrintUsage();
				return;
			}
			
			
			var watcher = new Watcher();
			if (args[0] == "Install")
			{
				watcher.TrackInstall(args[1],args[2]);
			}
			
			if (args[0] == "Uninstall")
			{
				watcher.TrackUninstall(args[1],args[2]);
			}
		}
		
		public static void PrintUsage()
		{
			Console.WriteLine("Usage: deskmetrics [operation] app_id app_version");
			Console.WriteLine("Where: ");
			Console.WriteLine("   operation: One of supported operations (Install, Uninstall)");
			Console.WriteLine("   app_id: Your application ID (like 4d47d012d9340b116a000000)");
			Console.WriteLine("   app_version: Your application version (like 4.2)");
		}
	}
}

