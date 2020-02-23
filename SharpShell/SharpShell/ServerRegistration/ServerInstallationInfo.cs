using System;
using System.Linq;

namespace SharpShell.ServerRegistration
{
    public class ServerInstallationInfo
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ShellExtensionRegisteredAssociationInfo" /> class.
        /// </summary>
        /// <param name="serverClassId">The server class id.</param>
        internal ServerInstallationInfo(Guid serverClassId)
        {
            ServerInstallationType = ServerInstallationType.PartiallyInstalled;
            ServerClassId = serverClassId;
        }

        internal ServerInstallationInfo(
            Guid serverClassId,
            string serverPath,
            string threadingModel) : this(serverClassId)
        {
            ServerInstallationType = ServerInstallationType.NativeInProcess32;
            ServerPath = serverPath;
            ThreadingModel = threadingModel;
        }

        internal ServerInstallationInfo(
            Guid serverClassId,
            string serverPath,
            string threadingModel,
            ManagedAssemblyInfo assembly,
            string managedClassName) : this(serverClassId, serverPath, threadingModel)
        {
            ServerInstallationType = ServerInstallationType.ManagedInProcess32;
            ManagedAssembly = assembly;
            ManagedClassName = managedClassName;
        }

        public ManagedAssemblyInfo ManagedAssembly { get; set; }

        /// <summary>
        ///     Gets the managed assembly class.
        /// </summary>
        public string ManagedClassName { get; }

        /// <summary>
        ///     Gets the server class id.
        /// </summary>
        public Guid ServerClassId { get; }

        /// <summary>
        ///     Gets the type of the server registration.
        /// </summary>
        /// <value>
        ///     The type of the server registration.
        /// </value>
        public ServerInstallationType ServerInstallationType { get; }

        /// <summary>
        ///     Gets the server path.
        /// </summary>
        public string ServerPath { get; }

        /// <summary>
        ///     Gets the threading model.
        /// </summary>
        public string ThreadingModel { get; }

        public SharpShellServerInfo GetSharpShellServerInformation()
        {
            if (ServerInstallationType != ServerInstallationType.ManagedInProcess32 || ManagedAssembly == null)
            {
                throw new InvalidOperationException(
                    "This operation can not be completed on a native or partially registered extension."
                );
            }

            return SharpShellServerInfo.FromAssembly(ManagedAssembly)
                .FirstOrDefault(info => info.ClassFullName == ManagedClassName);
        }
    }
}