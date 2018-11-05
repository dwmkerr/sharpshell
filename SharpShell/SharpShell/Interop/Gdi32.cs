using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Imports from gdi32.dll.
    /// </summary>
    public static class Gdi32
    {
        [DllImport("gdi32.dll", SetLastError = true)]
        internal static extern IntPtr CreateDIBSection(IntPtr hdc, [In] IntPtr pbmi,
            uint pila, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);
        
        [DllImport("gdi32.dll", SetLastError = true)]
        internal static extern IntPtr CreateDIBSection(IntPtr hdc, [In] BITMAPV5HEADER pbmi,
            uint pila, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport("gdi32.dll", SetLastError = true)]
        internal static extern IntPtr CreateDIBSection(IntPtr hdc, [In]ref  BITMAPINFOHEADER pbmi,
            uint pila, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport("gdi32.dll", SetLastError = true)]
        internal static extern IntPtr CreateDIBSection(IntPtr hdc, [In]ref  BITMAPINFO pbmi,
            uint pila, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport("gdi32.dll")]
        internal static extern IntPtr CreateDIBitmap(IntPtr hdc, [In] IntPtr lpbmih, uint fdwInit, IntPtr lpbInit, [In] IntPtr lpbmi, uint fuUsage);

        [DllImport("gdi32.dll")]
        internal static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll", SetLastError = true)]
        internal static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("user32.dll")]
        internal static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        internal static extern int SaveDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        internal static extern bool RestoreDC(IntPtr hdc, int nSavedDC);

        [DllImport("gdi32.dll")]
        internal static extern uint SetBkColor(IntPtr hdc, int crColor);

        [DllImport("gdi32.dll", EntryPoint = "ExtTextOutW")]
        internal static extern bool ExtTextOut(IntPtr hdc, int X, int Y, uint fuOptions,
            [In] ref RECT lprc, [MarshalAs(UnmanagedType.LPWStr)] string lpString,
            uint cbCount, [In] int[] lpDx);

        /// <summary>
        ///        Deletes a logical pen, brush, font, bitmap, region, or palette, freeing all system resources associated with the object. After the object is deleted, the specified handle is no longer valid.
        /// </summary>
        /// <param name="hObject">A handle to a logical pen, brush, font, bitmap, region, or palette.</param>
        /// <returns>
        ///        If the function succeeds, the return value is <c>true</c>. If the specified handle is not valid or is currently selected into a DC, the return value is <c>false</c>.
        /// </returns>
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        [DllImport("gdi32.dll", EntryPoint = "SelectObject", SetLastError = true)]
        internal static extern IntPtr SelectObject([In] IntPtr hdc, [In] IntPtr hgdiobj);

        /// <summary>
        ///        Deletes the specified device context (DC).
        /// </summary>
        /// <param name="hdc">A handle to the device context.</param>
        /// <returns>If the function succeeds, the return value is <c>true</c>. If the function fails, the return value is <c>false</c>.</returns>
        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        internal static extern bool DeleteDC([In] IntPtr hdc);
        
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool DrawIconEx(IntPtr hdc, int xLeft, int yTop, IntPtr hIcon,
            int cxWidth, int cyHeight, int istepIfAniCur, IntPtr hbrFlickerFreeDraw,
            int diFlags);

        /// <summary>
        ///        Retrieves the bits of the specified compatible bitmap and copies them into a buffer as a DIB using the specified format.
        /// </summary>
        /// <param name="hdc">A handle to the device context.</param>
        /// <param name="hbmp">A handle to the bitmap. This must be a compatible bitmap (DDB).</param>
        /// <param name="uStartScan">The first scan line to retrieve.</param>
        /// <param name="cScanLines">The number of scan lines to retrieve.</param>
        /// <param name="lpvBits">A pointer to a buffer to receive the bitmap data. If this parameter is <see cref="IntPtr.Zero"/>, the function passes the dimensions and format of the bitmap to the <see cref="BITMAPINFO"/> structure pointed to by the <paramref name="lpbi"/> parameter.</param>
        /// <param name="lpbi">A pointer to a <see cref="BITMAPINFO"/> structure that specifies the desired format for the DIB data.</param>
        /// <param name="uUsage">The format of the bmiColors member of the <see cref="BITMAPINFO"/> structure. It must be one of the following values.</param>
        /// <returns>If the lpvBits parameter is non-NULL and the function succeeds, the return value is the number of scan lines copied from the bitmap.
        /// If the lpvBits parameter is NULL and GetDIBits successfully fills the <see cref="BITMAPINFO"/> structure, the return value is nonzero.
        /// If the function fails, the return value is zero.
        /// This function can return the following value: ERROR_INVALID_PARAMETER (87 (0×57))</returns>
        [DllImport("gdi32.dll", EntryPoint = "GetDIBits")]
        internal static extern int GetDIBits([In] IntPtr hdc, [In] IntPtr hbmp, uint uStartScan, uint cScanLines, [Out] byte[] lpvBits, ref BITMAPINFO lpbi, uint uUsage);

        [DllImport("gdi32.dll", EntryPoint = "GetDIBits")]
        internal static extern int GetDIBits([In] IntPtr hdc, [In] IntPtr hbmp, uint uStartScan, uint cScanLines, ref IntPtr lpvBits, ref BITMAPINFO lpbi, uint uUsage);
    }
}