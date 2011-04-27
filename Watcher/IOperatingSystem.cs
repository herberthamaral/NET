using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DeskMetrics
{
    internal abstract class IOperatingSystem
    {
        abstract public string FrameworkVersion { get; set; }
        abstract public int Architecture { get; set; }
		abstract public string Version {get;set;}
		abstract public string FrameworkServicePack {get;set;}
        abstract public string Language { get; set; }
        abstract public int Lcid { get; set; }
        abstract public string JavaVersion { get; set; }
        abstract public string ServicePack { get; set; }
		
		internal static string GetCommandExecutionOutput(string command,string arguments)
		{
			var process = new Process();
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
			process.StartInfo.FileName = command;
			process.StartInfo.Arguments = arguments;
			process.Start();
            string output = process.StandardOutput.ReadToEnd();
            if (String.IsNullOrEmpty(output))
                output = process.StandardError.ReadToEnd();
			return output;
		}
    }
}
