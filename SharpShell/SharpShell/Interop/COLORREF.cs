using System.Drawing;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct COLORREF
    {
        public COLORREF(Color color)
        {
            Dword = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
        }

        public uint Dword;
        public Color Color
        {
            get
            {
                return Color.FromArgb(
                    (int)(0x000000FFU & Dword),
                    (int)(0x0000FF00U & Dword) >> 8,
                    (int)(0x00FF0000U & Dword) >> 16);
            }
        }
    }
}