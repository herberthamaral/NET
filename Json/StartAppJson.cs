using System;
using System.Collections;
using DeskMetrics.OperatingSystem;
using DeskMetrics.OperatingSystem.Hardware;

namespace DeskMetrics.Json
{
	public class StartAppJson:BaseJson
    {
        private Watcher Watcher;
        public StartAppJson(Watcher w):base(EventType.StartApplication,w.SessionGUID.ToString())
        {
            Watcher = w;
        }

        public override Hashtable GetJsonHashTable()
        {
            IOperatingSystem GetOsInfo = OperatingSystemFactory.GetOperatingSystem();
            Hardware GetHardwareInfo = new Hardware();
            var json = base.GetJsonHashTable();
			
            json.Add("aver",Watcher.ApplicationVersion);
            json.Add("ID", Watcher.UserGUID);
            json.Add("osv", GetOsInfo.Version);
            json.Add("ossp", GetOsInfo.ServicePack);
            json.Add("osar", GetOsInfo.Architecture);
            json.Add("osjv", GetOsInfo.JavaVersion);
            json.Add("osnet", GetOsInfo.FrameworkVersion);
            json.Add("osnsp", GetOsInfo.FrameworkServicePack);
            json.Add("oslng", GetOsInfo.Lcid);
            json.Add("osscn", GetHardwareInfo.ScreenResolution);
            json.Add("cnm", GetHardwareInfo.ProcessorName);
            json.Add("cbr", GetHardwareInfo.ProcessorBrand);
            json.Add("cfr", GetHardwareInfo.ProcessorFrequency);
            json.Add("ccr", GetHardwareInfo.ProcessorCores);
            json.Add("car", GetHardwareInfo.ProcessorArchicteture);
            json.Add("mtt", GetHardwareInfo.MemoryTotal);
            json.Add("mfr", GetHardwareInfo.MemoryFree);
            json.Add("dtt", "null");
            json.Add("dfr", "null");
            return json;
        }
    }	
}

