using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct CMINVOKECOMMANDINFOEX
    {
        public uint cbSize;
        public CMIC fMask;
        public IntPtr hwnd;
        public IntPtr verb;
        [MarshalAs(UnmanagedType.LPStr)]
        public string parameters;
        [MarshalAs(UnmanagedType.LPStr)]
        public string directory;
        public int nShow;
        public uint dwHotKey;
        public IntPtr hIcon;
        [MarshalAs(UnmanagedType.LPStr)]
        public string title;
        public IntPtr verbW;
        public string parametersW;
        public string directoryW;
        public string titleW;
        POINT ptInvoke;
    }
}