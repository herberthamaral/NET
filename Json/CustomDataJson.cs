using System;
using System.Collections;

namespace DeskMetrics.Json
{
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
}

