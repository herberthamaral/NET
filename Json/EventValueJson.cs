using System;
using System.Collections;

namespace DeskMetrics.Json
{
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

}

