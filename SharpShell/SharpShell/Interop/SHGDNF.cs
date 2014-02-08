using System;

namespace SharpShell.Interop
{
    // ReSharper disable InconsistentNaming
   
    /// <summary>
    /// Defines the values used with the IShellFolder::GetDisplayNameOf and IShellFolder::SetNameOf methods to specify the type of file or folder names used by those methods.
    /// </summary>
    [Flags]
    public enum SHGDNF
    {
        /// <summary>
        /// When not combined with another flag, return the parent-relative name that identifies the item, suitable for displaying to the user. This name often does not include extra information such as the file name extension and does not need to be unique. This name might include information that identifies the folder that contains the item. For instance, this flag could cause IShellFolder::GetDisplayNameOf to return the string "username (on Machine)" for a particular user's folder.
        /// </summary>
        SHGDN_NORMAL = 0x0000,

        /// <summary>
        /// The name is relative to the folder from which the request was made. This is the name display to the user when used in the context of the folder. For example, it is used in the view and in the address bar path segment for the folder. This name should not include disambiguation information—for instance "username" instead of "username (on Machine)" for a particular user's folder.
        /// Use this flag in combinations with SHGDN_FORPARSING and SHGDN_FOREDITING.
        /// </summary>
        SHGDN_INFOLDER = 0x0001,
        /// <summary>
        /// The name is used for in-place editing when the user renames the item.
        /// </summary>
        SHGDN_FOREDITING = 0x1000,
        /// <summary>
        /// The name is displayed in an address bar combo box.
        /// </summary>
        SHGDN_FORADDRESSBAR = 0x4000,
        /// <summary>
        /// The name is used for parsing. That is, it can be passed to IShellFolder::ParseDisplayName to recover the object's PIDL. The form this name takes depends on the particular object. When SHGDN_FORPARSING is used alone, the name is relative to the desktop. When combined with SHGDN_INFOLDER, the name is relative to the folder from which the request was made.
        /// </summary>
        SHGDN_FORPARSING = 0x8000,
    }

    // ReSharper restore InconsistentNaming
}