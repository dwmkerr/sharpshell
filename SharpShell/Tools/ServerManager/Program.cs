using SharpShell.Diagnostics;
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
            //  Write to the SharpShell log, can be useful to verify it is running.
            Logging.Log("Sharting SharpShell ServerManager..");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);       
            Application.Run(new ServerManagerForm());
        }
    }
}
