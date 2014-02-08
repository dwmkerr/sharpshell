using System;
using System.Drawing;
using System.Runtime.InteropServices;
using SharpShell.Interop;

namespace SharpShell.SharpContextMenu
{
    /// <summary>
    /// The PARGB32 Helper class is used to create Windows Vista PARGB32 bitmaps.
    /// </summary>
    public static class PARGB32
    {
        /// <summary>
        /// Creates a handle to a PARGB32 bitmap from an Icon.
        /// </summary>
        /// <param name="iconHandle">The handle to the icon.</param>
        /// <param name="iconSize">The iconSize of the icon.</param>
        /// <returns>A PARGB32 bitmap with the contents of the icon, including transparency.</returns>
        public static IntPtr CreatePARGB32HBitmap(IntPtr iconHandle, Size iconSize)
        {
            //  Make sure we're ready for buffered painting.
            //  TODO: Really we only need to do this once per thread, so an improvement
            //  might be to have a manager create this as required per thread.
            Uxtheme.BufferedPaintInit();

            //  Create a compatible device context to work with.
            var deviceContextHandle = Gdi32.CreateCompatibleDC(IntPtr.Zero);
            if (deviceContextHandle == IntPtr.Zero)
                return IntPtr.Zero;

            //  Now create a 32 bit bitmap of the appropriate size, getting it's bits and handle.
            IntPtr bits;
            IntPtr hBitmap;
            if (!Create32BitHBITMAP(deviceContextHandle, iconSize, out bits, out hBitmap))
            {
                Gdi32.DeleteDC(deviceContextHandle);
                return IntPtr.Zero;
            }

            //  Select the bitmap, keeping track of the old one. If this fails, 
            //  delete the device context and return a null handle.
            var oldBitmapHandle = Gdi32.SelectObject(deviceContextHandle, hBitmap);
            if (oldBitmapHandle == IntPtr.Zero)
            {
                Gdi32.DeleteDC(deviceContextHandle);
                return IntPtr.Zero;
            }

            //  Create paint params that represent our alpha blending.
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
            paintParams.pBlendFunction = Marshal.AllocHGlobal(Marshal.SizeOf(bfAlpha));
            Marshal.StructureToPtr(bfAlpha, paintParams.pBlendFunction, false);

            //  Create the pointer that'll hold the device context to the buffer, set the icon rectangle.
            IntPtr bufferDeviceContextHandle;
            var iconRect = new RECT(0, 0, iconSize.Width, iconSize.Height);

            //  Create a paint buffer handle.
            var paintBufferHandle = Uxtheme.BeginBufferedPaint(deviceContextHandle, ref iconRect,
                BP_BUFFERFORMAT.BPBF_DIB, ref paintParams, out bufferDeviceContextHandle);
            
            //  Free the memory we allocated for the blend function.
            Marshal.FreeHGlobal(paintParams.pBlendFunction);

            //  If we created a paint buffer successfully, we can draw the icon into it.
            if (paintBufferHandle != IntPtr.Zero)
            {
                //  Try and draw the icon.
                if (Gdi32.DrawIconEx(bufferDeviceContextHandle, 0, 0, iconHandle, iconSize.Width, iconSize.Height, 0, IntPtr.Zero,
                                     (int) DI_NORMAL))
                {
                    //  Now convert the buffer we've painted into PARGB32, meaning we'll end up
                    //  with a PARGB32 bitmap.
                    ConvertBufferToPARGB32(paintBufferHandle, deviceContextHandle, iconHandle, iconSize);
                }

                // This will write the buffer contents to the destination bitmap.
                Uxtheme.EndBufferedPaint(paintBufferHandle, true);
            }

            //  Select the old bitmpa and delete the device context.
            Gdi32.SelectObject(deviceContextHandle, oldBitmapHandle);
            Gdi32.DeleteDC(deviceContextHandle);

            //  We're done with buffered painting.
            Uxtheme.BufferedPaintUnInit();

            //  Baddabing-baddabom, PARGB32.
            return hBitmap;
        }

        /// <summary>
        /// Creates a 32 bit HBITMAP of the specified size.
        /// </summary>
        /// <param name="hdc">The HDC.</param>
        /// <param name="size">The size.</param>
        /// <param name="bits">The bits.</param>
        /// <param name="hBitmap">The bitmap handle.</param>
        /// <returns>True if the bitmap was created successfully.</returns>
        private static bool Create32BitHBITMAP(IntPtr hdc, Size size, out IntPtr bits, out IntPtr hBitmap)
        {
            //  Create a bitmap info setup for a 32 bit bitmap.
            var bi = new BITMAPINFO
                     {
                         bmiHeader = new BITMAPINFOHEADER
                                     {
                                         biSize = (uint) Marshal.SizeOf(typeof (BITMAPINFOHEADER)),
                                         biPlanes = 1,
                                         biCompression = (uint) BI.BI_RGB,
                                         biWidth = size.Width,
                                         biHeight = size.Height,
                                         biBitCount = 32
                                     }
                     };

            //  Create the DIB section.
            hBitmap = Gdi32.CreateDIBSection(hdc, ref bi, (uint)DIB.DIB_RGB_COLORS, out bits, IntPtr.Zero, 0);

            //  Return success only if we have a handle and bitmap bits.
            return hBitmap != IntPtr.Zero && bits != IntPtr.Zero;
        }

