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

            try
            {
                var guid = new Guid("A643C50D-4206-4121-A895-9EA5C919557A");
                var type = Type.GetTypeFromCLSID(guid);
                var instance = Activator.CreateInstance(type);
            }
            catch (Exception e)
            {
                
                //throw;
            }
            
            Application.Run(new ServerManagerForm());
        }
    }
}
