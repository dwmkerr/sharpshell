using System;

namespace SharpShell.Interop
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Determines the types of items included in an enumeration. These values are used with the IShellFolder::EnumObjects method.
    /// </summary>
    [Flags]
    public enum SHCONTF
    {
        /// <summary>
        /// Windows 7 and later. The calling application is checking for the existence of child items in the folder.
        /// </summary>
        SHCONTF_CHECKING_FOR_CHILDREN = 0x00010,

        /// <summary>
        /// Include items that are folders in the enumeration.
        /// </summary>
        SHCONTF_FOLDERS = 0x00020,

        /// <summary>
        /// Include items that are not folders in the enumeration.
        /// </summary>
        SHCONTF_NONFOLDERS = 0x00040,

        /// <summary>
        /// Include hidden items in the enumeration. This does not include hidden system items. (To include hidden system items, use SHCONTF_INCLUDESUPERHIDDEN.)
        /// </summary>
        SHCONTF_INCLUDEHIDDEN = 0x00080,

        /// <summary>
        /// No longer used; always assumed. IShellFolder::EnumObjects can return without validating the enumeration object. Validation can be postponed until the first call to IEnumIDList::Next. Use this flag when a user interface might be displayed prior to the first IEnumIDList::Next call. For a user interface to be presented, hwnd must be set to a valid window handle.
        /// </summary>
        SHCONTF_INIT_ON_FIRST_NEXT = 0x00100,

        /// <summary>
        /// The calling application is looking for printer objects.
        /// </summary>
        SHCONTF_NETPRINTERSRCH = 0x00200,

        /// <summary>
        /// The calling application is looking for resources that can be shared.
        /// </summary>
        SHCONTF_SHAREABLE = 0x00400,

        /// <summary>
        /// Include items with accessible storage and their ancestors, including hidden items.
        /// </summary>
        SHCONTF_STORAGE = 0x00800,

        /// <summary>
        /// Windows 7 and later. Child folders should provide a navigation enumeration.
        /// </summary>
        SHCONTF_NAVIGATION_ENUM = 0x01000,

        /// <summary>
        /// Windows Vista and later. The calling application is looking for resources that can be enumerated quickly.
        /// </summary>
        SHCONTF_FASTITEMS = 0x02000,

        /// <summary>
        /// Windows Vista and later. Enumerate items as a simple list even if the folder itself is not structured in that way.
        /// </summary>
        SHCONTF_FLATLIST = 0x04000,

        /// <summary>
        /// Windows Vista and later. The calling application is monitoring for change notifications. This means that the enumerator does not have to return all results. Items can be reported through change notifications.
        /// </summary>
        SHCONTF_ENABLE_ASYNC = 0x08000,

        /// <summary>
        /// Windows 7 and later. Include hidden system items in the enumeration. This value does not include hidden non-system items. (To include hidden non-system items, use SHCONTF_INCLUDEHIDDEN.)
        /// </summary>
        SHCONTF_INCLUDESUPERHIDDEN = 0x10000
    }

    // ReSharper restore InconsistentNaming
}