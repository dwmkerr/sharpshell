using System.Collections;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using SharpShell.Pidl;

namespace SharpShell.SharpNamespaceExtension
{
    /// <summary>
    /// The <see cref="IShellNamespaceItem"/> interface must be implemented
    /// on all types that are shell namespace types, i.e. entities that are folders
    /// in explorer or items within them.
    /// </summary>
    public interface IShellNamespaceItem
    {
        /// <summary>
        /// Gets the shell identifier for this item. The only constraint for this shell identifier
        /// is that it must be unique within the parent folder, i.e. there cannot be a sibling with
        /// the same identifier.
        /// </summary>
        /// <returns>A shell identifier for the item.</returns>
        /// <remarks>
        /// A shell identifier can be created from raw data with <see cref="ShellId.FromData"/> or 
        /// from a string with <see cref="ShellId.FromString"/>.
        /// </remarks>
        ShellId GetShellId();

        /// <summary>
        /// Gets the display name of the item, which may be different for different contexts.
        /// </summary>
        /// <returns>The namespace item's display name.</returns>
        string GetDisplayName(DisplayNameContext displayNameContext);

        /// <summary>
        /// Gets the attributes for the shell item.
        /// </summary>
        /// <returns>The attributes for the shell item.</returns>
        AttributeFlags GetAttributes();

        /// <summary>
        /// Gets the icon for a shell item. If none is provided, the system default is used.
        /// </summary>
        /// <returns>The icon to use for a shell item.</returns>
        Icon GetIcon();
    }
}