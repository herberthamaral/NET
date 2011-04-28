// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - JsonBuilder.cs                                  //
//     Copyright (c) 2010-2011 DeskMetrics Limited                       //
//                                                                       //
//     http://deskmetrics.com                                            //
//     http://support.deskmetrics.com                                    //
//                                                                       //
//     support@deskmetrics.com                                           //
//                                                                       //
//     This code is provided under the DeskMetrics Modified BSD License  //
//     A copy of this license has been distributed in a file called      //
//     LICENSE with this source code.                                    //
//                                                                       //
// **********************************************************************//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeskMetrics.OperatingSystem;

namespace DeskMetrics.Json
{
    public class JsonBuilder
    {
        public static string GetJsonFromHashTable(Hashtable hash)
        {
            var json = Json.JsonEncode(hash);
            return json;   
        }

        public static string GetJsonFromList(List<string> list)
        {
            return string.Join(",", list.ToArray());
        }
    }
}
