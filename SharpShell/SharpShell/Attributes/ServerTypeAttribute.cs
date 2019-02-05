using System;
using System.Linq;

namespace SharpShell.Attributes
{
    /// <summary>
    /// The ServerTypeAttribute can be used internally by SharpShell
    /// to mark the type of a server base class. By setting this type,
    /// classes derived from the decorated class will be able to use
    /// the COMServerAssociationAttribute without any extra configuration.
    /// </summary>
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
        /// Gets the server type of a type of the association.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The ServerType of the type, or None if not set.</returns>
        public static ServerType GetServerType(Type type)
        {
            var attribute = type.GetCustomAttributes(typeof(ServerTypeAttribute), true)
                .OfType<ServerTypeAttribute>().FirstOrDefault();
            return attribute != null ? attribute.ServerType : ServerType.None;
        }

        /// <summary>
        /// Gets the type of the server.
        /// </summary>
        /// <value>
        /// The type of the server.
        /// </value>
        public ServerType ServerType { get; private set; }
    }
}
