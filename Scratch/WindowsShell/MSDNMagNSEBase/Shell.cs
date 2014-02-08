using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MSDNMagazine.Shell.HelperItems
{
	/// <summary>
	/// 
	/// </summary>
	/// 

	[ComVisible(false)]
	internal class HookStuff 
	{

		[DllImport("user32.Dll")]
		internal static extern IntPtr SetWindowsHookEx( int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId );

		[DllImport("user32.Dll")]
		internal static extern bool UnhookWindowsHookEx( IntPtr hhk );

		[DllImport("user32.Dll")]
		internal static extern long CallNextHookEx( 
			IntPtr hhk,  // handle to current hook
			int nCode,	 // hook code passed to hook procedure
			uint wParam, // value passed to hook procedure
			long lParam	 // value passed to hook procedure
		);

		[DllImport("kernel32.dll")]
		internal static extern uint GetCurrentThreadId();

		[DllImport("user32.dll")]
		internal static extern int GetWindowText(
			IntPtr hWnd,					    // handle to window or control
			System.Text.StringBuilder lpString,	// text buffer
			int nMaxCount						// maximum number of characters to copy
			);

		[DllImport("user32.dll")]
		internal static extern uint RegisterWindowMessage( string lpString );

		[DllImport("user32.dll")]
		internal static extern IntPtr SetFocus( IntPtr hwnd );

		[DllImport("user32.dll")]
		internal static extern int GetDlgCtrlID( IntPtr hwndCtl );


		internal delegate long HookProc( int nCode, uint wParam, int lParam );

	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct MSG 
	{
		internal IntPtr   hwnd; 
		internal uint   message; 
		internal uint wParam; 
		internal long lParam; 
		internal uint  time; 
		internal int x;
		internal int y;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct CWPSTRUCT 
	{ 
		internal int    lParam; 
		internal uint   wParam; 
		internal uint   message; 
		internal IntPtr hwnd; 
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct CWPRETSTRUCT 
	{ 
		internal int	  lResult;
		internal int    lParam; 
		internal uint   wParam; 
		internal uint   message; 
		internal IntPtr hwnd; 
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct NMHDR 
	{ 
		internal uint hwndFrom; 
		internal uint idFrom; 
		internal uint code; 
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct SHITEMID 
	{
		public UInt16 cb;
		public Int16 cookie;
		public UInt16 terminator;

		public SHITEMID( short val )
		{
			if ( val == 0 ) 
			{
				cb = 0;
			}
			else
			{
				cb=4;
			}

			cookie = val;
			terminator = 0;
		}
	}

	[ComImport(), Guid("000214F2-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IEnumIDList 
	{
		void Next( UInt32 celt, ref SHITEMID rgelt, ref UInt32 pceltFetched);
		void Skip( UInt32 celt);
		void Reset();
		void Clone( ref IEnumIDList ppenum);
	}

	[ComVisible(false)]
	internal class EnumIDList : IEnumIDList
	{

		private System.Collections.ArrayList m_items = null;
		private uint m_position = 0;

		internal EnumIDList( object[] cookies )
		{
			if (cookies.Length == 0)
				return;

			this.m_items = new System.Collections.ArrayList( cookies );
			this.m_position = 0;
		}

		internal EnumIDList( MSDNMagazine.Shell.HelperItems.EnumIDList il )
		{
			this.m_items = new System.Collections.ArrayList( il.m_items );
			this.m_position = il.m_position;
		}

		void IEnumIDList.Next( UInt32 celt, ref SHITEMID rgelt, ref UInt32 pceltFetched) 
		{
			if ( m_position < (this.m_items.Count - 1) )
			{

				m_position++;

				rgelt = new MSDNMagazine.Shell.HelperItems.SHITEMID( (short) this.m_items[(int) this.m_position] );
				pceltFetched = 1;

			}
			else
			{
				rgelt = new MSDNMagazine.Shell.HelperItems.SHITEMID( 0 );
				pceltFetched = 0;

			}
		}

		void IEnumIDList.Skip( UInt32 celt)
		{
			this.m_position += celt;
		}

		void IEnumIDList.Reset()
		{
			this.m_position = 0;
		}

		void IEnumIDList.Clone( ref IEnumIDList ppenum)
		{
			ppenum = new MSDNMagazine.Shell.HelperItems.EnumIDList(this);
		}
	}

	[ComImport(), Guid("0000010c-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IPersist
	{
		void GetClassID( ref Guid pClassID);
	}

	[ComImport(), Guid("000214EA-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IPersistFolder
	{
		void GetClassID( ref Guid pClassID);

		[PreserveSig]
		long Initialize( IntPtr pidl );
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct STRRET 
	{
	
		public UInt32 uType;
		
		[MarshalAs(UnmanagedType.LPWStr)] 
		public string sString;

	}

	[ComImport(), Guid("000214E6-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IShellFolder 
	{
		void ParseDisplayName( IntPtr hwnd, IntPtr pbc, /*IntPtr*/[MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, out uint pchEaten, out IntPtr /*MSDNMagazine.Shell.HelperItems.SHITEMID*/ ppidl, ref uint pdwAttributes);
		void EnumObjects( int hwnd, int grfFlags, ref MSDNMagazine.Shell.HelperItems.IEnumIDList ppenumIDList);
		void BindToObject( IntPtr pidl, IntPtr pbc, ref Guid riid, ref IntPtr ppv);
		void BindToStorage( IntPtr pidl, IntPtr pbc, ref Guid riid, ref IntPtr ppv);
		void CompareIDs( int lParam, IntPtr pidl1, IntPtr pidl2);

		[PreserveSig]
		long CreateViewObject( IntPtr hwndOwner, ref Guid riid, out IntPtr ppv);

		[PreserveSig]
		long GetAttributesOf( uint cidl, IntPtr apidl, ref int rgfInOut);
		void GetUIObjectOf( IntPtr hwndOwner, uint cidl, IntPtr apidl, ref Guid riid, ref uint rgfReserved, ref IntPtr ppv);
		void GetDisplayNameOf( IntPtr pidl, uint uFlags, out MSDNMagazine.Shell.HelperItems.STRRET pName);
		void SetNameOf( IntPtr hwnd, IntPtr pidl, IntPtr pszName, long uFlags, out IntPtr ppidlOut);
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct FOLDERSETTINGS 
	{
		public UInt32 ViewMode;
		public UInt32 fFlags;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct RECT 
	{
		public int left;
		public int top;
		public int right;
		public int bottom;
	}

	[ComImport(), Guid("00000114-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IOleWindow 
	{
		void GetWindow( out IntPtr phwnd);
		void ContextSensitiveHelp( bool fEnterMode);
	}

	[ComImport(), Guid("000214E3-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IShellView 
	{
		void GetWindow( out IntPtr phwnd);
	    void ContextSensitiveHelp( bool fEnterMode);
		
		[PreserveSig]
		long TranslateAcceleratorA( IntPtr pmsg);

        void EnableModeless( bool fEnable);
        void UIActivate( uint uState);
        void Refresh();
		void CreateViewWindow( IntPtr psvPrevious, ref MSDNMagazine.Shell.HelperItems.FOLDERSETTINGS pfs, IShellBrowser psb, ref MSDNMagazine.Shell.HelperItems.RECT prcView, ref IntPtr phWnd);
		void DestroyViewWindow();
        void GetCurrentInfo( ref MSDNMagazine.Shell.HelperItems.FOLDERSETTINGS pfs);
		void AddPropertySheetPages( long dwReserved, ref IntPtr pfnPtr, int lparam);
        void SaveViewState();
        void SelectItem( IntPtr pidlItem, uint uFlags);

		[PreserveSig]
        long GetItemObject( uint uItem, ref Guid riid, ref IntPtr ppv);
	}

	[ComImport(), Guid("000214E2-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IShellBrowser
	{
		void GetWindow( out IntPtr phwnd);
		void ContextSensitiveHelp( bool fEnterMode);
		void InsertMenusSB( IntPtr hmenuShared, ref IntPtr lpMenuWidths);
		void SetMenuSB( IntPtr hmenuShared, IntPtr holemenuRes, IntPtr hwndActiveObject);
		void RemoveMenusSB( IntPtr hmenuShared);
		void SetStatusTextSB( string pszStatusText);
		void EnableModelessSB( bool fEnable);
		void TranslateAcceleratorSB( IntPtr pmsg, short wID);
		void BrowseObject( IntPtr pidl, uint wFlags);
		void GetViewStateStream( long grfMode, ref UCOMIStream ppStrm);
		void GetControlWindow( uint id, ref IntPtr phwnd);
		void SendControlMsg( uint id, uint uMsg, short wParam, long lParam, ref long pret);
		void QueryActiveShellView( ref IShellView ppshv);
		void OnViewWindowActive( IShellView pshv);
		void SetToolbarItems( IntPtr lpButtons, uint nButtons, uint uFlags);
	}

	[ComImport(), Guid("000214F1-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ICommDlgBrowser
	{
		void OnDefaultCommand ( IShellView ppshv);
		void OnStateChange ( IShellView ppshv, uint uChange);
		void IncludeObject ( IShellView ppshv, IntPtr pidl);
	}
	
}
