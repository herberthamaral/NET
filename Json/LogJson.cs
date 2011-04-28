using System;
using System.Collections;

namespace DeskMetrics.Json
{
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
}

