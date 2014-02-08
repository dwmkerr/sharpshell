using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace SharpShell.Interop
{
    /// <summary>
    /// Implemented by hosts of Shell views (objects that implement IShellView). Exposes methods that provide services for the view it is hosting and other objects that run in the context of the Explorer window.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214E2-0000-0000-C000-000000000046")]
    public interface IShellBrowser : IOleWindow
    {
        #region Explicit Overrides of IOleWindow functions

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

        #endregion

        /// <summary>
        /// Allows the container to insert its menu groups into the composite menu that is displayed when an extended namespace is being viewed or used.
        /// </summary>
        /// <param name="hmenuShared">A handle to an empty menu.</param>
        /// <param name="lpMenuWidths">The address of an OLEMENUGROUPWIDTHS array of six LONG values. The container fills in elements 0, 2, and 4 to reflect the number of menu elements it provided in the File, View, and Window menu groups.</param>
        /// <returns>Returns S_OK if successful, or a COM-defined error value otherwise.</returns>
        [PreserveSig]
        int InsertMenusSB(IntPtr hmenuShared, ref IntPtr lpMenuWidths);

        /// <summary>
        /// Installs the composite menu in the view window.
        /// </summary>
        /// <param name="hmenuShared">A handle to the composite menu constructed by calls to IShellBrowser::InsertMenusSB and the InsertMenu function.</param>
        /// <param name="holemenuRes"></param>
        /// <param name="hwndActiveObject">The view's window handle.</param>
        /// <returns>Returns S_OK if successful, or a COM-defined error value otherwise.</returns>
        [PreserveSig]
        int SetMenuSB(IntPtr hmenuShared, IntPtr holemenuRes, IntPtr hwndActiveObject);

        /// <summary>
        /// Permits the container to remove any of its menu elements from the in-place composite menu and to free all associated resources.
        /// </summary>
        /// <param name="hmenuShared">A handle to the in-place composite menu that was constructed by calls to IShellBrowser::InsertMenusSB and the InsertMenu function.</param>
        /// <returns>Returns S_OK if successful, or a COM-defined error value otherwise.</returns>
        [PreserveSig]
        int RemoveMenusSB(IntPtr hmenuShared);

        /// <summary>
        /// Sets and displays status text about the in-place object in the container's frame-window status bar.
        /// </summary>
        /// <param name="pszStatusText">A pointer to a null-terminated character string that contains the message to display.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int SetStatusTextSB(string pszStatusText);

        /// <summary>
        /// Tells Windows Explorer to enable or disable its modeless dialog boxes.
        /// </summary>
        /// <param name="fEnable">Specifies whether the modeless dialog boxes are to be enabled or disabled. If this parameter is nonzero, modeless dialog boxes are enabled. If this parameter is zero, modeless dialog boxes are disabled.</param>
        /// <returns>Returns S_OK if successful, or a COM-defined error value otherwise.</returns>
        [PreserveSig]
        int EnableModelessSB(bool fEnable);

        /// <summary>
        /// Translates accelerator keystrokes intended for the browser's frame while the view is active.
        /// </summary>
        /// <param name="pmsg">The address of an MSG structure containing the keystroke message.</param>
        /// <param name="wID">The command identifier value corresponding to the keystroke in the container-provided accelerator table. Containers should use this value instead of translating again.</param>
        /// <returns>Returns S_OK if successful, or a COM-defined error value otherwise.</returns>
        [PreserveSig]
        int TranslateAcceleratorSB(IntPtr pmsg, short wID);

        /// <summary>
        /// Informs Windows Explorer to browse to another folder.
        /// </summary>
        /// <param name="pidl">The address of an ITEMIDLIST (item identifier list) structure that specifies an object's location. This value is dependent on the flag or flags set in the wFlags parameter.</param>
        /// <param name="wFlags">Flags specifying the folder to be browsed. It can be zero or one or more of the following values.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int BrowseObject(IntPtr pidl, SBSP wFlags);

        /// <summary>
        /// Gets an IStream interface that can be used for storage of view-specific state information.
        /// </summary>
        /// <param name="grfMode">Read/write access of the IStream interface. This may be one of the following values.</param>
        /// <param name="ppStrm">The address that receives the IStream interface pointer.</param>
        /// <returns>Returns S_OK if successful, or a COM-defined error value otherwise.</returns>
        [PreserveSig]
        int GetViewStateStream(long grfMode, ref IStream ppStrm);

        /// <summary>
        /// Gets the window handle to a browser control.
        /// </summary>
        /// <param name="id">The control handle that is being requested. This parameter can be one of the following values:</param>
        /// <param name="phwnd">The address of the window handle to the Windows Explorer control.</param>
        /// <returns>Returns S_OK if successful, or a COM-defined error value otherwise.</returns>
        [PreserveSig]
        int GetControlWindow(uint id, ref IntPtr phwnd);

        /// <summary>
        /// Sends control messages to either the toolbar or the status bar in a Windows Explorer window.
        /// </summary>
        /// <param name="id">An identifier for either a toolbar (FCW_TOOLBAR) or for a status bar window (FCW_STATUS).</param>
        /// <param name="uMsg">The message to be sent to the control.</param>
        /// <param name="wParam">The value depends on the message specified in the uMsg parameter.</param>
        /// <param name="lParam">The value depends on the message specified in the uMsg parameter.</param>
        /// <param name="pret">The address of the return value of the SendMessage function.</param>
        /// <returns>Returns S_OK if successful, or a COM-defined error value otherwise.</returns>
        [PreserveSig]
        int SendControlMsg(uint id, uint uMsg, short wParam, long lParam, ref long pret);

        /// <summary>
        /// Retrieves the currently active (displayed) Shell view object.
        /// </summary>
        /// <param name="ppshv">The address of the pointer to the currently active Shell view object.</param>
        /// <returns>Returns S_OK if successful, or a COM-defined error value otherwise.</returns>
        [PreserveSig]
        int QueryActiveShellView(ref IShellView ppshv);

        /// <summary>
        /// Called by the Shell view when the view window or one of its child windows gets the focus or becomes active.
        /// </summary>
        /// <param name="pshv">Address of the view object's IShellView pointer.</param>
        /// <returns>Returns S_OK if successful, or a COM-defined error value otherwise.</returns>
        [PreserveSig]
        int OnViewWindowActive(IShellView pshv);

        /// <summary>
        /// Adds toolbar items to Windows Explorer's toolbar.
        /// </summary>
        /// <param name="lpButtons">The address of an array of TBBUTTON structures.</param>
        /// <param name="nButtons">The number of TBBUTTON structures in the lpButtons array.</param>
        /// <param name="uFlags">Flags specifying where the toolbar buttons should go. This parameter can be one or more of the following values.</param>
        /// <returns>Returns S_OK if successful, or a COM-defined error value otherwise.</returns>
        [PreserveSig]
        int SetToolbarItems(IntPtr lpButtons, uint nButtons, uint uFlags);
    }
}