using System;

namespace SharpShell.Interop
{
    /// <summary>
    /// GetInfoTip flags.
    /// </summary>
    [Flags]
    public enum QITIPF
    {
        /// <summary>
        /// No special handling.
        /// </summary>
        QITIPF_DEFAULT = 0x00000000,

        /// <summary>
        /// Provide the name of the item in ppwszTip rather than the info tip text.
        /// </summary>
        QITIPF_USENAME = 0x00000001,

        /// <summary>
        /// If the item is a shortcut, retrieve the info tip text of the shortcut rather than its target.
        /// </summary>
        QITIPF_LINKNOTARGET = 0x00000002,

        /// <summary>
        /// If the item is a shortcut, retrieve the info tip text of the shortcut's target.
        /// </summary>
        QITIPF_LINKUSETARGET = 0x00000004,

        /// <summary>
        /// Search the entire namespace for the information. This value can result in a delayed response time.
        /// </summary>
        QITIPF_USESLOWTIP = 0x00000008,

        /// <summary>
        /// Windows Vista and later. Put the info tip on a single line.
        /// </summary>
        QITIPF_SINGLELINE = 0x00000010
    }
}