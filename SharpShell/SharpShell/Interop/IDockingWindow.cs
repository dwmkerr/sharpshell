using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Exposes methods that notify the docking window object of changes, including showing, hiding, and impending removal. 
    /// This interface is implemented by window objects that can be docked within the border space of a Windows Explorer window.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("012dd920-7b26-11d0-8ca9-00a0c92dbfe8")]
    public interface IDockingWindow : IOleWindow
    {
        /// <summary>
        /// Instructs the docking window object to show or hide itself.
        /// </summary>
        /// <param name="bShow">TRUE if the docking window object should show its window. 
        /// FALSE if the docking window object should hide its window and return its border space by calling SetBorderSpaceDW with zero values.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int ShowDW(bool bShow);

        /// <summary>
        /// Notifies the docking window object that it is about to be removed from the frame. 
        /// The docking window object should save any persistent information at this time.
        /// </summary>
        /// <param name="dwReserved">Reserved. This parameter should always be zero.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int CloseDW(UInt32 dwReserved);

        /// <summary>
        /// Notifies the docking window object that the frame's border space has changed. 
        /// In response to this method, the IDockingWindow implementation must call SetBorderSpaceDW, even if no border space is required or a change is not necessary.
        /// </summary>
        /// <param name="rcBorder">Pointer to a RECT structure that contains the frame's available border space.</param>
        /// <param name="punkToolbarSite">Pointer to the site's IUnknown interface. The docking window object should call the QueryInterface method for this interface, requesting IID_IDockingWindowSite.
        /// The docking window object then uses that interface to negotiate its border space. It is the docking window object's responsibility to release this interface when it is no longer needed.</param>
        /// <param name="fReserved">Reserved. This parameter should always be zero.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int ResizeBorderDW(RECT rcBorder, IntPtr punkToolbarSite, bool fReserved);
    }
}