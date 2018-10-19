using System;

namespace SharpShell.Interop
{
// ReSharper disable InconsistentNaming

    /// <summary>
    /// A pointer to a value that contains flags that indicate the default column state. This parameter can include a combination of the following flags.
    /// </summary>
    [Flags]
    public enum SHCOLSTATEF
    {
        /// <summary>
        /// The default state.
        /// </summary>
        SHCOLSTATE_DEFAULT = 0,

        /// <summary>
        /// A string.
        /// </summary>
        SHCOLSTATE_TYPE_STR = 0x1,

        /// <summary>
        /// An integer.
        /// </summary>
        SHCOLSTATE_TYPE_INT = 0x2,

        /// <summary>
        /// A date.
        /// </summary>
        SHCOLSTATE_TYPE_DATE = 0x3,

        /// <summary>
        /// Should be shown by default in the Windows Explorer Details view.
        /// </summary>
        SHCOLSTATE_ONBYDEFAULT = 0x10,

        /// <summary>
        /// Recommends that the folder view extract column information asynchronously, on a background thread, because extracting this information can be time consuming.
        /// </summary>
        SHCOLSTATE_SLOW = 0x20,

        /// <summary>
        /// Provided by a handler, not the folder object.
        /// </summary>
        SHCOLSTATE_EXTENDED = 0x40,

        /// <summary>
        /// Not displayed in the shortcut menu, but listed in the More dialog box.
        /// </summary>
        SHCOLSTATE_SECONDARYUI = 0x80,

        /// <summary>
        /// Not displayed in the user interface.
        /// </summary>
        SHCOLSTATE_HIDDEN = 0x100,

        /// <summary>
        /// Uses default sorting rather than CompareIDs to get the sort order.
        /// </summary>
        SHCOLSTATE_PREFER_VARCMP = 0x200,

        /// <summary>
        /// 
        /// </summary>
        SHCOLSTATE_PREFER_FMTCMP = 0x400,
        
        /// <summary>
        /// 
        /// </summary>
        SHCOLSTATE_NOSORTBYFOLDERNESS = 0x800,

        /// <summary>
        /// 
        /// </summary>
        SHCOLSTATE_VIEWONLY = 0x10000,

        /// <summary>
        /// 
        /// </summary>
        SHCOLSTATE_BATCHREAD = 0x20000,

        /// <summary>
        /// 
        /// </summary>
        SHCOLSTATE_NO_GROUPBY = 0x40000,

        /// <summary>
        /// 
        /// </summary>
        SHCOLSTATE_FIXED_WIDTH = 0x1000,

        /// <summary>
        /// 
        /// </summary>
        SHCOLSTATE_NODPISCALE = 0x2000,

        /// <summary>
        /// 
        /// </summary>
        SHCOLSTATE_FIXED_RATIO = 0x4000,
    }
// ReSharper restore InconsistentNaming
}