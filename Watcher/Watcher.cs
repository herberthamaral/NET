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
using System.Collections.Generic;
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
        Thread StopThread;
        /// <summary>
        /// Thread Lock
        /// </summary>
        private System.Object ObjectLock = new System.Object();

        /// <summary>
        /// Field User GUID
        /// </summary>
        private object _userGUID;

        public object UserGUID
        {
            get {
                if (_userGUID == null)
                    _userGUID = GetUserID();
                return _userGUID; 
            }
        }
        /// <summary>
        /// Field Session Id
        /// </summary>
        private object _sessionGUID;

        public object SessionGUID
        {
            get {
                if (_sessionGUID == null)
                {
                    _sessionGUID = GetGUID();
                }
                return _sessionGUID;
            }
            //private set { _sessionGUID = value; }
        }
        /// <summary>
        /// Field Json
        /// </summary>
        private List<string> _json;
        /// <summary>
        /// Field Application Id
        /// </summary>
        private string _applicationId;
        /// <summary>
        /// Field Error Message
        /// </summary>
        private string _error;

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
        
        private string _postserver = Settings.DefaultServer;

        private int _postport = Settings.DefaultPort;

        private int _posttimeout = Settings.Timeout;

        private bool _postwaitresponse = false;

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

        public List<string> JSON
        {
            get
            {
                if (_json == null)
                    _json = new List<string>();
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

        public bool Start(string ApplicationId, string ApplicationVersion)
        {
            lock (ObjectLock)
            {
                try
                {
                    if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true))
                    {
                        this.ApplicationId = ApplicationId;
                        this.ApplicationVersion = ApplicationVersion;
                        var startjson = new StartAppJson(this);
                        JSON.Add(JsonBuilder.GetJsonFromHashTable(startjson.GetJsonHashTable()));
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
                    if (Started)
                    {
                        if (!string.IsNullOrEmpty(ApplicationId))
                        {
                            var json = new StopAppJson();
                            JSON.Add(JsonBuilder.GetJsonFromHashTable(json.GetJsonHashTable()));
                            string SingleJSON = JsonBuilder.GetJsonFromList(JSON);

                            string CacheData = GetCacheData();
                            if (!string.IsNullOrEmpty(CacheData))
                            {
                                SingleJSON = SingleJSON + "," + CacheData;
                            }

                            int ErrorID;

                            try
                            {
                                Services.PostData(out ErrorID, Settings.ApiEndpoint,JsonBuilder.GetJsonFromList(JSON));
                                JSON.Clear();
                            }
                            finally
                            {
                                JSON.Clear();
                                JSON.Add(SingleJSON);
                            }

                            if (ErrorID == 0)
                            {
                                DeleteCacheFile();
                            }
                            else
                            {
                                SaveCacheFile();
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
                    if (Started)
                    {
                        if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true))
                        {
                            var json = new EventJson(EventCategory, EventName, GetFlowNumber());
                            JSON.Add(JsonBuilder.GetJsonFromHashTable(json.GetJsonHashTable()));
                        }
                    }
                }
                catch
                {
                }
            }
        }

        public void TrackEventPeriod(string EventCategory, string EventName, int EventTime,bool Completed)
        {
            lock (ObjectLock)
            {
                try
                {
                    if (Started)
                    {
                        if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true))
                        {
                            var json = new EventPeriodJson(EventCategory, EventName, GetFlowNumber(), EventTime,Completed);
                            JSON.Add(JsonBuilder.GetJsonFromHashTable(json.GetJsonHashTable()));
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
                    if (Started)
                    {
                        if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true) && (ApplicationException != null))
                        {
                            var json = new ExceptionJson(ApplicationException,GetFlowNumber());
                            JSON.Add(JsonBuilder.GetJsonFromHashTable(json.GetJsonHashTable()));
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
        /// <param name="EventCategory">Event Category param</param>
        /// <param name="EventName">Event Name param</param>
        /// <param name="EventValue">Event Value param</param>
        public void TrackEventValue(string EventCategory, string EventName, string EventValue)
        {
            lock (ObjectLock)
            {
                try
                {
                    if (Started)
                    {
                        if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true))
                        {
                            var json = new EventValueJson(EventCategory, EventName, EventValue, GetFlowNumber());
                            JSON.Add(JsonBuilder.GetJsonFromHashTable(json.GetJsonHashTable()));
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
                    if (Started)
                    {
                        if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true))
                        {
                            var json = new CustomDataJson(CustomDataName, CustomDataValue, GetFlowNumber());
                            JSON.Add(JsonBuilder.GetJsonFromHashTable(json.GetJsonHashTable()));
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
                    if (Started)
                    {
                        if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true))
                        {
                            var json = new LogJson(Message,GetFlowNumber());
                            JSON.Add(JsonBuilder.GetJsonFromHashTable(json.GetJsonHashTable()));
                        }
                    }
                }
                catch
                {
                }
            }
        }


        public bool TrackCustomDataR(string CustomDataName, string CustomDataValue)
        {
            lock (ObjectLock)
            {
                try
                {
                    if (Started)
                    {
                        var json = new CustomDataRJson(CustomDataName, CustomDataValue, GetFlowNumber(), ApplicationId, ApplicationVersion);
                        if (!string.IsNullOrEmpty(ApplicationId) && (Enabled == true))
                        {
                            try
                            {
                                int ErrorID;
                                Services.PostData(out ErrorID, Settings.ApiEndpoint, JsonBuilder.GetJsonFromHashTable(json.GetJsonHashTable()));

                                if (ErrorID == 0)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            catch (Exception)
                            {
                                //saves to send later
                                JSON.Add(JsonBuilder.GetJsonFromHashTable(json.GetJsonHashTable()));
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

        public bool SetUserID(string UserID)
        {
            lock (ObjectLock)
            {
                try
                {
                    RegistryKey reg = Registry.CurrentUser.OpenSubKey("Sofware\\dskMetrics");

                    if (reg == null)
                        reg = Registry.CurrentUser.CreateSubKey("Software\\dskMetrics");

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

                        StreamFile.Write(Util.EncodeTo64(JsonBuilder.GetJsonFromList(JSON)));
                        StreamFile.Flush();

                        StreamFile.Close();
                        FileS.Close();

                        File.SetAttributes(FileName, FileAttributes.Hidden);

                        return true;
                    }
                    else
                    {
                        StreamWriter OldFile = File.AppendText(FileName);

                        OldFile.Write("," + Util.EncodeTo64(JsonBuilder.GetJsonFromList(JSON)));
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

        public void SendDataAsync()
        {
            Services.SendDataAsync(JsonBuilder.GetJsonFromList(JSON));
            JSON.Clear();
        }
    } 
}
