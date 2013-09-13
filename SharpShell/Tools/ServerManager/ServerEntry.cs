using System;
using System.Reflection;
using SharpShell;

namespace ServerManager
{
    /// <summary>
    /// A Server Entry in the application. Keeps track of viewmodel style data 
    /// for a sharp shell server.
    /// </summary>
    public class ServerEntry
    {
        /// <summary>
        /// Gets or sets the name of the server.
        /// </summary>
        /// <value>
        /// The name of the server.
        /// </value>
        public string ServerName { get; set; }

        /// <summary>
        /// Gets or sets the server path.
        /// </summary>
        /// <value>
        /// The server path.
        /// </value>
        public string ServerPath { get; set; }

        /// <summary>
        /// Gets or sets the type of the server.
        /// </summary>
        /// <value>
        /// The type of the server.
        /// </value>
        public ServerType ServerType { get; set; }

        /// <summary>
        /// Gets or sets the class id.
        /// </summary>
        /// <value>
        /// The class id.
        /// </value>
        public Guid ClassId { get; set; }

        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        /// <value>
        /// The server.
        /// </value>
        public ISharpShellServer Server { get; set; }

        /// <summary>
        /// Gets the security status.
        /// </summary>
        /// <returns></returns>
        public string GetSecurityStatus()
        {
            AssemblyName asmName = AssemblyName.GetAssemblyName(ServerPath);
            var key = asmName.GetPublicKey();
            return key != null && key.Length > 0 ? "Signed" : "Not Signed";
        }
    }
}
