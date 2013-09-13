using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PROPSHEETHEADER
    {
        public int dwSize;
        public PSH dwFlags;
        public IntPtr hwndParent;
        public IntPtr hInstance;
        public IntPtr hIcon;
        public string pszCaption;
        public uint nPages;
        public IntPtr nStartPage;
        public IntPtr phpage;
        public IntPtr /*PFNPROPSHEETCALLBACK*/ pfnCallback ;
        public IntPtr hbmWatermark;
        public IntPtr hplWatermark;
        public IntPtr hbmHeader;
    }
}