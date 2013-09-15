namespace ServerRegistrationManager.OutputService
{
    /// <summary>
    /// Represents a service that is used to write output (perhaps to the console
    /// or a file).
    /// </summary>
    public interface IOutputService
    {
        /// <summary>
        /// Writes a message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="log">if set to <c>true</c> the message is also logged.</param>
        void WriteMessage(string message, bool log = false);

        /// <summary>
        /// Writes the a success message. Implementations may show a tick or the 
        /// message in green etc.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="log">if set to <c>true</c> [log].</param>
        void WriteSuccess(string message, bool log = false);

        /// <summary>
        /// Writes an error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="log">if set to <c>true</c> the message is also logged.</param>
        void WriteError(string error, bool log = false);
    }
}