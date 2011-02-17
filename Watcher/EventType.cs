// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - DeskMetricsOS.cs                                //
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeskMetrics
{
    public class EventType
    {
        public static const string StartApplication = "strApp";
        public static const string StopApplication = "stApp";
        public static const string Event = "ev";
        public static const string CustomData = "ctD";
        public static const string CustomDataRealTime = "ctDR";
        public static const string Log = "lg";
        public static const string Exception = "exC";
        public static const string EventStart = "evS";
        public static const string EventStop = "evST";
        public static const string EventCancel = "evC";
        public static const string EventValue = "evV";
        public static const string EventPeriod = "evP";
        public static const string Install = "ist";
        public static const string Uninstall = "ust";
    }
}
