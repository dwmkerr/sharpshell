using System;
using System.Linq;
using SharpShell.ServerRegistration;

namespace SharpShell.Attributes
{
    /// <summary>
    /// The ServerTypeAttribute can be used internally by SharpShell
    /// to mark the type of a server base class. By setting this type,
    /// classes derived from the decorated class will be able to use
    /// the COMServerAssociationAttribute without any extra configuration.
    /// </summary>
    [Serializable]
    public class ServerTypeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerTypeAttribute"/> class.
        /// </summary>
        /// <param name="serverType">Type of the server.</param>
        public ServerTypeAttribute(ServerType serverType)
        {
            //  Set the server type.
            ServerType = serverType;
        }

        /// <summary>
        /// Gets the server type attribute of a type of the association.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The ServerTypeAttribute of the type, or null if not set.</returns>
        public static ServerTypeAttribute GetServerTypeAttribute(Type type)
        {
            var attribute = ServerSandBox
                .GetAttributesSafe(type, nameof(ServerTypeAttribute), true)
                .FirstOrDefault();

            if (attribute == null)
            {
                return null;
            }

            var serverType = ServerSandBox.GetByValPropertySafe<ServerType>(attribute, nameof(ServerType));

            if (serverType != null)
            {
                return new ServerTypeAttribute(serverType.Value);
            }

            return null;
        }

        public ShellExtensionType ShellExtensionType
        {
            get
            {
                switch (ServerType)
                {
                    case ServerType.ShellContextMenu:

                        return ShellExtensionType.ShellContextMenu;
                    case ServerType.ShellPropertySheet:

                        return ShellExtensionType.ShellPropertySheet;
                    case ServerType.ShellIconHandler:

                        return ShellExtensionType.ShellIconHandler;
                    case ServerType.ShellInfoTipHandler:

                        return ShellExtensionType.ShellInfoTipHandler;
                    case ServerType.ShellDropHandler:

                        return ShellExtensionType.ShellDropHandler;
                    case ServerType.ShellPreviewHandler:

                        return ShellExtensionType.ShellPreviewHandler;
                    case ServerType.ShellDataHandler:

                        return ShellExtensionType.ShellDataHandler;
                    case ServerType.ShellThumbnailHandler:

                        return ShellExtensionType.ShellThumbnailHandler;
                    case ServerType.None:
                    case ServerType.ShellIconOverlayHandler:
                    case ServerType.ShellNamespaceExtension:
                    case ServerType.ShellDeskBand:
                    default:

                        return ShellExtensionType.None;
                }
            }
        }

        /// <summary>
        /// Gets the type of the server.
        /// </summary>
        /// <value>
        /// The type of the server.
        /// </value>
        public ServerType ServerType { get; }
    }
}
