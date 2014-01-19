using System.Collections.Generic;
using Microsoft.Win32;
using SharpShell.SharpNamespaceExtension;

namespace RegistryNamespaceExtension
{
    /// <summary>
    /// 
    /// </summary>
    [NamespaceExtensionJunctionPoint(NamespaceExtensionAvailability.Everyone, VirtualFolder.MyComputer, "Registry")]
    public class RegistryNamespaceExtenson : SharpNamespaceExtension
    {
        public RegistryNamespaceExtenson()
        {
            //  Create the hive folders.
            hives = new List<RegistryKeyItem>
            {
                new RegistryKeyItem(Registry.LocalMachine, "Local Machine"),
                new RegistryKeyItem(Registry.CurrentConfig, "Current Config")
            };
        }

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
            if(flags.HasFlag(ShellNamespaceEnumerationFlags.Folders))
                foreach (var hive in hives)
                    yield return hive;
        }

        protected override ShellNamespaceFolderView GetView()
        {
            return new DefaultNamespaceFolderView(new [] {new ShellDetailColumn("Name"), new ShellDetailColumn("Value") });
        }

        private readonly List<RegistryKeyItem> hives;
    }
}