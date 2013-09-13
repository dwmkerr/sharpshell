using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace ServerManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if(EventLog.SourceExists("SharpShell") == false)
                EventLog.CreateEventSource("SharpShell", "Application");

            Application.Run(new ServerManagerForm());
        }
    }
}
