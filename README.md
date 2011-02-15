DeskMetrics.NET -- .NET assembly for DeskMetrics Analytics
===========================================================

DeskMetrics is a Google Analytics-like service for non-html based apps. You can read more at [DeskMetrics website](http://deskmetrics.com/)

This assembly aims to provide a simple interface to integrate your .NET-based app with DeskMetrics service.


How to use:
------------

There are two very important functions to integrate your application to our service:

 - `DeskMetricsStart`
 - `DeskMetricsStop`

Calling these two methods is mandatory inside your app, because they're responsible for session generating, system information tracking and for reporting the captured events to DeskMetrics server.

Here is an example:

    using DeskMetrics;

    namespace MyApplication
    {
        public partial class Form1 : Form
        {
            DeskMetrics.Watcher DeskMetrics = new DeskMetrics.Watcher();

            public Form1()
            {
                DeskMetrics.Start("YOUR APPLICATION ID", "1.0", true);
                InitializeComponent();
            }

            private void Form1_FormClosing(object sender, FormClosingEventArgs e)
            {
                DeskMetrics.Stop();
            }
        }
    }

You can read more in [DeskMetrics support page](http://support.deskmetrics.com/kb/getting-started/integrating-the-component)

License
--------

This code is provided under the DeskMetrics Modified BSD License  
A copy of this license has been distributed in a file called      
LICENSE with this source code.                                    
