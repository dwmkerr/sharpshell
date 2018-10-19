using System.Windows.Forms;
using SharpShell.Interop;

namespace SharpShell.SharpNamespaceExtension
{
    /// <summary>
    /// A NamespaceFolderView which uses a custom control.
    /// </summary>
    /// <seealso cref="SharpShell.SharpNamespaceExtension.ShellNamespaceFolderView" />
    public sealed class CustomNamespaceFolderView : ShellNamespaceFolderView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomNamespaceFolderView"/> class.
        /// </summary>
        /// <param name="customView">The custom view.</param>
        public CustomNamespaceFolderView(UserControl customView)
        {
            this.customView = customView;
        }

        /// <summary>
        /// The custom view.
        /// </summary>
        private readonly UserControl customView;

        /// <summary>
        /// Creates the shell view.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <returns>A shell view.</returns>
        internal override IShellView CreateShellView(IShellFolder folder)
        {
            return new ShellViewHost(customView);
        }
    }
}