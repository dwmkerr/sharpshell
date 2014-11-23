using System;
using System.Collections.Generic;
using System.Diagnostics;
using SharpShell.Configuration;
using SharpShell.Diagnostics.Loggers;

namespace SharpShell.Diagnostics
{
    /// <summary>
    /// The logging class is used for SharpShell logging.
    /// </summary>
    public static class Logging
    {
        /// <summary>
        /// The loggers used.
        /// </summary>
        private static readonly List<ILogger> loggers = new List<ILogger>();

        /// <summary>
        /// Initializes the <see cref="Logging"/> class.
        /// </summary>
        static Logging()
        {
            //  Get the system config (if present).
            var config = SystemConfigurationProvider.Configuration;

            //  Add configured loggers.
            try
            {
                if(config.LoggingMode.HasFlag(LoggingMode.Debug))
                    loggers.Add(new DebugLogger());
                if(config.LoggingMode.HasFlag(LoggingMode.EventLog))
                    loggers.Add(new EventLogLogger());
                if(config.LoggingMode.HasFlag(LoggingMode.File))
                    loggers.Add(new FileLogger(config.LogPath));
            }
            catch (Exception exception)
            {
                //  There's not much we can do here except fall back on the debug log.
                Debug.WriteLine("An unhandled exception occured trying to initialise SharpShell logging.");
                Debug.WriteLine(exception.ToString());
            }

            //  Always log our host process.
            Log(string.Format("SharpShell Diagnostics Initialised. Process {0}.", Process.GetCurrentProcess().ProcessName));

            //  We will log unhandled exceptions.
            AppDomain.CurrentDomain.UnhandledException += (sender, args) => Error("SharpShell - Unhandled Exception in the AppDomain", args.ExceptionObject as Exception);
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Log(string message)
        {
            try
            {
                loggers.ForEach(l => l.LogMessage(message));
            }
            catch (Exception exception)
            {
                Debug.WriteLine("An unhandled exception occured logging the message {0}. Exception details: {1}", 
                    message, exception);
            }
        }

        /// <summary>
        /// Errors the specified message as an error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Error(string message, Exception exception = null)
        {
            try
            {
                loggers.ForEach(l =>
                    {
                        l.LogError(message);
                        if (exception != null) l.LogError(exception.ToString());
                    });
                }
            catch (Exception e)
            {
                Debug.WriteLine("An unhandled exception occured logging the error {0}. Exception details: {1}",
                    message, e);
            }
        }
    }
}
