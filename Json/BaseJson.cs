using System;
using System.Collections;
namespace DeskMetrics.Json
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
}

