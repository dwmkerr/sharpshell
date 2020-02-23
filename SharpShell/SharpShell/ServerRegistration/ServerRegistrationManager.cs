using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using Microsoft.Win32;
using SharpShell.Attributes;
using SharpShell.Diagnostics;
using SharpShell.Extensions;
using SharpShell.Interop;
using SharpShell.Registry;

namespace SharpShell.ServerRegistration
{
    /// <summary>
    ///     THe Server Registration Manager is an object that can be used to
    ///     help with Server Registration tasks, such as registering, un-registering
    ///     and checking servers. It will work with SharpShell Server objects or
    ///     other servers.
    /// </summary>
    public static class ServerRegistrationManager
    {
        /// <summary>
        ///     Opens the classes root.
        /// </summary>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <returns>The classes root key.</returns>
        private static IRegistryKey OpenClassesRootKey(RegistrationScope registrationScope)
        {
            //  Get the registry.
            var registry = ServiceRegistry.ServiceRegistry.GetService<IRegistry>();

            //  Get the classes base key.
            var classesBaseKey = registrationScope == RegistrationScope.OS64Bit
                ? registry.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64)
                : registry.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry32);

            //  Return the classes key.
            return classesBaseKey;
        }

        #region Installation

        /// <summary>
        ///     The classes key name.
        /// </summary>
        private const string RegistryClassesKeyName = @"CLSID";

        /// <summary>
        ///     The InProc32 key name.
        /// </summary>
        private const string RegistryInProcess32KeyName = @"InprocServer32";

        /// <summary>
        ///     The value for the net framework servers.
        /// </summary>
        private const string RegistryNetFrameworkServerKeyValue = @"mscoree.dll";

        /// <summary>
        ///     The threading model key name.
        /// </summary>
        private const string RegistryThreadingModelValueName = @"ThreadingModel";

        /// <summary>
        ///     THe assembly key name.
        /// </summary>
        private const string RegistryAssemblyValueName = @"Assembly";

        /// <summary>
        ///     The class key name.
        /// </summary>
        private const string RegistryClassValueName = @"Class";

        /// <summary>
        ///     The runtime version key name.
        /// </summary>
        private const string RegistryRuntimeVersionValueName = @"RuntimeVersion";

        /// <summary>
        ///     The codebase key name.
        /// </summary>
        private const string RegistryCodeBaseValueName = @"CodeBase";

        /// <summary>
        ///     Gets the server registration info.
        /// </summary>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <returns>
        ///     The ServerRegistrationInfo if the server is registered, otherwise false.
        /// </returns>
        public static ServerInstallationInfo GetExtensionInstallationInfo<T>(RegistrationScope registrationScope)
            where T : SharpShellServer
        {
            //  Call the main function.
            return GetExtensionInstallationInfo(SharpShellServerInfo.GetServerClassId<T>(), registrationScope);
        }

        /// <summary>
        ///     Gets the server registration info.
        /// </summary>
        /// <param name="serverInfo">The managed server information.</param>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <returns>
        ///     The ServerRegistrationInfo if the server is registered, otherwise false.
        /// </returns>
        public static ServerInstallationInfo GetExtensionInstallationInfo(
            SharpShellServerInfo serverInfo,
            RegistrationScope registrationScope)
        {
            //  Call the main function.
            return GetExtensionInstallationInfo(serverInfo.ClassId, registrationScope);
        }

        /// <summary>
        ///     Gets the server registration info.
        /// </summary>
        /// <param name="serverClassId">The server class id.</param>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <returns>
        ///     The ServerRegistrationInfo if the server is registered, otherwise false.
        /// </returns>
        public static ServerInstallationInfo GetExtensionInstallationInfo(
            Guid serverClassId,
            RegistrationScope registrationScope)
        {
            //  Open the classes.
            using (var classesKey = OpenInstalledClassesRoot(registrationScope, RegistryKeyPermissionCheck.ReadSubTree))
            {
                //  Do we have a sub-key for the server?
                using (var serverClassKey = classesKey.OpenSubKey(serverClassId.ToRegistryString()))
                {
                    //  If there's no sub-key, the server isn't registered.
                    if (serverClassKey == null)
                    {
                        return null;
                    }

                    //  Do we have an InProc32 server?
                    using (var inProcess32ServerKey = serverClassKey.OpenSubKey(RegistryInProcess32KeyName))
                    {
                        //  If we do, we can return the server info for an inProcess32 server.
                        if (inProcess32ServerKey != null)
                        {
                            //  Get the default value.
                            var inProcessLibraryName = inProcess32ServerKey.GetValue(null).ToString();

                            //  If we default value is null or empty, we've got a partially registered server.
                            if (string.IsNullOrEmpty(inProcessLibraryName))
                            {
                                return new ServerInstallationInfo(serverClassId);
                            }

                            //  Get the threading model.
                            var threadingModel = inProcess32ServerKey.GetValue(RegistryThreadingModelValueName)
                                .ToString();

                            //  Is it a .NET server?
                            if (inProcessLibraryName == RegistryNetFrameworkServerKeyValue)
                            {
                                //  We've got a .NET server. We should have one sub-key, with the assembly version.
                                var assemblyVersionSubKeyPath = inProcess32ServerKey.GetSubKeyNames().FirstOrDefault();

                                //  If we have no sub-key name, we've got a partially registered server.
                                if (assemblyVersionSubKeyPath == null)
                                {
                                    return new ServerInstallationInfo(serverClassId);
                                }

                                //  Open the assembly sub-key.
                                using (var assemblyVersionSubKey =
                                    inProcess32ServerKey.OpenSubKey(assemblyVersionSubKeyPath))
                                {
                                    //  If we can't open the key, we've got a problem.
                                    if (assemblyVersionSubKey == null)
                                    {
                                        throw new InvalidOperationException("Can't open the details of the server.");
                                    }

                                    //  Read the managed server details.
                                    var assemblyName = assemblyVersionSubKey.GetValue(RegistryAssemblyValueName)
                                        .ToString();
                                    var runtimeVersion = assemblyVersionSubKey.GetValue(RegistryRuntimeVersionValueName)
                                        .ToString();
                                    var codeBase = assemblyVersionSubKey.GetValue(RegistryCodeBaseValueName).ToString();

                                    var assembly = new ManagedAssemblyInfo(
                                        assemblyName,
                                        assemblyVersionSubKeyPath,
                                        runtimeVersion,
                                        codeBase
                                    );

                                    var managedClass = assemblyVersionSubKey.GetValue(RegistryClassValueName)
                                        .ToString();

                                    //  Return the managed server info.
                                    return new ServerInstallationInfo(
                                        serverClassId,
                                        inProcessLibraryName,
                                        threadingModel,
                                        assembly,
                                        managedClass
                                    );
                                }
                            }

                            //  We've got a native COM server.
                            return new ServerInstallationInfo(
                                serverClassId,
                                inProcessLibraryName,
                                threadingModel
                            );
                        }
                    }

                    //  If by this point we haven't return server info, we've got a partially registered server.
                    return new ServerInstallationInfo(serverClassId);
                }
            }
        }

        /// <summary>
        ///     Installs a SharpShell COM server.
        /// </summary>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <param name="codeBase">if set to <c>true</c> use code base registration (i.e full assembly path, not the GAC).</param>
        public static void InstallServer<T>(RegistrationScope registrationScope, bool codeBase)
            where T : SharpShellServer
        {
            InstallServer(SharpShellServerInfo.FromServer<T>(), registrationScope, codeBase);
        }

        /// <summary>
        ///     Installs a SharpShell COM server.
        /// </summary>
        /// <param name="serverInfo">The managed server info.</param>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <param name="codeBase">if set to <c>true</c> use code base registration (i.e full assembly path, not the GAC).</param>
        public static void InstallServer(
            SharpShellServerInfo serverInfo,
            RegistrationScope registrationScope,
            bool codeBase)
        {
            //  Get the server installation information.
            var serverInstallationInfo = GetExtensionInstallationInfo(serverInfo.ClassId, registrationScope);

            //  If it is installed, uninstall first.
            if (serverInstallationInfo != null)
            {
                UninstallServer(serverInfo.ClassId, registrationScope);
            }

            //  Open the classes.
            using (var classesKey =
                OpenInstalledClassesRoot(registrationScope, RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                //  Create the server key.
                using (var serverKey = classesKey.CreateSubKey(serverInfo.ClassId.ToRegistryString()))
                {
                    if (serverKey == null)
                    {
                        throw new InvalidOperationException("Cannot create server key.");
                    }

                    //  We always set the server key default value to the display name if we can.
                    if (serverInfo.IsDisplayNameDefined)
                    {
                        serverKey.SetValue(null, serverInfo.DisplayName, RegistryValueKind.String);
                    }

                    //  Create the process key.
                    using (var inProcess32Key = serverKey.CreateSubKey(RegistryInProcess32KeyName))
                    {
                        //  Check the key.
                        if (inProcess32Key == null)
                        {
                            throw new InvalidOperationException("Cannot create InProc32 key.");
                        }

                        //  Set the .NET value.
                        inProcess32Key.SetValue(null, RegistryNetFrameworkServerKeyValue);

                        //  Install all details at server level.
                        inProcess32Key.SetValue(RegistryClassValueName, serverInfo.ClassFullName);
                        inProcess32Key.SetValue(RegistryAssemblyValueName, serverInfo.AssemblyInfo.FullName);
                        inProcess32Key.SetValue(RegistryRuntimeVersionValueName,
                            serverInfo.AssemblyInfo.RuntimeVersion);
                        inProcess32Key.SetValue(RegistryThreadingModelValueName, "Both");

                        if (codeBase)
                        {
                            inProcess32Key.SetValue(RegistryCodeBaseValueName, serverInfo.AssemblyInfo.CodeBase);
                        }

                        //  Create the version key.
                        using (var versionKey = inProcess32Key.CreateSubKey(serverInfo.AssemblyInfo.Version))
                        {
                            //  Check the key.
                            if (versionKey == null)
                            {
                                throw new InvalidOperationException("Cannot create assembly version key.");
                            }

                            //  Set the values.
                            versionKey.SetValue(RegistryClassValueName, serverInfo.ClassFullName);
                            versionKey.SetValue(RegistryAssemblyValueName, serverInfo.AssemblyInfo.FullName);
                            versionKey.SetValue(RegistryRuntimeVersionValueName,
                                serverInfo.AssemblyInfo.RuntimeVersion);

                            if (codeBase)
                            {
                                versionKey.SetValue(RegistryCodeBaseValueName, serverInfo.AssemblyInfo.CodeBase);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Uninstalls the server.
        /// </summary>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <returns>True if the server WAS installed and has been uninstalled, false if the server was not found.</returns>
        public static bool UninstallServer<T>(RegistrationScope registrationScope) where T : SharpShellServer
        {
            return UninstallServer(SharpShellServerInfo.FromServer<T>(), registrationScope);
        }

        /// <summary>
        ///     Uninstalls the server.
        /// </summary>
        /// <param name="serverInfo">The managed server info.</param>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <returns>True if the server WAS installed and has been uninstalled, false if the server was not found.</returns>
        public static bool UninstallServer(SharpShellServerInfo serverInfo, RegistrationScope registrationScope)
        {
            // Check passed type
            return UninstallServer(serverInfo.ClassId, registrationScope);
        }

        /// <summary>
        ///     Uninstalls the server.
        /// </summary>
        /// <param name="serverClassId">The server's class id.</param>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <returns>True if the server WAS installed and has been uninstalled, false if the server was not found.</returns>
        public static bool UninstallServer(Guid serverClassId, RegistrationScope registrationScope)
        {
            //  Open classes.
            using (var classesKey =
                OpenInstalledClassesRoot(registrationScope, RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                var serverSubKeyPath = serverClassId.ToRegistryString();

                //  If the sub-key doesn't exist, we can return false - we're already uninstalled.
                using (var serverSubKey = classesKey.OpenSubKey(serverSubKeyPath, false))
                {
                    if (serverSubKey == null)
                    {
                        return false;
                    }
                }

                //  Delete the sub-key tree.
                classesKey.DeleteSubKeyTree(serverSubKeyPath);

                return true;
            }
        }

        /// <summary>
        ///     Opens the classes key.
        /// </summary>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <param name="permissions">The permissions.</param>
        /// <returns></returns>
        private static IRegistryKey OpenInstalledClassesRoot(
            RegistrationScope registrationScope,
            RegistryKeyPermissionCheck permissions)
        {
            //  Open classes.
            return OpenClassesRootKey(registrationScope)?
                       .OpenSubKey(
                           RegistryClassesKeyName,
                           permissions,
                           RegistryRights.QueryValues | RegistryRights.ReadPermissions | RegistryRights.EnumerateSubKeys
                       ) ??
                   throw new InvalidOperationException("Cannot open classes.");
        }

        #endregion

        #region Registration

        /// <summary>
        ///     The default icon key name.
        /// </summary>
        private const string RegistryDefaultIconKeyName = @"DefaultIcon";

        /// <summary>
        ///     The default icon backup value name.
        /// </summary>
        private const string RegistryDefaultIconBackupValueName = @"SharpShell_Backup_DefaultIcon";

        /// <summary>
        ///     Registers a SharpShell server. This will create the associations defined by the
        ///     server's COMServerAssociation attribute.
        /// </summary>
        /// <param name="registrationScope">Type of the registration.</param>
        public static void RegisterAndApproveServer<T>(RegistrationScope registrationScope) where T : SharpShellServer
        {
            RegisterAndApproveServer(SharpShellServerInfo.FromServer<T>(), registrationScope);
        }

        /// <summary>
        ///     Registers a SharpShell server. This will create the associations defined by the
        ///     server's COMServerAssociation attribute.
        /// </summary>
        /// <param name="serverInfo">The managed server info.</param>
        /// <param name="registrationScope">Type of the registration.</param>
        public static void RegisterAndApproveServer(
            SharpShellServerInfo serverInfo,
            RegistrationScope registrationScope)
        {
            //  Pass the server type to the SharpShellServer internal registration function and let it 
            //  take over from there.
            InternalRegisterServer(serverInfo, registrationScope);

            //  Approve the extension.
            ApproveExtension(serverInfo, registrationScope);
        }

        /// <summary>
        ///     Actually performs registration. The ComRegisterFunction decorated method will call this function
        ///     internally with the flag appropriate for the operating system processor architecture.
        ///     However, this function can also be called manually if needed.
        /// </summary>
        /// <param name="serverInfo">The managed server info.</param>
        /// <param name="registrationScope">Type of the registration.</param>
        private static void InternalRegisterServer(SharpShellServerInfo serverInfo, RegistrationScope registrationScope)
        {
            Logging.Log(
                $"Preparing to register SharpShell Server {serverInfo.DisplayName} in scope {registrationScope}.");


            //  Register the server associations
            if (serverInfo.ShellExtensionType != ShellExtensionType.None)
            {
                //  Get the association data.
                var associationClassNames =
                    registrationScope == RegistrationScope.OS64Bit
                        ? serverInfo.AssociationClassNamesX64
                        : serverInfo.AssociationClassNamesX32;

                if (associationClassNames?.Any() == true)
                {
                    foreach (var associationClassName in associationClassNames)
                    {
                        RegisterShellExtensionAssociation(
                            serverInfo.ClassId,
                            serverInfo.ShellExtensionType,
                            serverInfo.RegistrationName,
                            associationClassName,
                            registrationScope
                        );
                    }
                }
            }

            //  Execute the custom register function, if there is one.
            try
            {
                serverInfo.InvokeCustomRegisterMethodIfExists(registrationScope);
            }
            catch (Exception e)
            {
                Logging.Error("Custom register function failed.", e);
            }

            //  Notify the shell we've updated associations.
            Shell32.SHChangeNotify(Shell32.SHCNE_ASSOCCHANGED, 0, IntPtr.Zero, IntPtr.Zero);
            Logging.Log($"Registration of {serverInfo.DisplayName} completed.");
        }

        /// <summary>
        ///     Un-registers a SharpShell server. This will remove the associations defined by the
        ///     server's COMServerAssociation attribute.
        /// </summary>
        /// <param name="registrationScope">Type of the registration to undo.</param>
        public static void UnregisterAndUnApproveServer<T>(RegistrationScope registrationScope)
            where T : SharpShellServer
        {
            UnregisterAndUnApproveServer(SharpShellServerInfo.FromServer<T>(), registrationScope);
        }

        /// <summary>
        ///     Un-registers a SharpShell server. This will remove the associations defined by the
        ///     server's COMServerAssociation attribute.
        /// </summary>
        /// <param name="serverInfo">The server type.</param>
        /// <param name="registrationScope">Type of the registration to undo.</param>
        public static void UnregisterAndUnApproveServer(
            SharpShellServerInfo serverInfo,
            RegistrationScope registrationScope)
        {
            //  Un-approve the extension.
            UnApproveExtension(serverInfo.ClassId, registrationScope);

            //  Pass the server type to the SharpShellServer internal un-registration function and let it 
            //  take over from there.
            InternalUnregisterServer(serverInfo, registrationScope);
        }

        /// <summary>
        ///     Un-registers a SharpShell server. This will remove the associations defined by the
        ///     server's COMServerAssociation attribute.
        /// </summary>
        /// <param name="serverClassId">The server class id.</param>
        /// <param name="registrationScope">Type of the registration to undo.</param>
        public static void UnregisterAndUnApproveServer(Guid serverClassId, RegistrationScope registrationScope)
        {
            //  Un-approve the extension.
            UnApproveExtension(serverClassId, registrationScope);

            var associations = GetExtensionRegistrationInfo(serverClassId, registrationScope);

            if (associations == null)
            {
                return;
            }

            foreach (var association in associations.Associations)
            {
                UnregisterShellExtensionAssociation(
                    association.ShellExtensionType,
                    association.RegistrationName,
                    association.AssociationClassName,
                    registrationScope
                );
            }
        }

        /// <summary>
        ///     Actually performs un-registration. The ComUnregisterFunction decorated method will call this function
        ///     internally with the flag appropriate for the operating system processor architecture.
        ///     However, this function can also be called manually if needed.
        /// </summary>
        /// <param name="serverInfo">The server type.</param>
        /// <param name="registrationScope">Type of the registration to unregister.</param>
        private static void InternalUnregisterServer(
            SharpShellServerInfo serverInfo,
            RegistrationScope registrationScope)
        {
            Logging.Log(
                $"Preparing to unregister SharpShell Server {serverInfo.DisplayName} in scope {registrationScope}");

            //  Unregister the server associations
            if (serverInfo.ShellExtensionType != ShellExtensionType.None)
            {
                //  Get the association data.
                var associationClassNames =
                    registrationScope == RegistrationScope.OS64Bit
                        ? serverInfo.AssociationClassNamesX64
                        : serverInfo.AssociationClassNamesX32;

                if (associationClassNames?.Any() == true)
                {
                    foreach (var associationClassName in associationClassNames)
                    {
                        UnregisterShellExtensionAssociation(
                            serverInfo.ShellExtensionType,
                            serverInfo.RegistrationName,
                            associationClassName,
                            registrationScope
                        );
                    }
                }
            }

            //  Execute the custom unregister function, if there is one.
            try
            {
                serverInfo.InvokeCustomUnRegisterMethodIfExists(registrationScope);
            }
            catch (Exception e)
            {
                Logging.Error("Custom unregister function failed.", e);
            }

            //  Notify the shell we've updated associations.
            Shell32.SHChangeNotify(Shell32.SHCNE_ASSOCCHANGED, 0, IntPtr.Zero, IntPtr.Zero);
            Logging.Log($"Un-registration of {serverInfo.DisplayName} completed.");
        }

        /// <summary>
        ///     Enumerates shell extensions registered for a class.
        /// </summary>
        /// <param name="associationClassName">The class name to return registered shell extensions</param>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <returns></returns>
        public static IEnumerable<ShellExtensionRegisteredAssociationInfo> EnumerateRegisteredAssociations(
            string associationClassName,
            RegistrationScope registrationScope)
        {
            //  Go through every shell extension type.

            return EnumerateRegisteredAssociations(
                associationClassName,
                registrationScope,
                Enum.GetValues(typeof(ShellExtensionType)).OfType<ShellExtensionType>()
            );
        }

        /// <summary>
        ///     Enumerates shell extensions registered for a class.
        /// </summary>
        /// <param name="associationClassName">The class name to return registered shell extensions</param>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <param name="extensionTypes">Shell extension types to filter with.</param>
        /// <returns></returns>
        public static IEnumerable<ShellExtensionRegisteredAssociationInfo> EnumerateRegisteredAssociations(
            string associationClassName,
            RegistrationScope registrationScope,
            IEnumerable<ShellExtensionType> extensionTypes)
        {
            using (var classesKey = OpenClassesRootKey(registrationScope))
            {
                foreach (var extensionType in extensionTypes)
                {
                    // Get handler attribute
                    var handlerAttribute = HandlerSubKeyAttribute.GetHandlerSubKeyAttribute(extensionType);

                    // Ignore invalid or null handlers
                    if (string.IsNullOrEmpty(handlerAttribute?.HandlerSubKey))
                    {
                        continue;
                    }

                    //  Get the handler sub-key.
                    var handlerSubKeyRoot = $"{associationClassName}\\ShellEx\\{handlerAttribute.HandlerSubKey}";

                    using (var handlerSubKey = classesKey.OpenSubKey(
                        handlerSubKeyRoot,
                        RegistryKeyPermissionCheck.ReadSubTree,
                        RegistryRights.ReadKey | RegistryRights.QueryValues
                    ))
                    {
                        //  Skip empty handlers.
                        if (handlerSubKey == null)
                        {
                            yield break;
                        }

                        if (handlerAttribute.AllowMultipleEntries) // We should check for multiple sub-keys
                        {
                            //  Read sub-keys.
                            foreach (var entrySubKeyName in handlerSubKey.GetSubKeyNames())
                            {
                                using (var entrySubKey = handlerSubKey.OpenSubKey(
                                    entrySubKeyName,
                                    RegistryKeyPermissionCheck.ReadSubTree,
                                    RegistryRights.QueryValues | RegistryRights.ReadKey
                                ))
                                {
                                    var serverClassIdValue = entrySubKey.GetValue(null, string.Empty).ToString();

                                    if (Guid.TryParse(serverClassIdValue, out var serverClassId) == false)
                                    {
                                        continue;
                                    }

                                    Trace.WriteLine(string.Format(
                                        "Class {0} has {1} with id {2} ({3})",
                                        associationClassName,
                                        extensionType.ToString(),
                                        serverClassId,
                                        entrySubKeyName
                                    ));

                                    yield return new ShellExtensionRegisteredAssociationInfo(
                                        extensionType, serverClassId, associationClassName, entrySubKeyName
                                    );
                                }
                            }
                        }
                        else // We should check the root handler sub-key
                        {
                            var serverClassIdValue = handlerSubKey.GetValue(null, string.Empty).ToString();

                            if (Guid.TryParse(serverClassIdValue, out var serverClassId) == false)
                            {
                                yield break;
                            }

                            Trace.WriteLine(string.Format(
                                "Class {0} has {1} with id {2}",
                                associationClassName,
                                extensionType.ToString(),
                                serverClassId
                            ));

                            yield return new ShellExtensionRegisteredAssociationInfo(
                                extensionType, serverClassId, associationClassName
                            );
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Enumerates Shell extensions.
        /// </summary>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <returns></returns>
        public static IEnumerable<ShellExtensionRegistrationInfo> EnumerateRegisteredExtensions(
            RegistrationScope registrationScope)
        {
            //  Go through all classes.
            using (var classesKey = OpenClassesRootKey(registrationScope))
            {
                return classesKey.GetSubKeyNames()
                    .Where(cn => !cn.StartsWith("{"))
                    .SelectMany(className => EnumerateRegisteredAssociations(className, registrationScope))
                    .GroupBy(info => info.ServerClassId)
                    .Select(group =>
                    {
                        var isApproved = IsExtensionApproved(group.Key, registrationScope);

                        return new ShellExtensionRegistrationInfo(group.Key, isApproved, group);
                    });
            }
        }

        /// <summary>
        ///     Get shell extension registration information.
        /// </summary>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <returns></returns>
        public static ShellExtensionRegistrationInfo GetExtensionRegistrationInfo<T>(
            RegistrationScope registrationScope) where T : SharpShellServer
        {
            return GetExtensionRegistrationInfo(SharpShellServerInfo.FromServer<T>(), registrationScope);
        }

        /// <summary>
        ///     Get shell extension registration information.
        /// </summary>
        /// <param name="serverInfo">The managed server info</param>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <returns></returns>
        public static ShellExtensionRegistrationInfo GetExtensionRegistrationInfo(
            SharpShellServerInfo serverInfo,
            RegistrationScope registrationScope)
        {
            IEnumerable<ShellExtensionRegisteredAssociationInfo> associations =
                new ShellExtensionRegisteredAssociationInfo[0];

            if (serverInfo.ShellExtensionType != ShellExtensionType.None)
            {
                //  Get the association data.
                var associationClassNames =
                    registrationScope == RegistrationScope.OS64Bit
                        ? serverInfo.AssociationClassNamesX64
                        : serverInfo.AssociationClassNamesX32;

                if (associationClassNames.Any())
                {
                    associations = associationClassNames.SelectMany(className =>
                        EnumerateRegisteredAssociations(className, registrationScope,
                                new[] {serverInfo.ShellExtensionType})
                            .Where(info => info.ServerClassId == serverInfo.ClassId)
                    );
                }
            }

            var isApproved = IsExtensionApproved(serverInfo.ClassId, registrationScope);

            return new ShellExtensionRegistrationInfo(serverInfo.ClassId, isApproved, associations);
        }

        /// <summary>
        ///     Get shell extension registration information.
        /// </summary>
        /// <param name="serverClassId">The server class id</param>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <returns></returns>
        public static ShellExtensionRegistrationInfo GetExtensionRegistrationInfo(
            Guid serverClassId,
            RegistrationScope registrationScope)
        {
            //  Go through all classes.
            using (var classesKey = OpenClassesRootKey(registrationScope))
            {
                var associations = classesKey.GetSubKeyNames()
                    .Where(cn => !cn.StartsWith("{"))
                    .SelectMany(className => EnumerateRegisteredAssociations(className, registrationScope))
                    .Where(info => info.ServerClassId == serverClassId);

                var isApproved = IsExtensionApproved(serverClassId, registrationScope);

                return new ShellExtensionRegistrationInfo(serverClassId, isApproved, associations);
            }
        }

        /// <summary>
        ///     Registers the server associations.
        /// </summary>
        /// <param name="serverClassId">The server's class id.</param>
        /// <param name="extensionType">Type of the server.</param>
        /// <param name="registrationName">Name of the server.</param>
        /// <param name="associationClassName">The association class name.</param>
        /// <param name="registrationScope">Type of the registration.</param>
        internal static void RegisterShellExtensionAssociation(
            Guid serverClassId,
            ShellExtensionType extensionType,
            string registrationName,
            string associationClassName,
            RegistrationScope registrationScope)
        {
            using (var classesKey = OpenClassesRootKey(registrationScope))
            {
                var attribute = HandlerSubKeyAttribute.GetHandlerSubKeyAttribute(extensionType);

                if (attribute == null)
                {
                    throw new ArgumentException("This server type does not have a handler sub key.",
                        nameof(extensionType));
                }

                var subKeyName = attribute.HandlerSubKey;

                if (string.IsNullOrEmpty(subKeyName))
                {
                    return;
                }

                var associationKeyPath = $"{associationClassName}\\ShellEx\\{subKeyName}";

                if (attribute.AllowMultipleEntries)
                {
                    associationKeyPath += $"\\{registrationName}";
                }

                //  Create the server key.
                using (var serverKey = classesKey.CreateSubKey(associationKeyPath))
                {
                    //  Set the server class id.
                    serverKey?.SetValue(null, serverClassId.ToRegistryString());
                }

                //  If we're a shell icon handler, we must also set the default icon.
                if (extensionType == ShellExtensionType.ShellIconHandler)
                {
                    SetIconHandlerDefaultIcon(registrationScope, associationClassName);
                }
            }
        }

        /// <summary>
        ///     Sets the icon handler default icon, enabling an icon handler extension.
        /// </summary>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <param name="associationClassName">Association class name.</param>
        private static void SetIconHandlerDefaultIcon(RegistrationScope registrationScope, string associationClassName)
        {
            using (var classesKey = OpenClassesRootKey(registrationScope))
            {
                //  Open the class.
                using (var classKey = classesKey.OpenSubKey(associationClassName))
                {
                    //  Check we have the class.
                    if (classKey == null)
                    {
                        throw new InvalidOperationException("Cannot open class " + associationClassName);
                    }

                    //  Open the default icon.
                    using (var defaultIconKey = classKey.OpenSubKey(
                        RegistryDefaultIconKeyName,
                        RegistryKeyPermissionCheck.ReadWriteSubTree,
                        RegistryRights.ReadKey | RegistryRights.WriteKey
                    ))
                    {
                        //  Check we have the key.
                        if (defaultIconKey == null)
                        {
                            // if not, we create the key.
                            var tempDefaultIconKey = classesKey.CreateSubKey(
                                associationClassName + @"\" + RegistryDefaultIconKeyName,
                                RegistryKeyPermissionCheck.ReadWriteSubTree
                            );
                            tempDefaultIconKey.SetValue(null, "%1");
                        }
                        else
                        {
                            //  Get the default icon.
                            var defaultIcon = defaultIconKey.GetValue(null, string.Empty).ToString();

                            //  Save the default icon.
                            defaultIconKey.SetValue(RegistryDefaultIconBackupValueName, defaultIcon);
                            defaultIconKey.SetValue(null, "%1");
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Un-sets the icon handler default icon sharp shell value, restoring the backed up value.
        /// </summary>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <param name="associationClassName">Name of the class.</param>
        private static void UnsetIconHandlerDefaultIcon(
            RegistrationScope registrationScope,
            string associationClassName)
        {
            using (var classesKey = OpenClassesRootKey(registrationScope))
            {
                //  Open the class.
                using (var classKey = classesKey.OpenSubKey(associationClassName))
                {
                    //  Check we have the class.
                    if (classKey == null)
                    {
                        throw new InvalidOperationException("Cannot open class " + associationClassName);
                    }

                    //  Open the default icon.
                    using (var defaultIconKey = classKey.OpenSubKey(
                        RegistryDefaultIconKeyName,
                        RegistryKeyPermissionCheck.ReadWriteSubTree,
                        RegistryRights.ReadKey | RegistryRights.WriteKey
                    ))
                    {
                        //  Check we have the key.
                        if (defaultIconKey == null)
                        {
                            throw new InvalidOperationException(
                                "Cannot open default icon key for class " + associationClassName);
                        }

                        //  Get the backup default icon.
                        var backupDefaultIcon = defaultIconKey.GetValue(RegistryDefaultIconBackupValueName)?.ToString();

                        if (backupDefaultIcon != null)
                        {
                            //  Save the default icon, delete the backup.
                            defaultIconKey.SetValue(null, backupDefaultIcon);
                            defaultIconKey.DeleteValue(RegistryDefaultIconBackupValueName);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Un-registers the server associations.
        /// </summary>
        /// <param name="extensionType">Type of the shell extension type.</param>
        /// <param name="registrationName">Name of the server.</param>
        /// <param name="associationClassName">The association class name.</param>
        /// <param name="registrationScope">Type of the registration.</param>
        private static void UnregisterShellExtensionAssociation(
            ShellExtensionType extensionType,
            string registrationName,
            string associationClassName,
            RegistrationScope registrationScope)
        {
            //  Open the classes key...
            using (var classesKey = OpenClassesRootKey(registrationScope))
            {
                var attribute = HandlerSubKeyAttribute.GetHandlerSubKeyAttribute(extensionType);

                if (attribute == null)
                {
                    throw new ArgumentException("This server type does not have a handler sub key.",
                        nameof(extensionType));
                }

                var subKeyName = attribute.HandlerSubKey;

                if (string.IsNullOrEmpty(subKeyName))
                {
                    return;
                }

                //  Get the key for the association.
                var associationKeyPath = $"{associationClassName}\\ShellEx\\{subKeyName}";

                if (attribute.AllowMultipleEntries)
                {
                    associationKeyPath += $"\\{registrationName}";
                }


                //  Delete it if it exists.
                classesKey.DeleteSubKeyTree(associationKeyPath, false);

                //  If we're a shell icon handler, we must also unset the default icon.
                if (extensionType == ShellExtensionType.ShellIconHandler)
                {
                    UnsetIconHandlerDefaultIcon(registrationScope, associationClassName);
                }
            }
        }

        /// <summary>
        ///     Creates the class names for associations.
        /// </summary>
        /// <param name="associationType">Type of the association.</param>
        /// <param name="associations">The associations.</param>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <returns>
        ///     The class names for the associations.
        /// </returns>
        internal static IEnumerable<string> GetAssociationClassNames(
            AssociationType associationType,
            IEnumerable<string> associations,
            RegistrationScope registrationScope)
        {
            //  Switch on the association type.
            switch (associationType)
            {
                //  We are handling the obsolete file extension type for backwards compatibility.
#pragma warning disable 618
                case AssociationType.FileExtension:
#pragma warning restore 618

                    //  We're dealing with file extensions only, so we can return them directly.
                    return associations;

                case AssociationType.ClassOfExtension:

                    //  Open the classes sub key and get or create each file extension classes.
                    using (var classesKey = OpenClassesRootKey(registrationScope))
                    {
                        return associations
                            .Select(extension => FileExtensionClass.Get(classesKey, extension, true))
                            .ToArray();
                    }

                case AssociationType.Class:

                    //  We're dealing with classes only, so we can return them directly.
                    return associations;

                default:

                    //  If this is a predefined shell object, return the class for it.
                    var className = PredefinedShellObjectAttribute.GetPredefinedShellObjectAttribute(associationType)
                        ?.ClassName;

                    if (className != null)
                    {
                        return new[] {className};
                    }

                    //  It's not a type we know how to deal with, so bail.
                    throw new InvalidOperationException(
                        $@"Unable to determine associations for AssociationType '{associationType}'");
            }
        }

        /// <summary>
        ///     Approves an extension.
        /// </summary>
        /// <param name="serverInfo">The server info.</param>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <exception cref="InvalidOperationException">Failed to open the Approved Extensions key.</exception>
        private static void ApproveExtension(SharpShellServerInfo serverInfo, RegistrationScope registrationScope)
        {
            //  Open the approved extensions key.
            using (var approvedKey = RegistryKey.OpenBaseKey(
                RegistryHive.LocalMachine,
                registrationScope == RegistrationScope.OS64Bit ? RegistryView.Registry64 : RegistryView.Registry32
            ).OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved",
                RegistryKeyPermissionCheck.ReadWriteSubTree
            ))
            {
                //  If we can't open the key, we're going to have problems.
                if (approvedKey == null)
                {
                    throw new InvalidOperationException("Failed to open the Approved Extensions key.");
                }

                //  Create an entry for the server.
                approvedKey.SetValue(serverInfo.ClassId.ToRegistryString(), serverInfo.DisplayName);
            }
        }

        /// <summary>
        ///     Determines whether an extension is approved.
        /// </summary>
        /// <param name="serverClassId">The server class id.</param>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <returns>
        ///     <c>true</c> if the extension is approved; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="InvalidOperationException">Failed to open the Approved Extensions key.</exception>
        private static bool IsExtensionApproved(Guid serverClassId, RegistrationScope registrationScope)
        {
            //  Open the approved extensions key.
            using (var approvedKey = RegistryKey.OpenBaseKey(
                RegistryHive.LocalMachine,
                registrationScope == RegistrationScope.OS64Bit ? RegistryView.Registry64 : RegistryView.Registry32
            ).OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved",
                RegistryKeyPermissionCheck.ReadSubTree
            ))
            {
                //  If we can't open the key, we're going to have problems.
                if (approvedKey == null)
                {
                    throw new InvalidOperationException("Failed to open the Approved Extensions key.");
                }

                return approvedKey.GetValueNames().Any(valueName =>
                    valueName.Equals(serverClassId.ToRegistryString(), StringComparison.OrdinalIgnoreCase)
                );
            }
        }

        /// <summary>
        ///     UnApproves an extension.
        /// </summary>
        /// <param name="serverClassId">The server's class id.</param>
        /// <param name="registrationScope">Type of the registration.</param>
        /// <exception cref="InvalidOperationException">Failed to open the Approved Extensions key.</exception>
        private static void UnApproveExtension(Guid serverClassId, RegistrationScope registrationScope)
        {
            //  Open the approved extensions key.
            using (var approvedKey = RegistryKey.OpenBaseKey(
                RegistryHive.LocalMachine,
                registrationScope == RegistrationScope.OS64Bit ? RegistryView.Registry64 : RegistryView.Registry32
            ).OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved",
                RegistryKeyPermissionCheck.ReadWriteSubTree
            ))
            {
                //  If we can't open the key, we're going to have problems.
                if (approvedKey == null)
                {
                    throw new InvalidOperationException("Failed to open the Approved Extensions key.");
                }

                //  Delete the value if it's there.
                approvedKey.DeleteValue(serverClassId.ToRegistryString(), false);
            }
        }

        #endregion
    }
}