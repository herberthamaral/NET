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
        
        protected static string Session
        {
            get;
            set
            {
                //ensure that Session will be filled only once
                if (string.IsNullOrEmpty(Session) && !string.IsNullOrEmpty(value))
                    Session = value;
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
        public StopAppJson(string session)
            : base(EventType.StopApplication, session)
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

        public EventJson(string session, string category, string name, int flow)
            : base(EventType.Event, session)
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

        public EventValueJson(string session, string category, string name,string value, int flow)
            : base(session, category, name, flow)
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

    public class EventStartJson : EventJson
    {
        public EventStartJson(string session, string category, string name, string value, int flow)
            : base(session, category, name, flow)
        {
            Type = EventType.EventStart;
        }
    }

    public class EventStopJson : EventJson
    {
        public EventStopJson(string session, string category, string name, string value, int flow)
            : base(session, category, name, flow)
        {
            Type = EventType.EventStop;
        }
    }

    public class EventCancelJson : EventJson
    {
        public EventCancelJson(string session, string category, string name, string value, int flow)
            : base(session, category, name, flow)
        {
            Type = EventType.EventCancel;
        }
    }

    public class LogJson : BaseJson
    {
        protected string Message;
        public LogJson(string session, string msg)
            : base(EventType.Log, session)
        {
            Message = msg;
        }

        public override Hashtable GetJsonHashTable()
        {
            var json = base.GetJsonHashTable();
            json.Add("ms", Message);
            return json;
        }
    }

    public class StartAppJson:BaseJson
    {
        public StartAppJson(string session):base(EventType.StartApplication,session)
        {
            BaseJson.Session = session;
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
    }
}
