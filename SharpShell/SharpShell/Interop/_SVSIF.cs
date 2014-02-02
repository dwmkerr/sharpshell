namespace SharpShell.Interop
{
    // ReSharper disable InconsistentNaming
 
    /// <summary>
    /// Indicates flags used by IFolderView, IFolderView2, IShellView and IShellView2 to specify a type of selection to apply.
    /// </summary>
    public enum _SVSIF : uint
    {
        /// <summary>
        /// Deselect the item.
        /// </summary>
        SVSI_DESELECT = 0x00000000,

        /// <summary>
        /// Select the item.
        /// </summary>
        SVSI_SELECT = 0x00000001,

        /// <summary>
        /// Put the name of the item into rename mode. This value includes SVSI_SELECT.
        /// </summary>
        SVSI_EDIT = 0x00000003,

        /// <summary>
        /// Deselect all but the selected item. If the item parameter is NULL, deselect all items.
        /// </summary>
        SVSI_DESELECTOTHERS = 0x00000004,

        /// <summary>
        /// In the case of a folder that cannot display all of its contents on one screen, display the portion that contains the selected item.
        /// </summary>
        SVSI_ENSUREVISIBLE = 0x00000008,

        /// <summary>
        /// Give the selected item the focus when multiple items are selected, placing the item first in any list of the collection returned by a method.
        /// </summary>
        SVSI_FOCUSED = 0x00000010,

        /// <summary>
        /// Convert the input point from screen coordinates to the list-view client coordinates.
        /// </summary>
        SVSI_TRANSLATEPT = 0x00000020,

        /// <summary>
        /// Mark the item so that it can be queried using IFolderView::GetSelectionMarkedItem.
        /// </summary>
        SVSI_SELECTIONMARK = 0x00000040,

        /// <summary>
        /// Allows the window's default view to position the item. In most cases, this will place the item in the first available position. However, if the call comes during the processing of a mouse-positioned context menu, the position of the context menu is used to position the item.
        /// </summary>
        SVSI_POSITIONITEM = 0x00000080,

        /// <summary>
        /// The item should be checked. This flag is used with items in views where the checked mode is supported.
        /// </summary>
        SVSI_CHECK = 0x00000100,

        /// <summary>
        /// The second check state when the view is in tri-check mode, in which there are three values for the checked state. You can indicate tri-check mode by specifying FWF_TRICHECKSELECT in IFolderView2::SetCurrentFolderFlags. The 3 states for FWF_TRICHECKSELECT are unchecked, SVSI_CHECK and SVSI_CHECK2.
        /// </summary>
        SVSI_CHECK2 = 0x00000200,

        /// <summary>
        /// Selects the item and marks it as selected by the keyboard. This value includes SVSI_SELECT.
        /// </summary>
        SVSI_KEYBOARDSELECT = 0x00000401,

        /// <summary>
        /// An operation to select or focus an item should not also set focus on the view itself.
        /// </summary>
        SVSI_NOTAKEFOCUS = 0x40000000
    }

    // ReSharper restore InconsistentNaming
}