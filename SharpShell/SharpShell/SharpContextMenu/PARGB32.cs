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

        private static IntPtr BitmapToIcon(IntPtr hbitmap, int width, int height)
        {
            ICONINFO ii = new ICONINFO();
            ii.IsIcon = true;
            ii.ColorBitmap = hbitmap;
            ii.MaskBitmap = Gdi32.CreateCompatibleBitmap(User32.GetDC(IntPtr.Zero), width, height);

            var icon = CreateIconIndirect(ref ii);
            DeleteObject(ii.MaskBitmap);
            return icon;
        }

        public static IntPtr CreatePARGB32HBitmap(Image source)
        {
            IntPtr hDCDest = Gdi32.CreateCompatibleDC(IntPtr.Zero);
            if (hDCDest == IntPtr.Zero)
                return IntPtr.Zero;

            IntPtr bits;
            IntPtr hBitmap;
            if (!Create32BitHBITMAP(hDCDest, source.Size, out bits, out hBitmap))
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
            paintParams.cbSize = (uint)Marshal.SizeOf(paintParams);
            paintParams.dwFlags = BPPF_ERASE;
            paintParams.pBlendFunction = bfAlpha;
            IntPtr hdcBuffer;
            var rcIcon = new RECT(0, 0, source.Width, source.Height);
            IntPtr hPaintBuffer = BeginBufferedPaint(hDCDest, ref rcIcon,
                                                     BP_BUFFERFORMAT.BPBF_DIB, paintParams, out hdcBuffer);
            if(hPaintBuffer != IntPtr.Zero)
            {
                var hIcon = BitmapToIcon(source.GetHBitmap());

                if (Gdi32.DrawIconEx(hdcBuffer, 0, 0, hIcon, source.Width, source.Height, 0, IntPtr.Zero, (int)DI_NORMAL))
                {
                    // If icon did not have an alpha channel, we need to convert buffer to PARGB.
                    ConvertBufferToPARGB32(hPaintBuffer, hdcDest, hIcon, sizIcon);
                }

                // This will write the buffer contents to the destination bitmap.
                EndBufferedPaint(hPaintBuffer, true);
            }
            Gdi32.SelectObject(hDCDest, hbmpOld);
            Gdi32.DeleteDC(hDCDest);

            return hBitmap;

        }

        [DllImport("uxtheme.dll")]
        public static extern IntPtr BeginBufferedPaint(IntPtr hdc, ref RECT prcTarget, BP_BUFFERFORMAT dwFormat, BP_PAINTPARAMS pPaintParams, out IntPtr phdc);

        [DllImport("uxtheme.dll")]
        public static extern IntPtr EndBufferedPaint(IntPtr hBufferedPaint, bool fUpdateTarget);
        /// <summary>
        ///        Deletes a logical pen, brush, font, bitmap, region, or palette, freeing all system resources associated with the object. After the object is deleted, the specified handle is no longer valid.
        /// </summary>
        /// <param name="hObject">A handle to a logical pen, brush, font, bitmap, region, or palette.</param>
        /// <returns>
        ///        If the function succeeds, the return value is <c>true</c>. If the specified handle is not valid or is currently selected into a DC, the return value is <c>false</c>.
        /// </returns>
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DeleteObject([In] IntPtr hObject);
        [StructLayout(LayoutKind.Sequential)]
        private struct ICONINFO
        {
            public bool IsIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr MaskBitmap;
            public IntPtr ColorBitmap;
        };

        [DllImport("user32.dll")]
        private static extern IntPtr CreateIconIndirect([In] ref ICONINFO iconInfo);

        private const byte AC_SRC_OVER = 0x00;
        private const byte AC_SRC_ALPHA = 0x01;
        private const uint BPPF_ERASE = 0x0001;
        private const uint DI_NORMAL = 0x0003;
        [StructLayout(LayoutKind.Sequential)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }
        
        [StructLayout(LayoutKind.Sequential)]
