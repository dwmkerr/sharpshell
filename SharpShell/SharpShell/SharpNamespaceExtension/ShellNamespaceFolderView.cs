using SharpShell.Interop;

namespace SharpShell.SharpNamespaceExtension
{
    /// <summary>
    /// A base class for a Shell Namespace Folder View.
    /// </summary>
    public abstract class ShellNamespaceFolderView
    {
        /// <summary>
        /// Creates a Shell View from a Shell Folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <returns>The Shell View for the folder.</returns>
        internal abstract IShellView CreateShellView(IShellFolder folder);
    }
}