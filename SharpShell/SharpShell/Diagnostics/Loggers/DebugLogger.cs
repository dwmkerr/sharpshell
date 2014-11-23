using System.Diagnostics;

namespace SharpShell.Diagnostics.Loggers
{
    /// <summary>
    /// A logger which logs to standard debug output.
    /// </summary>
    internal class DebugLogger : ILogger
    {
        /// <summary>
        /// Logs an error.
        /// </summary>
        /// <param name="error">The error.</param>
        public void LogError(string error)
        {
            Debug.WriteLine(error, "Error");
        }

        /// <summary>
        /// Logs a warning.
        /// </summary>
        /// <param name="warning">The warning.</param>
        public void LogWarning(string warning)
        {
            Debug.WriteLine(warning, "Warning");
        }

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogMessage(string message)
        {
            Debug.WriteLine(message, "Message");
        }
    }
}