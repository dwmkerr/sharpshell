using System;

namespace SharpShell.Interop
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// The flags that specify the file information to retrieve.
    /// </summary>
    [Flags]
    public enum SHGFI
    {
        /// <summary>
        /// Retrieve the handle to the icon that represents the file and the index of the icon within the system image list. The handle is copied to the hIcon member of the structure specified by psfi, and the index is copied to the iIcon member.
        /// </summary>
        SHGFI_ICON = 0x000000100,

        /// <summary>
        /// Retrieve the display name for the file. The name is copied to the szDisplayName member of the structure specified in psfi. The returned display name uses the long file name, if there is one, rather than the 8.3 form of the file name.
        /// </summary>
        SHGFI_DISPLAYNAME = 0x000000200,

        /// <summary>
        /// Retrieve the string that describes the file's type. The string is copied to the szTypeName member of the structure specified in psfi.
        /// </summary>
        SHGFI_TYPENAME = 0x000000400,

        /// <summary>
        /// Retrieve the item attributes. The attributes are copied to the dwAttributes member of the structure specified in the psfi parameter. These are the same attributes that are obtained from IShellFolder::GetAttributesOf.
        /// </summary>
        SHGFI_ATTRIBUTES = 0x000000800,

        /// <summary>
        /// Retrieve the name of the file that contains the icon representing the file specified by pszPath, as returned by the IExtractIcon::GetIconLocation method of the file's icon handler. Also retrieve the icon index within that file. The name of the file containing the icon is copied to the szDisplayName member of the structure specified by psfi. The icon's index is copied to that structure's iIcon member.
        /// </summary>
        SHGFI_ICONLOCATION = 0x000001000,

        /// <summary>
        /// Retrieve the type of the executable file if pszPath identifies an executable file. The information is packed into the return value. This flag cannot be specified with any other flags.
        /// </summary>
        SHGFI_EXETYPE = 0x000002000,

        /// <summary>
        /// Retrieve the index of a system image list icon. If successful, the index is copied to the iIcon member of psfi. The return value is a handle to the system image list. Only those images whose indices are successfully copied to iIcon are valid. Attempting to access other images in the system image list will result in undefined behavior.
        /// </summary>
        SHGFI_SYSICONINDEX = 0x000004000,

        /// <summary>
        /// Modify SHGFI_ICON, causing the function to add the link overlay to the file's icon. The SHGFI_ICON flag must also be set.
        /// </summary>
        SHGFI_LINKOVERLAY = 0x000008000,

        /// <summary>
        /// Modify SHGFI_ICON, causing the function to blend the file's icon with the system highlight color. The SHGFI_ICON flag must also be set.
        /// </summary>
        SHGFI_SELECTED = 0x000010000,

        /// <summary>
        /// Modify SHGFI_ATTRIBUTES to indicate that the dwAttributes member of the SHFILEINFO structure at psfi contains the specific attributes that are desired. These attributes are passed to IShellFolder::GetAttributesOf. If this flag is not specified, 0xFFFFFFFF is passed to IShellFolder::GetAttributesOf, requesting all attributes. This flag cannot be specified with the SHGFI_ICON flag.
        /// </summary>
        SHGFI_ATTR_SPECIFIED = 0x000020000,

        /// <summary>
        /// Modify SHGFI_ICON, causing the function to retrieve the file's large icon. The SHGFI_ICON flag must also be set.
        /// </summary>
        SHGFI_LARGEICON = 0x000000000,

        /// <summary>
        /// Modify SHGFI_ICON, causing the function to retrieve the file's small icon. Also used to modify SHGFI_SYSICONINDEX, causing the function to return the handle to the system image list that contains small icon images. The SHGFI_ICON and/or SHGFI_SYSICONINDEX flag must also be set.
        /// </summary>
        SHGFI_SMALLICON = 0x000000001,

        /// <summary>
        /// Modify SHGFI_ICON, causing the function to retrieve the file's open icon. Also used to modify SHGFI_SYSICONINDEX, causing the function to return the handle to the system image list that contains the file's small open icon. A container object displays an open icon to indicate that the container is open. The SHGFI_ICON and/or SHGFI_SYSICONINDEX flag must also be set.
        /// </summary>
        SHGFI_OPENICON = 0x000000002,

        /// <summary>
        /// Modify SHGFI_ICON, causing the function to retrieve a Shell-sized icon. If this flag is not specified the function sizes the icon according to the system metric values. The SHGFI_ICON flag must also be set.
        /// </summary>
        SHGFI_SHELLICONSIZE = 0x000000004,

        /// <summary>
        /// Indicate that pszPath is the address of an ITEMIDLIST structure rather than a path name.
        /// </summary>
        SHGFI_PIDL = 0x000000008,

        /// <summary>
        /// Indicates that the function should not attempt to access the file specified by pszPath. Rather, it should act as if the file specified by pszPath exists with the file attributes passed in dwFileAttributes. This flag cannot be combined with the SHGFI_ATTRIBUTES, SHGFI_EXETYPE, or SHGFI_PIDL flags.
        /// </summary>
        SHGFI_USEFILEATTRIBUTES = 0x000000010,

        /// <summary>
        /// Version 5.0. Apply the appropriate overlays to the file's icon. The SHGFI_ICON flag must also be set.
        /// </summary>
        SHGFI_ADDOVERLAYS = 0x000000020,

        /// <summary>
        /// Version 5.0. Return the index of the overlay icon. The value of the overlay index is returned in the upper eight bits of the iIcon member of the structure specified by psfi. This flag requires that the SHGFI_ICON be set as well.
        /// </summary>
        SHGFI_OVERLAYINDEX = 0x000000040
    }

    // ReSharper restore InconsistentNaming
}