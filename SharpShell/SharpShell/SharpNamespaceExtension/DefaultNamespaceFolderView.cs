using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using SharpShell.Interop;

namespace SharpShell.SharpNamespaceExtension
{
    public sealed class DefaultNamespaceFolderView : ShellNamespaceFolderView
    {

        public DefaultNamespaceFolderView(IEnumerable<ShellDetailColumn> detailColumns,
            Func<IShellNamespaceItem, ShellDetailColumn, object> itemDetailProvider)
        {
            this.itemDetailProvider = itemDetailProvider;
            columns = new List<ShellDetailColumn>(detailColumns);
        }

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
            if (Shell32.SHCreateShellFolderView(ref createInfo, out view) != WinError.S_OK)
            {
                throw new Exception("An error occured creating the default folder view.");
            }

            return view;
        }

        internal object GetItemDetail(IShellNamespaceItem item, ShellDetailColumn column)
        {
            return itemDetailProvider(item, column);
        }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        public ReadOnlyCollection<ShellDetailColumn> Columns { get { return columns.AsReadOnly(); } }

        /// <summary>
        /// The internal list of columns.
        /// </summary>
        private readonly List<ShellDetailColumn> columns;

        /// <summary>
        /// A provider to get the details of an item.
        /// </summary>
        private readonly Func<IShellNamespaceItem, ShellDetailColumn, object> itemDetailProvider;
    }
}