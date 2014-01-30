using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Exposes methods that present a view in the Windows Explorer or folder windows.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214E3-0000-0000-C000-000000000046")]
    public interface IShellView  : IOleWindow
    {
        /// <summary>
        /// Retrieves a handle to one of the windows participating in in-place activation (frame, document, parent, or in-place object window).
        /// </summary>
        /// <param name="phwnd">A pointer to a variable that receives the window handle.</param>
        /// <returns>This method returns S_OK on success. </returns>
        [PreserveSig]
        new int GetWindow(out IntPtr phwnd);

        /// <summary>
        /// Determines whether context-sensitive help mode should be entered during an in-place activation session.
        /// </summary>
        /// <param name="fEnterMode">TRUE if help mode should be entered; FALSE if it should be exited.</param>
        /// <returns>This method returns S_OK if the help mode was entered or exited successfully, depending on the value passed in fEnterMode.</returns>
        [PreserveSig]
        new int ContextSensitiveHelp(bool fEnterMode);

        /// <summary>
        /// Translates keyboard shortcut (accelerator) key strokes when a namespace extension's view has the focus.
        /// </summary>
        /// <param name="lpmsg">The address of the message to be translated.</param>
        /// <returns>Returns S_OK if successful, or a COM-defined error value otherwise. If the view returns S_OK, it indicates that the message was translated and should not be translated or dispatched by Windows Explorer. </returns>
        [PreserveSig]
        int TranslateAcceleratorA(MSG lpmsg);

        /// <summary>
        /// Enables or disables modeless dialog boxes. This method is not currently implemented.
        /// </summary>
        /// <param name="fEnable">Nonzero to enable modeless dialog box windows or zero to disable them.</param>
        [PreserveSig] 
        int EnableModeless(bool fEnable);
    
        /// <summary>
        /// Called when the activation state of the view window is changed by an event that is not caused by the Shell view itself. For example, if the TAB key is pressed when the tree has the focus, the view should be given the focus.
        /// </summary>
        /// <param name="uState">Flag specifying the activation state of the window.</param>
        /// <returns>Returns S_OK if successful, or a COM-defined error value otherwise.</returns>
        [PreserveSig]
        int UIActivate(SVUIA_STATUS uState);

        /// <summary>
        /// Refreshes the view's contents in response to user input.
        /// </summary>
        /// <returns>Returns S_OK if successful, or a COM-defined error value otherwise.</returns>
        [PreserveSig]
        int Refresh();

        /// <summary>
        /// Creates a view window. This can be either the right pane of Windows Explorer or the client window of a folder window..
        /// </summary>
        /// <param name="psvPrevious">The address of the IShellView interface of the view window being exited. Views can use this parameter to communicate with a previous view of the same implementation. This interface can be used to optimize browsing between like views. This pointer may be NULL.</param>
        /// <param name="pfs">The address of a FOLDERSETTINGS structure. The view should use this when creating its view.</param>
        /// <param name="psb">The address of the current instance of the IShellBrowser interface. The view should call this interface's AddRef method and keep the interface pointer to allow communication with the Windows Explorer window.</param>
        /// <param name="prcView">The dimensions of the new view, in client coordinates.</param>
        /// <param name="phWnd">The address of the window handle being created.</param>
        /// <returns>Returns a success code if successful, or a COM error code otherwise.</returns>
        [PreserveSig]
        int CreateViewWindow([In, MarshalAs(UnmanagedType.Interface)] IShellView psvPrevious,
             [In] ref FOLDERSETTINGS pfs, [In, MarshalAs(UnmanagedType.Interface)] IShellBrowser psb, [In]  ref RECT prcView, [In, Out] ref IntPtr phWnd);

        /// <summary>
        /// Destroys the view window.
        /// </summary>
        /// <returns>Returns a success code if successful, or a COM error code otherwise.</returns>
        [PreserveSig]
        int DestroyViewWindow();

        /// <summary>
        /// Gets the current folder settings.
        /// </summary>
        /// <param name="pfs">The address of a FOLDERSETTINGS structure to receive the settings.</param>
        /// <returns>Returns S_OK if successful, or a COM-defined error value otherwise.</returns>
        [PreserveSig]
        int GetCurrentInfo(ref FOLDERSETTINGS pfs);

        /// <summary>
        /// Allows the view to add pages to the Options property sheet from the View menu.
        /// </summary>
        /// <param name="dwReserved">Reserved.</param>
        /// <param name="lpfn">The address of the callback function used to add the pages.</param>
        /// <param name="lparam">A value that must be passed as the callback function's lparam parameter.</param>
        /// <returns>Returns S_OK if successful, or a COM-defined error value otherwise.</returns>
        [PreserveSig]
        int AddPropertySheetPages(long dwReserved, ref IntPtr lpfn, IntPtr lparam);

        /// <summary>
        /// Saves the Shell's view settings so the current state can be restored during a subsequent browsing session.
        /// </summary>
        [PreserveSig]
        int SaveViewState();

        /// <summary>
        /// Changes the selection state of one or more items within the Shell view window.
        /// </summary>
        /// <param name="pidlItem">The address of the ITEMIDLIST structure.</param>
        /// <param name="uFlags">One of the _SVSIF constants that specify the type of selection to apply.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int SelectItem(IntPtr pidlItem, _SVSIF uFlags);

        /// <summary>
        /// Gets an interface that refers to data presented in the view.
        /// </summary>
        /// <param name="uItem">The constants that refer to an aspect of the view. This parameter can be any one of the _SVGIO constants.</param>
        /// <param name="riid">The identifier of the COM interface being requested.</param>
        /// <param name="ppv">The address that receives the interface pointer. If an error occurs, the pointer returned must be NULL.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int GetItemObject(_SVGIO uItem, ref Guid riid, ref IntPtr ppv);
    }
}