using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SharpShell.Components;
using SharpShell.Interop;
using SharpShell.SharpNamespaceExtension;

namespace EnvironmentVariablesNamespaceExtension
{
    [ComVisible(true)]
    [NamespaceExtensionJunctionPoint(NamespaceExtensionAvailability.Everyone, VirtualFolder.Desktop, "Environment Variables")]
    public class EnvironmentVariablesNamespaceExtension : SharpNamespaceExtension
    {
        private ExtractIconImpl impl;

        public EnvironmentVariablesNamespaceExtension()
        {
            impl = new ExtractIconImpl {Icon = Properties.Resources.Settings};
        }

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
            yield break;
        }

        protected override ShellNamespaceFolderView GetView()
        {
            return new CustomNamespaceFolderView(new EnvironmentVariablesView());
        }
    }
}
