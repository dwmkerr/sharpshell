using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// The RECT structure defines the coordinates of the upper-left and lower-right corners of a rectangle.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RECT"/> struct.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        public RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        /// <summary>
        /// The x-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int left;

        /// <summary>
        /// The y-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int top;

        /// <summary>
        /// The x-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int right;

        /// <summary>
        /// The y-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int bottom;

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <returns>The width.</returns>
        public int Width()
        {
            return right - left;
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <returns>The height.</returns>
        public int Height()
        {
            return bottom - top;
        }

        /// <summary>
        /// Offsets the rectangle.
        /// </summary>
        /// <param name="x">The x offset.</param>
        /// <param name="y">The y offset.</param>
        public void Offset(int x, int y)
        {
            left += x;
            right += x;
            top += y;
            bottom += y;
        }

        /// <summary>
        /// Sets the rectangle coordinates.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        public void Set(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        /// <summary>
        /// Determines whether this rectangle is empty.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this rectangle is empty; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEmpty()
        {
            return Width() == 0 && Height() == 0;
        }
    }
}