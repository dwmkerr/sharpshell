namespace SharpShell.ServerRegistration
{
    /// <summary>
    /// Represents the type of server registration.
    /// </summary>
    public enum ServerRegistationType
    {
        /// <summary>
        /// The Server is partially registered only - there's no process models.
        /// </summary>
        PartiallyRegistered,

        /// <summary>
        /// It's a native InProc32 server.
        /// </summary>
        NativeInProc32,

        /// <summary>
        /// It's a managed InProc32 server.
        /// </summary>
        ManagedInProc32,
    }
}