        /// <summary>
        /// Converts a buffer to PARGB32.
        /// </summary>
        /// <param name="hPaintBuffer">The paint buffer handle.</param>
        /// <param name="hdc">The device context handle.</param>
        /// <param name="iconHandle">The icon handle.</param>
        /// <param name="iconSize">The icon size.</param>
        private static void ConvertBufferToPARGB32(IntPtr hPaintBuffer, IntPtr hdc, IntPtr iconHandle, Size iconSize)
        {
            //  Get the actual paint bits of the buffer. If this fails, we return.
            IntPtr prgbQuad;
            int cxRow;
            var hr = Uxtheme.GetBufferedPaintBits(hPaintBuffer, out prgbQuad, out cxRow);
            if (hr != WinError.S_OK)
                return;

            unsafe
            {
                //  Get the pointer to the bits.
                var pargb = (UInt32*)prgbQuad.ToPointer();

                //  If out pixels have any alpha values, we're done.
                if (HasAlpha(pargb, iconSize, cxRow)) 
                    return;

                //  As we don't have alpha values, we need to get the icon info for
                //  a mask image.
                ICONINFO info;
                if (!User32.GetIconInfo(iconHandle, out info))
                    return;

                //  If there's a mask, we can apply it.
                if (info.MaskBitmap != IntPtr.Zero)
                    ConvertToPARGB32(hdc, pargb, info.MaskBitmap, iconSize, cxRow);

                //  Free the icon info and we're done.
                Gdi32.DeleteObject(info.ColorBitmap);
                Gdi32.DeleteObject(info.MaskBitmap);
            }
        }

        /// <summary>
        /// Determines whether any pixel in an image has an alpha component.
        /// </summary>
        /// <param name="pargb">Pixel data.</param>
        /// <param name="imageSize">The image size.</param>
        /// <param name="rowLength">The row length.</param>
        /// <returns>
        ///   <c>true</c> if the specified pargb has alpha; otherwise, <c>false</c>.
        /// </returns>
        private unsafe static bool HasAlpha(UInt32* pargb, Size imageSize, int rowLength)
        {
            var cxDelta = (uint) (rowLength - imageSize.Width);
            for (var y = imageSize.Height; y != 0; --y)
            {
                for (var x = imageSize.Width; x != 0; --x)
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

        /// <summary>
        /// Converts raw image data into PARGB32.
        /// </summary>
        /// <param name="hdc">The handle to the device context..</param>
        /// <param name="pargb">The pixel data.</param>
        /// <param name="hbmp">The bitmap handle.</param>
        /// <param name="imageSize">The image size.</param>
        /// <param name="rowLength">The row length.</param>
        private unsafe static void ConvertToPARGB32(IntPtr hdc, UInt32* pargb, IntPtr hbmp, Size imageSize, int rowLength)
        {
            var bmi = new BITMAPINFO
            {
                bmiHeader = new BITMAPINFOHEADER
                {
                    biSize = (uint) Marshal.SizeOf(typeof (BITMAPINFOHEADER)),
                    biPlanes = 1,
                    biCompression = (uint) BI.BI_RGB,
                    biWidth = imageSize.Width,
                    biHeight = imageSize.Height,
                    biBitCount = 32
                }
            };

            //  Allocate data sufficient for the pixel data.
            IntPtr hHeap = Kernel32.GetProcessHeap();
            void* pvBits = Kernel32.HeapAlloc(hHeap, 0, new UIntPtr((uint)(bmi.bmiHeader.biWidth * 4 * bmi.bmiHeader.biHeight))).ToPointer();
            if (pvBits == (void*) 0)
                return;
            
            //  Get the bitmap bits.
            var ptr = new IntPtr(pvBits);
            if (Gdi32.GetDIBits(hdc, hbmp, 0, (uint) bmi.bmiHeader.biHeight, ref ptr,ref  bmi, (uint)DIB.DIB_RGB_COLORS) ==
                bmi.bmiHeader.biHeight)
            {
                //  Now handle each pixel.
                UInt32 cxDelta = (uint)(rowLength - bmi.bmiHeader.biWidth);
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

        private const byte AC_SRC_OVER = 0x00;
        private const byte AC_SRC_ALPHA = 0x01;
        private const uint BPPF_ERASE = 0x0001;
        private const uint DI_NORMAL = 0x0003;
    }
}