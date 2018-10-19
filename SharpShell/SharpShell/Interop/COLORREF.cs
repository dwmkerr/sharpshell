using System.Drawing;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// The COLORREF value is used to specify an RGB color.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct COLORREF
    {
        /// <summary>
        /// Initializes a new instance of a <see cref="COLORREF"/> from a <see cref="Color"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        public COLORREF(Color color)
        {
            Dword = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
        }

        /// <summary>
        /// The DWORD representation of the color.
        /// </summary>
        public uint Dword;

        /// <summary>
        /// Gets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color Color => Color.FromArgb(
            (int)(0x000000FFU & Dword),
            (int)(0x0000FF00U & Dword) >> 8,
            (int)(0x00FF0000U & Dword) >> 16);
    }
}