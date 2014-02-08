using System.Collections.Generic;

namespace SharpShell.SharpNamespaceExtension
{
    public interface IShellNamespaceFolder : IShellNamespaceItem
    {
        IEnumerable<IShellNamespaceItem> GetChildren(ShellNamespaceEnumerationFlags flags);

        ShellNamespaceFolderView GetView();
    }
}