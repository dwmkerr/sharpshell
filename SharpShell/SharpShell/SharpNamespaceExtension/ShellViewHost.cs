using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using SharpShell.Interop;

namespace SharpShell.SharpNamespaceExtension
{
    /// <summary>
    /// The ShellViewHost is the window created in the to host custom shell views.
    /// </summary>
    internal class ShellViewHost : UserControl, IShellView
    {
        private Control customView  ;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellViewHost"/> class.
        /// </summary>
        public ShellViewHost()
        {
            //  Initialize the component.
            InitializeComponent();

            //  Invisible by default.
            Visible = false;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ShellViewHost
            // 
            this.Name = "ShellViewHost";
            this.ResumeLayout(false);

            BackColor = Color.White;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

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

        int IShellView.TranslateAccelerator(ref MSG lpmsg)
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

        int IShellView.CreateViewWindow(IShellView psvPrevious, ref FOLDERSETTINGS pfs, IShellBrowser psb, ref RECT prcView,
            out IntPtr phWnd)
        {
            phWnd = customView.Handle;
            //  TODO
            return WinError.S_OK;
        }

        int IShellView.DestroyViewWindow()
        {
            //  TODO
            return WinError.S_OK;
        }

        int IShellView.GetCurrentInfo(out FOLDERSETTINGS pfs)
        {
            pfs = new FOLDERSETTINGS {fFlags = 0, ViewMode = FOLDERVIEWMODE.FVM_AUTO};
            return WinError.S_OK;
        }

        int IShellView.AddPropertySheetPages(uint dwReserved, IntPtr lpfn, IntPtr lparam)
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
            //  TODO
            return WinError.S_OK;
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
    }
}
