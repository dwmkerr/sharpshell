using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Auto)]
    internal struct DLGTEMPLATE
    {
        internal uint style;
        internal uint extendedStyle;
        internal ushort cdit;
        internal short x;
        internal short y;
        internal short cx;
        internal short cy;
        internal short menuResource;
        internal short windowClass;
        internal short titleArray;
        internal short fontPointSize;
        [MarshalAs(UnmanagedType.LPWStr)]
        internal string fontTypeface;
    }
}