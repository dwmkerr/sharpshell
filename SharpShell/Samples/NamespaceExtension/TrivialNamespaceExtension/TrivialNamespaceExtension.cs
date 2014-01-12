using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
using SharpShell.Attributes;
using SharpShell.Pidl;
using SharpShell.SharpNamespaceExtension;

namespace TrivialNamespaceExtension
{
    /*
    [ComVisible(true)]
    [NamespaceExtensionJunctionPoint(NamespaceExtensionAvailability.Everyone, VirtualFolder.MyComputer, "Trivial Extension")]
    public class TrivialNamespaceExtension : SharpNamespaceExtension
    {
        public override IEnumerable<IShellNamespaceItem> EnumerateChildren(uint index, uint count, EnumerateChildrenFlags flags)
        {
            yield break;
        }

        public override IShellNamespaceItem GetChildItem(IdList idList)
        {
            return null;
        }
    } */
}
