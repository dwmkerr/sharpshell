using System;

namespace SharpShell
{
    /// <summary>
    /// Core interface for SharpShell COM servers.
    /// </summary>
    public interface ISharpShellServer
    {
        /// <summary>
        /// Gets the type of the server.
        /// </summary>
        /// <value>
        /// The type of the server.
        /// </value>
        ServerType ServerType { get; }

        /// <summary>
        /// Gets the server CLSID.
        /// </summary>
        Guid ServerClsid { get; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        string DisplayName { get; }
    }
}
