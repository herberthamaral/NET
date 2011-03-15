using System;
using DeskMetrics;

namespace DeskMetricsConsole
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var watcher = new DeskMetrics.Watcher();
			Console.Write("Starting...");
			watcher.Start("4d47c012d9340b116a000000","0.1");
			Console.WriteLine("[ok]");
			
			Console.Write("Adding event...");
			watcher.TrackEvent("DeskMetricsConsole","InitialEvent");
			Console.WriteLine("[ok]");
			
			Console.Write("Adding event value...");
			watcher.TrackEventValue("DeskMetricsConsole","EventValue","Aha!");
			Console.WriteLine("[ok]");
			
			Console.Write("Adding event timed...");
			watcher.TrackEventPeriod("DeskMetricsConsole","EventValue",10,true);
			Console.WriteLine("[ok]");
			
			Console.Write("Adding custom data...");
			watcher.TrackCustomData("DeskMetricsConsole","CustomData");
			Console.WriteLine("[ok]");
			
			Console.Write("Adding custom data in real time...");
			watcher.TrackCustomDataR("DeskMetricsConsole","MyCustomDataInRealTime");
			Console.WriteLine("[ok]");
			
			Console.Write("Finishing app and sending data to DeskMetrics");
			watcher.Stop();
			Console.ReadLine();
		}
	}
}

