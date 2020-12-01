using System;
using System.Collections.Generic;

namespace SharpShell.ServerRegistration
{
    /// <summary>
    /// Represents registration info for a server.
    /// </summary>
    public class ShellExtensionRegistrationInfo
    {
        public ShellExtensionRegistrationInfo()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellExtensionRegistrationInfo"/> class.
        /// </summary>
        /// <param name="serverRegistationType">Type of the server registation.</param>
        /// <param name="serverCLSID">The server CLSID.</param>
        public ShellExtensionRegistrationInfo(ServerRegistationType serverRegistationType, Guid serverCLSID)
        {
            ServerRegistationType = serverRegistationType;
            ServerCLSID = serverCLSID;
        }

        /// <summary>
        /// The class registrations.
        /// </summary>
        public readonly List<ClassRegistration> classRegistrations = new List<ClassRegistration>();

        /// <summary>
        /// Gets the server CLSID.
        /// </summary>
        public Guid ServerCLSID { get; set; }

        /// <summary>
        /// Gets the type of the shell extension.
        /// </summary>
        /// <value>
        /// The type of the shell extension.
        /// </value>
        public ShellExtensionType ShellExtensionType { get; set; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets the server path.
        /// </summary>
        public string ServerPath { get; set; }

        /// <summary>
        /// Gets the threading model.
        /// </summary>
        public string ThreadingModel { get; set; }

        /// <summary>
        /// Gets the assembly version.
        /// </summary>
        public string AssemblyVersion { get; set; }

        /// <summary>
        /// Gets the assembly.
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// Gets the class.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets the runtime version.
        /// </summary>
        public string RuntimeVersion { get; set; }

        /// <summary>
        /// Gets the codebase path.
        /// </summary>
        public string CodeBase { get; set; }

        /// <summary>
        /// Gets a value indicating whether this extension is on the approved list.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is approved; otherwise, <c>false</c>.
        /// </value>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Gets the type of the server registation.
        /// </summary>
        /// <value>
        /// The type of the server registation.
        /// </value>
        public ServerRegistationType ServerRegistationType { get; set; }

        /// <summary>
        /// Gets the class registrations.
        /// </summary>
        /// <value>
        /// The class registrations.
        /// </value>
        public IEnumerable<ClassRegistration> ClassRegistrations { get { return classRegistrations; } }
    }
}
