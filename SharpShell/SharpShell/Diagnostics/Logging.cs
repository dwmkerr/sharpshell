using System;
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
        /// The logger used.
        /// </summary>
        private static readonly ILogger logger;

        /// <summary>
        /// Initializes the <see cref="Logging"/> class.
        /// </summary>
        static Logging()
        {
            //  Get the system config (if present).
            var config = SystemConfigurationProvider.Configuration;
            
            //  Create the appropriate logger.
            switch (config.LoggingMode)
            {
                case LoggingMode.EventLog:
                    logger = new EventLogLogger();
                    break;
                case LoggingMode.File:
                    logger = new FileLogger(config.LogPath);
                    break;
            }

            //  Always log our host process.
            logger.LogMessage(string.Format("SharpShell Diagnostics Initialised. Process {0}.", Process.GetCurrentProcess().ProcessName));

            //  We will log unhandled exceptions.
            AppDomain.CurrentDomain.UnhandledException += (sender, args) => Error("SharpShell - Unhandled Exception in the AppDomain", args.ExceptionObject as Exception);
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Log(string message)
        {
            if(logger != null)
                logger.LogMessage(message);
        }

        /// <summary>
        /// Errors the specified message as an error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Error(string message, Exception exception = null)
        {
            if (logger == null) return;
            
            logger.LogError(message);
            if (exception != null)
                logger.LogError(exception.ToString());
        }
    }
}
