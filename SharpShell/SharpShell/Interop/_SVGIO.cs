namespace SharpShell.Interop
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Used with the IFolderView::Items, IFolderView::ItemCount, and IShellView::GetItemObject methods to restrict or control the items in their collections.
    /// </summary>
    public enum _SVGIO : uint
    {
        /// <summary>
        /// Refers to the background of the view. It is used with IID_IContextMenu to get a shortcut menu for the view background and with IID_IDispatch to get a dispatch interface that represents the ShellFolderView object for the view.
        /// </summary>
        SVGIO_BACKGROUND = 0x00000000,

        /// <summary>
        /// Refers to the currently selected items. Used with IID_IDataObject to retrieve a data object that represents the selected items.
        /// </summary>
        SVGIO_SELECTION = 0x00000001,

        /// <summary>
        /// Used in the same way as SVGIO_SELECTION but refers to all items in the view.
        /// </summary>
        SVGIO_ALLVIEW = 0x00000002,

        /// <summary>
        /// Used in the same way as SVGIO_SELECTION but refers to checked items in views where checked mode is supported. For more details on checked mode, see FOLDERFLAGS.
        /// </summary>
        SVGIO_CHECKED = 0x00000003,

        /// <summary>
        /// Used in the same way as SVGIO_SELECTION but refers to checked items in views where checked mode is supported. For more details on checked mode, see FOLDERFLAGS.
        /// </summary>
        SVGIO_TYPE_MASK = 0x0000000F,

        /// <summary>
        /// Returns the items in the order they appear in the view. If this flag is not set, the selected item will be listed first.
        /// </summary>
        SVGIO_FLAG_VIEWORDER = 0x80000000
    }

    // ReSharper restore InconsistentNaming
}