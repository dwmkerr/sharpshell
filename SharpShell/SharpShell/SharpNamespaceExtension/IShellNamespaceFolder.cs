using System.Collections.Generic;

namespace SharpShell.SharpNamespaceExtension
{
    /// <summary>
    /// A ShellNamespaceFolder is a Shell Namespace Item which contains other items.
    /// </summary>
    /// <seealso cref="SharpShell.SharpNamespaceExtension.IShellNamespaceItem" />
    public interface IShellNamespaceFolder : IShellNamespaceItem
    {
        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <param name="flags">The enumeration flags.</param>
        /// <returns>The folder children.</returns>
        IEnumerable<IShellNamespaceItem> GetChildren(ShellNamespaceEnumerationFlags flags);

        /// <summary>
        /// Gets a view for the folder.
        /// </summary>
        /// <returns>A view for the folder.</returns>
        ShellNamespaceFolderView GetView();
    }
}