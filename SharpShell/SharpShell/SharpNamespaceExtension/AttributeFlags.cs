using System;

namespace SharpShell.SharpNamespaceExtension
{
    /// <summary>
    /// Flags that can be specified for <see cref="IShellNamespaceItem"/> items.
    /// </summary>
    [Flags]
    public enum AttributeFlags : uint
    {
        /// <summary>
        /// Indicates that the item can be copied.
        /// </summary>
        CanByCopied = 0x1,

        /// <summary>
        /// Indicates that the item can be moved.
        /// </summary>
        CanBeMoved = 0x2,

        /// <summary>
        /// Indicates that the item can be linked to.
        /// </summary>
        CanBeLinked = 0x4,

        /// <summary>
        /// Indicates that the item is storable and can be bound to the storage system.
        /// </summary>
        IsStorage = 0x00000008,

        /// <summary>
        /// Indicates that the object can be renamed.
        /// </summary>
        CanBeRenamed = 0x00000010,

        /// <summary>
        /// Indicates that the object can be deleted.
        /// </summary>
        CanBeDeleted = 0x00000020,

        /// <summary>
        /// Indicates that the object has property sheets.
        /// </summary>
        HasPropertySheets = 0x00000040,

        /// <summary>
        /// Indicates that the object is a drop target.
        /// </summary>
        IsDropTarget = 0x00000100,

        /// <summary>
        /// Indicates that the object is encrypted and is shown in an alternate colour.
        /// </summary>
        IsEncrypted = 0x00002000,

        /// <summary>
        /// Indicates the object is 'slow' and should be treated by the shell as such.
        /// </summary>
        IsSlow = 0x00004000,

        /// <summary>
        /// Indicates that the icon should be shown with a 'ghosted' icon.
        /// </summary>
        IsGhosted = 0x00008000,

        /// <summary>
        /// Indicates that the item is a link or shortcut.
        /// </summary>
        IsLink = 0x00010000,

        /// <summary>
        /// Indicates that the item is shared.
        /// </summary>
        IsShared = 0x00020000,

        /// <summary>
        /// Indicates that the item is read only.
        /// </summary>
        IsReadOnly = 0x00040000,

        /// <summary>
        /// Indicates that the item is hidden.
        /// </summary>
        IsHidden = 0x00080000,

        /// <summary>
        /// Indicates that item may contain children that are part of the filesystem.
        /// </summary>
        IsFileSystemAncestor = 0x10000000,

        /// <summary>
        /// Indicates that this item is a shell folder, i.e. it can contain 
        /// other shell items.
        /// </summary>
        IsFolder = 0x20000000,

        /// <summary>
        /// Indicates that the object is part of the Windows file system.
        /// </summary>
        IsFileSystem = 0x40000000,

        /// <summary>
        /// Indicates that the item may contain sub folders.
        /// </summary>
        MayContainSubFolders = 0x80000000,

        /// <summary>
        /// Indicates that the object is volatile, and the shell shouldn't cache
        /// data relating to it.
        /// </summary>
        IsVolatile = 0x01000000,

        /// <summary>
        /// Indicates that the object is removable media.
        /// </summary>
        IsRemovableMedia = 0x02000000,

        /// <summary>
        /// Indicates that the object is compressed and should be shown in an alternative colour.
        /// </summary>
        IsCompressed = 0x04000000,

        /// <summary>
        /// The item has children and is browsed with the default explorer UI.
        /// </summary>
        IsBrowsable = 0x08000000,

        /// <summary>
        /// A hint to explorer to show the item bold as it has or is new content.
        /// </summary>
        HasOrIsNewContent = 0x00200000,

        /// <summary>
        /// Indicates that the item is a stream and supports binding to a stream.
        /// </summary>
        IsStream = 0x00400000,

        /// <summary>
        /// Indicates that the item can contain storage items, either streams or files.
        /// </summary>
        IsStorageAncestor = 0x00800000,
    }
}