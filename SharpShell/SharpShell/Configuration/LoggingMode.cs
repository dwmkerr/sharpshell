namespace SharpShell.Configuration
{
    /// <summary>
    /// Represents the SharpShell logging mode.
    /// </summary>
    public enum LoggingMode
    {
        /// <summary>
        /// Logging is disabled.
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// Log to the Event Log.
        /// </summary>
        EventLog = 1,

        /// <summary>
        /// Log to file.
        /// </summary>
        File = 2
    }
}