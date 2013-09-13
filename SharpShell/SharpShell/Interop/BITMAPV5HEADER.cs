using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
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

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDIBitmap(IntPtr hdc, [In] IntPtr lpbmih, uint fdwInit, IntPtr lpbInit, [In] IntPtr lpbmi, uint fuUsage);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("user32.dll")]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);
    }
}