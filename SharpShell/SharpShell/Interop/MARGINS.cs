using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Returned by the GetThemeMargins function to define the margins of windows that have visual styles applied.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MARGINS
    {
        /// <summary>
        /// Width of left border that retains its size
        /// </summary>
        public Int32 cxLeftWidth;

        /// <summary>
        /// Width of right border that retains its size
        /// </summary>
        public Int32 cxRightWidth;

        /// <summary>
        /// Height of top border that retains its size
        /// </summary>
        public Int32 cyTopHeight;

        /// <summary>
        /// Height of bottom border that retains its size
        /// </summary>
        public Int32 cyBottomHeight;
    }
}