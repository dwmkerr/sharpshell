using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    internal static class Uxtheme
    {
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr OpenThemeData(IntPtr hWnd, String classList);
        
        [DllImport("uxtheme.dll", ExactSpelling = true)]
        public extern static Int32 CloseThemeData(IntPtr hTheme);
        
        [DllImport("uxtheme", ExactSpelling=true)]
        public extern static Int32 GetThemePartSize(IntPtr hTheme, IntPtr hdc, int part, WindowPartState state, ref RECT pRect, int eSize, out SIZE size);
        
        [DllImport("uxtheme", ExactSpelling=true)]
        public extern static Int32 GetThemePartSize(IntPtr hTheme, IntPtr hdc, int part, WindowPartState state, IntPtr pRect, int eSize, out SIZE size);

        [DllImport("uxtheme", ExactSpelling = true)]
        public extern static Int32 GetThemeInt(IntPtr hTheme, int iPartId, int iStateId, int iPropId, out int piVal);

        [DllImport("uxtheme", ExactSpelling = true)]
        public extern static Int32 GetThemeMargins(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, int iPropId, IntPtr prc, out MARGINS pMargins);

        [DllImport("uxtheme", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public extern static Int32 GetThemeTextExtent(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, String text, int textLength, UInt32 textFlags, ref RECT boundingRect, out RECT extentRect);

        [DllImport("uxtheme", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public extern static Int32 GetThemeTextExtent(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, String text, int textLength, UInt32 textFlags, IntPtr boundingRect, out RECT extentRect);
        
        [DllImport("uxtheme", ExactSpelling = true)]
        public extern static Int32 DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId,
           int iStateId, ref RECT pRect, IntPtr pClipRect);

        [DllImport("uxtheme", ExactSpelling = true)]
        public extern static int IsThemeBackgroundPartiallyTransparent(IntPtr hTheme, int iPartId, int iStateId);

        [DllImport("uxtheme", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public extern static Int32 DrawThemeText(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, String text, int textLength, UInt32 textFlags, UInt32 textFlags2, ref RECT pRect);

        [DllImport("uxtheme", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern Int32 GetBufferedPaintBits(IntPtr hBufferedPaint, out IntPtr ppbBuffer, out int pcxRow);

        [DllImport("uxtheme.dll", SetLastError = true)]
        public static extern IntPtr BeginBufferedPaint(IntPtr hdc, ref RECT prcTarget, BP_BUFFERFORMAT dwFormat,
                                                       ref BP_PAINTPARAMS pPaintParams, out IntPtr phdc);

        [DllImport("uxtheme.dll")]
        public static extern IntPtr EndBufferedPaint(IntPtr hBufferedPaint, bool fUpdateTarget);

        [DllImport("uxtheme.dll", SetLastError = true)]
        [PreserveSig]
        public static extern IntPtr BufferedPaintInit();

        [DllImport("uxtheme.dll", SetLastError = true)]
        [PreserveSig]
        public static extern IntPtr BufferedPaintUnInit();

        public enum WindowPartState : int
        {
            // Frame States
            FS_ACTIVE = 1,
            FS_INACTIVE = 2,

            // Caption States
            CS_ACTIVE = 1,
            CS_INACTIVE = 2,
            CS_DISABLED = 3,

            // Max Caption States
            MXCS_ACTIVE = 1,
            MXCS_INACTIVE = 2,
            MXCS_DISABLED = 3,

            // Min Caption States
            MNCS_ACTIVE = 1,
            MNCS_INACTIVE = 2,
            MNCS_DISABLED = 3,

            // Horizontal Scrollbar States
            HSS_NORMAL = 1,
            HSS_HOT = 2,
            HSS_PUSHED = 3,
            HSS_DISABLED = 4,

            // Horizontal Thumb States
            HTS_NORMAL = 1,
            HTS_HOT = 2,
            HTS_PUSHED = 3,
            HTS_DISABLED = 4,

            // Vertical Scrollbar States
            VSS_NORMAL = 1,
            VSS_HOT = 2,
            VSS_PUSHED = 3,
            VSS_DISABLED = 4,

            // Vertical Thumb States
            VTS_NORMAL = 1,
            VTS_HOT = 2,
            VTS_PUSHED = 3,
            VTS_DISABLED = 4,

            // System Button States
            SBS_NORMAL = 1,
            SBS_HOT = 2,
            SBS_PUSHED = 3,
            SBS_DISABLED = 4,

            // Minimize Button States
            MINBS_NORMAL = 1,
            MINBS_HOT = 2,
            MINBS_PUSHED = 3,
            MINBS_DISABLED = 4,

            // Maximize Button States
            MAXBS_NORMAL = 1,
            MAXBS_HOT = 2,
            MAXBS_PUSHED = 3,
            MAXBS_DISABLED = 4,

            // Restore Button States
            RBS_NORMAL = 1,
            RBS_HOT = 2,
            RBS_PUSHED = 3,
            RBS_DISABLED = 4,

            // Help Button States
            HBS_NORMAL = 1,
            HBS_HOT = 2,
            HBS_PUSHED = 3,
            HBS_DISABLED = 4,

            // Close Button States
            CBS_NORMAL = 1,
            CBS_HOT = 2,
            CBS_PUSHED = 3,
            CBS_DISABLED = 4
        }
    }

    internal enum POPUPITEMSTATES
    {
        MPI_NORMAL = 1,
        MPI_HOT = 2,
        MPI_DISABLED = 3,
        MPI_DISABLEDHOT = 4,
    }

    internal enum POPUPCHECKBACKGROUNDSTATES
    {
        MCB_DISABLED = 1,
        MCB_NORMAL = 2,
        MCB_BITMAP = 3,
    }


    internal enum POPUPCHECKSTATES
    {
        MC_CHECKMARKNORMAL = 1,
        MC_CHECKMARKDISABLED = 2,
        MC_BULLETNORMAL = 3,
        MC_BULLETDISABLED = 4,
    }

    //todo tidy up and name properly.
    [StructLayout(LayoutKind.Sequential)]
    internal struct ICONINFO
    {
        public bool IsIcon;
        public int xHotspot;
        public int yHotspot;
        public IntPtr MaskBitmap;
        public IntPtr ColorBitmap;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct BLENDFUNCTION
    {
        public byte BlendOp;
        public byte BlendFlags;
        public byte SourceConstantAlpha;
        public byte AlphaFormat;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BP_PAINTPARAMS
    {
        public uint cbSize;
        public uint dwFlags; // BPPF_ flags
        public IntPtr prcExclude;
        public IntPtr pBlendFunction;
    }

    internal enum BP_BUFFERFORMAT
    {
        BPBF_COMPATIBLEBITMAP, // Compatible bitmap
        BPBF_DIB, // Device-independent bitmap
        BPBF_TOPDOWNDIB, // Top-down device-independent bitmap
        BPBF_TOPDOWNMONODIB // Top-down monochrome device-independent bitmap
    }
}