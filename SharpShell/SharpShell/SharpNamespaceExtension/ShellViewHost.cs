using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Forms;
using SharpShell.Interop;

namespace SharpShell.SharpNamespaceExtension
{
    /// <summary>
    /// The ShellViewHost is the window created in the to host custom shell views.
    /// </summary>
    internal class ShellViewHost : IShellView
    {
        private Control customView;

        public ShellViewHost(Control customView)
        {
            this.customView = customView;
        }

        int IOleWindow.GetWindow(out IntPtr phwnd)
        {
            phwnd = customView.Handle;
            //  TODO
            return WinError.S_OK;
        }

        int IShellView.ContextSensitiveHelp(bool fEnterMode)
        {
            //  TODO
            return WinError.S_OK;
        }

        int IShellView.TranslateAcceleratorA(MSG lpmsg)
        {
            //  TODO
            return WinError.S_OK;
        }

        int IShellView.EnableModeless(bool fEnable)
        {
            //  TODO
            return WinError.S_OK;
        }

        int IShellView.UIActivate(SVUIA_STATUS uState)
        {
            //  TODO
            return WinError.S_OK;
        }

        int IShellView.Refresh()
        {
            //  TODO
            return WinError.S_OK;
        }

        int IShellView.CreateViewWindow([In, MarshalAs(UnmanagedType.Interface)] IShellView psvPrevious,
             [In] ref FOLDERSETTINGS pfs, [In, MarshalAs(UnmanagedType.Interface)] IShellBrowser psb, [In]  ref RECT prcView, [In, Out] ref IntPtr phWnd)
        {
            //  Store the shell browser.
            shellBrowser = psb;

            //  Resize the custom view.
            customView.Bounds = new Rectangle(prcView.left, prcView.top, prcView.Width(), prcView.Height());
            customView.Visible = true;
            
            //  Set the handle to the handle of the custom view.
            phWnd = customView.Handle;

            //  Set the custom view to be a child of the shell browser.
            IntPtr parentWindowHandle;
            psb.GetWindow(out parentWindowHandle);
            User32.SetParent(phWnd, parentWindowHandle);

            //  TODO: finish this function off.
            return WinError.S_OK;
        }

        /// <summary>
        /// Destroys the view window.
        /// </summary>
        /// <returns>
        /// Returns a success code if successful, or a COM error code otherwise.
        /// </returns>
        int IShellView.DestroyViewWindow()
        {
            //  Hide the view window, remove it from the parent.
            customView.Visible = false;
            User32.SetParent(customView.Handle, IntPtr.Zero);

            //  Dispose of the view window.
            customView.Dispose();

            //  And we're done.
            return WinError.S_OK;
        }

        int IShellView.GetCurrentInfo(ref FOLDERSETTINGS pfs)
        {
            pfs = new FOLDERSETTINGS {fFlags = 0, ViewMode = FOLDERVIEWMODE.FVM_AUTO};
            return WinError.S_OK;
        }

        int IShellView.AddPropertySheetPages(long dwReserved, ref IntPtr lpfn, IntPtr lparam)
        {
            //  TODO
            return WinError.S_OK;
        }

        int IShellView.SaveViewState()
        {
            //  TODO
            return WinError.S_OK;
        }

        int IShellView.SelectItem(IntPtr pidlItem, _SVSIF uFlags)
        {
            //  TODO
            return WinError.S_OK;
        }

        int IShellView.GetItemObject(_SVGIO uItem, ref Guid riid, ref IntPtr ppv)
        {
            // Returning S_OK will cause Explorer to crash when navigating away from namespace View
            return WinError.E_NOTIMPL;
        }

        int IShellView.GetWindow(out IntPtr phwnd)
        {
            phwnd = customView.Handle;
            //  TODO
            return WinError.S_OK;
        }

        int IOleWindow.ContextSensitiveHelp(bool fEnterMode)
        {
            //  TODO
            return WinError.S_OK;
        }

        private IShellBrowser shellBrowser;
    }
}
