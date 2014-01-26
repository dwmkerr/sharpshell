using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Defines information used by AssocCreateForClasses to retrieve an IQueryAssociations interface for a given file association.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ASSOCIATIONELEMENT
    {
        /// <summary>
        /// Where to obtain association data and the form the data is stored in. One of the following values from the ASSOCCLASS enumeration.
        /// </summary>
        public ASSOCCLASS ac;

        /// <summary>
        /// A registry key that specifies a class that contains association information.
        /// </summary>
        public IntPtr hkClass;

        /// <summary>
        /// A pointer to the name of a class that contains association information.
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)] public string pszClass;
    }

    // ReSharper restore InconsistentNaming
}