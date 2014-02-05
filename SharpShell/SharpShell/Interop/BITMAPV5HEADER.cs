using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPINFOHEADER
    {
        public uint biSize;
        public int biWidth;
        public int biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        public uint biCompression;
        public uint biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct BITMAPV5HEADER
    {
        [FieldOffset(0)]
        public uint bV5Size;
        [FieldOffset(4)]
        public int bV5Width;
        [FieldOffset(8)]
        public int bV5Height;
        [FieldOffset(12)]
        public ushort bV5Planes;
        [FieldOffset(14)]
        public ushort bV5BitCount;
        [FieldOffset(16)]
        public uint bV5Compression;
        [FieldOffset(20)]
        public uint bV5SizeImage;
        [FieldOffset(24)]
        public int bV5XPelsPerMeter;
        [FieldOffset(28)]
        public int bV5YPelsPerMeter;
        [FieldOffset(32)]
        public uint bV5ClrUsed;
        [FieldOffset(36)]
        public uint bV5ClrImportant;
        [FieldOffset(40)]
        public uint bV5RedMask;
        [FieldOffset(44)]
        public uint bV5GreenMask;
        [FieldOffset(48)]
        public uint bV5BlueMask;
        [FieldOffset(52)]
        public uint bV5AlphaMask;
        [FieldOffset(56)]
        public uint bV5CSType;
        [FieldOffset(60)]
        public CIEXYZTRIPLE bV5Endpoints;
        [FieldOffset(96)]
        public uint bV5GammaRed;
        [FieldOffset(100)]
        public uint bV5GammaGreen;
        [FieldOffset(104)]
        public uint bV5GammaBlue;
        [FieldOffset(108)]
        public uint bV5Intent;
        [FieldOffset(112)]
        public uint bV5ProfileData;
        [FieldOffset(116)]
        public uint bV5ProfileSize;
        [FieldOffset(120)]
        public uint bV5Reserved;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct CIEXYZ
    {
        public uint ciexyzX; //FXPT2DOT30
        public uint ciexyzY; //FXPT2DOT30
        public uint ciexyzZ; //FXPT2DOT30
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CIEXYZTRIPLE
    {
        public CIEXYZ ciexyzRed;
        public CIEXYZ ciexyzGreen;
        public CIEXYZ ciexyzBlue;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BITFIELDS
    {
        public uint BlueMask;
        public uint GreenMask;
        public uint RedMask;
    }

    internal enum BI : uint
    {
        BI_RGB = 0,
        BI_RLE8 = 1,
        BI_RLE4 = 2,
        BI_BITFIELDS = 3,
        BI_JPEG = 4,
        BI_PNG = 5
    }

    internal static class Gdi32
    {
        [DllImport("gdi32.dll", SetLastError = true)]
public static extern IntPtr CreateDIBSection(IntPtr hdc, [In] IntPtr pbmi,
   uint pila, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);


        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, [In] BITMAPV5HEADER pbmi,
   uint pila, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, [In]ref  BITMAPINFOHEADER pbmi,
   uint pila, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, [In]ref  BITMAPINFO pbmi,
   uint pila, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDIBitmap(IntPtr hdc, [In] IntPtr lpbmih, uint fdwInit, IntPtr lpbInit, [In] IntPtr lpbmi, uint fuUsage);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("user32.dll")]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern int SaveDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern bool RestoreDC(IntPtr hdc, int nSavedDC);

        [DllImport("gdi32.dll")]
        public static extern uint SetBkColor(IntPtr hdc, int crColor);

        [DllImport("gdi32.dll", EntryPoint = "ExtTextOutW")]
        public static extern bool ExtTextOut(IntPtr hdc, int X, int Y, uint fuOptions,
           [In] ref RECT lprc, [MarshalAs(UnmanagedType.LPWStr)] string lpString,
           uint cbCount, [In] int[] lpDx);

        [DllImport("gdi32.dll", EntryPoint = "SelectObject", SetLastError = true)]
        public static extern IntPtr SelectObject([In] IntPtr hdc, [In] IntPtr hgdiobj);
        /// <summary>
        ///        Deletes the specified device context (DC).
        /// </summary>
        /// <param name="hdc">A handle to the device context.</param>
        /// <returns>If the function succeeds, the return value is <c>true</c>. If the function fails, the return value is <c>false</c>.</returns>
        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern bool DeleteDC([In] IntPtr hdc);


        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DrawIconEx(IntPtr hdc, int xLeft, int yTop, IntPtr hIcon,
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
        public static extern int GetDIBits([In] IntPtr hdc, [In] IntPtr hbmp, uint uStartScan, uint cScanLines, [Out] byte[] lpvBits, ref BITMAPINFO lpbi, uint uUsage);
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    struct BITMAPINFO
    {
        /// <summary>
        /// A BITMAPINFOHEADER structure that contains information about the dimensions of color format.
        /// </summary>
        public BITMAPINFOHEADER bmiHeader;

        /// <summary>
        /// An array of RGBQUAD. The elements of the array that make up the color table.
        /// </summary>
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 1, ArraySubType = UnmanagedType.Struct)]
        public RGBQUAD[] bmiColors;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RGBQUAD
    {
        public byte rgbBlue;
        public byte rgbGreen;
        public byte rgbRed;
        public byte rgbReserved;
    }
}