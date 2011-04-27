﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace DeskMetrics
{
    internal abstract class IOperatingSystem
    {
        abstract public string FrameworkVersion { get; set; }
        abstract public int Architecture { get; set; }
		abstract public string Version {get;set;}
		abstract public string FrameworkServicePack {get;set;}
        
        abstract public string JavaVersion { get; set; }
        abstract public string ServicePack { get; set; }
		
		
		string _language;
		public string Language { 
			get 
			{
				if (_language == null)
					_language = GetLanguage();
				return _language;
			}
		}
		
		int _lcid = 0;
        public int Lcid { 
			get
			{
				if (_lcid == 0)
					_lcid = GetLcid();
				
				return _lcid;
			}
		}
		
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
		
		string GetLanguage()
        {
            try
            {
                return Thread.CurrentThread.CurrentCulture.DisplayName;
                
            }
            catch
            {
                return  "null";
                
            }
        }
		
		int GetLcid()
		{
			try
			{
				return Thread.CurrentThread.CurrentCulture.LCID;
			}
			catch
			{
				return -1;
			}
				
		}
    }
}
