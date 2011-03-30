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
using System.IO;
using System.Net;
using System.Collections;
using System.Net.Security;
using System.Threading;
using System.Security.Cryptography.X509Certificates;

namespace DeskMetrics
{
    public class Services
    {
        private Watcher watcher;
        public Services(Watcher watcher)
        {
            this.watcher = watcher;
        }

        protected object ObjectLock = new Object();

        Thread SendDataThread;

        public string PostData(out int ErrorID, string PostMode,string json)
        {
            lock (ObjectLock)
            {
                try
                {
                    ErrorID = 0;

                    if (!string.IsNullOrEmpty(watcher.ApplicationId) && (watcher.Enabled == true))
                    {
                        string url;

                        if (watcher.PostPort == 443)
                        {
                            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                                delegate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslError)
                                {
                                    bool validationResult = true;
                                    return validationResult;
                                };

                            url = "https://" + watcher.ApplicationId + "." + Settings.DefaultServer + PostMode;
                        }
                        else
                        {
                            url = "http://" + watcher.ApplicationId + "." + Settings.DefaultServer + PostMode;
                        }

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.Timeout = Settings.Timeout;

                        if (!string.IsNullOrEmpty(watcher.ProxyHost))
                        {
                            string uri;

                            WebProxy myProxy = new WebProxy();

                            if (watcher.ProxyPort != 0)
                            {
                                uri = watcher.ProxyHost + ":" + watcher.ProxyPort ;
                            }
                            else
                            {
                                uri = watcher.ProxyHost;
                            }

                            Uri newUri = new Uri(uri);
                            myProxy.Address = newUri;
                            myProxy.Credentials = new NetworkCredential(watcher.ProxyUserName, watcher.ProxyPassword);
                            request.Proxy = myProxy;
                        }
                        else
                        {
                            request.Proxy = WebRequest.DefaultWebProxy;
                        }

                        request.UserAgent = Settings.UserAgent;
                        request.KeepAlive = false;
                        request.ProtocolVersion = HttpVersion.Version10;
                        request.Method = "POST";

                        byte[] postBytes = null;

                        postBytes = Encoding.UTF8.GetBytes("data=[" + json + "]");

                        request.ContentType = "application/x-www-form-urlencoded";
                        request.ContentLength = postBytes.Length;

                        Stream requestStream = request.GetRequestStream();
                        requestStream.Write(postBytes, 0, postBytes.Length);
                        requestStream.Close();

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        StreamReader streamreader = new StreamReader(response.GetResponseStream());
                        Console.WriteLine(streamreader.ReadToEnd());
                        streamreader.Close();

                        ErrorID = 0;
                        return "";
                    }
                    else
                    {
                        watcher.Error = Settings.ErrorCodes["-11"].ToString();
                        ErrorID = -11;
                        return "";
                    }
                }
                catch (WebException webException)
                {
                    watcher.Error = webException.ToString();
                    ErrorID = -1;
                    return "";
                }
                catch
                {
                    ErrorID = -1;
                    return "";
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="Log">json message</param>
        public bool SendData(string json)
        {
            lock (ObjectLock)
            {
                try
                {
                    if (watcher.Started)
                    {
                        if (!string.IsNullOrEmpty(watcher.ApplicationId) && (watcher.Enabled == true))
                        {
                            int ErrorID;
                            PostData(out ErrorID, Settings.ApiEndpoint,json);
                            return ErrorID == 0;
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

        private string _json;
        public bool SendDataAsync(string json)
        {
            lock (ObjectLock)
            {
                try
                {
                    if (!string.IsNullOrEmpty(watcher.ApplicationId) && (watcher.Enabled == true))
                    {
                        _json = json;
                        if (SendDataThread == null)
                        {
                            SendDataThread = new Thread(_SendDataThreadFunc);
                        }

                        if ((SendDataThread != null) && (SendDataThread.IsAlive == false))
                        {
                            SendDataThread = new Thread(_SendDataThreadFunc);
                            SendDataThread.Name = "SendDataSender";
                            SendDataThread.Start();
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
                catch
                {
                    return false;
                }
            }
        }

        private void _SendDataThreadFunc()
        {
            lock (ObjectLock)
            {
                try
                {
                    int ErrorID;
                    try
                    {
                        PostData(out ErrorID, Settings.ApiEndpoint,_json);
                    }
                    catch (Exception)
                    {
                    }
                }
                catch
                {
                }
            }

        }
    }
}
