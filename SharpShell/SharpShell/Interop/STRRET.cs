using System;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    // ReSharper disable InconsistentNaming
    
    /// <summary>
    /// Contains strings returned from the IShellFolder interface methods.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size=272)]
    public struct STRRET
    {
        /// <summary>
        /// Creates a unicode <see cref="STRRET"/> from a string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>A unicode <see cref="STRRET"/> allocated on the shell allocator.</returns>
        public static STRRET CreateUnicode(string str)
        {
            //  Return a unicode string. In this case, data is just a pointer to the 
            //  unicode string.
            return new STRRET
            {
                uType = STRRETTYPE.STRRET_WSTR,
                data = Marshal.StringToCoTaskMemUni(str)
            };
        }

        /// <summary>
        /// Gets the actual string value of a STRRET.
        /// </summary>
        /// <returns>The string represented by the STRRET.</returns>
        /// <exception cref="System.NotImplementedException">
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public string GetStringValue()
        {
            //  TODO: one day support all string types.
            switch (uType)
            {
                case STRRETTYPE.STRRET_WSTR:
                    return Marshal.PtrToStringUni(data);
                case STRRETTYPE.STRRET_OFFSET:
                    throw new NotImplementedException();
                case STRRETTYPE.STRRET_CSTR:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
        public IntPtr data;
    }

    // ReSharper restore InconsistentNaming
}