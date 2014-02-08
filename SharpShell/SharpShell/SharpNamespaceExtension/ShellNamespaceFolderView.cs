using SharpShell.Interop;

namespace SharpShell.SharpNamespaceExtension
{
    public abstract class ShellNamespaceFolderView
    {
        internal abstract IShellView CreateShellView(IShellFolder folder);
    }
}