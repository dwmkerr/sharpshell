using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using SharpShell.Interop;
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
    }

    public interface IShellNamespaceFolder : IShellNamespaceItem
    {
        IEnumerable<IShellNamespaceItem> GetChildren(ShellNamespaceEnumerationFlags flags);

        ShellNamespaceFolderView GetView();
    }

    public abstract class ShellNamespaceFolderView
    {
        internal abstract IShellView CreateShellView(IShellFolder folder);
    }

    public sealed class DefaultNamespaceFolderView : ShellNamespaceFolderView
    {
        public DefaultNamespaceFolderView(IEnumerable<ShellDetailColumn> detailColumns)
        {
            columns = new List<ShellDetailColumn>(detailColumns);
        }

        private readonly List<ShellDetailColumn> columns;
        internal override IShellView CreateShellView(IShellFolder folder)
        {
            //  Setup create info for a new default folder view.
            var createInfo = new SFV_CREATE
                {
                    cbSize = (uint) Marshal.SizeOf(typeof (SFV_CREATE)),
                    pshf = folder,
                    psvOuter = null,
                    psfvcb = null
                };
                IShellView view;
                if (Shell32.SHCreateShellFolderView(createInfo, out view) != WinError.S_OK)
                {
                    throw new Exception("An error occured creating the default folder view.");
                }

            return view;
        }
    }

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

    public class ShellDetailColumn
    {
        public ShellDetailColumn(string name)
        {
            this.name = name;
        }

        private readonly string name;
    }
}