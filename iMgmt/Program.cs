
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace iMgmt
{
    static class Program
    {
       static string sSource = "dotNET Sample App";
       static string sLog = "Application";
       static string sEvent = "Sample Event";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool result;
            var mutex = new System.Threading.Mutex(true, "IMgmt", out result);
            try
            {
                
                if (!result)
                {
                    MessageBox.Show("Another instance is already running.");
                    return;
                }
                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);
                Application.Run(new frmSplash());
                GC.KeepAlive(mutex);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Inventory Mgmt", ex.Message + ":" + ex.InnerException + ":" + ex.StackTrace.ToString());
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }
}
