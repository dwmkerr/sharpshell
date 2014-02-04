using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    internal static class Comctl32
    {
        [DllImport("comctl32.dll", SetLastError = true)]
        public static extern IntPtr CreatePropertySheetPage(ref PROPSHEETPAGE psp);

        [DllImport("comctl32.dll", SetLastError = true)]
        public static extern IntPtr PropertySheet(ref PROPSHEETHEADER psh);
    }
}
