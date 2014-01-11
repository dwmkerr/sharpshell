using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpShell.SharpNamespaceExtension
{
    public interface IShellNamespaceIdentifiable
    {
        byte[] GetUniqueId();
    }

    interface INamespaceFolder<INamespaceIdentifiable>
    {
    }
}
