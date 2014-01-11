using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Exposes a method that allows communication between Windows Explorer and a folder view implemented using the system folder view object (the IShellView object returned through SHCreateShellFolderView) so that the folder view can be notified of events and modify its view accordingly.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("2047E320-F2A9-11CE-AE65-08002B2E1262")]
    public interface IShellFolderViewCB
    {
        /// <summary>
        /// Allows communication between the system folder view object and a system folder view callback object.
        /// </summary>
        /// <param name="uMsg">One of the following notifications.</param>
        /// <param name="wParam">Additional information.</param>
        /// <param name="lParam">Additional information.</param>
        /// <param name="plResult">Additional information.</param>
        /// <returns>S_OK if the message was handled, E_NOTIMPL if the shell should perform default processing.</returns>
        [PreserveSig]
        int MessageSFVCB(uint uMsg, IntPtr wParam, IntPtr lParam, ref IntPtr plResult);
        
    }
}