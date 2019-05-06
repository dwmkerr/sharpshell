using System;
using System.Linq;
using System.Security.AccessControl;
using Microsoft.Win32;
using SharpShell.Attributes;
using SharpShell.Exceptions;
using SharpShell.Extensions;
using SharpShell.Registry;
using SharpShell.ServerRegistration;

namespace SharpShell.SharpPreviewHandler
{
    /// <summary>
    /// A logic only class which can register a Preview Handler.
    /// </summary>
    internal static class PreviewHandlerRegistrar
    {
        private const string PreviewHandlersKey = @"Software\Microsoft\Windows\CurrentVersion\PreviewHandlers";

        public static void Register(Type serverType, RegistrationType registrationType)
        {
            //  Get the preview handler attribute. If it is missing, throw a registration exception.
            var previewHandlerAttribute = PreviewHandlerAttribute.GetPreviewHandlerAttribute(serverType);
            if (previewHandlerAttribute == null)
            {
                throw new ServerRegistrationException("The server does not have a [PreviewHandler] attribute set.");
            }

            //  We will use the display name a few times.
            var displayName = DisplayNameAttribute.GetDisplayNameOrTypeName(serverType);

            //  Open the local machine.
            using (var localMachineBaseKey = registrationType == RegistrationType.OS64Bit
                                                 ? RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                                                                           RegistryView.Registry64)
                                                 : RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                                                                           RegistryView.Registry32))
            {
                //  Open the Preview Handlers.
                using (var previewHandlersKey = localMachineBaseKey
                    .OpenSubKey(PreviewHandlersKey,
                                RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.WriteKey))
                {
                    //  If we don't have the key, we've got a problem.
                    if (previewHandlersKey == null)
                        throw new InvalidOperationException("Cannot open the PreviewHandlers key.");

                    //  Write the server guid as a name, and the display name as the value.
                    //  The display name isn't needed, it's just helpful for debugging and checking the registry.
                    previewHandlersKey.SetValue(serverType.GUID.ToRegistryString(), displayName);
                }
            }

