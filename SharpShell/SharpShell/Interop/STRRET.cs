using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    // ReSharper disable InconsistentNaming
    
    /// <summary>
    /// Contains strings returned from the IShellFolder interface methods.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct STRRET
    {
        /// <summary>
        /// Struct used internally to fake a C union.
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size = 520)]
        public struct STRRETUNION
        {
            /// <summary>
            /// A pointer to the string. This memory must be allocated with CoTaskMemAlloc. It is the calling application's responsibility to free this memory with CoTaskMemFree when it is no longer needed.
            /// </summary>
            [FieldOffset(0)]
            public IntPtr pOleStr;

            /// <summary>
            /// The buffer to receive the display name.
            /// </summary>
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
            public string cStr;

            /// <summary>
            /// The offset into the item identifier list.
            /// </summary>
            [FieldOffset(0)]
            public uint uOffset;

        }

        /// <summary>
        /// A value that specifies the desired format of the string. This can be one of the following values.
        /// </summary>
        public enum STRRETTYPE
        {
            /// <summary>
            /// The string is at the address specified by pOleStr member.
            /// </summary>
            STRRET_WSTR = 0x0000,
            /// <summary>
            /// The uOffset member value indicates the number of bytes from the beginning of the item identifier list where the string is located.
            /// </summary>
            STRRET_OFFSET = 0x0001,

            /// <summary>
            /// The string is returned in the cStr member.
            /// </summary>
            STRRET_CSTR = 0x0002
        }

        /// <summary>
        /// A value that specifies the desired format of the string.
        /// </summary>
        public STRRETTYPE uType;

        /// <summary>
        /// The string data.
        /// </summary>
        public STRRETUNION data;
    }

    // ReSharper restore InconsistentNaming
}