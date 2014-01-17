using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
// ReSharper disable InconsistentNaming

    /// <summary>
    /// Used with the SHCreateShellFolderViewEx function.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CSFV
    {
        /// <summary>
        /// The size of the CSFV structure, in bytes.
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// A pointer to the IShellFolder object for which to create the view.
        /// </summary>
        public IShellFolder pshf;

        /// <summary>
        /// A pointer to the parent IShellView interface. This parameter can be NULL.
        /// </summary>
        public IShellView psvOuter;

        /// <summary>
        /// Ignored.
        /// </summary>
        public IntPtr pidl;

        /// <summary>
        /// 
        /// </summary>
        public uint lEvents;

        /// <summary>
        /// A pointer to the LPFNVIEWCALLBACK function used by this folder view to handle callback messages. This parameter can be NULL.
        /// </summary>
        public IntPtr pfnCallback;

        /// <summary>
        /// 
        /// </summary>
        public FOLDERVIEWMODE fvm;
    }

// ReSharper restore InconsistentNaming
}