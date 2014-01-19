using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.CSharp.RuntimeBinder;
using SharpShell.Attributes;
using SharpShell.Pidl;
using SharpShell.SharpNamespaceExtension;

namespace TrivialNamespaceExtension
{
    
    [ComVisible(true)]
    [NamespaceExtensionJunctionPoint(NamespaceExtensionAvailability.Everyone, VirtualFolder.MyComputer, "Trivial Extension")]
    public class TrivialNamespaceExtension : SharpNamespaceExtension
    {
        public override NamespaceExtensionRegistrationSettings GetRegistrationSettings()
        {
            return new NamespaceExtensionRegistrationSettings
            {
                ExtensionAttributes = AttributeFlags.IsFolder | AttributeFlags.MayContainSubFolders
            };
        }

        protected override IEnumerable<IShellNamespaceItem> GetChildren(ShellNamespaceEnumerationFlags flags)
        {
            yield break;
        }

        protected override ShellNamespaceFolderView GetView()
        {
            return new CustomNamespaceFolderView(new ExtensionViewControl());
        }
    }
}
