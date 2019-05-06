using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using Microsoft.Win32;
using SharpShell.SharpNamespaceExtension;

namespace RegistryNamespaceExtension
{
    /// <inheritdoc />
    /// <summary>
    /// The <see cref="T:RegistryNamespaceExtension.RegistryNamespaceExtension" /> is an example shell namespace extension
    /// which presents the contents of the registry in the Shell under My Computer.
    /// </summary>
    [ComVisible(true)]
    [NamespaceExtensionJunctionPoint(NamespaceExtensionAvailability.Everyone, VirtualFolder.MyComputer, "Registry")]
    public class RegistryNamespaceExtension : SharpNamespaceExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryNamespaceExtension"/> class.
        /// </summary>
        public RegistryNamespaceExtension()
        {
            //  Create the hive folders.
            hives = new List<RegistryKeyItem>
            {
                new RegistryKeyItem(Registry.LocalMachine, "HKEY_LOCAL_MACHINE"),
                new RegistryKeyItem(Registry.CurrentUser, "HKEY_CURRENT_USER"),
                new RegistryKeyItem(Registry.ClassesRoot, "HKEY_CLASSES_ROOT"),
                new RegistryKeyItem(Registry.Users, "HKEY_USERS_ROOT"),
                new RegistryKeyItem(Registry.CurrentConfig, "HKEY_CURRENT_CONFIG"),
            };
        }

        /// <summary>
        /// Gets the registration settings. This function is called only during the initial
        /// registration of a shell namespace extension to provide core configuration.
        /// </summary>
        /// <returns>
        /// Registration settings for the server.
        /// </returns>
        public override NamespaceExtensionRegistrationSettings GetRegistrationSettings()
        {
            //  The settings indicate we are a folder (as we have child items) and that we contain
            //  subfolders.
            return new NamespaceExtensionRegistrationSettings
            {
                ExtensionAttributes = AttributeFlags.IsFolder | AttributeFlags.MayContainSubFolders,
            };
        }

        protected override IEnumerable<IShellNamespaceItem> GetChildren(ShellNamespaceEnumerationFlags flags)
        {
            if (!flags.HasFlag(ShellNamespaceEnumerationFlags.Folders)) yield break;
            foreach (var hive in hives)
                yield return hive;
        }

        protected override ShellNamespaceFolderView GetView()
        {
            var columns = new[]
            {
                new ShellDetailColumn("Name", new PropertyKey(StandardPropertyKey.PKEY_ItemNameDisplay)),
                new ShellDetailColumn("Value", new PropertyKey(KeyProperties.valueGuid, KeyProperties.valuePid)),
            };
            return new DefaultNamespaceFolderView(columns, (item, column) => item.GetDisplayName(DisplayNameContext.Normal));
        }

        private readonly List<RegistryKeyItem> hives;
    }

    public static class KeyProperties
    {
        public static Guid valueGuid = new Guid("{71DDFA53-2148-4C21-8C99-F619308FC73B}");
        public static uint valuePid = 3;
    }


}