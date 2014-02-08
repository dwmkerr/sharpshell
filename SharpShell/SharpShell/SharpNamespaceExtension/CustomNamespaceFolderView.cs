using System.Windows.Forms;
using SharpShell.Interop;

namespace SharpShell.SharpNamespaceExtension
{
    public sealed class CustomNamespaceFolderView : ShellNamespaceFolderView
    {
        public CustomNamespaceFolderView(UserControl customView)
        {
            this.customView = customView;
        }

        private readonly UserControl customView;


        internal override IShellView CreateShellView(IShellFolder folder)
        {
            return new ShellViewHost(customView);
        }
    }
}