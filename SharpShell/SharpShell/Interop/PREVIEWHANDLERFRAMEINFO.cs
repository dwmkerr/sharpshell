using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Accelerator table structure. Used by IPreviewHandlerFrame::GetWindowContext.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PREVIEWHANDLERFRAMEINFO
    {
        /// <summary>
        /// A handle to the accelerator table.
        /// </summary>
        public IntPtr haccel;

        /// <summary>
        /// The number of entries in the accelerator table.
        /// </summary>
        public uint cAccelEntries;
    }
}