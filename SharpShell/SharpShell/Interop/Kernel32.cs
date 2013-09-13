using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpShell.Interop
{
    internal static class Kernel32
    {
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

  //      [DllImport("kernel32.dll", SetLastError = true)]
//        public extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll")]
        public static extern IntPtr FindResource(IntPtr hModule, IntPtr lpName, IntPtr lpType);

        [DllImport("kernel32.dll")]
        public static extern IntPtr FindResource(IntPtr hModule, int lpName, int lpType);

        [DllImport("kernel32.dll")]
        public static extern IntPtr FindResource(IntPtr hModule, int lpName, string lpType);

        [DllImport("kernel32.dll")]
        public static extern IntPtr FindResource(IntPtr hModule, string lpName, int lpType);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr FindResource(IntPtr hModule, string lpName, string lpType);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        public const uint RT_CURSOR = 0x00000001;
        public const uint RT_BITMAP = 0x00000002;
        public const uint RT_ICON = 0x00000003;
        public const uint RT_MENU = 0x00000004;
        public const uint RT_DIALOG = 0x00000005;
        public const uint RT_STRING = 0x00000006;
        public const uint RT_FONTDIR = 0x00000007;
        public const uint RT_FONT = 0x00000008;
        public const uint RT_ACCELERATOR = 0x00000009;
        public const uint RT_RCDATA = 0x0000000a;
        public const uint RT_MESSAGETABLE = 0x0000000b;
    }
}
