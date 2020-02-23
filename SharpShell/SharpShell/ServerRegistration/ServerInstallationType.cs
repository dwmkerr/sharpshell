namespace SharpShell.ServerRegistration
{
    /// <summary>
    ///     Represents the type of server registration.
    /// </summary>
    public enum ServerInstallationType
    {
        /// <summary>
        ///     The server is partially installed only - there's no process models.
        /// </summary>
        PartiallyInstalled,

        /// <summary>
        ///     It's a native InProc32 server.
        /// </summary>
        NativeInProcess32,

        /// <summary>
        ///     It's a managed InProc32 server.
        /// </summary>
        ManagedInProcess32
    }
}