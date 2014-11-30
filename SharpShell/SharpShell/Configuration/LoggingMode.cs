using System;

namespace SharpShell.Configuration
{
    /// <summary>
    /// Represents the SharpShell logging mode.
    /// </summary>
    [Flags]
    public enum LoggingMode
    {
        /// <summary>
        /// Logging is disabled.
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// Win32 debug output, suitable for dbmon or DbgView.
        /// </summary>
        Debug = 1,

        /// <summary>
        /// Log to the Event Log.
        /// </summary>
        EventLog = 2,

        /// <summary>
        /// Log to file.
        /// </summary>
        File = 4
    }
}