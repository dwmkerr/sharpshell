using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Exposes methods that retrieve information about a folder's display options, select specified items in that folder, and set the folder's view mode.
    /// </summary>
    [ComImport, Guid("cde725b0-ccc9-4519-917e-325d72fab4ce")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IFolderView
    {
        /// <summary>
        /// Gets an address containing a value representing the folder's current view mode.
        /// </summary>
        /// <param name="pViewMode">A pointer to a memory location at which to store the folder's current view mode.</param>
        /// TODO: Use FOLDERVIEWMODE
        void GetCurrentViewMode(out uint pViewMode);

        /// <summary>
        /// Sets the selected folder's view mode.
        /// </summary>
        /// <param name="ViewMode">One of the following values from the FOLDERVIEWMODE enumeration.</param>
        /// TODO: Use FOLDERVIEWMODE
        void SetCurrentViewMode(uint ViewMode);

        /// <summary>
        /// Gets the folder object.
        /// </summary>
        /// <param name="riid">Reference to the desired IID to represent the folder.</param>
        /// <param name="ppv">When this method returns, contains the interface pointer requested in riid. This is typically IShellFolder or a related interface. This can also be an IShellItemArray with a single element.</param>
        void GetFolder(ref Guid riid, ref IntPtr ppv);

        /// <summary>
        /// Gets the identifier of a specific item in the folder view, by index.
        /// </summary>
        /// <param name="iItemIndex">The index of the item in the view.</param>
        /// <param name="ppidl">The address of a pointer to a PIDL containing the item's identifier information.</param>
        void Item(int iItemIndex, out IntPtr ppidl);

        /// <summary>
        /// Gets the number of items in the folder. This can be the number of all items, or a subset such as the number of selected items.
        /// </summary>
        /// <param name="uFlags">Flags from the _SVGIO enumeration that limit the count to certain types of items.</param>
        /// <param name="pcItems">Pointer to an integer that receives the number of items (files and folders) displayed in the folder view.</param>
        void ItemCount(uint uFlags, out int pcItems);

        /// <summary>
        /// Gets the address of an enumeration object based on the collection of items in the folder view.
        /// </summary>
        /// <param name="uFlags">_SVGIO values that limit the enumeration to certain types of items.</param>
        /// <param name="riid">Reference to the desired IID to represent the folder.</param>
        /// <param name="ppv">When this method returns, contains the interface pointer requested in riid. This is typically an IEnumIDList, IDataObject, or IShellItemArray. If an error occurs, this value is NULL.</param>
        void Items(uint uFlags, ref Guid riid, ref IntPtr ppv);

        /// <summary>
        /// Gets the index of an item in the folder's view which has been marked by using the SVSI_SELECTIONMARK in IFolderView::SelectItem.
        /// </summary>
        /// <param name="piItem">A pointer to the index of the marked item.</param>
        void GetSelectionMarkedItem(out int piItem);

        /// <summary>
        /// Gets the index of the item that currently has focus in the folder's view.
        /// </summary>
        /// <param name="piItem">A pointer to the index of the item.</param>
        void GetFocusedItem(out int piItem);

        /// <summary>
        /// Gets the position of an item in the folder's view.
        /// </summary>
        /// <param name="pidl">A pointer to an ITEMIDLIST interface.</param>
        /// <param name="ppt">A pointer to a structure that receives the position of the item's upper-left corner.</param>
        void GetItemPosition(IntPtr pidl, out POINT ppt);

        /// <summary>
        /// Gets a POINT structure containing the width (x) and height (y) dimensions, including the surrounding white space, of an item.
        /// </summary>
        /// <param name="ppt">A pointer to an existing structure to be filled with the current sizing dimensions of the items in the folder's view.</param>
        void GetSpacing(out POINT ppt);

        /// <summary>
        /// Gets a pointer to a POINT structure containing the default width (x) and height (y) measurements of an item, including the surrounding white space.
        /// </summary>
        /// <param name="ppt">Pointer to an existing structure to be filled with the default sizing dimensions of the items in the folder's view.</param>
        void GetDefaultSpacing(out POINT ppt);

        /// <summary>
        /// Gets the current state of the folder's Auto Arrange mode.
        /// </summary>
        /// <returns>Returns S_OK if the folder is in Auto Arrange mode; S_FALSE if it is not.</returns>
        [PreserveSig]
        int GetAutoArrange();

        /// <summary>
        /// Selects an item in the folder's view.
        /// </summary>
        /// <param name="iItem">The index of the item to select in the folder's view.</param>
        /// <param name="dwFlags">One of the _SVSIF constants that specify the type of selection to apply.</param>
        void SelectItem(int iItem, uint dwFlags);

        /// <summary>
        /// Allows the selection and positioning of items visible in the folder's view.
        /// </summary>
        /// <param name="cidl">The number of items to select.</param>
        /// <param name="apidl">A pointer to an array of size cidl that contains the PIDLs of the items.</param>
        /// <param name="apt">A pointer to an array of cidl structures containing the locations each corresponding element in apidl should be positioned.</param>
        /// <param name="dwFlags">One of the _SVSIF constants that specifies the type of selection to apply.</param>
        void SelectAndPositionItems(uint cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] IntPtr[] apidl, ref POINT apt, uint dwFlags);
    }
}