            //  Open the classes root.
            using (var classesBaseKey = registrationType == RegistrationType.OS64Bit
                                            ? RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64)
                                            : RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry32))
            {
                //  Our server guid.
                var serverGuid = serverType.GUID.ToRegistryString();

                //  Open the Class Key.
                using (var classKey = classesBaseKey
                    .OpenSubKey(string.Format(@"CLSID\{0}", serverGuid),
                                RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.WriteKey))
                {
                    //  If we don't have the key, we've got a problem.
                    if (classKey == null)
                        throw new InvalidOperationException("Cannot open the class key.");

                    //  For informational purposes, set the server as the default value for the CLSID.
                    classKey.SetValue(null, serverType.Name);

                    //  Set up the surrogate host app id.
                    switch (previewHandlerAttribute.SurrogateHostType)
                    {
                        case SurrogateHostType.DedicatedPrevhost:
                            //  Get the appid.
                            var appId = GetAppIdForServerClsid(serverType.GUID).ToRegistryString();
                            classKey.SetValue("AppID", appId);
                            CreatePrevhostApp(appId);
                            break;
                        case SurrogateHostType.Prevhost:
                            //  Standard prevhost App ID.
                            classKey.SetValue("AppID", "{6d2b5079-2f0b-48dd-ab7f-97cec514d30b}");
                            break;
                        case SurrogateHostType.Prevhost32On64:
                            //  Specialised prevhost for x86 handler on x64.
                            classKey.SetValue("AppID", "{534A1E02-D58F-44f0-B58B-36CBED287C7C}");
                            break;
                        default:
                            throw new ServerRegistrationException(
                                string.Format("{0} is not a valid value for the surrogate host type.",
                                              previewHandlerAttribute.SurrogateHostType));
                    }

                    //  Set the display name and TODO icon.
                    classKey.SetValue("DisplayName", displayName, RegistryValueKind.String);
                    classKey.SetValue("Icon", "%SystemRoot%\\system32\\fontext.dll,10", RegistryValueKind.ExpandString);

                    //  Disable low integrity process isolation if specified.
                    if (previewHandlerAttribute.DisableLowILProcessIsolation)
                        classKey.SetValue("DisableLowILProcessIsolation", 1, RegistryValueKind.DWord);
                }
            }
        }

        /// <summary>
        /// Unregisters the SharpShell Preview Handler with the given type.
        /// </summary>
        /// <param name="serverType">Type of the server.</param>
        /// <param name="registrationType">Type of the registration.</param>
        public static void Unregister(Type serverType, RegistrationType registrationType)
        {
            //  Open the local machine.
            using (var localMachineBaseKey = registrationType == RegistrationType.OS64Bit
                ? RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64) :
                  RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            {
                //  Open the Preview Handlers.
                using (var previewHandlersKey = localMachineBaseKey
                    .OpenSubKey(PreviewHandlersKey,
                    RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.WriteKey | RegistryRights.ReadKey))
                {
                    //  If we don't have the key, we've got a problem.
                    if (previewHandlersKey == null)
                        throw new InvalidOperationException("Cannot open the PreviewHandlers key.");

                    //  If there's a value for the server, delete it.
                    var serverGuid = serverType.GUID.ToRegistryString();
                    if (previewHandlersKey.GetValueNames().Any(vm => vm == serverGuid))
                        previewHandlersKey.DeleteValue(serverGuid);

                    //  If the server has an AppID registered for it's surrogate host, clean it up.
                    var appId = GetAppIdForServerClsid(serverType.GUID).ToRegistryString();
                    DeletePrevhostApp(appId);
                }
            }
        }

        /// <summary>
        /// Registers a new AppID pointing to prevhost.exe.
        /// </summary>
        /// <param name="appId">The application identifier.</param>
        private static void CreatePrevhostApp(string appId)
        {
            //  Get the registry service.
            var registry = ServiceRegistry.ServiceRegistry.GetService<IRegistry>();

            using (var classesRoot = registry.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Default))
            using (var appIdsKey = classesRoot.OpenSubKey("AppID", true))
            {
                if (appIdsKey == null)
                    throw new InvalidOperationException("An exception occured trying to open the App IDs.");

                using (var appIdKey = appIdsKey.CreateSubKey(appId))
                {
                    if (appIdKey == null)
                        throw new InvalidOperationException("An exception occured trying to create an App ID.");
                    appIdKey.SetValue("DllSurrogate", @"%SystemRoot%\system32\prevhost.exe", RegistryValueKind.ExpandString);
                }
            }
        }

        /// <summary>
        /// Deletes a prevhost application registered for <paramref name="appId"/>, if it exixsts.
        /// </summary>
        /// <param name="appId">The application identifier.</param>
        private static void DeletePrevhostApp(string appId)
        {
            //  Get the registry service.
            var registry = ServiceRegistry.ServiceRegistry.GetService<IRegistry>();

            using (var classesRoot = registry.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Default))
            using (var appIdsKey = classesRoot.OpenSubKey("AppID", true))
            {
                if (appIdsKey == null)
                    throw new InvalidOperationException("An exception occured trying to open the App IDs.");

                appIdsKey.DeleteSubKey(appId, false);
            }
        }

        /// <summary>
        /// Gets the application identifier for server CLSID.
        /// This is used when creating a new AppID registration for 
        /// a dedicate preview handler host.
        /// </summary>
        /// <param name="serverClsid">The server CLSID.</param>
        /// <returns>An AppID for the CLSID.</returns>
        private static Guid GetAppIdForServerClsid(Guid serverClsid)
        {
            //  Just add one to the guid.
            var bytes = serverClsid.ToByteArray();
            bytes[15] = unchecked((byte)(bytes[15] + 1));
            return new Guid(bytes);
        }
    }
}
