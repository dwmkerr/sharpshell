using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Exposed by the common file dialog boxes to be used when they host a Shell browser. If supported, ICommDlgBrowser exposes methods that allow a Shell view to handle several cases that require different behavior in a dialog box than in a normal Shell view. You obtain an ICommDlgBrowser interface pointer by calling QueryInterface on the IShellBrowser object.
    /// </summary>
    [ComImport, Guid("000214f1-0000-0000-c000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ICommDlgBrowser
    {
        /// <summary>
        /// Called when a user double-clicks in the view or presses the ENTER key.
        /// </summary>
        /// <param name="ppshv">A pointer to the view's IShellView interface.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int OnDefaultCommand([In] IntPtr ppshv);

        /// <summary>
        /// Called after a state, identified by the uChange parameter, has changed in the IShellView interface.
        /// </summary>
        /// <param name="ppshv">A pointer to the view's IShellView interface.</param>
        /// <param name="uChange">Change in the selection state. This parameter can be one of the following values.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int OnStateChange([In] IntPtr ppshv, IntPtr uChange);

        /// <summary>
        /// Allows the common dialog box to filter objects that the view displays.
        /// </summary>
        /// <param name="ppshv">A pointer to the view's IShellView interface.</param>
        /// <param name="pidl">A PIDL, relative to the folder, that identifies the object.</param>
        /// <returns>The browser should return S_OK to include the object in the view, or S_FALSE to hide it.</returns>
        [PreserveSig]
        int IncludeObject([In] IntPtr ppshv, IntPtr pidl);
    }
}