using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Contains information about a menu item.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct MENUITEMINFO
    {
        /// <summary>
        /// The size of the structure, in bytes. The caller must set this member to sizeof(MENUITEMINFO).
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// Indicates the members to be retrieved or set.
        /// </summary>
        public uint fMask;

        /// <summary>
        /// The menu item type.
        /// </summary>
        public uint fType;

        /// <summary>
        /// The menu item state. 
        /// </summary>
        public uint fState;

        /// <summary>
        /// An application-defined value that identifies the menu item. Set fMask to MIIM_ID to use wID.
        /// </summary>
        public uint wID;

        /// <summary>
        /// A handle to the drop-down menu or submenu associated with the menu item. If the menu item is not an item that opens a drop-down menu or submenu, this member is NULL. Set fMask to MIIM_SUBMENU to use hSubMenu.
        /// </summary>
        public IntPtr hSubMenu;

        /// <summary>
        /// A handle to the bitmap to display next to the item if it is selected. If this member is NULL, a default bitmap is used. If the MFT_RADIOCHECK type value is specified, the default bitmap is a bullet. Otherwise, it is a check mark. Set fMask to MIIM_CHECKMARKS to use hbmpChecked.
        /// </summary>
        public IntPtr hbmpChecked;

        /// <summary>
        /// A handle to the bitmap to display next to the item if it is not selected. If this member is NULL, no bitmap is used. Set fMask to MIIM_CHECKMARKS to use hbmpUnchecked.
        /// </summary>
        public IntPtr hbmpUnchecked;

        /// <summary>
        /// An application-defined value associated with the menu item. Set fMask to MIIM_DATA to use dwItemData.
        /// </summary>
        public IntPtr dwItemData;

        /// <summary>
        /// The contents of the menu item. The meaning of this member depends on the value of fType and is used only if the MIIM_TYPE flag is set in the fMask member.
        /// </summary>
        public string dwTypeData;

        /// <summary>
        /// The length of the menu item text, in characters, when information is received about a menu item of the MFT_STRING type. However, cch is used only if the MIIM_TYPE flag is set in the fMask member and is zero otherwise. Also, cch is ignored when the content of a menu item is set by calling SetMenuItemInfo.
        /// </summary>
        public uint cch;

        /// <summary>
        /// A handle to the bitmap to be displayed, or it can be one of the values in the following table. It is used when the MIIM_BITMAP flag is set in the fMask member.
        /// </summary>
        public IntPtr hbmpItem;
    }
}