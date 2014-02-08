using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
// ReSharper disable InconsistentNaming

    /// <summary>
    /// Specifies the FMTID/PID identifier of a column that will be displayed by the Windows Explorer Details view.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SHCOLUMNID
    {
        /// <summary>
        /// A property set format identifier or FMTID (a GUID). The Shell supports the storage, Shell details, and summary information property sets. Other property sets can be supported by particular folders.
        /// </summary>
        public Guid fmtid;

        /// <summary>
        /// The column's property identifier (PID).
        /// </summary>
        public uint pid;
    }

// ReSharper restore InconsistentNaming
}