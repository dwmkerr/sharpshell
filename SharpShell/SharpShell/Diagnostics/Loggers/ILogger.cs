namespace SharpShell.Diagnostics.Loggers
{
    /// <summary>
    /// Defines a contract for a type which can log messages.
    /// </summary>
    internal interface ILogger
    {
        /// <summary>
        /// Logs an error.
        /// </summary>
        /// <param name="error">The error.</param>
        void LogError(string error);

        /// <summary>
        /// Logs a warning.
        /// </summary>
        /// <param name="warning">The warning.</param>
        void LogWarning(string warning);

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">The message.</param>
        void LogMessage(string message);
    }
}
