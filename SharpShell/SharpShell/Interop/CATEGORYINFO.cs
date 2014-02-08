using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Describes a component category.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
    public struct CATEGORYINFO
    {
        /// <summary>
        /// The category identifier for the component.
        /// </summary>
        public Guid catid;

        /// <summary>
        /// The locale identifier. See Language Identifier Constants and Strings.
        /// </summary>
        public uint lcid;

        /// <summary>
        /// The description of the category (cannot exceed 128 characters).
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szDescription;
    }
}