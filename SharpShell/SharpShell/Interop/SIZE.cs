using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// The SIZE structure specifies the width and height of a rectangle.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        /// <summary>
        /// Specifies the rectangle's width. The units depend on which function uses this.
        /// </summary>
        public int cx;

        /// <summary>
        /// Specifies the rectangle's height. The units depend on which function uses this.
        /// </summary>
        public int cy;

        public SIZE(int cx, int cy)
        {
            this.cx = cx;
            this.cy = cy;
        }
    }
}