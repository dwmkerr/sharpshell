using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SharpShell.Interop;

namespace SharpShell.SharpContextMenu
{
    public static class PARGB32
    {
        public static bool IsSupported()
        {
            return false;
        }

        public static IntPtr CreatePARGB32HBitmap(IntPtr hIcon, Size size)
        {
            IntPtr hDCDest = Gdi32.CreateCompatibleDC(IntPtr.Zero);
            if (hDCDest == IntPtr.Zero)
                return IntPtr.Zero;

            IntPtr bits;
            IntPtr hBitmap;
            if (!Create32BitHBITMAP(hDCDest, size, out bits, out hBitmap))
            {
                Gdi32.DeleteDC(hDCDest);
                return IntPtr.Zero;
            }
            IntPtr hbmpOld = Gdi32.SelectObject(hDCDest, hBitmap);
            if (hbmpOld == IntPtr.Zero)
            {
                Gdi32.DeleteDC(hDCDest);
                return IntPtr.Zero;
            }

            var bfAlpha = new BLENDFUNCTION
                              {
                                  BlendOp = AC_SRC_OVER,
                                  BlendFlags = 0,
                                  SourceConstantAlpha = 255,
                                  AlphaFormat = AC_SRC_ALPHA
                              };

            var paintParams = new BP_PAINTPARAMS();
            paintParams.cbSize = (uint) Marshal.SizeOf(paintParams);
            paintParams.dwFlags = BPPF_ERASE;
            paintParams.pBlendFunction = bfAlpha;
            IntPtr hdcBuffer;
            var rcIcon = new RECT(0, 0, size.Width, size.Height);

            //  *********** we're here. todo pass in paint params
            //  Note: Currently, marshalling the paint params directly or allocating
            //  them as an IntPtr both lead to null results - only not passing gives us a buffer....
            var res = Uxtheme.BufferedPaintInit();
            var memory = Marshal.AllocHGlobal((int) paintParams.cbSize);
            Marshal.StructureToPtr(paintParams, memory, false);
            IntPtr hPaintBuffer = Uxtheme.BeginBufferedPaint(hDCDest, ref rcIcon,
                                                     BP_BUFFERFORMAT.BPBF_DIB, IntPtr.Zero, out hdcBuffer);
            Marshal.FreeHGlobal(memory);
            var er = Marshal.GetLastWin32Error();
            if (hPaintBuffer != IntPtr.Zero)
            {
                if (Gdi32.DrawIconEx(hdcBuffer, 0, 0, hIcon, size.Width, size.Height, 0, IntPtr.Zero,
                                     (int) DI_NORMAL))
                {
                    // If icon did not have an alpha channel, we need to convert buffer to PARGB.
                    ConvertBufferToPARGB32(hPaintBuffer, hDCDest, hIcon, size);
                }

                // This will write the buffer contents to the destination bitmap.
                Uxtheme.EndBufferedPaint(hPaintBuffer, true);
            }
            Gdi32.SelectObject(hDCDest, hbmpOld);
            Gdi32.DeleteDC(hDCDest);
            res = Uxtheme.BufferedPaintUnInit();

            return hBitmap;

        }




        


        private const byte AC_SRC_OVER = 0x00;
        private const byte AC_SRC_ALPHA = 0x01;
        private const uint BPPF_ERASE = 0x0001;
        private const uint DI_NORMAL = 0x0003;


        private static bool Create32BitHBITMAP(IntPtr hdc, Size size, out IntPtr bits, out IntPtr hBitmap)
        {
            bits = IntPtr.Zero;
            hBitmap = IntPtr.Zero;
            BITMAPINFO bi = new BITMAPINFO();
            bi.bmiHeader = new BITMAPINFOHEADER();
            bi.bmiHeader.biSize = (uint) Marshal.SizeOf(typeof (BITMAPINFOHEADER));
            bi.bmiHeader.biPlanes = 1;
            bi.bmiHeader.biCompression = BI_RGB;
            bi.bmiHeader.biWidth = size.Width;
            bi.bmiHeader.biHeight = size.Height;
            bi.bmiHeader.biBitCount = 32;

            hBitmap = Gdi32.CreateDIBSection(hdc, ref bi, DIB_RGB_COLORS, out bits, IntPtr.Zero, 0);

            return true;
        }

