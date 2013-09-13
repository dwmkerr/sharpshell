using System;

namespace SharpShell.Interop
{
    /// <summary>
    /// Flags for IShellFolder::GetAttributesOf.
    /// </summary>
    [Flags]
    public enum SFGAO : uint
    {
        /// <summary>
        /// Objects can be copied  (DROPEFFECT_COPY)
        /// </summary>
        SFGAO_CANCOPY = 0x1,

        /// <summary>
        /// Objects can be moved   (DROPEFFECT_MOVE)
        /// </summary>
        SFGAO_CANMOVE = 0x2,

        /// <summary>
        /// Objects can be linked  (DROPEFFECT_LINK)
        /// </summary>
        SFGAO_CANLINK = 0x4,

        /// <summary>
        /// Supports BindToObject(IID_IStorage)
        /// </summary>
        SFGAO_STORAGE = 0x00000008,

        /// <summary>
        /// Objects can be renamed
        /// </summary>
        SFGAO_CANRENAME = 0x00000010,

        /// <summary>
        /// Objects can be deleted
        /// </summary>
        SFGAO_CANDELETE = 0x00000020,

        /// <summary>
        ///  Objects have property sheets
        /// </summary>
        SFGAO_HASPROPSHEET = 0x00000040,

        /// <summary>
        /// Objects are drop target
        /// </summary>
        SFGAO_DROPTARGET = 0x00000100,

        /// <summary>
        /// Mask for capabilities.
        /// </summary>
        SFGAO_CAPABILITYMASK = 0x00000177,

        /// <summary>
        /// Object is encrypted (use alt color)
        /// </summary>
        SFGAO_ENCRYPTED = 0x00002000,

        /// <summary>
        /// 'Slow' object
        /// </summary>
        SFGAO_ISSLOW = 0x00004000,

        /// <summary>
        /// Ghosted icon
        /// </summary>
        SFGAO_GHOSTED = 0x00008000,

        /// <summary>
        /// Shortcut (link)
        /// </summary>
        SFGAO_LINK = 0x00010000,

        /// <summary>
        /// Shared
        /// </summary>
        SFGAO_SHARE = 0x00020000,

        /// <summary>
        /// Read-only
        /// </summary>
        SFGAO_READONLY = 0x00040000,

        /// <summary>
        /// Hidden object
        /// </summary>
        SFGAO_HIDDEN = 0x00080000,

        /// <summary>
        /// Display attribute mask.
        /// </summary>
        SFGAO_DISPLAYATTRMASK = 0x000FC000,

        /// <summary>
        ///  May contain children with SFGAO_FILESYSTEM
        /// </summary>
        SFGAO_FILESYSANCESTOR = 0x10000000,

        /// <summary>
        /// Support BindToObject(IID_IShellFolder)
        /// </summary>
        SFGAO_FOLDER = 0x20000000,

        /// <summary>
        /// Is a win32 file system object (file/folder/root)
        /// </summary>
        SFGAO_FILESYSTEM = 0x40000000,

        /// <summary>
        /// May contain children with SFGAO_FOLDER
        /// </summary>
        SFGAO_HASSUBFOLDER = 0x80000000,

        /// <summary>
        /// Contents mask.
        /// </summary>
        SFGAO_CONTENTSMASK = 0x80000000,

        /// <summary>
        /// Invalidate cached information
        /// </summary>
        SFGAO_VALIDATE = 0x01000000,

        /// <summary>
        /// Is this removeable media?
        /// </summary>
        SFGAO_REMOVABLE = 0x02000000,

        /// <summary>
        /// Object is compressed (use alt color)
        /// </summary>
        SFGAO_COMPRESSED = 0x04000000,

        /// <summary>
        /// Supports IShellFolder, but only implements CreateViewObject() (non-folder view)
        /// </summary>
        SFGAO_BROWSABLE = 0x08000000,

        /// <summary>
        ///  Is a non-enumerated object
        /// </summary>
        SFGAO_NONENUMERATED = 0x00100000,

        /// <summary>
        /// Should show bold in explorer tree
        /// </summary>
        SFGAO_NEWCONTENT = 0x00200000,

        /// <summary>
        /// Defunct
        /// </summary>
        SFGAO_CANMONIKER = 0x00400000,

        /// <summary>
        /// Defunct
        /// </summary>
        SFGAO_HASSTORAGE = 0x00400000,

        /// <summary>
        /// Supports BindToObject(IID_IStream)
        /// </summary>
        SFGAO_STREAM = 0x00400000,

        /// <summary>
        /// May contain children with SFGAO_STORAGE or SFGAO_STREAM
        /// </summary>
        SFGAO_STORAGEANCESTOR = 0x00800000,

        /// <summary>
        /// For determining storage capabilities, ie for open/save semantics
        /// </summary>
        SFGAO_STORAGECAPMASK = 0x70C50008
    }
}