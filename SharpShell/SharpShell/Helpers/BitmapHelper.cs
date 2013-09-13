using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using SharpShell.Interop;

namespace SharpShell.Helpers
{
    internal class BitmapHelper
    {
        public const uint BI_BITFIELDS = 3;
        public const uint LCS_WINDOWS_COLOR_SPACE = 2;
        public const uint LCS_GM_IMAGES = 4;
        public const uint CF_DIBV5 = 17;
        public const uint GMEM_MOVEABLE = 0x00000002;
        public const uint GMEM_ZEROINIT = 0x00000040;
        public const uint GMEM_DDESHARE = 0x00002000;
        public const uint GHND = GMEM_MOVEABLE | GMEM_ZEROINIT;

        [DllImport("kernel32.dll")]
        static extern IntPtr GlobalAlloc(uint uFlags, uint dwBytes);

        [DllImport("kernel32.dll")]
        static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        static extern bool GlobalUnlock(IntPtr hMem);

        const int CBM_INIT = 0x04;//   /* initialize bitmap */

        public static IntPtr Create32BppBitmap(Image sourceImage)
        {
            BITMAPV5HEADER bi = new BITMAPV5HEADER();
            bi.bV5Size = (uint)Marshal.SizeOf(bi);
            bi.bV5Width = sourceImage.Width;
            bi.bV5Height = sourceImage.Height;
            bi.bV5Planes = 1;
            bi.bV5BitCount = 32;
            bi.bV5Compression = BI_BITFIELDS;
            // The following mask specification specifies a supported 32 BPP
            // alpha format for Windows XP.
            bi.bV5RedMask = 0x00FF0000;
            bi.bV5GreenMask = 0x0000FF00;
            bi.bV5BlueMask = 0x000000FF;
            bi.bV5AlphaMask = 0xFF000000;

            IntPtr hdc = User32.GetDC(IntPtr.Zero);
            IntPtr bits = IntPtr.Zero;

            // Create the DIB section with an alpha channel.
            IntPtr hBitmap = Gdi32.CreateDIBSection(hdc, bi, (uint)DIB.DIB_RGB_COLORS,
                                                    out bits, IntPtr.Zero, 0);

            var hMemDC = Gdi32.CreateCompatibleDC(hdc);
            Gdi32.ReleaseDC(IntPtr.Zero, hdc);

            var sourceBits = ((Bitmap) sourceImage).LockBits(
                new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            var stride = sourceImage.Width*4;
            for (int y = 0; y < sourceImage.Height; y++)
            {
                IntPtr DstDib = (IntPtr)(bits.ToInt32() + (y * stride));
                IntPtr SrcDib = (IntPtr)(sourceBits.Scan0.ToInt32() + ((sourceImage.Height - 1 - y) *
                                                                       stride));

                for (int x = 0; x < sourceImage.Width; x++)
                {
                    Marshal.WriteInt32(DstDib, Marshal.ReadInt32(SrcDib));
                    DstDib = (IntPtr)(DstDib.ToInt32() + 4);
                    SrcDib = (IntPtr)(SrcDib.ToInt32() + 4);
                }
            }

            return hBitmap;
        }

        public static IntPtr CreatePackedDIBV5(Bitmap bm)
        {
            BitmapData bmData = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height),
                                            ImageLockMode.ReadOnly, bm.PixelFormat);
            uint bufferLen = (uint) (Marshal.SizeOf(typeof (BITMAPV5HEADER)) +
                                     (Marshal.SizeOf(typeof (uint))*3) + bmData.Height*bmData.Stride);
            IntPtr hMem = GlobalAlloc(GHND | GMEM_DDESHARE, bufferLen);
            IntPtr packedDIBV5 = GlobalLock(hMem);
            BITMAPV5HEADER bmi = (BITMAPV5HEADER) Marshal.PtrToStructure(packedDIBV5,
                                                                         typeof (BITMAPV5HEADER));
            bmi.bV5Size = (uint) Marshal.SizeOf(typeof (BITMAPV5HEADER));
            bmi.bV5Width = bmData.Width;
            bmi.bV5Height = bmData.Height;
            bmi.bV5BitCount = 32;
            bmi.bV5Planes = 1;
            bmi.bV5Compression = BI_BITFIELDS;
            bmi.bV5XPelsPerMeter = 0;
            bmi.bV5YPelsPerMeter = 0;
            bmi.bV5ClrUsed = 0;
            bmi.bV5ClrImportant = 0;
            bmi.bV5BlueMask = 0x000000FF;
            bmi.bV5GreenMask = 0x0000FF00;
            bmi.bV5RedMask = 0x00FF0000;
            bmi.bV5AlphaMask = 0xFF000000;
            bmi.bV5CSType = LCS_WINDOWS_COLOR_SPACE;
            bmi.bV5GammaBlue = 0;
            bmi.bV5GammaGreen = 0;
            bmi.bV5GammaRed = 0;
            bmi.bV5ProfileData = 0;
            bmi.bV5ProfileSize = 0;
            bmi.bV5Reserved = 0;
            bmi.bV5Intent = LCS_GM_IMAGES;
            bmi.bV5SizeImage = (uint) (bmData.Height*bmData.Stride);
            bmi.bV5Endpoints.ciexyzBlue.ciexyzX =
                bmi.bV5Endpoints.ciexyzBlue.ciexyzY =
                bmi.bV5Endpoints.ciexyzBlue.ciexyzZ = 0;
            bmi.bV5Endpoints.ciexyzGreen.ciexyzX =
                bmi.bV5Endpoints.ciexyzGreen.ciexyzY =
                bmi.bV5Endpoints.ciexyzGreen.ciexyzZ = 0;
            bmi.bV5Endpoints.ciexyzRed.ciexyzX =
                bmi.bV5Endpoints.ciexyzRed.ciexyzY =
                bmi.bV5Endpoints.ciexyzRed.ciexyzZ = 0;
            Marshal.StructureToPtr(bmi, packedDIBV5, false);

