using SharpShell.Interop;
using SharpShell.Pidl;
using SharpShell.SharpContextMenu;

namespace SharpShell.SharpNamespaceExtension
{
    /// <summary>
    /// Defines the contract of an object that can provide a context menu
    /// for a Shell Namespace Folder. Typically this interface is implemented on classes
    /// that also implement <see cref="IShellNamespaceFolder" /> and is used in conjunction
    /// with <see cref="SharpContextMenu"/>.
    /// </summary>
    public interface IShellNamespaceFolderContextMenuProvider
    {
        ///<summary>
        /// Creates a context menu for a set of folder items.
        /// </summary>
        /// <param name="folderIdList">The folder identifier list.</param>
        /// <param name="folderItemIdLists">The folder item identifier lists. The user may
        /// select more than one item before right clicking.</param>
        /// <returns>A context menu for the item(s). This can be a custom type
        /// or more typically a <see cref="SharpContextMenu"/>.
        /// </returns>
        IContextMenu CreateContextMenu(IdList folderIdList, IdList[] folderItemIdLists);
    }
}