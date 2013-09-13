using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Contains information needed by IContextMenu::InvokeCommand to invoke a shortcut menu command.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct CMINVOKECOMMANDINFO
    {
        /// <summary>
        /// The size of this structure, in bytes.
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// Zero, or one or more of the following flags.
        /// </summary>
        public CMIC fMask;

        /// <summary>
        /// A handle to the window that is the owner of the shortcut menu. An extension can also use this handle as the owner of any message boxes or dialog boxes it displays.
        /// </summary>
        public IntPtr hwnd;

        /// <summary>
        /// The address of a null-terminated string that specifies the language-independent name of the command to carry out. This member is typically a string when a command is being activated by an application. 
        /// </summary>
        public IntPtr verb;

        /// <summary>
        /// An optional string containing parameters that are passed to the command. The format of this string is determined by the command that is to be invoked. This member is always NULL for menu items inserted by a Shell extension.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)] public string parameters;

        /// <summary>
        /// An optional working directory name. This member is always NULL for menu items inserted by a Shell extension.
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)] public string directory;

        /// <summary>
        /// 
        /// </summary>
        public int nShow;

        /// <summary>
        /// An optional keyboard shortcut to assign to any application activated by the command. If the fMask parameter does not specify CMIC_MASK_HOTKEY, this member is ignored.
        /// </summary>
        public uint dwHotKey;

        /// <summary>
        /// An icon to use for any application activated by the command. If the fMask member does not specify CMIC_MASK_ICON, this member is ignored.
        /// </summary>
        public IntPtr hIcon;
    }
}