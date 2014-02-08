using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
// ReSharper disable InconsistentNaming

    /// <summary>
    /// Used by an IEnumExtraSearch enumerator object to return information on the search objects supported by a Shell Folder object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct EXTRASEARCH
    {
        /// <summary>
        /// A search object's GUID.
        /// </summary>
        public Guid guidSearch;

        /// <summary>
        /// A Unicode string containing the search object's friendly name. It will be used to identify the search engine on the Search Assistant menu.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string wszFriendlyName;

        /// <summary>
        /// The URL that will be displayed in the search pane.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)]
        public string wszUrl;
    }

// ReSharper restore InconsistentNaming
}