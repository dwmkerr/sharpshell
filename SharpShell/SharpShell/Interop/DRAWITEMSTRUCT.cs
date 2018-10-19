using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct DRAWITEMSTRUCT
    {
        public UInt32 CtlType;
        public UInt32 CtlID;
        public UInt32 itemID;
        public UInt32 itemAction;
        public UInt32 itemState;
        public IntPtr hwndItem;
        public IntPtr hDC;
        public RECT rcItem;
        public IntPtr itemData;
    }
}