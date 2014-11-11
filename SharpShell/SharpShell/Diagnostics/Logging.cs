using System;
using System.Diagnostics;

namespace SharpShell.Diagnostics
{
    /// <summary>
    /// The logging class is used for SharpShell logging.
    /// </summary>
    public static class Logging
    {
        /// <summary>
        /// Initializes the <see cref="Logging"/> class.
        /// </summary>
        static Logging()
        {
            DebugLog(string.Format("SharpShell Diagnostics Initialised. Process {0}.", Process.GetCurrentProcess().ProcessName));

#if DEBUG
            //  If we're in debug mode, we can also register a handler for unhandled exceptions in the domain.
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
#endif
        }

        /// <summary>
        /// Handles the UnhandledException event of the CurrentDomain.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //  DebugLog the unhandled exception.
            DebugError("SharpShell - Unhandled Exception in the AppDomain", e.ExceptionObject as Exception);
        }

        /// <summary>
        /// Logs the specified message to the Event Log. This message is only logged in Debug mode.
        /// </summary>
        /// <param name="message">The message.</param>
        [Conditional("DEBUG")]
        public static void DebugLog(string message)
        {
            //  DebugLog to the event log.
            EventLog.WriteEntry(EventLog_Source, message);
        }

        /// <summary>
        /// Logs the specified message to the Event Log.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Log(string message)
        {
            DebugLog(message);
        }

        /// <summary>
        /// Logs the specified message as an error.
        /// This message is only logged in Debug Mode.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        [Conditional("DEBUG")]
        public static void DebugError(string message, Exception exception = null)
        {
            //  Write the message.
            EventLog.WriteEntry(EventLog_Source, message, EventLogEntryType.Error);
            
            //  Write the exception, if it exists.
            if(exception != null)
                EventLog.WriteEntry(EventLog_Source, exception.ToString(), EventLogEntryType.Error);
        }

        /// <summary>
        /// Errors the specified message as an error to the Event Log.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Error(string message, Exception exception = null)
        {
            //  Write the message.
            EventLog.WriteEntry(EventLog_Source, message, EventLogEntryType.Error);

            //  Write the exception, if it exists.
            if (exception != null)
                EventLog.WriteEntry(EventLog_Source, exception.ToString(), EventLogEntryType.Error);
        }

        /// <summary>
        /// Determines whether logging is enabled.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if logging is enabled; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLoggingEnabled()
        {
            //  Check whether the source exists.
            return EventLog.SourceExists(EventLog_Source);
        }

        /// <summary>
        /// Enables or disables logging.
        /// </summary>
        /// <param name="enable">if set to <c>true</c> enable logging, otherwise, disable logging.</param>
        /// <returns></returns>
        public static void EnableLogging(bool enable)
        {
            if(IsLoggingEnabled() && !enable)
                EventLog.DeleteEventSource(EventLog_Source);
            else if(!IsLoggingEnabled() && enable)
                EventLog.CreateEventSource(EventLog_Source, EventLog_Log);
        }

        /// <summary>
        /// The event log log.
        /// </summary>
        private const string EventLog_Log = @"Application";

        /// <summary>
        /// The EventLog Source for SharpShell.
        /// </summary>
        public const string EventLog_Source = @"SharpShell";
    }
}
