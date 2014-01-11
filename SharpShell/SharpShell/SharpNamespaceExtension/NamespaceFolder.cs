using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SharpShell.SharpNamespaceExtension
{
    public interface IShellNamespaceItem
    {
        /// <summary>
        /// Gets the unique identifier for the namespace item.
        /// </summary>
        /// <returns>The unique identifier for the namespace item.</returns>
        byte[] GetUniqueId();

        /// <summary>
        /// Gets the display name of the item, which may be different for different contexts.
        /// </summary>
        /// <returns>The namespace item's display name.</returns>
        string GetDisplayName(DisplayNameContext displayNameContext);
    }

    interface INamespaceFolder<INamespaceIdentifiable>
    {
    }
}
