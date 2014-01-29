using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text;

namespace MSDNMagazine.Shell
{
	/// <summary>
	/// 
	/// </summary>
	/// 

	[Guid("FB2EC55F-FB7B-432a-A54E-EDB524B951A6")]
	public class ShellView : MSDNMagazine.Shell.HelperItems.IShellView
	{
		internal ShellFolder m_mc = null;
		internal Form m_form = null;
		public MSDNMagazine.Shell.HelperItems.IShellBrowser m_shell = null;
		internal MSDNMagazine.Shell.HelperItems.FOLDERSETTINGS m_folderSettings = new MSDNMagazine.Shell.HelperItems.FOLDERSETTINGS();

		private IntPtr m_hhook = IntPtr.Zero;

		private MSDNMagazine.Shell.HelperItems.HookStuff.HookProc h = null;

		private bool m_fired = false;

		public ShellView( ShellFolder m ) 
		{
			this.m_mc = m;

			this.h = new MSDNMagazine.Shell.HelperItems.HookStuff.HookProc( this.OnDialogMsg );

		}

		void MSDNMagazine.Shell.HelperItems.IShellView.GetWindow( out IntPtr phwnd) 
		{
			phwnd = this.m_form.Handle;
		}

		void MSDNMagazine.Shell.HelperItems.IShellView.ContextSensitiveHelp( bool fEnterMode)
		{
		}

		long MSDNMagazine.Shell.HelperItems.IShellView.TranslateAcceleratorA( IntPtr pmsg)
		{
			return 1;
		}

		void MSDNMagazine.Shell.HelperItems.IShellView.EnableModeless( bool fEnable)
		{
		}

		void MSDNMagazine.Shell.HelperItems.IShellView.UIActivate( uint uState)
		{
			if ( uState == 0 ) //destroy
				this.m_form.Close();
		}

		void MSDNMagazine.Shell.HelperItems.IShellView.Refresh()
		{
		}

		private long OnDialogMsg( int nCode, uint wParam, int lParam ) 
		{
			
			IntPtr p = new IntPtr(lParam);

			MSDNMagazine.Shell.HelperItems.CWPRETSTRUCT m = (MSDNMagazine.Shell.HelperItems.CWPRETSTRUCT) Marshal.PtrToStructure( p, typeof(MSDNMagazine.Shell.HelperItems.CWPRETSTRUCT) );
			
			bool handled = false;

			if( m.message == 0x004E && handled == false ) //WM_NOTIFY
			{ 

				MSDNMagazine.Shell.HelperItems.NMHDR n = (MSDNMagazine.Shell.HelperItems.NMHDR) Marshal.PtrToStructure( new IntPtr(m.lParam), typeof(MSDNMagazine.Shell.HelperItems.NMHDR) );

				if ( n.code == 0xFFFFFDA2 ) // the special common dialog notifier 
				{
					handled = true;

					if ( this.m_mc.OnAction != null ) 
					{
						if ( !m_fired ) 
						{
							this.m_mc.OnAction( this.GetActionType() );
							m_fired = true;
						}
					}
				}

			}
			
			if ( m.message == 0x00F3 && handled == false ) // BN_SETSTATE
			{
				if ( m.wParam != 0 ) 
				{
					int id = MSDNMagazine.Shell.HelperItems.HookStuff.GetDlgCtrlID( m.hwnd );

					if ( id == 1 ) 
					{
						handled = true;

						if ( this.m_mc.OnAction != null ) 
						{
							if ( !m_fired ) 
							{
								this.m_mc.OnAction( this.GetActionType() );
								m_fired = true;
							}
						}
					}
				}
			}

			uint specialCode = MSDNMagazine.Shell.HelperItems.HookStuff.RegisterWindowMessage("WM_OBJECTSEL");

			if ( m.message == specialCode && m.lParam == 0x25 && handled == false ) //WM_OBJECTSEL
			{
				handled = true;

				if ( this.m_mc.OnAction != null ) 
				{
					this.m_mc.OnAction( this.GetActionType() );
					m_fired = true;
				}
			}

			return MSDNMagazine.Shell.HelperItems.HookStuff.CallNextHookEx( this.m_hhook, nCode, wParam, lParam );
		}

