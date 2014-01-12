using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using SharpShell.Interop;

namespace SharpShell.SharpNamespaceExtension
{
    /// <summary>
    /// Represents a folder in the shell. Most Shell Namespace Extensions will offer
    /// access to a set of these folders.
    /// </summary>
    //  TODO: derive from IShellFolder and then derive Shell Namespace Extension from this.
    public abstract class ShellNamespaceFolder : IShellNamespaceItem
    {
        /// <summary>
        /// Gets the unique identifier for the namespace item.
        /// </summary>
        /// <returns>
        /// The unique identifier for the namespace item.
        /// </returns>
        public abstract byte[] GetUniqueId();

        /// <summary>
        /// Gets the display name of the item, which may be different for different contexts.
        /// </summary>
        /// <param name="displayNameContext"></param>
        /// <returns>
        /// The namespace item's display name.
        /// </returns>
        public abstract string GetDisplayName(DisplayNameContext displayNameContext);

        /// <summary>
        /// Gets the attributes for the shell item.
        /// </summary>
        /// <returns>
        /// The attributes for the shell item.
        /// </returns>
        public abstract AttributeFlags GetAttributes();
    }
}
