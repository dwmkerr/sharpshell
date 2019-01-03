using System;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using SharpShell.Attributes;
using SharpShell.Diagnostics;
using SharpShell.ServerRegistration;

namespace SharpShell
{
    /// <summary>
    /// The SharpShellServer class is the base class for all SharpShellServers.
    /// It provides the core standard functionality - registration, unregistration,
    /// identity information (as required by ISharpShellServer), MEF contract inheritance
    /// and definitions of virtual functions that can be overriden by advanced users
    /// to hook into key points in Server Lifecycle.
    ///
    /// Note that ALL derived classes will Export ISharpShellServer - this is a useful
    /// feature as it means that the ServerManager tool (and other tools) can interrogate
    /// assemblies via MEF to get information on servers they contain.
    /// </summary>
    [InheritedExport(typeof(ISharpShellServer))]
    public abstract class SharpShellServer : ISharpShellServer
    {
        /// <summary>
        /// The COM Register function. Called by regasm to register a COM server
        /// in the system. This function will register the server associations defined
        /// by the type's COMServerAssociation attributes.
        /// </summary>
        /// <param name="type">The type.</param>
        [ComRegisterFunction]
        internal static void Register(Type type)
        {
            Logging.Log("Registering server for type " + type.Name);

            //  Register the type, use the operating system architecture to determine
            //  what registration type to perform.
            ServerRegistrationManager.UnregisterServerType(
                type,
                Environment.Is64BitOperatingSystem ? RegistrationType.OS64Bit : RegistrationType.OS32Bit
            );
        }

        /// <summary>
        /// The COM Unregister function. Called by regasm to unregister a COM server
        /// in the system. This function will unregister the server associations defined
        /// by the type's COMServerAssociation attributes.
        /// </summary>
        /// <param name="type">The type.</param>
        [ComUnregisterFunction]
        internal static void Unregister(Type type)
        {
            Logging.Log("Unregistering server for type " + type.Name);

            //  Unregister the type, use the operating system architecture to determine
            //  what registration type to unregister.
            ServerRegistrationManager.RegisterServerType(
                type,
                Environment.Is64BitOperatingSystem ? RegistrationType.OS64Bit : RegistrationType.OS32Bit
            );
        }
        
        /// <summary>
        /// Logs the specified message to the SharpShell log, with the name of the type.
        /// </summary>
        /// <param name="message">The message.</param>
        protected virtual void Log(string message)
        {
            //  Log the message, but put our type name first.
            Logging.Log(DisplayName + ": " + message);
        }

        /// <summary>
        /// Logs the specified message to the SharpShell log as an error, with the name of the type.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        protected virtual void LogError(string message, Exception exception = null)
        {
            //  Log the error, but put our type name first.
            Logging.Error(DisplayName + ": " + message, exception);
        }

        /// <summary>
        /// Gets a display name for the server.
        /// If the [DisplayName] attribute is defined on the type, then the value
        /// of this attribute will be used. If not, then the type name will be used.
        /// </summary>
        /// <value>
        /// The name of the server.
        /// </value>
        public string DisplayName => DisplayNameAttribute.GetDisplayNameOrTypeName(GetType());

        /// <summary>
        /// Gets the type of the server.
        /// </summary>
        /// <value>
        /// The type of the server.
        /// </value>
        public ServerType ServerType => ServerTypeAttribute.GetServerType(GetType());

        /// <summary>
        /// Gets the server CLSID.
        /// </summary>
        public Guid ServerClsid => GetType().GUID;
    }
}
