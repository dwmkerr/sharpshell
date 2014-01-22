using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SharpShell.Interop;
using IServiceProvider = SharpShell.Interop.IServiceProvider;

namespace ServerManager.ShellDebugger
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public partial class ShellDebuggerForm : Form, IShellBrowser, IServiceProvider
    {
        public ShellDebuggerForm()
        {
            InitializeComponent();

            //  Get the desktop folder PIDL and interface.
            Shell32.SHGetFolderLocation(IntPtr.Zero, CSIDL.CSIDL_DESKTOP, IntPtr.Zero, 0, out desktopFolderPidl);
            Shell32.SHGetDesktopFolder(out desktopFolder);

            shellTreeView.OnShellItemSelected += shellTreeView_OnShellItemSelected;
        }

        void shellTreeView_OnShellItemSelected(object sender, ShellTreeEventArgs e)
        {
            //  TODO: Update the browser.
         //   ((IShellBrowser) this).BrowseObject(e.ShellItem.PIDL, SBSP.SBSP_SAMEBROWSER);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            ((IShellBrowser)this).BrowseObject(desktopFolderPidl,
                                        SBSP.SBSP_ABSOLUTE);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            currentAbsolutePidl = IntPtr.Zero;

            // Release the IShellView
            if (shellView != null)
            {
                shellView.UIActivate((uint)SVUIA_STATUS.SVUIA_DEACTIVATE);
                shellView.DestroyViewWindow();

                //  The shell view may have come from COM but may be a SharpShell view, so check if it's COM
                //  before we release it.
                if(Marshal.IsComObject(shellView))
                    Marshal.ReleaseComObject(shellView);
                
                shellView = null;
            }

            base.OnHandleDestroyed(e);
        }

        #region IOleBrowser implementation

        int IOleWindow.GetWindow(out IntPtr phwnd)
        {
            return ((IShellBrowser)this).GetWindow(out phwnd);
        }

        int IOleWindow.ContextSensitiveHelp(bool fEnterMode)
        {
            return ((IShellBrowser)this).ContextSensitiveHelp(fEnterMode);
        }

        #endregion

        #region IShellBrowser implementation

        int IShellBrowser.GetWindow(out IntPtr phwnd)
        {
            phwnd = GetFolderViewHost();
            return WinError.S_OK;
        }

        int IShellBrowser.ContextSensitiveHelp(bool fEnterMode)
        {
            return WinError.E_NOTIMPL;
        }

        int IShellBrowser.InsertMenusSB(IntPtr hmenuShared, OLEMENUGROUPWIDTHS lpMenuWidths)
        {
            return WinError.E_NOTIMPL;
        }

        int IShellBrowser.SetMenuSB(IntPtr hmenuShared, IntPtr holemenuRes, IntPtr hwndActiveObject)
        {
            return WinError.E_NOTIMPL;
        }

        int IShellBrowser.RemoveMenusSB(IntPtr hmenuShared)
        {
            return WinError.E_NOTIMPL;
        }

        int IShellBrowser.SetStatusTextSB(string pszStatusText)
        {
            return WinError.E_NOTIMPL;
        }

        int IShellBrowser.EnableModelessSB(bool fEnable)
        {
            return WinError.E_NOTIMPL;
        }

        int IShellBrowser.TranslateAcceleratorSB(MSG pmsg, short wID)
        {
            return WinError.S_OK;
        }

        int IShellBrowser.BrowseObject(IntPtr pidl, SBSP wFlags)
        {
            if (this.InvokeRequired)
            {
                AutoResetEvent theEvent = new AutoResetEvent(false);
                int result = WinError.E_FAIL;
                this.Invoke((Action)(() =>
                {
                    result = ((IShellBrowser)this).BrowseObject(pidl, wFlags);
                    theEvent.Set();
                }));
                theEvent.WaitOne();
                return result;
            }

            int hr;
            IntPtr folderTmpPtr;
            IShellFolder folderTmp;
            IntPtr pidlTmp;

            //  We'll need the shell folder GUID.
            var shellFolderGuid = typeof (IShellFolder).GUID;
            var shellViewGuid = typeof (IShellView).GUID;

            //  Check to see if we have a desktop pidl, relative pidl or absolite pidl.
            if (Shell32.ILIsEqual(pidl, desktopFolderPidl))
            {
                //  The provided PIDL is the desktop folder.
                pidlTmp = Shell32.ILClone(desktopFolderPidl);
                folderTmp = desktopFolder;
            }
            else if ((wFlags & SBSP.SBSP_RELATIVE) != 0)
            {
                // SBSP_RELATIVE - pidl is relative from the current folder
                if ((hr = currentFolder.BindToObject(pidl, IntPtr.Zero,
                    ref shellFolderGuid,
                    out folderTmpPtr)) != WinError.S_OK)
                    return hr;
                pidlTmp = Shell32.ILCombine(currentAbsolutePidl, pidl);
                folderTmp = (IShellFolder)Marshal.GetObjectForIUnknown(folderTmpPtr);
            }
            else
            {
                // SBSP_ABSOLUTE - pidl is an absolute pidl (relative from desktop)
                pidlTmp = Shell32.ILClone(pidl);
                if ((hr = desktopFolder.BindToObject(pidlTmp, IntPtr.Zero,
                    ref shellFolderGuid,
                    out folderTmpPtr)) != WinError.S_OK)
                    return hr;
                folderTmp = (IShellFolder)Marshal.GetObjectForIUnknown(folderTmpPtr);
            }

            if (folderTmp == null)
            {
                Shell32.ILFree(pidlTmp);
                return WinError.E_FAIL;
            }

            // Check that we have a new pidl
            if (Shell32.ILIsEqual(pidlTmp, currentAbsolutePidl))
            {
                Marshal.ReleaseComObject(folderTmp);
                Shell32.ILFree(pidlTmp);
                return WinError.S_OK;
            }

            currentFolder = folderTmp;

            FOLDERSETTINGS fs = new FOLDERSETTINGS();
            IShellView lastIShellView = shellView;

            if (lastIShellView != null)
                lastIShellView.GetCurrentInfo(out fs);
            // Copy the old folder settings
            else
            {
                fs = new FOLDERSETTINGS();
                fs.fFlags = folderFlags;
                fs.ViewMode = folderViewMode;
            }

            // Create the IShellView
            IntPtr iShellViewPtr;
            hr = folderTmp.CreateViewObject(Handle,
                 ref shellViewGuid, out iShellViewPtr);
            if (hr == WinError.S_OK)
            {
                shellView = (IShellView)
                               Marshal.GetObjectForIUnknown(iShellViewPtr);

                hWndListView = IntPtr.Zero;
                RECT rc =
                    new RECT(8, 8,
                   ClientSize.Width - 8,
                   ClientSize.Height - 8);

                int res;

                try
                {
                    // Create the actual list view.
                    res = shellView.CreateViewWindow(lastIShellView, ref fs,
                          this, ref rc, out hWndListView);
                }
                catch (COMException)
                {
                    return WinError.E_FAIL;
                }

                if (res < 0)
                    return WinError.E_FAIL;

                // Release the old IShellView
                if (lastIShellView != null)
                {
                    lastIShellView.GetCurrentInfo(out fs);
                    lastIShellView.UIActivate(SVUIA_STATUS.SVUIA_DEACTIVATE);
                    lastIShellView.DestroyViewWindow();
                    Marshal.ReleaseComObject(lastIShellView);
                }

                // Set focus to the IShellView
                shellView.UIActivate(SVUIA_STATUS.SVUIA_ACTIVATE_FOCUS);
                currentAbsolutePidl = pidlTmp;
            }

            return WinError.S_OK;
        }

        int IShellBrowser.GetViewStateStream(uint grfMode, out IStream ppStrm)
        {
            ppStrm = null;
            return WinError.E_NOTIMPL;
        }

        int IShellBrowser.GetControlWindow(uint id, out IntPtr phwnd)
        {
            phwnd = IntPtr.Zero;
            return WinError.S_FALSE;
        }

        int IShellBrowser.SendControlMsg(uint id, uint uMsg, IntPtr wParam, IntPtr lParam, out IntPtr pret)
        {
            pret = IntPtr.Zero;
            return WinError.E_NOTIMPL;
        }

        int IShellBrowser.QueryActiveShellView(out IShellView ppshv)
        {
            Marshal.AddRef(Marshal.GetIUnknownForObject(shellView));
            ppshv = shellView;
            return WinError.S_OK;
        }

        int IShellBrowser.OnViewWindowActive(IShellView pshv)
        {
            return WinError.E_NOTIMPL;
        }

        int IShellBrowser.SetToolbarItems(TBBUTTON[] lpButtons, uint nButtons, uint uFlags)
        {
            return WinError.E_NOTIMPL;
        }
        #endregion

        #region IServiceProvider implementation

        int IServiceProvider.QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject)
        {
            var shellBrowserGuid = typeof (IShellBrowser).GUID;

            if (riid == shellBrowserGuid)
            {
                ppvObject = Marshal.GetComInterfaceForObject(this, typeof (IShellBrowser));
                return WinError.S_OK;
            }

            ppvObject = IntPtr.Zero;
            return WinError.E_NOINTERFACE;
        }

        #endregion

        private IShellFolder currentFolder;
        private IntPtr currentAbsolutePidl;
        private IShellView shellView;
        private IntPtr hWndListView;
        private readonly IShellFolder desktopFolder;
        private readonly IntPtr desktopFolderPidl;

        private readonly FOLDERVIEWMODE folderViewMode = FOLDERVIEWMODE.FVM_DETAILS;

        private readonly FOLDERFLAGS folderFlags =  FOLDERFLAGS.FWF_SHOWSELALWAYS |
                                                   FOLDERFLAGS.FWF_SINGLESEL | FOLDERFLAGS.FWF_NOWEBVIEW;

        private IntPtr GetFolderViewHost()
        {
            return splitContainerTreeAndDetails.Panel2.Handle;
        }
    }
}
