// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - DeskMetricsOS.cs                                //
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeskMetrics
{
    public abstract class BaseJson
    {
        protected string Type;

        private static string _session;
        protected static string Session
        {
            get
            {
                return _session;
            }
            set
            {
                //ensure that Session will be filled only once
                if (string.IsNullOrEmpty(_session) && !string.IsNullOrEmpty(value))
                    _session = value;
            }
        }

        protected int TimeStamp;
        protected Hashtable json;

        public BaseJson(string type,string session)
        {
            Session = session;
            Type = type;
            TimeStamp = Util.GetTimeStamp();
            json = new Hashtable();
        }

        public virtual Hashtable GetJsonHashTable()
        {
            json.Add("tp", Type);
            json.Add("ss", Session);
            json.Add("ts", TimeStamp);
            return json;
        }
    }

    public class StopAppJson : BaseJson
    {
        public StopAppJson()
            : base(EventType.StopApplication, BaseJson.Session)
        { 

        }

        public override Hashtable GetJsonHashTable()
        {
            return base.GetJsonHashTable(); 
        }
    }

    public class EventJson : BaseJson
    {
        protected string Category;
        protected string Name;
        protected int Flow;

        public EventJson(string category, string name, int flow)
            : base(EventType.Event, BaseJson.Session)
        {
            Category = category;
            Name = name;
            Flow = flow;
        }

        public override Hashtable GetJsonHashTable()
        {
            var json = base.GetJsonHashTable();
            json.Add("ca", Category);
            json.Add("nm", Name);
            json.Add("fl", Flow);

            return json;
        }
    }

    public class EventValueJson : EventJson
    {
        protected string Value;

        public EventValueJson(string category, string name,string value, int flow)
            : base(category, name, flow)
        {
            Type = EventType.EventValue;
            Value = value;
        }

        public override Hashtable GetJsonHashTable()
        {
            var json = base.GetJsonHashTable();
            json.Add("vl", Value);
            return json;
        }
    }


    public class EventCancelJson : EventJson
    {
        public EventCancelJson(string category, string name, int flow)
            : base(category, name, flow)
        {
            Type = EventType.EventCancel;
        }
    }

    public class LogJson : BaseJson
    {
        protected string Message;
        protected int Flow;
        public LogJson(string msg,int flow)
            : base(EventType.Log, BaseJson.Session)
        {
            Message = msg;
            Flow = flow;
        }

        public override Hashtable GetJsonHashTable()
        {
            var json = base.GetJsonHashTable();
            json.Add("ms", Message);
            json.Add("fl", Flow);
            return json;
        }
    }

    public class CustomDataJson : BaseJson
    {
        protected string Name;
        protected string Value;
        protected int Flow;

        public CustomDataJson(string name,string value, int flow)
            : base(EventType.CustomData, BaseJson.Session)
        {
            Name = name;
            Value = value;
            Flow = flow;
        }

        public override Hashtable GetJsonHashTable()
        {
            var json = base.GetJsonHashTable();
            json.Add("nm", Name);
            json.Add("vl", Value);
            json.Add("fl", Flow);
            return json;
        }

    }

    public class CustomDataRJson : CustomDataJson
    {
        protected string ID;
        protected string  AppVersion;
        public CustomDataRJson(string name, string value, int flow, string ID, string app_version):base(name,value,flow)
        {
            this.ID = ID;
            AppVersion = app_version;
        }

        public override Hashtable GetJsonHashTable()
        {
            var json = base.GetJsonHashTable();
            json.Add("aver", AppVersion);
            json.Add("ID", ID);
            return json;
        }
    }

    public class ExceptionJson : BaseJson
    {
        protected Exception Exception;
        public ExceptionJson(Exception e)
            : base(EventType.Exception, BaseJson.Session)
        {
            Exception = e;
        }

        public override Hashtable GetJsonHashTable()
        {
            var json = base.GetJsonHashTable();
            json.Add("msg", Exception.Message.Trim().Replace("\r\n", "").Replace("  ", " ").Replace("\n", "").Replace(@"\n", "").Replace("\r", "").Replace("&", "").Replace("|", "").Replace(">", "").Replace("<", "").Replace("\t", "").Replace(@"\", @"\\"));
            json.Add("stk", Exception.StackTrace.Trim().Replace("\r\n", "").Replace("  ", " ").Replace("\n", "").Replace(@"\n", "").Replace("\r", "").Replace("&", "").Replace("|", "").Replace(">", "").Replace("<", "").Replace("\t", "").Replace(@"\", @"\\"));
            json.Add("src", Exception.Source.Trim().Replace("\r\n", "").Replace("  ", " ").Replace("\n", "").Replace(@"\n", "").Replace("\r", "").Replace("&", "").Replace("|", "").Replace(">", "").Replace("<", "").Replace("\t", "").Replace(@"\", @"\\"));
            json.Add("tgs", Exception.TargetSite);
            return json;
        }
        

    }


    public class StartAppJson:BaseJson
    {
        private Watcher Watcher;
        public StartAppJson(Watcher w):base(EventType.StartApplication,w.SessionGUID.ToString())
        {
            Watcher = w;
        }

        public override Hashtable GetJsonHashTable()
        {
            OperatingSystem GetOsInfo = new OperatingSystem();
            Hardware GetHardwareInfo = new Hardware();
            var json = base.GetJsonHashTable();
            
            GetOsInfo.GetFrameworkVersion();
            GetOsInfo.GetArchicteture();
            GetOsInfo.GetLanguage();
            GetOsInfo.GetVersion();
            GetOsInfo.GetJavaVersion();
            GetHardwareInfo.GetProcessorData();
            GetHardwareInfo.GetMemoryData();
            GetHardwareInfo.GetDiskData();
            GetHardwareInfo.GetScreenResolution();

            json.Add("aver",Watcher.ApplicationVersion);
            json.Add("ID", Watcher.UserGUID);
            json.Add("osv", GetOsInfo.Version);
            json.Add("ossp", GetOsInfo.ServicePack);
            json.Add("osar", GetOsInfo.Archicteture);
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
            json.Add("dtt", GetHardwareInfo.DiskTotal);
            json.Add("dfr", GetHardwareInfo.DiskFree);
            return json;
        }
    }

    public class EventPeriodJson : EventJson
    {
        protected int Time;
        protected bool Completed;

        public EventPeriodJson(string category, string name, int flow,int time,bool completed)
            : base(category, name, flow)
        {
            Time = time;
            Completed = completed;
        }

        public override Hashtable GetJsonHashTable()
        {
            var json = base.GetJsonHashTable();
            json.Add("tm", Time);
            json.Add("ec", Completed?1:0);
            return json;
        }
    }

    public class JsonBuilder
    {
        public static string GetJsonFromHashTable(Hashtable hash)
        {
            var json = Json.JsonEncode(hash);
            return json;   
        }

        public static string GetJsonFromList(List<string> list)
        {
            return string.Join(",", list.ToArray());
        }
    }
}
