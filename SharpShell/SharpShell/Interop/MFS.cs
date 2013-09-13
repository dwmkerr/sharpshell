using System;

namespace SharpShell.Interop
{
    [Flags]
    internal enum MFS : uint
    {
        /// <summary>
        /// Indicates that the menu item is enabled and restored from a grayed state so that it can be selected.
        /// </summary>
        MFS_ENABLED = 0x00000000,
        MFS_UNCHECKED = 0x00000000,
        MFS_UNHILITE = 0x00000000,
        /// <summary>
        /// Indicates that the menu item is disabled and grayed so that it cannot be selected.
        /// </summary>
        MFS_GRAYED = 0x00000001,
        /// <summary>
        /// Indicates that the menu item is disabled, but not grayed, so it cannot be selected.
        /// </summary>
        MFS_DISABLED = 0x00000002,
        MFS_CHECKED = 0x00000008,
        MFS_HILITE = 0x00000080,
        MFS_DEFAULT = 0x00001000
    }
}