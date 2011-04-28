using System;
using System.Collections;

namespace DeskMetrics.Json
{
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
}

