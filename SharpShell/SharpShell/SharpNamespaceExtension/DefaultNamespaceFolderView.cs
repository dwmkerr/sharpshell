using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using SharpShell.Interop;

namespace SharpShell.SharpNamespaceExtension
{
    /// <summary>
    /// The default shell namespace folder view.
    /// </summary>
    /// <seealso cref="SharpShell.SharpNamespaceExtension.ShellNamespaceFolderView" />
    /// <seealso cref="SharpShell.Interop.IShellFolderViewCB" />
    public sealed class DefaultNamespaceFolderView : ShellNamespaceFolderView, IShellFolderViewCB
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultNamespaceFolderView"/> class.
        /// </summary>
        /// <param name="detailColumns">The detail columns.</param>
        /// <param name="itemDetailProvider">The item detail provider.</param>
        public DefaultNamespaceFolderView(IEnumerable<ShellDetailColumn> detailColumns,
            Func<IShellNamespaceItem, ShellDetailColumn, object> itemDetailProvider)
        {
            this.itemDetailProvider = itemDetailProvider;
            columns = new List<ShellDetailColumn>(detailColumns);
        }

        /// <summary>
        /// Creates a Shell View from a Shell Folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <returns>
        /// The Shell View for the folder.
        /// </returns>
        /// <exception cref="Exception">An error occured creating the default folder view.</exception>
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

            //  TODO: IMPORTANT: This is the function that's failing now for creating the 
            //  view, it seems that it could be due to not providing psvOuter (which may be
            //  required as we're not far off being a common dialog) or more likely because we
            //  are not providing the callback. Try both...
            //  NOTE: A quick test shows it's unlikely to be the psvOuter, try the CB.
            //  NOTE: adding the callback hasn't helped, we can try the alternative call
            //  which is shcreateshellfolderviewex
            //  NOTE: None of those suggestions worked.
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

        /// <summary>
        /// Allows communication between the system folder view object and a system folder view callback object.
        /// </summary>
        /// <param name="uMsg">One of the following notifications.</param>
        /// <param name="wParam">Additional information.</param>
        /// <param name="lParam">Additional information.</param>
        /// <param name="plResult">Additional information.</param>
        /// <returns>
        /// S_OK if the message was handled, E_NOTIMPL if the shell should perform default processing.
        /// </returns>
        public int MessageSFVCB(SFVM uMsg, IntPtr wParam, IntPtr lParam, ref IntPtr plResult)
        {
            return WinError.E_NOTIMPL;
        }
    }
}