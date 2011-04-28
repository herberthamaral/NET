using System;
using System.Collections;

namespace DeskMetrics.Json
{
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
}