		private ShellActionType GetActionType() 
		{
			string text = this.m_mc.ParentWindowText;

			if ( text.ToLower().IndexOf("open") != -1 )
				return ShellActionType.Open;

			if ( text.ToLower().IndexOf("save") != -1 )
				return ShellActionType.Save;

			return ShellActionType.Unknown;
		}

		void MSDNMagazine.Shell.HelperItems.IShellView.CreateViewWindow( IntPtr psvPrevious, ref MSDNMagazine.Shell.HelperItems.FOLDERSETTINGS pfs, MSDNMagazine.Shell.HelperItems.IShellBrowser psb, ref MSDNMagazine.Shell.HelperItems.RECT prcView, ref IntPtr phWnd)
		{

			this.m_folderSettings.ViewMode = pfs.ViewMode;
			this.m_folderSettings.fFlags = pfs.fFlags;

			this.m_form = this.m_mc.m_form;

			IntPtr hwnd = IntPtr.Zero;

			this.m_shell = psb;
			this.m_shell.GetWindow(out hwnd);
						
			ShellFolder.SetParent(m_form.Handle, hwnd);

			phWnd = m_form.Handle;

			int w = prcView.right - prcView.left;
			int h = prcView.bottom - prcView.top;

			ShellFolder.SetWindowLong( m_form.Handle, -16, 0x40000000 );
			ShellFolder.SetWindowPos( m_form.Handle, 0, 0, 0, w, h, 0x0017 );

			m_form.Location = new System.Drawing.Point(prcView.left, prcView.top);
			m_form.Size = new System.Drawing.Size(prcView.right - prcView.left, prcView.bottom - prcView.top);

			m_form.Show();

			MSDNMagazine.Shell.HelperItems.HookStuff.SetFocus( hwnd );

//			CWPRETSTRUCT
			this.m_hhook = MSDNMagazine.Shell.HelperItems.HookStuff.SetWindowsHookEx( 12, this.h, IntPtr.Zero, MSDNMagazine.Shell.HelperItems.HookStuff.GetCurrentThreadId() );

		}

		void MSDNMagazine.Shell.HelperItems.IShellView.DestroyViewWindow()
		{

			this.m_form.Close();

			bool b = MSDNMagazine.Shell.HelperItems.HookStuff.UnhookWindowsHookEx(this.m_hhook);

		}

		void MSDNMagazine.Shell.HelperItems.IShellView.GetCurrentInfo( ref MSDNMagazine.Shell.HelperItems.FOLDERSETTINGS pfs)
		{
			pfs = this.m_folderSettings;
		}

		void MSDNMagazine.Shell.HelperItems.IShellView.AddPropertySheetPages( long dwReserved, ref IntPtr pfnPtr, int lparam)
		{
			pfnPtr = IntPtr.Zero;
		}

		void MSDNMagazine.Shell.HelperItems.IShellView.SaveViewState()
		{
		}

		void MSDNMagazine.Shell.HelperItems.IShellView.SelectItem( IntPtr pidlItem, uint uFlags)
		{
		}

		long MSDNMagazine.Shell.HelperItems.IShellView.GetItemObject( uint uItem, ref Guid riid, ref IntPtr ppv)
		{
			ppv = IntPtr.Zero;

			if ( riid == typeof(MMC.IDataObject).GUID && (uItem == 1 || uItem == 2) ) 
			{
				if ( uItem == 1 ) 
					this.m_mc.m_mdo = new MMC.MyDataObject(255, this.m_mc.m_pidlData);

				if ( uItem == 2 )
					this.m_mc.m_mdo = new MMC.MyDataObject(0, this.m_mc.m_pidlData);

				ppv = Marshal.GetComInterfaceForObject( this.m_mc.m_mdo, typeof(MMC.IDataObject) );
				return 0;
			}

			Marshal.QueryInterface( Marshal.GetIUnknownForObject(this), ref riid, out ppv );

			if ( ppv.ToInt32() == 0 ) 
				Marshal.QueryInterface( Marshal.GetIUnknownForObject(this.m_mc), ref riid, out ppv );

			if ( ppv.ToInt32() == 0 ) 
				return 0x80004002L;

			return 0;

		}

	}
}
