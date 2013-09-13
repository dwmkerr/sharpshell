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
        void WriteMessage(string message);

        /// <summary>
        /// Writes an error.
        /// </summary>
        /// <param name="error">The error.</param>
        void WriteError(string error);
    }
}