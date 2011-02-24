// **********************************************************//
//                                                           //
//     DeskMetrics .NET - Application Example                //
//     Copyright (c) 2010-2011                               //
//                                                           //
//     http://deskmetrics.com                                //
//     support@deskmetrics.com                               //
//                                                           //
//     The entire contents of this file is protected by      //
//     International Copyright Laws. Unauthorized            //
//     reproduction, reverse-engineering, and distribution   //
//     of all or any portion of the code contained in this   //
//     file is strictly prohibited and may result in severe  //
//     civil and criminal penalties and will be prosecuted   //
//     to the maximum extent possible under the law.         //
//                                                           //
// **********************************************************//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.IO;

namespace componentNETapp
{
    public partial class Form1 : Form
    {
        DeskMetrics.Watcher DeskMetrics = new DeskMetrics.Watcher();

        public Form1()
        {
            // Tracks Exceptions Automatically
            AppDomain.CurrentDomain.UnhandledException += (s, e) => DeskMetrics.TrackException(e.ExceptionObject as Exception);

            // Track WinForms Exceptions Automatically
            Application.ThreadException += (s, e) => DeskMetrics.TrackException(e.Exception);

            // Starts the DeskMetrics component (required)
            // IMPORTANT! Do not forget to set your application ID
            DeskMetrics.Start("4d47c012d9340b116a000000", "1.0", true);

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Tracks a window event
            // IMPORTANT! The event category must to be "Window"
            DeskMetrics.TrackEvent("Window", "MainWindow");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Finishes the DeskMetrics component (required)
            DeskMetrics.Stop();
        }

        private void btnTrackLog_Click(object sender, EventArgs e)
        {
            DeskMetrics.TrackLog(edtLogs.Text);
        }

        private void btnTrackCustomData_Click(object sender, EventArgs e)
        {
            // Tracks a custom data
            DeskMetrics.TrackCustomData("ZipCode", edtCustomData.Text);
        }

        private void btnTrackValue_Click(object sender, EventArgs e)
        {
            // Tracks a simple event with its retuned value
            DeskMetrics.TrackEventValue("Values", "Size", edtEventValue.Text);
        }

        private void btnSetProxy_Click(object sender, EventArgs e)
        {
            // Set proxy configuration
            DeskMetrics.ProxyHost = edtProxyHost.Text;
            DeskMetrics.ProxyPort = Int32.Parse(edtProxyPort.Text);
            DeskMetrics.ProxyUserName = edtProxyUser.Text;
            DeskMetrics.ProxyPassword = edtProxyPass.Text;

            MessageBox.Show("OK!");
        }

        private void btnCDRealTime_Click(object sender, EventArgs e)
        {
            bool Result;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                // Sends a custom data to server and WAIT a response
                //Result = DeskMetrics.TrackCustomDataR("Username", edtCDEmail.Text);

                // Sends a custom data (async) to server
                 Result = DeskMetrics.TrackCustomDataRASync("Username", edtCDEmail.Text);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

            
            if (Result) 
            {
                // Data stored successfully
                MessageBox.Show("OK!");
            }
            else
            {
                // An error occurred
                MessageBox.Show("Sorry! Try again later.");
            }
        }

        private void btnTrackEvent_Click(object sender, EventArgs e)
        {
             // Sends a simple log message
            DeskMetrics.TrackEvent("Buttons", "MyButton");
        }

        private void btnTrackWindow_Click(object sender, EventArgs e)
        {
            // Tracks a window event
            // IMPORTANT! The event category must to be "Window"
            DeskMetrics.TrackEvent("Window", "WindowName");

            // Create a window here!
            //
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (YesRadio.Checked == true)
            {
                DeskMetrics.Enabled = true;
                DeskMetricsPanel.Visible = true;
            }
            else
            {
                if (NoRadio.Checked == true)
                {
                    DeskMetrics.Enabled = false;
                    DeskMetricsPanel.Visible = false;
                }
            }
        }

        private void btnTrackException_Click(object sender, EventArgs e)
        {
            try
            {
                FileStream fs = new FileStream ("C:\\InvalidFile", FileMode.Open);
            }
            catch (Exception ex)
            {
                // Tracks an exception
                DeskMetrics.TrackException(ex);
            }
        }

        private void btnTrackExceptionsUn_Click(object sender, EventArgs e)
        {
            // Instructions --> public Form1() {}
            int A = 0;
            int B = 10;
            int C;
            C = B / A;
        }

        private void btnSyncNow_Click(object sender, EventArgs e)
        {
            DeskMetrics.Services.SendDataAsync();
        }

        private void btnEventPeriod_Click(object sender, EventArgs e)
        {
            // Tracks a period
            DeskMetrics.TrackEventPeriod("EventsPeriod", "MyEvent", 50);
            // 50 == 50 seconds
        }

        private void btnTrackLicense_Click(object sender, EventArgs e)
        {
            // Tracks a license
            // IMPORTANT! The event category must to be "License"

            if (rFree.Checked == true)
                DeskMetrics.TrackCustomData("License", "F");

            if (fTrial.Checked == true)
                DeskMetrics.TrackCustomData("License", "T");

            if (fDemo.Checked == true)
                DeskMetrics.TrackCustomData("License", "D");

            if (fRegistered.Checked == true)
                DeskMetrics.TrackCustomData("License", "R");

            if (fCracked.Checked == true)
                DeskMetrics.TrackCustomData("License", "C");
        }
    }
}
