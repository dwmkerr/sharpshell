using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Exposes methods for the display of rich previews.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("8895b1c6-b41f-4c1c-a562-0d564250836f")]
    public interface IPreviewHandler
    {
        /// <summary>
        /// Sets the parent window of the previewer window, as well as the area within the parent to be used for the previewer window.
        /// </summary>
        /// <param name="hwnd">A handle to the parent window.</param>
        /// <param name="prc">A pointer to a RECT defining the area for the previewer.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int SetWindow(IntPtr hwnd, ref RECT prc);

        /// <summary>
        /// Directs the preview handler to change the area within the parent hwnd that it draws into.
        /// </summary>
        /// <param name="prc">A pointer to a RECT to be used for the preview.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig] 
        int SetRect(ref RECT prc);

        /// <summary>
        /// Directs the preview handler to load data from the source specified in an earlier Initialize method call, and to begin rendering to the previewer window.
        /// </summary>
        /// <returns>This method can return one of these values.</returns>
        [PreserveSig] 
        int DoPreview();

        /// <summary>
        /// Directs the preview handler to cease rendering a preview and to release all resources that have been allocated based on the item passed in during the initialization.
        /// </summary>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig] 
        int Unload();

        /// <summary>
        /// Directs the preview handler to set focus to itself.
        /// </summary>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig] 
        int SetFocus();

        /// <summary>
        /// Directs the preview handler to return the HWND from calling the GetFocus Function.
        /// </summary>
        /// <param name="phwnd">When this method returns, contains a pointer to the HWND returned from calling the GetFocus Function from the preview handler's foreground thread.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig] 
        int QueryFocus(out IntPtr phwnd);

        /// <summary>
        /// Directs the preview handler to handle a keystroke passed up from the message pump of the process in which the preview handler is running.
        /// </summary>
        /// <param name="pmsg">A pointer to a window message.</param>
        /// <returns>If the keystroke message can be processed by the preview handler, the handler will process it and return S_OK. If the preview handler cannot process the keystroke message, it will offer it to the host using TranslateAccelerator. If the host processes the message, this method will return S_OK. If the host does not process the message, this method will return S_FALSE.</returns>
        [PreserveSig] 
        int TranslateAccelerator(MSG pmsg);
    };
}