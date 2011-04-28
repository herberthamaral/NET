using System;
using System.Collections;

namespace DeskMetrics.Json
{
	public class EventPeriodJson : EventJson
    {
        protected int Time;
        protected bool Completed;

        public EventPeriodJson(string category, string name, int flow,int time,bool completed)
            : base(category, name, flow)
        {
            Time = time;
            Completed = completed;
			Type = EventType.EventPeriod;
        }

        public override Hashtable GetJsonHashTable()
        {
            var json = base.GetJsonHashTable();
            json.Add("tm", Time);
            json.Add("ec", Completed?"1":"0");
            return json;
        }
    }
}

