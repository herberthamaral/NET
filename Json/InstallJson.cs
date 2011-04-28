using System;
using System.Collections;

namespace DeskMetrics.Json
{
	public class InstallJson: BaseJson
	{
		public string ID;
		public string Version;
		
		public InstallJson(string version)
			:base("ist",BaseJson.Session)
		{
			ID = System.Guid.NewGuid().ToString().Replace("-", "").ToUpper();
			Version = version;
            if (BaseJson.Session == null)
            {
                BaseJson.Session = System.Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            }
		}
		
		public override Hashtable GetJsonHashTable ()
		{
			var json = base.GetJsonHashTable();
			json.Add("ID",ID);
			json.Add("aver",Version);
			return json;
		}
	}
}