            BITFIELDS Masks = (BITFIELDS) Marshal.PtrToStructure(
                (IntPtr) (packedDIBV5.ToInt32() + bmi.bV5Size), typeof (BITFIELDS));
            Masks.BlueMask = 0x000000FF;
            Masks.GreenMask = 0x0000FF00;
            Masks.RedMask = 0x00FF0000;
            Marshal.StructureToPtr(Masks, (IntPtr) (packedDIBV5.ToInt32() +
                                                    bmi.bV5Size), false);

            long offsetBits = bmi.bV5Size + Marshal.SizeOf(typeof (uint))*3;
            IntPtr bits = (IntPtr) (packedDIBV5.ToInt32() + offsetBits);


            for (int y = 0; y < bmData.Height; y++)
            {
                IntPtr DstDib = (IntPtr) (bits.ToInt32() + (y*bmData.Stride));
                IntPtr SrcDib = (IntPtr) (bmData.Scan0.ToInt32() + ((bmData.Height - 1 - y)*
                                                                    bmData.Stride));

                for (int x = 0; x < bmData.Width; x++)
                {
                    Marshal.WriteInt32(DstDib, Marshal.ReadInt32(SrcDib));
                    DstDib = (IntPtr) (DstDib.ToInt32() + 4);
                    SrcDib = (IntPtr) (SrcDib.ToInt32() + 4);
                }
            }
            
            // Create the DIB section with an alpha channel.
            IntPtr hdc = User32.GetDC(IntPtr.Zero);
            //IntPtr hdc = Gdi32.CreateCompatibleDC(IntPtr.Zero);
            GCHandle handle = GCHandle.Alloc(bmi, GCHandleType.Pinned);
            /*IntPtr hBitmap = Gdi32.CreateDIBSection(hdc, handle.AddrOfPinnedObject(), (uint)SharpThumbnailHandler.DIB.DIB_RGB_COLORS,
                out bits, IntPtr.Zero, 0);*/
            IntPtr hBitmap = Gdi32.CreateDIBitmap(hdc, handle.AddrOfPinnedObject(), CBM_INIT,
                                                  bits, handle.AddrOfPinnedObject(), (uint)DIB.DIB_RGB_COLORS);
            bm.UnlockBits(bmData);

            GlobalUnlock(hMem);

            
            return hBitmap;
        }
    }
}