using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using Microsoft.Win32;
using SharpShell.Attributes;
using SharpShell.Extensions;
using SharpShell.Registry;


namespace SharpShell.ServerRegistration
{
    /// <summary>
    /// THe Server Registration Manager is an object that can be used to
    /// help with Server Registration tasks, such as registering, unregistering
    /// and checking servers. It will work with SharpShell Server objects or
    /// other servers.
    /// </summary>
    public static class ServerRegistrationManager
    {
        /// <summary>
        /// Installs a SharpShell COM server.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="registrationType">Type of the registration.</param>
        /// <param name="codeBase">if set to <c>true</c> use code base registration (i.e full assembly path, not the GAC).</param>
        public static void InstallServer(ISharpShellServer server, RegistrationType registrationType, bool codeBase)
        {
            //  Get the server registration information.
            var serverRegistrationInformation = GetServerRegistrationInfo(server, registrationType);

            //  If it is registered, unregister first.
            if (serverRegistrationInformation != null)
                UninstallServer(server, registrationType);
            
            //  Open the classes.
            using (var classesKey = OpenClassesKey(registrationType, RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                //  Create the server key.
                using (var serverKey = classesKey.CreateSubKey(server.ServerClsid.ToRegistryString()))
                {
                    if(serverKey == null)
                        throw new InvalidOperationException("Cannot create server key.");

                    //  We always set the server key default value to the display name if we can.
                    if(!string.IsNullOrEmpty(server.DisplayName))
                        serverKey.SetValue(null, server.DisplayName, RegistryValueKind.String);

                    //  Create the inproc key.
                    using (var inproc32Key = serverKey.CreateSubKey(KeyName_InProc32))
                    {
                        //  Check the key.
                        if(inproc32Key == null)
                            throw new InvalidOperationException("Cannot create InProc32 key.");

                        //  Set the .NET value.
                        inproc32Key.SetValue(null, KeyValue_NetFrameworkServer);

                        //  Create the values.
                        var assemblyVersion = server.GetType().Assembly.GetName().Version.ToString();
                        var assemblyFullName = server.GetType().Assembly.FullName;
                        var className = server.GetType().FullName;
                        var runtimeVersion = server.GetType().Assembly.ImageRuntimeVersion;
                        var codeBaseValue = server.GetType().Assembly.CodeBase;
                        const string threadingModel = "Both";

                        //  Register all details at server level.
                        inproc32Key.SetValue(KeyName_Assembly, assemblyFullName);
                        inproc32Key.SetValue(KeyName_Class, className);
                        inproc32Key.SetValue(KeyName_RuntimeVersion, runtimeVersion);
                        inproc32Key.SetValue(KeyName_ThreadingModel, threadingModel);
                        if (codeBase)
                            inproc32Key.SetValue(KeyName_CodeBase, codeBaseValue);

                        //  Create the version key.
                        using (var versionKey = inproc32Key.CreateSubKey(assemblyVersion))
                        {
                            //  Check the key.
                            if(versionKey == null)
                                throw new InvalidOperationException("Cannot create assembly version key.");

                            //  Set the values.
                            versionKey.SetValue(KeyName_Assembly, assemblyFullName);
                            versionKey.SetValue(KeyName_Class, className);
                            versionKey.SetValue(KeyName_RuntimeVersion, runtimeVersion);
                            if (codeBase)
                                versionKey.SetValue(KeyName_CodeBase, codeBaseValue);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Uninstalls the server.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="registrationType">Type of the registration.</param>
        /// <returns>True if the server WAS installed and has been uninstalled, false if the server was not found.</returns>
        public static bool UninstallServer(ISharpShellServer server, RegistrationType registrationType)
        {
            //  Open classes.
            using (var classesKey = OpenClassesKey(registrationType, RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                var subKeyTreeName = server.ServerClsid.ToRegistryString();

                //  If the subkey doesn't exist, we can return false - we're already uninstalled.
                if (classesKey.GetSubKeyNames().Any(skn => skn.Equals(subKeyTreeName, StringComparison.OrdinalIgnoreCase)) == false)
                    return false;

                //  Delete the subkey tree.
                classesKey.DeleteSubKeyTree(subKeyTreeName);
                return true;
            }
        }

        /// <summary>
        /// Registers a SharpShell server. This will create the associations defined by the
        /// server's COMServerAssociation attribute.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="registrationType">Type of the registration.</param>
        public static void RegisterServer(ISharpShellServer server, RegistrationType registrationType)
        {
            //  Pass the server type to the SharpShellServer internal registration function and let it 
            //  take over from there.
            SharpShellServer.DoRegister(server.GetType(), registrationType);

            //  Approve the extension.
            ApproveExtension(server, registrationType);
        }

        /// <summary>
        /// Unregisters a SharpShell server. This will remove the associations defined by the
        /// server's COMServerAssociation attribute.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="registrationType">Type of the registration to undo.</param>
        public static void UnregisterServer(ISharpShellServer server, RegistrationType registrationType)
        {
            //  Unapprove the extension.
            UnapproveExtension(server, registrationType);

            //  Pass the server type to the SharpShellServer internal unregistration function and let it 
            //  take over from there.
            SharpShellServer.DoUnregister(server.GetType(), registrationType);
        }

        /// <summary>
        /// Enumerates Shell extensions.
        /// </summary>
        /// <param name="registrationType">Type of the registration.</param>
        /// <param name="shellExtensionTypes">The shell extension types.</param>
        /// <returns></returns>
        public static IEnumerable<ShellExtensionRegistrationInfo> EnumerateExtensions(RegistrationType registrationType, ShellExtensionType shellExtensionTypes)
        {
            var shellExtensionsGuidMap = new Dictionary<Guid, ShellExtensionRegistrationInfo>();

            //  Go through all classes.
            using (var classes = OpenClassesRoot(registrationType))
            {
                //  Read each subkey.
                foreach (var className in classes.GetSubKeyNames().Where(cn => !cn.StartsWith("{")))
                {
                    //  Go through every shell extension type.
                    foreach (ShellExtensionType shellExtensionType in Enum.GetValues(typeof (ShellExtensionType)))
                    {
                        //  Get the handler subkey.
                        var handlerSubkey = shellExtensionType.GetAttribute<HandlerSubkeyAttribute>();

                        if(handlerSubkey == null)
                            continue;

                        //  Check for the subkey.
                        if (handlerSubkey.AllowMultipleEntries)
                        {
                            //  Do we have the single subkey?
                            var handlerKeyPath = $"{className}\\ShellEx\\{handlerSubkey.HandlerSubkey}";
                            using (var handlerSubKey = classes.OpenSubKey(handlerKeyPath, RegistryKeyPermissionCheck.ReadSubTree, RegistryRights.ReadKey | RegistryRights.QueryValues))
                            {
                                //  Skip empty handlers.
                                if (handlerSubKey == null) continue;

                                //  Read entries.
                                foreach (var entry in handlerSubKey.GetSubKeyNames())
                                {
                                    using (var entryKey = handlerSubKey.OpenSubKey(entry, RegistryKeyPermissionCheck.ReadSubTree, RegistryRights.QueryValues | RegistryRights.ReadKey))
                                    {
                                        var guidVal = entryKey.GetValue(null, string.Empty).ToString();

                                        Guid guid;
                                        if (Guid.TryParse(guidVal, out guid) == false)
                                            continue;
                                        System.Diagnostics.Trace.WriteLine(string.Format("{0} has {3} {1} guid {2}", className,
                                            shellExtensionType.ToString(), guid, entry));

                                        //  If we do not have a shell extension info for this extension, create one.
                                        if (!shellExtensionsGuidMap.ContainsKey(guid))
                                        {
                                            shellExtensionsGuidMap[guid] = new ShellExtensionRegistrationInfo
                                            {
                                                DisplayName = entry,
                                                ShellExtensionType = shellExtensionType,
                                                ServerCLSID = guid,
                                            };
                                        }

                                        //  Add the class association.
                                        shellExtensionsGuidMap[guid].classRegistrations.Add(new ClassRegistration(className));
                                    }
                                }
                            }
                        }
                        else
                        {
                            //  Do we have the single subkey?
                            var handlerKeyPath = string.Format("{0}\\ShellEx\\{1}", className, handlerSubkey.HandlerSubkey);
                            using (var handlerSubKey = classes.OpenSubKey(handlerKeyPath, RegistryKeyPermissionCheck.ReadSubTree, RegistryRights.ReadKey | RegistryRights.QueryValues))
                            {
                                if (handlerSubKey != null)
                                {
                                    var guidVal = handlerSubKey.GetValue(null, string.Empty).ToString();

                                    Guid guid;
                                    if (Guid.TryParse(guidVal, out guid) == false)
                                        continue;
                                    System.Diagnostics.Trace.WriteLine(string.Format("{0} has {1} guid {2}", className, 
                                        shellExtensionType.ToString(), guid));
                                    
                                    //  If we do not have a shell extension info for this extension, create one.
                                    if (!shellExtensionsGuidMap.ContainsKey(guid))
                                    {
                                        shellExtensionsGuidMap[guid] = new ShellExtensionRegistrationInfo
                                        {
                                            ShellExtensionType = shellExtensionType,
                                            ServerCLSID = guid,
                                        };
                                    }

                                    //  Add the class association.
                                    shellExtensionsGuidMap[guid].classRegistrations.Add(new ClassRegistration(className));
                                }
                            }
                        }
                    }
                }
            }

            return shellExtensionsGuidMap.Values;
        }

        /// <summary>
        /// Gets the server registration info.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="registrationType">Type of the registration.</param>
        /// <returns>
        /// The ServerRegistrationInfo if the server is registered, otherwise false.
        /// </returns>
        public static ShellExtensionRegistrationInfo GetServerRegistrationInfo(ISharpShellServer server, RegistrationType registrationType)
        {
            //  Call the main function.
            return GetServerRegistrationInfo(server.ServerClsid, registrationType);
        }

        /// <summary>
        /// Gets the server registration info.
        /// </summary>
        /// <param name="serverCLSID">The server CLSID.</param>
        /// <param name="registrationType">Type of the registration.</param>
        /// <returns>
        /// The ServerRegistrationInfo if the server is registered, otherwise false.
        /// </returns>
        public static ShellExtensionRegistrationInfo GetServerRegistrationInfo(Guid serverCLSID, RegistrationType registrationType)
        {
            //  We can very quickly check to see if the server is approved.
            bool serverApproved = IsExtensionApproved(serverCLSID, registrationType);

            //  Open the classes.
            using (var classesKey = OpenClassesKey(registrationType, RegistryKeyPermissionCheck.ReadSubTree))
            {
                //  Do we have a subkey for the server?
                using (var serverClassKey = classesKey.OpenSubKey(serverCLSID.ToRegistryString()))
                {
                    //  If there's no subkey, the server isn't registered.
                    if (serverClassKey == null)
                        return null;

                    //  Do we have an InProc32 server?
                    using(var inproc32ServerKey = serverClassKey.OpenSubKey(KeyName_InProc32))
                    {
                        //  If we do, we can return the server info for an inproc 32 server.
                        if (inproc32ServerKey != null)
                        {
                            //  Get the default value.
                            var defaultValue = GetValueOrEmpty(inproc32ServerKey, null);

                            //  If we default value is null or empty, we've got a partially registered server.
                            if (string.IsNullOrEmpty(defaultValue))
                                return new ShellExtensionRegistrationInfo(ServerRegistationType.PartiallyRegistered, serverCLSID);

                            //  Get the threading model.
                            var threadingModel = GetValueOrEmpty(inproc32ServerKey, KeyName_ThreadingModel);

                            //  Is it a .NET server?
                            if (defaultValue == KeyValue_NetFrameworkServer)
                            {
                                //  We've got a .NET server. We should have one subkey, with the assembly version.
                                var subkeyName = inproc32ServerKey.GetSubKeyNames().FirstOrDefault();

                                //  If we have no subkey name, we've got a partially registered server.
                                if (subkeyName == null)
                                    return new ShellExtensionRegistrationInfo(ServerRegistationType.PartiallyRegistered, serverCLSID);

                                //  Otherwise we now have the assembly version.
                                var assemblyVersion = subkeyName;

                                //  Open the assembly subkey.
                                using (var assemblySubkey = inproc32ServerKey.OpenSubKey(assemblyVersion))
                                {
                                    //  If we can't open the key, we've got a problem.
                                    if (assemblySubkey == null)
                                        throw new InvalidOperationException("Can't open the details of the server.");

                                    //  Read the managed server details.
                                    var assembly = GetValueOrEmpty(assemblySubkey, KeyName_Assembly);
                                    var @class = GetValueOrEmpty(assemblySubkey, KeyName_Class);
                                    var runtimeVersion = GetValueOrEmpty(assemblySubkey, KeyName_RuntimeVersion);
                                    var codeBase = assemblySubkey.GetValue(KeyName_CodeBase, null);

                                    //  Return the server info.
                                    return new ShellExtensionRegistrationInfo(ServerRegistationType.ManagedInProc32, serverCLSID)
                                               {
                                                   ThreadingModel = threadingModel,
                                                   Assembly = assembly,
                                                   AssemblyVersion = assemblyVersion,
                                                   Class = @class,
                                                   RuntimeVersion = runtimeVersion,
                                                   CodeBase = codeBase != null ? codeBase.ToString() : null,
                                                   IsApproved = serverApproved
                                               };
                                }
                            }

                            //  We've got a native COM server.

                            //  Return the server info.
                            return new ShellExtensionRegistrationInfo(ServerRegistationType.NativeInProc32, serverCLSID)
                                       {
                                           ThreadingModel = threadingModel,
                                           ServerPath = defaultValue,
                                           IsApproved = serverApproved
                                       };
                        }
                    }

                    //  If by this point we haven't return server info, we've got a partially registered server.
                    return new ShellExtensionRegistrationInfo(ServerRegistationType.PartiallyRegistered, serverCLSID);
                }
            }
        }

        /// <summary>
        /// Sets the display name of the a COM server.
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="registrationType">Type of the registration.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void SetServerDisplayName(Guid classId, string displayName, RegistrationType registrationType)
        {
            //  Open the classes.
            using (var classesKey = OpenClassesKey(registrationType, RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                //  Create the server key.
                using (var serverKey = classesKey.OpenSubKey(classId.ToRegistryString(), RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.SetValue))
                {
                    if (serverKey == null)
                        throw new InvalidOperationException($"Cannot open class id key {classId}");

                    //  Set the display name.
                    serverKey.SetValue(null, displayName, RegistryValueKind.String);
                }
            }
        }

        /// <summary>
        /// Registers the server associations.
        /// </summary>
        /// <param name="serverClsid">The server CLSID.</param>
        /// <param name="serverType">Type of the server.</param>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="associationAttributes">The association attributes.</param>
        /// <param name="registrationType">Type of the registration.</param>
        internal static void RegisterServerAssociations(Guid serverClsid, ServerType serverType, string serverName, 
            IEnumerable<COMServerAssociationAttribute> associationAttributes, RegistrationType registrationType)
        {
            //  Go through each association.
            foreach (var associationAttribute in associationAttributes)
            {
                //  Get the association classes.
                var associationClassNames = CreateClassNamesForAssociations(associationAttribute.AssociationType, 
                    associationAttribute.Associations, registrationType);

                //  Open the classes key.
                using (var classesKey = OpenClassesRoot(registrationType))
                {
                    //  For each one, create the server type key.
                    foreach (var associationClassName in associationClassNames)
                    {
                        //  Create the server key.
                        using (var serverKey = classesKey.CreateSubKey(GetKeyForServerType(associationClassName, serverType, serverName)))
                        {
                            //  Set the server class id.
                            if (serverKey != null)
                                serverKey.SetValue(null, serverClsid.ToRegistryString());
                        }

                        //  If we're a shell icon handler, we must also set the defaulticon.
                        if (serverType == ServerType.ShellIconHandler)
                            SetIconHandlerDefaultIcon(classesKey, associationClassName);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the icon handler default icon, enabling an icon handler extension.
        /// </summary>
        /// <param name="classesKey">The classes key.</param>
        /// <param name="className">Name of the class.</param>
        private static void SetIconHandlerDefaultIcon(IRegistryKey classesKey, string className)
        {
            //  Open the class.
            using (var classKey = classesKey.OpenSubKey(className))
            {
                //  Check we have the class.
                if(classKey == null)
                    throw new InvalidOperationException("Cannot open class " + className);

                //  Open the default icon.
                using (var defaultIconKey = classKey.OpenSubKey(KeyName_DefaultIcon, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.ReadKey | RegistryRights.WriteKey))
                {
                    //  Check we have the key.
                    if (defaultIconKey == null)
                    {
                        // if not, we create the key.
                        var tempDefaultIconKey = classesKey.CreateSubKey(className + @"\" + KeyName_DefaultIcon, RegistryKeyPermissionCheck.ReadWriteSubTree);
                        tempDefaultIconKey.SetValue(null, "%1");
                    } 
                    else 
                    {
                        //  Get the default icon.
                        var defaultIcon = defaultIconKey.GetValue(null, string.Empty).ToString();

                        //  Save the default icon.
                        defaultIconKey.SetValue(ValueName_DefaultIconBackup, defaultIcon);
                        defaultIconKey.SetValue(null, "%1");
                    }
                }
            }
        }

        /// <summary>
        /// Unsets the icon handler default icon sharp shell value, restoring the backed up value.
        /// </summary>
        /// <param name="classesKey">The classes key.</param>
        /// <param name="className">Name of the class.</param>
        private static void UnsetIconHandlerDefaultIcon(IRegistryKey classesKey, string className)
        {
            //  Open the class.
            using (var classKey = classesKey.OpenSubKey(className))
            {
                //  Check we have the class.
                if (classKey == null)
                    throw new InvalidOperationException("Cannot open class " + className);

                //  Open the default icon.
                using (var defaultIconKey = classKey.OpenSubKey(KeyName_DefaultIcon, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.ReadKey | RegistryRights.WriteKey))
                {
                    //  Check we have the key.
                    if (defaultIconKey == null)
                        throw new InvalidOperationException("Cannot open default icon key for class " + className);

                    //  Do we have a backup default icon to restore?
                    if (defaultIconKey.GetValueNames().Any(vm => vm == ValueName_DefaultIconBackup))
                    {
                        //  Get the backup default icon.
                        var backupDefaultIcon = defaultIconKey.GetValue(ValueName_DefaultIconBackup, string.Empty).ToString();

                        //  Save the default icon, delete the backup.
                        defaultIconKey.SetValue(null, backupDefaultIcon);
                        defaultIconKey.DeleteValue(ValueName_DefaultIconBackup);
                    }
                }
            }
        }

        /// <summary>
        /// Unregisters the server associations.
        /// </summary>
        /// <param name="serverClsid">The server CLSID.</param>
        /// <param name="serverType">Type of the server.</param>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="associationAttributes">The association attributes.</param>
        /// <param name="registrationType">Type of the registration.</param>
        internal static void UnregisterServerAssociations(Guid serverClsid, ServerType serverType, string serverName,
            IEnumerable<COMServerAssociationAttribute> associationAttributes, RegistrationType registrationType)
        {
            //  Go through each association attribute.
            foreach (var associationAttribute in associationAttributes)
            {
                //  Get the assocation classes.
                var associationClassNames = CreateClassNamesForAssociations(associationAttribute.AssociationType,
                    associationAttribute.Associations, registrationType);

                //  Open the classes key...
                using (var classesKey = OpenClassesRoot(registrationType))
                {
                    //  ...then go through each association class.
                    foreach (var associationClassName in associationClassNames)
                    {
                        //  Get the key for the association.
                        var associationKeyPath = GetKeyForServerType(associationClassName, serverType, serverName);

                        //  Delete it if it exists.
                        classesKey.DeleteSubKeyTree(associationKeyPath, false);

                        //  If we're a shell icon handler, we must also unset the defaulticon.
                        if (serverType == ServerType.ShellIconHandler)
                            UnsetIconHandlerDefaultIcon(classesKey, associationClassName);
                    }
                }
            }
        }

        /// <summary>
        /// Creates the class names for associations.
        /// </summary>
        /// <param name="associationType">Type of the association.</param>
        /// <param name="associations">The associations.</param>
        /// <param name="registrationType">Type of the registration.</param>
        /// <returns>
        /// The class names for the associations.
        /// </returns>
        private static IEnumerable<string> CreateClassNamesForAssociations(AssociationType associationType, 
            IEnumerable<string> associations, RegistrationType registrationType)
        {
            //  Switch on the association type.
            switch (associationType)
            {
                //  We are handling the obsolete file extension type for backwards compatiblity.
#pragma warning disable 618
                case AssociationType.FileExtension:
#pragma warning restore 618

                    //  We're dealing with file extensions only, so we can return them directly.
                    return associations;

                case AssociationType.ClassOfExtension:

                    //  Open the classes sub key and get or create each file extension classes.
                    using (var classesKey = OpenClassesRoot(registrationType))
                    {
                        return associations.Select(extension => FileExtensionClass.Get(classesKey, extension, true)).ToArray();
                    }

                case AssociationType.Class:

                    //  We're dealing with classes only, so we can return them directly.
                    return associations;

                default:

                    //  If this is a predefined shell object, return the class for it.
                    var className = PredefinedShellObjectAttribute.GetClassName(associationType);
                    if (className != null) return new[] {className};
                    
                    //  It's not a type we know how to deal with, so bail.
                    throw new InvalidOperationException($@"Unable to determine associations for AssociationType '{associationType}'");
            }
        }

        /// <summary>
        /// Gets the type of the key for server.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <param name="serverType">Type of the server.</param>
        /// <param name="serverName">Name of the server.</param>
        /// <returns></returns>
        private static string GetKeyForServerType(string className, ServerType serverType, string serverName)
        {
            //  Create the server type name.
            switch (serverType)
            {
                case ServerType.ShellContextMenu:
                    
                    //  Create the key name for a context menu.
                    return string.Format(@"{0}\ShellEx\ContextMenuHandlers\{1}", className, serverName);

                case ServerType.ShellPropertySheet:

                    //  Create the key name for a property sheet.
                    return string.Format(@"{0}\ShellEx\PropertySheetHandlers\{1}", className, serverName);

                case ServerType.ShellIconHandler:

                    //  Create the key name for an icon handler. This has no server name, 
                    //  as there cannot be multiple icon handlers.
                    return string.Format(@"{0}\ShellEx\IconHandler", className);

                case ServerType.ShellInfoTipHandler:

                    //  Create the key name for an info tip handler. This has no server name, 
                    //  as there cannot be multiple info tip handlers.
                    return string.Format(@"{0}\ShellEx\{{00021500-0000-0000-C000-000000000046}}", className);

                case ServerType.ShellDropHandler:

                    //  Create the key name for a drop handler. This has no server name, 
                    //  as there cannot be multiple drop handlers.
                    return string.Format(@"{0}\ShellEx\DropHandler", className);

                case ServerType.ShellPreviewHander:
                    
                    //  Create the key name for a preview handler. This has no server name, 
                    //  as there cannot be multiple preview handlers.
                    return string.Format(@"{0}\ShellEx\{{8895b1c6-b41f-4c1c-a562-0d564250836f}}", className);

                case ServerType.ShellDataHandler:
                    
                    //  Create the key name for a data handler. This has no server name, 
                    //  as there cannot be multiple data handlers.
                    return string.Format(@"{0}\ShellEx\DataHandler", className);

                case ServerType.ShellThumbnailHandler:

                    //  Create the key name for a thumbnail handler. This has no server name, 
                    //  as there cannot be multiple data handlers.
                    return string.Format(@"{0}\ShellEx\{{e357fccd-a995-4576-b01f-234630154e96}}", className);

                case ServerType.ShellNamespaceExtension:

                    //  We don't have a key for shell namespace extensions.
                    return null;
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(serverType));
            }
        }

        /// <summary>
        /// Opens the classes key.
        /// </summary>
        /// <param name="registrationType">Type of the registration.</param>
        /// <param name="permissions">The permissions.</param>
        /// <returns></returns>
        private static IRegistryKey OpenClassesKey(RegistrationType registrationType, RegistryKeyPermissionCheck permissions)
        {
            //  Get the classes base key.
            var classesBaseKey = OpenClassesRoot(registrationType);
            
            //  Open classes.
            var classesKey = classesBaseKey.OpenSubKey(KeyName_Classes, permissions, RegistryRights.QueryValues | RegistryRights.ReadPermissions | RegistryRights.EnumerateSubKeys);
            if (classesKey == null)
                throw new InvalidOperationException("Cannot open classes.");

            return classesKey;
        }

        /// <summary>
        /// Opens the classes root.
        /// </summary>
        /// <param name="registrationType">Type of the registration.</param>
        /// <returns>The classes root key.</returns>
        private static IRegistryKey OpenClassesRoot(RegistrationType registrationType)
        {
            //  Get the registry.
            var registry = ServiceRegistry.ServiceRegistry.GetService<IRegistry>();

            //  Get the classes base key.
            var classesBaseKey = registrationType == RegistrationType.OS64Bit
                ? registry.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64) :
                registry.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry32);

            //  Return the classes key.
            return classesBaseKey;
        }

        /// <summary>
        /// Gets the value or empty.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <returns></returns>
        private static string GetValueOrEmpty(IRegistryKey key, string valueName)
        {
            object value = key.GetValue(valueName);
            if (value == null)
                return string.Empty;
            return value.ToString();
        }

        /// <summary>
        /// Approves an extension.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="registrationType">Type of the registration.</param>
        /// <exception cref="System.InvalidOperationException">Failed to open the Approved Extensions key.</exception>
        private static void ApproveExtension(ISharpShellServer server, RegistrationType registrationType)
        {
            //  Open the approved extensions key.
            using(var approvedKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, 
                registrationType == RegistrationType.OS64Bit ? RegistryView.Registry64 : RegistryView.Registry32)
                .OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                //  If we can't open the key, we're going to have problems.
                if(approvedKey == null)
                    throw new InvalidOperationException("Failed to open the Approved Extensions key.");

                //  Create an entry for the server.
                approvedKey.SetValue(server.ServerClsid.ToRegistryString(), server.DisplayName);
            }
        }

        /// <summary>
        /// Determines whether an extension is approved.
        /// </summary>
        /// <param name="serverClsid">The server CLSID.</param>
        /// <param name="registrationType">Type of the registration.</param>
        /// <returns>
        ///   <c>true</c> if the extension is approved; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">Failed to open the Approved Extensions key.</exception>
        private static bool IsExtensionApproved(Guid serverClsid, RegistrationType registrationType)
        {
            //  Open the approved extensions key.
            using (var approvedKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                registrationType == RegistrationType.OS64Bit ? RegistryView.Registry64 : RegistryView.Registry32)
                .OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", RegistryKeyPermissionCheck.ReadSubTree))
            {
                //  If we can't open the key, we're going to have problems.
                if (approvedKey == null)
                    throw new InvalidOperationException("Failed to open the Approved Extensions key.");

                return approvedKey.GetValueNames().Any(vn => vn.Equals(serverClsid.ToRegistryString(), StringComparison.OrdinalIgnoreCase));
            }
        }

        /// <summary>
        /// Unapproves an extension.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="registrationType">Type of the registration.</param>
        /// <exception cref="System.InvalidOperationException">Failed to open the Approved Extensions key.</exception>
        private static void UnapproveExtension(ISharpShellServer server, RegistrationType registrationType)
        {
            //  Open the approved extensions key.
            using (var approvedKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                registrationType == RegistrationType.OS64Bit ? RegistryView.Registry64 : RegistryView.Registry32)
                .OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                //  If we can't open the key, we're going to have problems.
                if (approvedKey == null)
                    throw new InvalidOperationException("Failed to open the Approved Extensions key.");

                //  Delete the value if it's there.
                approvedKey.DeleteValue(server.ServerClsid.ToRegistryString(), false);
            }
        }

        /// <summary>
        /// The classes key name.
        /// </summary>
        private const string KeyName_Classes = @"CLSID";

        /// <summary>
        /// The InProc32 key name.
        /// </summary>
        private const string KeyName_InProc32 = @"InprocServer32";

        /// <summary>
        /// The value for the net framework servers.
        /// </summary>
        private const string KeyValue_NetFrameworkServer = @"mscoree.dll";

        /// <summary>
        /// The threading model key name.
        /// </summary>
        private const string KeyName_ThreadingModel = @"ThreadingModel";

        /// <summary>
        /// THe assembly key name.
        /// </summary>
        private const string KeyName_Assembly = @"Assembly";

        /// <summary>
        /// The class key name.
        /// </summary>
        private const string KeyName_Class = @"Class";

        /// <summary>
        /// The runtime version key name.
        /// </summary>
        private const string KeyName_RuntimeVersion = @"RuntimeVersion";

        /// <summary>
        /// The codebase keyname.
        /// </summary>
        private const string KeyName_CodeBase = @"CodeBase";

        /// <summary>
        /// The default icon keyname.
        /// </summary>
        private const string KeyName_DefaultIcon = @"DefaultIcon";

        /// <summary>
        /// The default icon backup value name.
        /// </summary>
        private const string ValueName_DefaultIconBackup = @"SharpShell_Backup_DefaultIcon";
    }
}