        private const int BI_RGB = 0;
        private const uint DIB_RGB_COLORS = 0;


        private unsafe static bool ConvertBufferToPARGB32(IntPtr hPaintBuffer, IntPtr hdc, IntPtr hicon, Size sizIcon)
        {
            IntPtr prgbQuad;
            int cxRow;
            var hr = Uxtheme.GetBufferedPaintBits(hPaintBuffer, out prgbQuad, out cxRow);
            if (hr != WinError.S_OK)
                return false;

            UInt32* pargb = (UInt32*)prgbQuad.ToPointer();
            if (!HasAlpha(pargb, sizIcon, cxRow))
            {
                ICONINFO info;
                if (User32.GetIconInfo(hicon, out info))
                {
                    if (info.MaskBitmap != IntPtr.Zero)
                    {
                        var result = ConvertToPARGB32(hdc, pargb, info.MaskBitmap, sizIcon, cxRow);
                    }

                    Gdi32.DeleteObject(info.ColorBitmap);
                    Gdi32.DeleteObject(info.MaskBitmap);
                }
            }

            return true;
        }

        private unsafe static bool HasAlpha(UInt32* pargb, Size sizImage, int cxRow)
        {
            UInt32 cxDelta = (uint) (cxRow - sizImage.Width);
            for (var y = sizImage.Height; y != 0; --y)
            {
                for (var x = sizImage.Width; x != 0; --x)
                {
                    if ((*pargb++ & 0xFF000000) != 0)
                    {
                        return true;
                    }
                }

                pargb += cxDelta;
            }

            return false;
        }

        private unsafe static bool ConvertToPARGB32(IntPtr hdc, UInt32* pargb, IntPtr hbmp, Size sizImage, int cxRow)
        {
            BITMAPINFO bmi = new BITMAPINFO();
            bmi.bmiHeader = new BITMAPINFOHEADER();
            bmi.bmiHeader.biSize = (uint) Marshal.SizeOf(typeof (BITMAPINFOHEADER));
            bmi.bmiHeader.biPlanes = 1;
            bmi.bmiHeader.biCompression = BI_RGB;
            bmi.bmiHeader.biWidth = sizImage.Width;
            bmi.bmiHeader.biHeight = sizImage.Height;
            bmi.bmiHeader.biBitCount = 32;

            IntPtr hHeap = Kernel32.GetProcessHeap();
            void* pvBits = Kernel32.HeapAlloc(hHeap, 0, new UIntPtr((uint)(bmi.bmiHeader.biWidth * 4 * bmi.bmiHeader.biHeight))).ToPointer();
            if (pvBits != (void*)0)
            {
                var ptr = new IntPtr(pvBits);
                if (Gdi32.GetDIBits(hdc, hbmp, 0, (uint) bmi.bmiHeader.biHeight, ref ptr,ref  bmi, DIB_RGB_COLORS) ==
                    bmi.bmiHeader.biHeight)
                {
                    UInt32 cxDelta = (uint)(cxRow - bmi.bmiHeader.biWidth);
                    UInt32* pargbMask = (uint*)pvBits;

                    for (UInt32 y = (UInt32) bmi.bmiHeader.biHeight; y != 0; --y)
                    {
                        for (UInt32 x = (UInt32) bmi.bmiHeader.biWidth; x != 0; --x)
                        {
                            if ((*pargbMask++) != 0)
                            {
                                // transparent pixel
                                *pargb++ = 0;
                            }
                            else
                            {
                                // opaque pixel
                                *pargb++ |= 0xFF000000;
                            }
                        }

                        pargb += cxDelta;
                    }
                }

                Kernel32.HeapFree(hHeap, 0, new IntPtr(pvBits));
            }

            return true;
        }
    }
}