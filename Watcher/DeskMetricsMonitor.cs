// **********************************************************************//
//                                                                       //
//     DeskMetrics NET - DeskMetricsMonitor.cs                           //
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
using System.Reflection;
using System.Collections;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Win32;

namespace DeskMetrics
{
    public class Watcher : IDisposable
    {
        Thread StartThread;
        Thread StopThread;
        Thread CustomDataThread;

        /// <summary>
        /// Thread Lock
        /// </summary>
        private System.Object ObjectLock = new System.Object();
        /// <summary>
        /// Field Timestamp
        /// </summary>
        private int _timestamp;
        /// <summary>
        /// Field User GUID
        /// </summary>
        private object _userGUID;
        /// <summary>
        /// Field Session Id
        /// </summary>
        private object _sessionGUID;
        /// <summary>
        /// Field Json
        /// </summary>
        private string _json;
        /// <summary>
        /// Field Application Id
        /// </summary>
        private string _applicationId;
        /// <summary>
        /// Field Error Message
        /// </summary>
        private string _error;
        /// <summary>
        /// Field Test mode
        /// </summary>
        private int _test;
        /// <summary>
        /// Field Type JSON
        /// </summary>
        private string _type;

        private int _flownumber;
        /// <summary>
        /// Field Event Category
        /// </summary>
        private string _eventCategory;
        /// <summary>
        /// Field Event Name
        /// </summary>
        private string _eventName;
        /// <summary>
        /// Field EventValue Value
        /// </summary>
        private string _eventValue;
        /// <summary>
        /// Field Custom Name
        /// </summary>
        private int _eventTime;
        
        private string _exceptionMessage;
        
        private string _exceptionStackTrace;
        
        private string _exceptionSource;
        
        private string _exceptionTargetSite;

        private string _customDataName;
        /// <summary>
        /// Field Custom Data Value
        /// </summary>
        private string _customDataValue;
        /// <summary>
        /// Field Custom Log
        /// </summary>
        private string _log;
        /// <summary>
        /// Field ApplicationVersion
        /// </summary>
        private string _applicationVersion;
        /// <summary>
        /// Field Component Name
        /// </summary>
        private string _componentName;
        /// <summary>
        /// Field Component Version
        /// </summary>
        private string _componentVersion;

        private int _flowglobalnumber = 0;

        private bool _started = false;

        public bool Started
        {
            get { return _started; }
        }
        
        private int _bandwidth = Settings.MaxDailyNetwork;
        
        private int _storage = Settings.MaxStorageData;

        private string _postserver = Settings.DefaultServer;

        private int _postport = Settings.DefaultPort;

        private int _posttimeout = Settings.Timeout;

        private bool _postwaitresponse = false;

        private bool _realtime;

        private string _proxyusername;

        private string _proxypassword;

        private string _proxyhost;

        private Int32 _proxyport;

        private bool _enabled = true;

        public string ApplicationId
        {
            get
            {
                return _applicationId;
            }
            set
            {
                _applicationId = value;
            }
        }

        public string ApplicationVersion
        {
            get
            {
                return _applicationVersion;
            }
            set
            {
                _applicationVersion = value;
            }
        }

        public string JSON
        {
            get
            {
                return _json;
            }
            set
            {
                _json = value;
            }
        }

