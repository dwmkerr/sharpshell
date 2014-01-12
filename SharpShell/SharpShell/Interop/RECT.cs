using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }


        public int left, top, right, bottom;

        public int Width()
        {
            return right - left;
        }

        public int Height()
        {
            return bottom - top;
        }
    }
}