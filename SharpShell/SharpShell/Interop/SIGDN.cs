namespace SharpShell.Interop
{
    /// <summary>Requests the form of an item's display name to retrieve through IShellItem::GetDisplayName and SHGetNameFromIDList.</summary>
    /// <remarks>
    /// Different forms of an item's name can be retrieved through the item's properties, including those listed here. Note that not all
    /// properties are present on all items, so only those appropriate to the item will appear.
    /// </remarks>
    public enum SIGDN : uint
    {
        /// <summary>Returns the editing name relative to the desktop. In UI this name is suitable for display to the user.</summary>
        SIGDN_DESKTOPABSOLUTEEDITING = 0x8004c000,

        /// <summary>Returns the parsing name relative to the desktop. This name is not suitable for use in UI.</summary>
        SIGDN_DESKTOPABSOLUTEPARSING = 0x80028000,

        /// <summary>
        /// Returns the item's file system path, if it has one. Only items that report SFGAO_FILESYSTEM have a file system path. When an
        /// item does not have a file system path, a call to IShellItem::GetDisplayName on that item will fail. In UI this name is
        /// suitable for display to the user in some cases, but note that it might not be specified for all items.
        /// </summary>
        SIGDN_FILESYSPATH = 0x80058000,

        /// <summary>
        /// Returns the display name relative to the parent folder. In UI this name is generally ideal for display to the user.
        /// </summary>
        SIGDN_NORMALDISPLAY = 0,

        /// <summary>Returns the path relative to the parent folder.</summary>
        SIGDN_PARENTRELATIVE = 0x80080001,

        /// <summary>Returns the editing name relative to the parent folder. In UI this name is suitable for display to the user.</summary>
        SIGDN_PARENTRELATIVEEDITING = 0x80031001,

        /// <summary>
        /// Returns the path relative to the parent folder in a friendly format as displayed in an address bar. This name is suitable for
        /// display to the user.
        /// </summary>
        SIGDN_PARENTRELATIVEFORADDRESSBAR = 0x8007c001,

        /// <summary>Introduced in Windows 8.</summary>
        SIGDN_PARENTRELATIVEFORUI = 0x80094001,

        /// <summary>Returns the parsing name relative to the parent folder. This name is not suitable for use in UI.</summary>
        SIGDN_PARENTRELATIVEPARSING = 0x80018001,

        /// <summary>
        /// Returns the item's URL, if it has one. Some items do not have a URL, and in those cases a call to IShellItem::GetDisplayName
        /// will fail. This name is suitable for display to the user in some cases, but note that it might not be specified for all items.
        /// </summary>
        SIGDN_URL = 0x80068000
    }
}