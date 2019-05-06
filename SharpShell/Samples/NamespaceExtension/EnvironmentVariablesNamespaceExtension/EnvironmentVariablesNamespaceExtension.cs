using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SharpShell.SharpNamespaceExtension;

namespace EnvironmentVariablesNamespaceExtension
{
    [ComVisible(true)]
    [NamespaceExtensionJunctionPoint(NamespaceExtensionAvailability.Everyone, VirtualFolder.MyComputer, "Environment Variables")]
    public class EnvironmentVariablesNamespaceExtension : SharpNamespaceExtension
    {
        public override NamespaceExtensionRegistrationSettings GetRegistrationSettings()
        {
            return new NamespaceExtensionRegistrationSettings
            {
                //  We must be able to browse the item and open it (like a folder).
                ExtensionAttributes = AttributeFlags.IsBrowsable | AttributeFlags.IsFolder,
                HideFolderVerbs = true,
                UseAssemblyIcon = true,
                Tooltip = "Manages System Environment Variables"
            };
        }

        protected override IEnumerable<IShellNamespaceItem> GetChildren(ShellNamespaceEnumerationFlags flags)
        {
            foreach (var environmentVariable in Environment.GetEnvironmentVariables().Keys)
            {
                yield return new EnvironmentVariableNamespaceItem(environmentVariable.ToString());
            }
        }

        protected override ShellNamespaceFolderView GetView()
        {
            var columns = new[]
            {
                new ShellDetailColumn("Name", new PropertyKey(StandardPropertyKey.PKEY_ItemNameDisplay)),
                new ShellDetailColumn("Value", new PropertyKey(KeyProperties.valueGuid, KeyProperties.valuePid)),
            };
            return new DefaultNamespaceFolderView(columns, (item, column) =>
            {
                //  Get the environment variable. If it is not the correct type, return null.
                if (!(item is EnvironmentVariableNamespaceItem environmentVariableItem)) return null;

                //  Return the appropriate column items.
                if (column.PropertyKey.Equals(StandardPropertyKey.PKEY_ItemNameDisplay))
                    return environmentVariableItem.GetDisplayName(DisplayNameContext.Normal);
                if (column.PropertyKey.FormatId == KeyProperties.valueGuid)
                    return environmentVariableItem.GetValue();
                
                return null;
            });
        }
    }

    public static class KeyProperties
    {
        public static Guid valueGuid = new Guid("{71DDFA53-2148-4C21-8C99-F619308FC73B}");
        public static uint valuePid = 3;
    }
}
