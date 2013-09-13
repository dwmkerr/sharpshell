using System;

namespace SharpShell.Interop
{
    /// <summary>
    /// Flags for IShellIconOverlayIdentifer::GetOverlayInfo.
    /// </summary>
    [Flags]
    public enum ISIOI : uint
    {
        /// <summary>
        /// The path of the icon file is returned through pwszIconFile.
        /// </summary>
        ISIOI_ICONFILE = 0x00000001,
        
        /// <summary>
        /// There is more than one icon in pwszIconFile. The icon's index is returned through pIndex.
        /// </summary>
        ISIOI_ICONINDEX = 0x00000002
    }
}