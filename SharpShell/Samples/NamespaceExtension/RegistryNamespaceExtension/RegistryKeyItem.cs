using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using SharpShell.Diagnostics;
using SharpShell.Pidl;
using SharpShell.SharpNamespaceExtension;

namespace RegistryNamespaceExtension
{
    public class RegistryKeyItem : IShellNamespaceFolder
    {
        private readonly RegistryKey hiveKey;
        private readonly string displayName;

        public RegistryKeyItem(RegistryKey hiveKey, string displayName)
        {
            this.hiveKey = hiveKey;
            this.displayName = displayName;
            lazyChildKeys = new Lazy<List<RegistryKeyItem>>(CreateChildKeys);
                
            lazyAttributes = new Lazy<List<KeyAttribute>>(() =>
                hiveKey.GetValueNames().Select(valueName => 
                    new KeyAttribute(valueName, hiveKey.GetValue(valueName).ToString())).ToList());
        }

        private List<RegistryKeyItem> CreateChildKeys()
        {
            var childKeys = new List<RegistryKeyItem>();
            foreach (var subkeyName in hiveKey.GetSubKeyNames())
            {
                try
                {
                    childKeys.Add( new RegistryKeyItem(hiveKey.OpenSubKey(subkeyName), subkeyName));
                }
                catch (Exception exception)
                {
                    Logging.Error($"An error occurred enumerating subkeys of {displayName}", exception);
                }
            }
            return childKeys;
        }

        ShellId IShellNamespaceItem.GetShellId()
        {
            return ShellId.FromString(displayName);
        }

        string IShellNamespaceItem.GetDisplayName(DisplayNameContext displayNameContext)
        {
            return displayName;
        }

        AttributeFlags IShellNamespaceItem.GetAttributes()
        {
            return AttributeFlags.IsFolder | AttributeFlags.MayContainSubFolders;
        }

        public Icon GetIcon()
        {
            return Properties.Resources.RegistryKey;
        }

        IEnumerable<IShellNamespaceItem> IShellNamespaceFolder.GetChildren(ShellNamespaceEnumerationFlags flags)
        {
            Logging.Log($"Enumerating children for {displayName}");
            //  If we've been asked for folders, return all subkeys.
            if (flags.HasFlag(ShellNamespaceEnumerationFlags.Folders))
            {
                foreach (var childKey in lazyChildKeys.Value)
                    yield return childKey;
            }

            //  If we've been asked for items, return all items.
            if (flags.HasFlag(ShellNamespaceEnumerationFlags.Items))
            {
                foreach (var childAttribute in lazyAttributes.Value)
                    yield return childAttribute;
            }
        }

        public ShellNamespaceFolderView GetView()
        {
            var columns = new[]
            {
                new ShellDetailColumn("Name", new PropertyKey(StandardPropertyKey.PKEY_ItemNameDisplay)),
                new ShellDetailColumn("Value", new PropertyKey(KeyProperties.valueGuid, KeyProperties.valuePid))
            };
            return new DefaultNamespaceFolderView(columns, (item, column) => item.GetDisplayName(DisplayNameContext.Normal));
        }

        private readonly Lazy<List<RegistryKeyItem>> lazyChildKeys;
        private readonly Lazy<List<KeyAttribute>> lazyAttributes;
    }
}