        public string Error
        {
            get
            {
                return _error;
            }
            set
            {
                _error = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }

        public string ComponentName
        {
            get
            {
                Assembly thisAsm = this.GetType().Assembly;
                object[] attrs = thisAsm.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                _componentName = ((AssemblyTitleAttribute)attrs[0]).Title;
                return _componentName;
            }

        }

        public string ComponentVersion
        {
            get
            {
                _componentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                return _componentVersion;
            }

        }

        public string PostServer
        {
            get
            {
                return _postserver;
            }
            set
            {
                _postserver = value;
            }
        }

        public int PostPort
        {
            get
            {
                return _postport;
            }
            set
            {
                _postport = value;
            }
        }

        public int PostTimeOut{
            get
            {
                return _posttimeout;
            }
            set 
            {
                _posttimeout = value;
            }
        }

        private Services _services;

        internal Services Services
        {
            get {
                if (_services == null)
                    _services = new Services(this);
                return _services; 
            }
            set { _services = value; }
        }

        public bool PostWaitResponse
        {
            get
            {
                return _postwaitresponse;
            }
            set
            {
                _postwaitresponse = value;
            }
        }

        public string ProxyHost
        {
            get
            {
                return _proxyhost;
            }
            set
            {
                _proxyhost = value;
            }
        }

        public string ProxyUserName
        {
            get
            {
                return _proxyusername;
            }
            set
            {
                _proxyusername = value;
            }
        }

        public string ProxyPassword
        {
            get
            {
                return _proxypassword;
            }
            set
            {
                _proxypassword = value;
            }
        }
 
        public Int32 ProxyPort
        {
            get
            {
                return _proxyport;
            }
            set
            {
                _proxyport = value;
            }
        }

        public bool Start(string ApplicationId, string ApplicationVersion, bool RealTime)
        {
            lock (ObjectLock)
            {
                try
                {
                    if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true))
                    {
                        _applicationId = ApplicationId;
                        _applicationVersion = ApplicationVersion;

                        _type = EventType.StartApplication;
                        _timestamp = Util.GetTimeStamp();
                        _userGUID = GetUserID();
                        _sessionGUID = GetGUID();
                        _realtime = RealTime;

                        SetJSON();

                        if (_realtime == true)
                        {

                            if (StartThread == null)
                            {
                                StartThread = new Thread(_StartThreadFunc);
                            }

                            if ((StartThread != null) && (StartThread.IsAlive == false))
                            {
                                StartThread = new Thread(_StartThreadFunc);
                                StartThread.Name = "StartSender";
                                StartThread.Start();
                            }
                        }

                        _started = true;
                        return _started;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    _error = Settings.ErrorCodes["-1"].ToString();
                    return false;
                }
            }
        }

