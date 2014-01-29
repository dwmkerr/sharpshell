using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace MSDNMagazine.Shell
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	/// 

	public delegate void OnDefaultAction(ShellActionType t);

	[Guid("0979A716-5B17-4e0c-BF72-EDAE9EA592DF")]
	public class ShellFolder : MSDNMagazine.Shell.HelperItems.IPersistFolder, MSDNMagazine.Shell.HelperItems.IShellFolder
	{
		public ShellView m_view = null;
		internal byte[] m_pidlData = null;
		internal MMC.MyDataObject m_mdo = null;

		private string m_workingDir = "";
		private string m_currentItem = "";
		internal Form m_form = null;

		private IntPtr m_parentWindow = IntPtr.Zero;
		private string m_parentWindowText = "";

		public string ParentWindowText 
		{
			get 
			{
				return m_parentWindowText;
			}
		}

		public IntPtr ParentWindow 
		{
			get 
			{
				return m_parentWindow;
			}
		}

		private string WorkingDirectory 
		{
			get 
			{
				return m_workingDir;
			}

			set
			{
				if ( value[value.Length - 1] != '\\' ) 
				{
					m_workingDir = value + "\\";
				}
				else
				{
					m_workingDir = value;
				}
			}
		}

		public string SelectedItem
		{
			get 
			{
				return m_currentItem;
			}

			set
			{
				if ( value.IndexOf("\\") == -1 ) 
				{
					m_currentItem = m_workingDir + value;
				}
				else
				{
					m_currentItem = value;
				}

				MSDNMagazine.Shell.HelperItems.ICommDlgBrowser cdlg = null;

				try 
				{
					cdlg = (MSDNMagazine.Shell.HelperItems.ICommDlgBrowser) Marshal.GetObjectForIUnknown( Marshal.GetComInterfaceForObject(this.m_view.m_shell, typeof(MSDNMagazine.Shell.HelperItems.ICommDlgBrowser)) );
				}
				catch 
				{
					return;
				}
				
				cdlg.OnStateChange( this.m_view, 0x00000002 );

			}
		}

		internal OnDefaultAction OnAction = null;

		public ShellFolder()
		{
			this.m_view = new ShellView(this);
			m_mdo = new MMC.MyDataObject(0, new byte[]{});
		}

		[DllImport("user32.dll")]
		internal static extern IntPtr SetParent( IntPtr hWndChild, IntPtr hWndNewParent );

		[DllImport("user32.dll")]
		internal static extern int SetWindowLong( IntPtr hWnd, int nIndex, int dwNewLong );

		[DllImport("user32.dll")]
		internal static extern int GetWindowLong( IntPtr hWnd, int nIndex );

		[DllImport("user32.dll")]
		internal static extern bool SetWindowPos( IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags );

		[DllImport("shell32.dll")]
		internal static extern bool SHGetPathFromIDList( IntPtr pidl, System.Text.StringBuilder pszPath );

		[DllImport("kernel32.dll")]
		internal static extern bool IsBadWritePtr( IntPtr lp, int ucb );

		[DllImport("kernel32.dll")]
		internal static extern bool IsBadReadPtr( IntPtr lp, int ucb );

		[DllImport("shlwapi.dll")]
		internal static extern bool PathMakeSystemFolder( string pszPath );

		private byte[] PidlToBytes( IntPtr pidl ) 
		{

			// compute the total size..

			ushort total = 0;
			ushort cb = (ushort) Marshal.ReadInt16(pidl);

			while ( cb != 0 ) 
			{
				total += cb;

				cb = (ushort) Marshal.ReadInt16( pidl, total );
			}

			// allocate...

			byte[] ret = new byte[total];


			// read...

			Marshal.Copy( pidl, ret, 0, total );

			// return...

			return ret;

		}

		void MSDNMagazine.Shell.HelperItems.IPersistFolder.GetClassID( ref Guid pClassID) 
		{

			pClassID = new Guid("0979A716-5B17-4e0c-BF72-EDAE9EA592DF");
		}

		long MSDNMagazine.Shell.HelperItems.IPersistFolder.Initialize( IntPtr pidl ) 
		{

			this.m_pidlData = this.PidlToBytes( pidl );

			this.m_mdo = new MMC.MyDataObject(0, this.m_pidlData);

			return 0;

		}

		void MSDNMagazine.Shell.HelperItems.IShellFolder.ParseDisplayName( IntPtr hwnd, IntPtr pbc, string pszDisplayName, out uint pchEaten, out IntPtr /*MSDNMagazine.Shell.HelperItems.SHITEMID*/ ppidl, ref uint pdwAttributes) 
		{
			pchEaten = (uint) pszDisplayName.Length;
			pdwAttributes = 0;

			ppidl = this.m_mdo.m_pidl;
		}

		void MSDNMagazine.Shell.HelperItems.IShellFolder.EnumObjects( int hwnd, int grfFlags, ref MSDNMagazine.Shell.HelperItems.IEnumIDList ppenumIDList) 
		{
			ppenumIDList = null;
		}

		void MSDNMagazine.Shell.HelperItems.IShellFolder.BindToObject( IntPtr pidl, IntPtr pbc, ref Guid riid, ref IntPtr ppv) 
		{

			IntPtr iunkMe = Marshal.GetIUnknownForObject(this);

			Marshal.QueryInterface( iunkMe, ref riid, out ppv );
		}

		void MSDNMagazine.Shell.HelperItems.IShellFolder.BindToStorage( IntPtr pidl, IntPtr pbc, ref Guid riid, ref IntPtr ppv) 
		{
			ppv = IntPtr.Zero;
		}

		void MSDNMagazine.Shell.HelperItems.IShellFolder.CompareIDs( int lParam, IntPtr pidl1, IntPtr pidl2) 
		{
		}

		long MSDNMagazine.Shell.HelperItems.IShellFolder.CreateViewObject( IntPtr hwndOwner, ref Guid riid, out IntPtr ppv) 
		{
			long hr = 0x00;

			ppv = IntPtr.Zero;

			if ( riid == typeof(MSDNMagazine.Shell.HelperItems.IShellView).GUID )
				ppv = Marshal.GetComInterfaceForObject( this.m_view, typeof(MSDNMagazine.Shell.HelperItems.IShellView) );

			if ( ppv.ToInt32() == 0 )
				hr = 0x8000FFFFL;

			this.m_parentWindow = hwndOwner;

			System.Text.StringBuilder sb = new System.Text.StringBuilder(1024);

			MSDNMagazine.Shell.HelperItems.HookStuff.GetWindowText( this.m_parentWindow, sb, 1024 );

			this.m_parentWindowText = sb.ToString();

			return hr;

		}

		long MSDNMagazine.Shell.HelperItems.IShellFolder.GetAttributesOf( uint cidl, IntPtr apidl, ref int rgfInOut) 
		{
			if ( cidl == 1 ) 
			{
				rgfInOut &= 0x40000000;
			}
			else
			{
				rgfInOut = 0;
			}

			return 0;
		}

		void MSDNMagazine.Shell.HelperItems.IShellFolder.GetUIObjectOf( IntPtr hwndOwner, uint cidl, IntPtr apidl, ref Guid riid, ref uint rgfReserved, ref IntPtr ppv) 
		{
			ppv = IntPtr.Zero;
		}

		void MSDNMagazine.Shell.HelperItems.IShellFolder.GetDisplayNameOf( IntPtr pidl, uint uFlags, out MSDNMagazine.Shell.HelperItems.STRRET pName) 
		{
			pName.sString = this.m_workingDir;
			pName.uType = 0;

			if ( pidl.ToInt32() == 0 )
				throw( new COMException(null,0x8000) );
			

			short cookie = Marshal.ReadInt16( pidl, 2 );

			if ( cookie == 255 ) 
			{
				pName.sString = m_currentItem;
			}

		}

		void MSDNMagazine.Shell.HelperItems.IShellFolder.SetNameOf( IntPtr hwnd, IntPtr pidl, IntPtr pszName, long uFlags, out IntPtr ppidlOut) 
		{
			ppidlOut = IntPtr.Zero;
		}

		public void DoDefaultAction() 
		{

			MSDNMagazine.Shell.HelperItems.ICommDlgBrowser cdlg = null;
			
			try 
			{
				cdlg = (MSDNMagazine.Shell.HelperItems.ICommDlgBrowser) Marshal.GetObjectForIUnknown( Marshal.GetComInterfaceForObject(this.m_view.m_shell, typeof(MSDNMagazine.Shell.HelperItems.ICommDlgBrowser)) );
			}
			catch 
			{
				return;
			}

			cdlg.OnDefaultCommand(this.m_view);				
		}

		[ComRegisterFunctionAttribute]
		public static void RegisterFunction(Type t)
		{
		
			try 
			{
				// add the correct things to the CLSID so the thing works as an extension
				RegistryKey CLSID = Registry.ClassesRoot.OpenSubKey("CLSID");

				RegistryKey kClass = null;

				kClass = CLSID.OpenSubKey( "{" + t.GUID.ToString() + "}", true );

				RegistryKey ProgId = kClass.OpenSubKey("ProgId");

				kClass.SetValue( null, (string) ProgId.GetValue(null) );

				ProgId.Close();

				RegistryKey ShellFolder = kClass.CreateSubKey("ShellFolder");

				ShellFolder.SetValue( "Attributes", 0x78000040 );
				ShellFolder.SetValue( "WantsFORPARSING", "" );

				ShellFolder.Close();
				kClass.Close();
				CLSID.Close();

				// add it to the approved list of extensions
				RegistryKey Approved = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Shell Extensions\\Approved", true);

				Approved.SetValue( "{" + t.GUID.ToString() + "}", t.FullName );
				Approved.Close();


			}
			catch( Exception e ) 
			{
				MessageBox.Show( e.Message );
				return;
			}
		}
   
		[ComUnregisterFunctionAttribute]
		public static void UnregisterFunction(Type t)
		{
			RegistryKey CLSID = Registry.ClassesRoot.OpenSubKey("CLSID", true);

			CLSID.DeleteSubKeyTree( "{" + t.GUID.ToString() + "}" );
			CLSID.Close();

			RegistryKey Approved = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Shell Extensions\\Approved", true);

			Approved.DeleteValue( "{" + t.GUID.ToString() + "}", false );
			Approved.Close();


		}

		protected void Setup( string WorkingDirectory, Shell.OnDefaultAction EventToFire, Form FormToShow ) 
		{
			this.WorkingDirectory = WorkingDirectory;
			this.OnAction += EventToFire;
			this.m_form = FormToShow;
		}
	}

	[ComVisible(false)]
	public enum ShellActionType 
	{
		Open = 0,
		Save = 1,
		Unknown = 2
	}

}
