using System;
using System.Diagnostics;

namespace SharpShell.Diagnostics.Loggers
{
    /// <summary>
    /// A logger which logs to the Windows Event Log.
    /// </summary>
    internal class EventLogLogger : ILogger
    {
        /// <summary>
        /// The source created flag. If true, we have a source.
        /// </summary>
        private readonly bool sourceCreated;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogLogger" /> class.
        /// </summary>
        public EventLogLogger()
        {
            //  Check whether the source exists.
            try
            {
                if (EventLog.SourceExists(EventLog_Source) == false)
                    EventLog.CreateEventSource(EventLog_Source, EventLog_Log);
                sourceCreated = true;
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Exception creating SharpShell event log source. Details: {0}", exception);
            }
        }

        /// <summary>
        /// Logs an error.
        /// </summary>
        /// <param name="error">The error.</param>
        public void LogError(string error)
        {
            if (sourceCreated == false) return;
            EventLog.WriteEntry(EventLog_Source, error, EventLogEntryType.Error);
        }

        /// <summary>
        /// Logs a warning.
        /// </summary>
        /// <param name="warning">The warning.</param>
        public void LogWarning(string warning)
        {
            if (sourceCreated == false) return;
            EventLog.WriteEntry(EventLog_Source, warning, EventLogEntryType.Warning);
        }

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogMessage(string message)
        {
            if (sourceCreated == false) return;
            EventLog.WriteEntry(EventLog_Source, message, EventLogEntryType.Information);
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