        private void _StartThreadFunc()
        {
            lock (ObjectLock)
            {
                try
                {
                    lock (ObjectLock)
                    {
                        int ErrorID;
                        try
                        {
                            Services.PostData(out ErrorID, Settings.ApiEndpoint);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// </summary>
        public bool Stop()
        {
            lock (ObjectLock)
            {
                try
                {
                    if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true))
                    {
                        if (StopThread == null)
                        {
                            StopThread = new Thread(_StopThreadFunc);
                        }

                        if ((StopThread != null) && (StopThread.IsAlive == false))
                        {
                            StopThread = new Thread(_StopThreadFunc);
                            StopThread.Name = "StopSender";
                            StopThread.Start();
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return true;
                }
                catch
                {
                    _error = Settings.ErrorCodes["-1"].ToString();
                    return false;
                }
            }
        }

        private void _StopThreadFunc()
        {

            lock (ObjectLock)
            {
                try
                {
                    if (_started)
                    {
                        if (!string.IsNullOrEmpty(ApplicationId))
                        {
                            _type = EventType.StopApplication;
                            _timestamp = Util.GetTimeStamp();

                            SetJSON();

                            string SingleJSON = this.JSON;

                            string CacheData = GetCacheData();
                            if (!string.IsNullOrEmpty(CacheData))
                            {
                                this.JSON = this.JSON + "," + CacheData;
                            }

                            int ErrorID;

                            try
                            {
                                Services.PostData(out ErrorID, Settings.ApiEndpoint);
                            }
                            finally
                            {
                                this.JSON = SingleJSON;
                            }

                            if (ErrorID == 0)
                            {
                                DeleteCacheFile();
                            }
                            else
                            {
                                if (GetCacheSize() < GetMaxStorageSizeInKB())
                                {
                                    SaveCacheFile();
                                }
                            }

                        }
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="EventCategory">EventCategory Category</param>
        /// <param name="eventName">EventCategory Name</param>
        public void TrackEvent(string EventCategory, string EventName)
        {
            lock (ObjectLock)
            {
                try
                {
                    if (_started)
                    {
                        if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true))
                        {
                            _type = EventType.Event;
                            _timestamp = Util.GetTimeStamp();
                            _flownumber = GetFlowNumber();

                            _eventCategory = EventCategory;
                            _eventName = EventName;

                            SetJSON();
                        }
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="EventCategory">EventName Category Name</param>
        /// <param name="EventName">EventName Name</param>
        public void TrackEventStart(string EventCategory, string EventName)
        {
            lock (ObjectLock)
            {
                try
                {
                    if (_started)
                    {
                        if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true))
                        {
                            _type = EventType.EventStart;
                            _timestamp = Util.GetTimeStamp();
                            _flownumber = GetFlowNumber();

                            _eventCategory = EventCategory;
                            _eventName = EventName;

                            SetJSON();
                        }
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="EventCategory">EventName Category</param>
        /// <param name="EventName">EventName Name</param>
        public void TrackEventStop(string EventCategory, string EventName)
        {
            lock (ObjectLock)
            {
                try
                {
                    if (_started)
                    {
                        if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true))
                        {
                            _type = EventType.EventStop;
                            _timestamp = Util.GetTimeStamp();
                            _flownumber = GetFlowNumber();

                            _eventCategory = EventCategory;
                            _eventName = EventName;

                            SetJSON();
                        }
                    }
                }
                catch
                {
                }
            }
        }

        public void TrackEventPeriod(string EventCategory, string EventName, int EventTime)
        {
            lock (ObjectLock)
            {
                try
                {
                    if (_started)
                    {
                        if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true))
                        {
                            _type = EventType.EventPeriod;
                            _timestamp = Util.GetTimeStamp();
                            _flownumber = GetFlowNumber();

                            _eventCategory = EventCategory;
                            _eventName = EventName;
                            _eventTime = EventTime;

                            SetJSON();
                        }
                    }
                }
                catch
                {
                }
            }
        }

        

        /// <summary>
        /// </summary>
        /// <param name="Enabled">Enabled param</param>
        public void TrackException(Exception ApplicationException)
        {
            lock (ObjectLock)
            {
                try
                {
                    if (_started)
                    {
                        if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true) && (ApplicationException != null))
                        {
                            _type = EventType.Exception;
                            _timestamp = Util.GetTimeStamp();
                            _flownumber = GetFlowNumber();

                            _exceptionMessage = ApplicationException.Message.Trim().Replace("\r\n", "").Replace("  ", " ").Replace("\n", "").Replace(@"\n", "").Replace("\r", "").Replace("&", "").Replace("|", "").Replace(">", "").Replace("<", "").Replace("\t", "").Replace(@"\", @"\\");
                            _exceptionSource = ApplicationException.Source.Trim().Replace("\r\n", "").Replace("  ", " ").Replace("\n", "").Replace(@"\n", "").Replace("\r", "").Replace("&", "").Replace("|", "").Replace(">", "").Replace("<", "").Replace("\t", "").Replace(@"\", @"\\");
                            _exceptionStackTrace = ApplicationException.StackTrace.Trim().Replace("\r\n", "").Replace("  ", " ").Replace("\n", "").Replace(@"\n", "").Replace("\r", "").Replace("&", "").Replace("|", "").Replace(">", "").Replace("<", "").Replace("\t", "").Replace(@"\", @"\\");

                            if ((_exceptionStackTrace.Contains("\n")) || (_exceptionStackTrace.Contains(@"\n")))
                            {
                                _exceptionStackTrace = "Unavailable";
                            }

                            if (ApplicationException.TargetSite != null)
                            {
                                _exceptionTargetSite = ApplicationException.TargetSite.ToString().Trim().Replace("\r\n", "").Replace("  ", " ").Replace("\n", "").Replace(@"\n", "").Replace("\r", "").Replace("&", "").Replace("|", "").Replace(">", "").Replace("<", "").Replace("\t", "").Replace(@"\", @"\\");
                            }
                            else
                            {
                                _exceptionTargetSite = "";
                            }

                            SetJSON();
                        }
                    }
                }
                catch
                {
                }
            }
        }        
        /// <summary>
        /// </summary>OB
        /// <param name="ApplicationId">ApplicationId param</param>
        /// <param name="ApplicationVersion">Application ApplicationVersion param</param>
        /// <param name="TestMode">Test Mode param</param>
        public void TrackUninstallation(string ApplicationId, string ApplicationVersion)
        {
            lock (ObjectLock)
            {
                try
                {
                    if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true))
                    {
                        _applicationId = ApplicationId;
                        _type = EventType.Uninstall;
                        _timestamp = Util.GetTimeStamp();
                        _sessionGUID = GetGUID();
                        _applicationVersion = ApplicationVersion;

                        SetJSON();

                        int ErrorID;
                        Services.PostData(out ErrorID, Settings.ApiEndpoint);
                    }

                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// </summary>
        void IDisposable.Dispose()
        {
            try
            {
                this.Stop();
            }
            catch (Exception e)
            {
                _error = e.Message;
            }
        }

        

        protected int GetFlowNumber()
        {
            lock (ObjectLock)
            {
                try
                {
                    _flowglobalnumber = _flowglobalnumber + 1;
                    return _flowglobalnumber;
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// </summary>
        protected void SetJSON()
        {
                lock (ObjectLock)
                {
                    try
                    {
                        OperatingSystem GetOsInfo = new OperatingSystem();
                        Hardware GetHardwareInfo = new Hardware();
                        Hashtable o = new Hashtable();
                        StringBuilder _str = new StringBuilder();
                        switch (_type)
                        {
                            case EventType.StartApplication:

                                #region "Start"
                                GetOsInfo.GetFrameworkVersion();
                                GetOsInfo.GetArchicteture();
                                GetOsInfo.GetLanguage();
                                GetOsInfo.GetVersion();
                                GetOsInfo.GetJavaVersion();
                                GetHardwareInfo.GetProcessorData();
                                GetHardwareInfo.GetMemoryData();
                                GetHardwareInfo.GetDiskData();
                                GetHardwareInfo.GetScreenResolution();
                                o["type"] = _type;
                                o["appVersion"] = _applicationVersion;
                                o["userID"] = _userGUID;
                                o["session"] = _sessionGUID;
                                o["timestamp"] = _timestamp;
                                o["os_name"] = GetOsInfo.Version;
                                o["os_servicepack"] = GetOsInfo.ServicePack;
                                o["os_arch"] = GetOsInfo.Archicteture;
                                o["os_java"] = GetOsInfo.JavaVersion;
                                o["os_dotnet"] = GetOsInfo.FrameworkVersion;
                                o["os_dotnetsp"] = GetOsInfo.FrameworkServicePack;
                                o["os_lang"] = GetOsInfo.Lcid;
                                o["os_screen"] = GetHardwareInfo.ScreenResolution;
                                o["cpu_name"] = GetHardwareInfo.ProcessorName;
                                o["cpu_brand"] = GetHardwareInfo.ProcessorBrand;
                                o["cpu_freq"] = GetHardwareInfo.ProcessorFrequency;
                                o["cpu_cores"] = GetHardwareInfo.ProcessorCores;
                                o["cpu_arch"] = GetHardwareInfo.ProcessorArchicteture;
                                o["mem_total"] = GetHardwareInfo.MemoryTotal;
                                o["mem_free"] = GetHardwareInfo.MemoryFree;
                                o["disk_total"] = GetHardwareInfo.DiskTotal;
                                o["disk_free"] = GetHardwareInfo.DiskFree;

                                _str.Append("{\"tp\":\"" + o["type"] + "\",");
                                _str.Append("\"aver\":\"" + o["appVersion"] + "\",");
                                _str.Append("\"ID\":\"" + _userGUID + "\",");
                                _str.Append("\"ss\":\"" + _sessionGUID + "\",");
                                _str.Append("\"ts\":" + _timestamp + ",");
                                _str.Append("\"osv\":\"" + o["os_name"] + "\",");
                                _str.Append("\"ossp\":\"" + o["os_servicepack"] + "\",");
                                _str.Append("\"osar\":" + o["os_arch"] + ",");
                                _str.Append("\"osjv\":\"" + o["os_java"] + "\",");
                                _str.Append("\"osnet\":\"" + o["os_dotnet"] + "\",");
                                _str.Append("\"osnsp\":\"" + o["os_dotnetsp"] + "\",");
                                _str.Append("\"oslng\":" + o["os_lang"] + ",");
                                _str.Append("\"osscn\":\"" + o["os_screen"] + "\",");
                                _str.Append("\"cnm\":\"" + o["cpu_name"] + "\",");
                                _str.Append("\"cbr\":\"" + o["cpu_brand"] + "\",");
                                _str.Append("\"cfr\":" + o["cpu_freq"] + ",");
                                _str.Append("\"ccr\":" + o["cpu_cores"] + ",");
                                _str.Append("\"car\":" + o["cpu_arch"] + ",");
                                _str.Append("\"mtt\":" + o["mem_total"] + ",");
                                _str.Append("\"mfr\":" + o["mem_free"] + ",");
                                _str.Append("\"dtt\":" + o["disk_total"] + ",");
                                _str.Append("\"dfr\":" + o["disk_free"] + "}");
                                #endregion

                                break;
                            case EventType.StopApplication:

                                #region "Stop"
                                _str.Append(this.JSON + ",");
                                _str.Append("{\"tp\":\"" + _type + "\",");
                                _str.Append("\"ts\":" + _timestamp + ",");
                                _str.Append("\"ss\":\"" + _sessionGUID + "\"}");
                                #endregion

                                break;
                            case EventType.Event:

                                #region "Events"
                                _str.Append(this.JSON + ",");
                                _str.Append("{\"tp\":\"" + _type + "\",");
                                _str.Append("\"ca\":\"" + _eventCategory + "\",");
                                _str.Append("\"nm\":\"" + _eventName + "\",");
                                _str.Append("\"fl\":" + _flownumber + ",");
                                _str.Append("\"ts\":" + _timestamp + ",");
                                _str.Append("\"ss\":\"" + _sessionGUID + "\"}");
                                #endregion

                                break;
                            case EventType.CustomData:

                                #region "Custom Data"
                                _str.Append(this.JSON + ",");
                                _str.Append("{\"tp\":\"" + _type + "\",");
                                _str.Append("\"nm\":\"" + _customDataName + "\",");
                                _str.Append("\"vl\":\"" + _customDataValue + "\",");
                                _str.Append("\"fl\":" + _flownumber + ",");
                                _str.Append("\"ts\":" + _timestamp + ",");
                                _str.Append("\"ss\":\"" + _sessionGUID + "\"}");
                                #endregion

                                break;
                            case EventType.CustomDataRealTime:

                                #region "Custom Data R"
                                _str.Append("{\"tp\":\"" + _type + "\",");
                                _str.Append("\"nm\":\"" + _customDataName + "\",");
                                _str.Append("\"vl\":\"" + _customDataValue + "\",");
                                _str.Append("\"aver\":\"" + _applicationVersion + "\",");
                                _str.Append("\"ID\":\"" + _userGUID + "\",");
                                _str.Append("\"fl\":" + _flownumber + ",");
                                _str.Append("\"ts\":" + _timestamp + ",");
                                _str.Append("\"ss\":\"" + _sessionGUID + "\"}");
                                #endregion

                                break;
                            case EventType.Log:

                                #region "Logs"
                                _str.Append(this.JSON + ",");
                                _str.Append("{\"tp\":\"" + _type + "\",");
                                _str.Append("\"ms\":\"" + _log + "\",");
                                _str.Append("\"fl\":" + _flownumber + ",");
                                _str.Append("\"ts\":" + _timestamp + ",");
                                _str.Append("\"ss\":\"" + _sessionGUID + "\"}");
                                #endregion

                                break;
                            case EventType.Exception:

                                #region "Exceptions"
                                _str.Append(this.JSON + ",");
                                _str.Append("{\"tp\":\"" + _type + "\",");
                                _str.Append("\"msg\":\"" + _exceptionMessage + "\",");
                                _str.Append("\"stk\":\"" + _exceptionStackTrace + "\",");
                                _str.Append("\"src\":\"" + _exceptionSource + "\",");
                                _str.Append("\"tgs\":\"" + _exceptionTargetSite + "\",");
                                _str.Append("\"fl\":" + _flownumber + ",");
                                _str.Append("\"ts\":" + _timestamp + ",");
                                _str.Append("\"ss\":\"" + _sessionGUID + "\"}");
                                #endregion

                                break;
                            case EventType.EventStart:

                                #region "Event Start"
                                _str.Append(this.JSON + ",");
                                _str.Append("{\"tp\":\"" + _type + "\",");
                                _str.Append("\"ca\":\"" + _eventCategory + "\",");
                                _str.Append("\"nm\":\"" + _eventName + "\",");
                                _str.Append("\"fl\":" + _flownumber + ",");
                                _str.Append("\"ts\":" + _timestamp + ",");
                                _str.Append("\"ss\":\"" + _sessionGUID + "\"}");
                                #endregion

                                break;
                            case EventType.EventStop:

                                #region "Events Stop"
                                _str.Append(this.JSON + ",");
                                _str.Append("{\"tp\":\"" + _type + "\",");
                                _str.Append("\"ca\":\"" + _eventCategory + "\",");
                                _str.Append("\"nm\":\"" + _eventName + "\",");
                                _str.Append("\"fl\":" + _flownumber + ",");
                                _str.Append("\"ts\":" + _timestamp + ",");
                                _str.Append("\"ss\":\"" + _sessionGUID + "\"}");
                                #endregion

                                break;

                            case EventType.EventCancel:
                                #region "Events Cancel"
                                _str.Append(this.JSON + ",");
                                _str.Append("{\"tp\":\"" + _type + "\",");
                                _str.Append("\"ca\":\"" + _eventCategory + "\",");
                                _str.Append("\"nm\":\"" + _eventName + "\",");
                                _str.Append("\"fl\":" + _flownumber + ",");
                                _str.Append("\"ts\":" + _timestamp + ",");
                                _str.Append("\"ss\":\"" + _sessionGUID + "\"}");
                                #endregion

                                break;

                            case EventType.EventValue:

                                #region "Events Values"
                                _str.Append(this.JSON + ",");
                                _str.Append("{\"tp\":\"" + _type + "\",");
                                _str.Append("\"ca\":\"" + _eventCategory + "\",");
                                _str.Append("\"nm\":\"" + _eventName + "\",");
                                _str.Append("\"vl\":\"" + _eventValue + "\",");
                                _str.Append("\"fl\":" + _flownumber + ",");
                                _str.Append("\"ts\":" + _timestamp + ",");
                                _str.Append("\"ss\":\"" + _sessionGUID + "\"}");
                                #endregion

                                break;

                            case EventType.EventPeriod:
                                #region "Events Period"
                                _str.Append(this.JSON + ",");
                                _str.Append("{\"tp\":\"" + _type + "\",");
                                _str.Append("\"ca\":\"" + _eventCategory + "\",");
                                _str.Append("\"nm\":\"" + _eventName + "\",");
                                _str.Append("\"tm\":\"" + _eventTime + "\",");
                                _str.Append("\"fl\":" + _flownumber + ",");
                                _str.Append("\"ts\":" + _timestamp + ",");
                                _str.Append("\"ss\":\"" + _sessionGUID + "\"}");
                                #endregion

                                break;

                            case EventType.Install:

                                #region "Install"
                                _str.Append("{\"tp\":\"" + _type + "\",");
                                _str.Append("\"aver\":\"" + _applicationVersion + "\",");
                                _str.Append("\"ID\":\"" + _userGUID + "\",");
                                _str.Append("\"ts\":" + _timestamp + ",");
                                _str.Append("\"ss\":\"" + _sessionGUID + "\"}");
                                #endregion

                                break;
                            case EventType.Uninstall:

                                #region "Uninstall"
                                _str.Append("{\"tp\":\"" + _type + "\",");
                                _str.Append("\"aver\":\"" + _applicationVersion + "\",");
                                _str.Append("\"ID\":\"" + _userGUID + "\",");
                                _str.Append("\"ts\":" + _timestamp + ",");
                                _str.Append("\"ss\":\"" + _sessionGUID + "\"}");
                                #endregion

                                break;
                            default:
                                break;
                        }

                        this.JSON = _str.ToString().Trim();
                    }
                    catch
                    {
                        _error = Settings.ErrorCodes["-9"].ToString();
                    }
          
            }

        }

        /// <summary>
        /// </summary>
        /// <param name="EventCategory">Event Category param</param>
        /// <param name="EventName">Event Name param</param>
        /// <param name="EventValue">Event Value param</param>
        public void TrackEventValue(string EventCategory, string EventName, string EventValue)
        {
            lock (ObjectLock)
            {
                try
                {
                    if (_started)
                    {
                        if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true))
                        {
                            _type = EventType.EventValue;
                            _timestamp = Util.GetTimeStamp();
                            _flownumber = GetFlowNumber();

                            _eventCategory = EventCategory.Trim();
                            _eventName = EventName.Trim();
                            _eventValue = EventValue.Trim();

                            SetJSON();
                        }
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="EventCategory">Event Category param</param>
        /// <param name="EventName">Event Name param</param>
        public void TrackEventCancel(string EventCategory, string EventName)
        {
            lock (ObjectLock)
            {
                try
                {
                    if (_started)
                    {
                        if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true))
                        {
                            _type = EventType.EventCancel;
                            _timestamp = Util.GetTimeStamp();
                            _flownumber = GetFlowNumber();

                            _eventCategory = EventCategory.Trim();
                            _eventName = EventName.Trim();

                            SetJSON();
                        }
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="CustomDataName">Custom Data Name param</param>
        /// <param name="CustomDataValue">Custom Data Value param</param>
        public void TrackCustomData(string CustomDataName, string CustomDataValue)
        {
            lock (ObjectLock)
            {
                try
                {
                    if (_started)
                    {
                        if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true))
                        {
                            _type = EventType.CustomData;
                            _timestamp = Util.GetTimeStamp();
                            _flownumber = GetFlowNumber();

                            _customDataName = CustomDataName.Trim();
                            _customDataValue = CustomDataValue.Trim();

                            SetJSON();
                        }
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="Message">Custom Message param</param>
        public void TrackLog(string Message)
        {
            lock (ObjectLock)
            {
                try
                {
                    if (_started)
                    {
                        if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true))
                        {
                            _type = EventType.Log;
                            _timestamp = Util.GetTimeStamp();
                            _flownumber = GetFlowNumber();

                            _log = Message.Trim();

                            SetJSON();
                        }
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="ApplicationId">ApplicationId param</param>
        /// <param name="ApplicationVersion">Application ApplicationVersion param</param>
        /// <param name="TestMode">Test Mode param</param>
        public void TrackInstallation(string ApplicationId, string ApplicationVersion, bool TestMode)
        {
            lock (ObjectLock)
            {
                try
                {
                    if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true))
                    {
                        _applicationId = ApplicationId;
                        _applicationVersion = ApplicationVersion;

                        _type = EventType.Install;
                        _timestamp = Util.GetTimeStamp();
                        _sessionGUID = GetGUID();
                        _test = Convert.ToInt32(TestMode);

                        SetJSON();

                        int ErrorID;
                        Services.PostData(out ErrorID, Settings.ApiEndpoint);
                    }
                }
                catch
                {
                }
            }
        }

        public bool TrackCustomDataRealTime(string CustomDataName, string CustomDataValue)
        {
            lock (ObjectLock)
            {
                try
                {
                    if (_started)
                    {
                        if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true))
                        {
                            _type = EventType.CustomDataRealTime;
                            _timestamp = Util.GetTimeStamp();
                            _customDataName = CustomDataName;
                            _customDataValue = CustomDataValue;
                            _flownumber = GetFlowNumber();

                            string temp = this.JSON;
                            try
                            {
                                SetJSON();
                                int ErrorID;
                                Services.PostData(out ErrorID, Settings.ApiEndpoint);

                                if (ErrorID == 0)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            finally
                            {
                                this.JSON = temp;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch 
                {
                    return false;
                }
            }
        }

        public bool TrackCustomDataRASync(string CustomDataName, string CustomDataValue)
        {
            lock (ObjectLock)
            {
                try
                {
                    if (_started)
                    {
                        if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true))
                        {
                            _type = EventType.CustomDataRealTime;
                            _timestamp = Util.GetTimeStamp();
                            _customDataName = CustomDataName;
                            _customDataValue = CustomDataValue;
                            _flownumber = GetFlowNumber();


                            if (CustomDataThread == null)
                            {
                                CustomDataThread = new Thread(_CustomDataRThread);
                            }

                            if ((CustomDataThread != null) && (CustomDataThread.IsAlive == false))
                            {
                                CustomDataThread = new Thread(_CustomDataRThread);
                                CustomDataThread.Name = "CustomDataRASyncSender";
                                CustomDataThread.Start();
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        private void _CustomDataRThread()
        {
            lock (ObjectLock)
            {
                try
                {
                    string temp = this.JSON;

                    this.JSON = "";
                    SetJSON();
                    try
                    {
                        int ErrorID;
                        try
                        {
                            Services.PostData(out ErrorID, Settings.ApiEndpoint);
                        }
                        catch
                        {
                        }
                    }
                    finally
                    {
                        this.JSON = temp;
                    }
                }
                catch
                {
                }
            }
        }

        public bool SetUserID(string UserID)
        {
            lock (ObjectLock)
            {
                try
                {
                    RegistryKey reg = Registry.CurrentUser.CreateSubKey("Software\\dskMetrics");
                    reg.SetValue("ID", UserID);
                    reg.Close();

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public int GetDailyNetworkUtilizationInKB()
        {
            lock (ObjectLock)
            {
                try
                {
                    return _bandwidth;
                }
                catch
                {
                    return Settings.MaxDailyNetwork;
                }
            }
        }

        public void SetDailyNetworkUtilizationInKB(int FDataSize)
        {
            lock (ObjectLock)
            {
                try
                {
                    _bandwidth = FDataSize;
                }
                catch
                {
                }
            }
        }

        public int GetMaxStorageSizeInKB()
        {
            lock (ObjectLock)
            {
                try
                {
                    return _storage;
                }
                catch
                {
                    return Settings.MaxStorageData;
                }
            }
        }

        public void SetMaxStorageSizeInKB(int FDataSize)
        {
            lock (ObjectLock)
            {
                try
                {
                    _storage = FDataSize;
                }
                catch
                {
                }
            }
        }

        protected string GetGUID()
        {
            lock (ObjectLock)
            {
                try
                {
                    return System.Guid.NewGuid().ToString().Replace("-", "").ToUpper();
                }
                catch
                {
                    return "";
                }
            }
        }

        protected string GetUserID()
        {
            lock (ObjectLock)
            {
                try
                {
                    RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software\\dskMetrics", true);
                    if (reg == null)
                    {
                        string _UserID = GetGUID();
                        SetUserID(_UserID);
                        return _UserID;
                    }
                    else
                    {
                        string UserID = reg.GetValue("ID").ToString();
                        if (!string.IsNullOrEmpty(UserID))
                        {
                            return UserID;
                        }
                        else
                        {
                            UserID = GetGUID();
                            SetUserID(UserID);
                            return UserID;
                        }
                    }
                }
                catch
                {
                    return "";
                }
            }
        }

        private bool DeleteCacheFile()
        {
            lock (ObjectLock)
            {
                try
                {
                    string FileName = Path.GetTempPath() + _applicationId + ".dsmk";
                    if (File.Exists(FileName))
                    {
                        File.Delete(FileName);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        private string GetCacheData()
        {
            lock (ObjectLock)
            {
                try
                {
                    string FileName = Path.GetTempPath() + _applicationId + ".dsmk";
                    if (File.Exists(FileName))
                    {
                        FileStream FileS = new FileStream(@FileName, FileMode.Open, FileAccess.Read);
                        StreamReader Stream = new StreamReader(FileS);
                        try
                        {
                            return Util.DecodeFrom64(Stream.ReadToEnd());
                        }
                        finally
                        {
                            Stream.Close();
                            FileS.Close();
                        }
                    }
                    else
                    {
                        return "";
                    }
                }
                catch
                {
                    return "";
                }
            }
        }

        private Int64 GetCacheSize()
        {
            lock (ObjectLock)
            {
                try
                {
                    string FileName = Path.GetTempPath() + _applicationId + ".dsmk";
                    if (File.Exists(FileName))
                    {
                        FileInfo F = new FileInfo(FileName);
                        return F.Length;
                    }
                    else
                    {
                        return -1;
                    }
                }
                catch
                {
                    return -1;
                }
            }
        }

        private bool SaveCacheFile() 
        {
            lock (ObjectLock)
            {
                try
                {
                    string FileName = Path.GetTempPath() + _applicationId + ".dsmk";

                    if (!File.Exists(FileName))
                    {
                        FileStream FileS = new FileStream(@FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        StreamWriter StreamFile = new StreamWriter(FileS);

                        StreamFile.Write(Util.EncodeTo64(this.JSON));
                        StreamFile.Flush();

                        StreamFile.Close();
                        FileS.Close();

                        File.SetAttributes(FileName, FileAttributes.Hidden);

                        return true;
                    }
                    else
                    {
                        StreamWriter OldFile = File.AppendText(FileName);

                        OldFile.Write("," + Util.EncodeTo64(this.JSON));
                        OldFile.Flush();
                        OldFile.Close();

                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

    } 
}
