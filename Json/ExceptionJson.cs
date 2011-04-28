using System;
using System.Collections;

namespace DeskMetrics.Json
{
	public class ExceptionJson : BaseJson
    {
        protected Exception Exception;
        protected int Flow;
        public ExceptionJson(Exception e,int flow)
            : base(EventType.Exception, BaseJson.Session)
        {
            Exception = e;
            Flow = flow;
        }

        public override Hashtable GetJsonHashTable()
        {
            var json = base.GetJsonHashTable();
            json.Add("msg", Exception.Message.Trim().Replace("\r\n", "").Replace("  ", " ").Replace("\n", "").Replace(@"\n", "").Replace("\r", "").Replace("&", "").Replace("|", "").Replace(">", "").Replace("<", "").Replace("\t", "").Replace(@"\", @"/"));
            json.Add("stk", Exception.StackTrace.Trim().Replace("\r\n", "").Replace("  ", " ").Replace("\n", "").Replace(@"\n", "").Replace("\r", "").Replace("&", "").Replace("|", "").Replace(">", "").Replace("<", "").Replace("\t", "").Replace(@"\", @"/"));
            json.Add("src", Exception.Source.Trim().Replace("\r\n", "").Replace("  ", " ").Replace("\n", "").Replace(@"\n", "").Replace("\r", "").Replace("&", "").Replace("|", "").Replace(">", "").Replace("<", "").Replace("\t", "").Replace(@"\", @"/"));
            json.Add("tgs", Exception.TargetSite.ToString());
            json.Add("fl", Flow);
            return json;
        }
        

    }
}

