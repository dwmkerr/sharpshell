using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace SharpShell.Interop
{
    /// <summary>
    /// Exposes a method that enables the callback of a context menu. For example, to add a shield icon to a menuItem that requires elevation.
    /// </summary>
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("3409E930-5A39-11d1-83FA-00A0C90DC849")]
    public interface IContextMenuCB
    {
        /// <summary>
        /// Enables the callback function for a context menu.
        /// </summary>
        /// <param name="psf">A pointer to the IShellFolder interface of the object that supports the IContextMenuCB::CallBack interface. The context menu interface is returned on a call to GetUIObjectOf.</param>
        /// <param name="hwndOwner">A handle to the owner of the context menu. This value can be NULL.</param>
        /// <param name="pdtobj">A pointer to an IDataObject that contains information about a menu selection. Implement interface IDataObject, or call SHCreateDataObject for the default implementation.</param>
        /// <param name="uMsg">A notification from the Shell's default menu implementation. For example, the default menu implementation calls DFM_MERGECONTEXTMENU to allow the implementer of IContextMenuCB::CallBack to remove, add, or disable context menu items in this callback. Use one of the following notifications.</param>
        /// <param name="wParam">Data specific to the notification specified in uMsg. See the individual notification page for specific requirements.</param>
        /// <param name="lParam">Data specific to the notification specified in uMsg. See the individual notification page for specific requirements.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int CallBack(IShellFolder psf, IntPtr hwndOwner, IDataObject pdtobj, uint uMsg, IntPtr wParam, IntPtr lParam);
    };
}