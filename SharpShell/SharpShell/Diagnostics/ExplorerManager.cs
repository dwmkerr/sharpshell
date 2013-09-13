using System.Diagnostics;
using System.Linq;

namespace SharpShell.Diagnostics
{
    /// <summary>
    /// A Helper Class for managing certain features of Windows Explorer
    /// </summary>
    public static class ExplorerManager
    {
        /// <summary>
        /// Restarts the explorer process.
        /// </summary>
        public static void RestartExplorer()
        {
            var explorerProcess = Process.GetProcesses().FirstOrDefault(p => p.ProcessName == "explorer");
            if (explorerProcess != null)
                explorerProcess.Kill();
        }
    }
}