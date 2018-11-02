using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace SharpShell.Diagnostics.Loggers
{
    /// <summary>
    /// SharpShell logger to write to a log file. Safe across processes.
    /// </summary>
    internal class FileLogger : ILogger
    {
        /// <summary>
        /// Mutex to allow multiple processes to write to the file.
        /// </summary>
        private static readonly Mutex mutex = new Mutex(false, @"Global\SharpShellLogFile");

        /// <summary>
        /// The log file path.
        /// </summary>
        private readonly string logPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogger"/> class.
        /// </summary>
        /// <param name="logPath">The log path.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public FileLogger(string logPath)
        {
            this.logPath = logPath;
        }

        /// <summary>
        /// Writes the specified line to the log file.
        /// </summary>
        /// <param name="line">The line.</param>
        private void Write(string line)
        {
            try
            {
                //  Wait for access via the mutex.
                mutex.WaitOne();

                //  Write to the line to the file.
                using (var w = File.AppendText(logPath))
                {
                    var t = DateTime.Now.ToString(@"yyyy-MM-dd HH:mm:ss.fffZ");
                    w.WriteLine(
                        $"{t} - {Process.GetCurrentProcess().ProcessName} - {line}");
                    w.Flush();
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine("An exception occured trying to write to the file log. Details: {0}", exception);
            }
            finally
            {
                //  Release the mutex.
                mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Logs an error.
        /// </summary>
        /// <param name="error">The error.</param>
        public void LogError(string error)
        {
            Write("error: " + error);
        }

        /// <summary>
        /// Logs a warning.
        /// </summary>
        /// <param name="warning">The warning.</param>
        public void LogWarning(string warning)
        {
            Write("warning: " + warning);
        }

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogMessage(string message)
        {
            Write(message);
        }
    }
}