using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
// ReSharper disable InconsistentNaming

    /// <summary>
    /// Indicates the number of menu items in each of the six menu groups of a menu shared between a container and an object server during an in-place editing session. This is the mechanism for building a shared menu.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct OLEMENUGROUPWIDTHS
    {
        /// <summary>
        /// An array whose elements contain the number of menu items in each of the six menu groups of a shared in-place editing menu. Each menu group can have any number of menu items. The container uses elements 0, 2, and 4 to indicate the number of menu items in its File, View, and Window menu groups. The object server uses elements 1, 3, and 5 to indicate the number of menu items in its Edit, Object, and Help menu groups.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)] public uint[] width;
    }

// ReSharper restore InconsistentNaming
}