public struct BP_PAINTPARAMS
{
   public uint                       cbSize;
    public uint                       dwFlags; // BPPF_ flags
    public RECT                 prcExclude;
    public BLENDFUNCTION        pBlendFunction;
}
     public   enum BP_BUFFERFORMAT
{
    BPBF_COMPATIBLEBITMAP,    // Compatible bitmap
    BPBF_DIB,                 // Device-independent bitmap
    BPBF_TOPDOWNDIB,          // Top-down device-independent bitmap
    BPBF_TOPDOWNMONODIB       // Top-down monochrome device-independent bitmap
}

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

         var hdcUsed = User32.GetDC(IntPtr.Zero);
         if (hdcUsed != IntPtr.Zero)
         {
             hBitmap = Gdi32.CreateDIBSection(hdcUsed, ref bi, DIB_RGB_COLORS, out bits, IntPtr.Zero, 0);
             Gdi32.ReleaseDC(IntPtr.Zero, hdcUsed);
         }
         return true;
     }

        private const int BI_RGB = 0;
        private const uint DIB_RGB_COLORS = 0;
        /*
         * HRESULT ConvertToPARGB32(HDC hdc, __inout ARGB *pargb, HBITMAP hbmp, SIZE& sizImage, int cxRow)
{
    BITMAPINFO bmi;
    InitBitmapInfo(&bmi, sizeof(bmi), sizImage.cx, sizImage.cy, 32);

    HRESULT hr = E_OUTOFMEMORY;
    HANDLE hHeap = GetProcessHeap();
    void *pvBits = HeapAlloc(hHeap, 0, bmi.bmiHeader.biWidth * 4 * bmi.bmiHeader.biHeight);
    if (pvBits)
    {
        hr = E_UNEXPECTED;
        if (GetDIBits(hdc, hbmp, 0, bmi.bmiHeader.biHeight, pvBits, &bmi, DIB_RGB_COLORS) == bmi.bmiHeader.biHeight)
        {
            ULONG cxDelta = cxRow - bmi.bmiHeader.biWidth;
            ARGB *pargbMask = static_cast<ARGB *>(pvBits);

            for (ULONG y = bmi.bmiHeader.biHeight; y; --y)
            {
                for (ULONG x = bmi.bmiHeader.biWidth; x; --x)
                {
                    if (*pargbMask++)
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

            hr = S_OK;
        }

        HeapFree(hHeap, 0, pvBits);
    }

    return hr;
}

bool HasAlpha(__in ARGB *pargb, SIZE& sizImage, int cxRow)
{
    ULONG cxDelta = cxRow - sizImage.cx;
    for (ULONG y = sizImage.cy; y; --y)
    {
        for (ULONG x = sizImage.cx; x; --x)
        {
            if (*pargb++ & 0xFF000000)
            {
                return true;
            }
        }

        pargb += cxDelta;
    }

    return false;
}

HRESULT ConvertBufferToPARGB32(HPAINTBUFFER hPaintBuffer, HDC hdc, HICON hicon, SIZE& sizIcon)
{
    RGBQUAD *prgbQuad;
    int cxRow;
    HRESULT hr = GetBufferedPaintBits(hPaintBuffer, &prgbQuad, &cxRow);
    if (SUCCEEDED(hr))
    {
        ARGB *pargb = reinterpret_cast<ARGB *>(prgbQuad);
        if (!HasAlpha(pargb, sizIcon, cxRow))
        {
            ICONINFO info;
            if (GetIconInfo(hicon, &info))
            {
                if (info.hbmMask)
                {
                    hr = ConvertToPARGB32(hdc, pargb, info.hbmMask, sizIcon, cxRow);
                }

                DeleteObject(info.hbmColor);
                DeleteObject(info.hbmMask);
            }
        }
    }

    return hr;
}*/
    }
}
