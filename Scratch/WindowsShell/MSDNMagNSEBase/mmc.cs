using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MMC
{

	//
	// Structures
	//

	[StructLayout(LayoutKind.Sequential)]
	internal struct FORMATETC 
	{
		internal ushort cfFormat;
		internal IntPtr ptd;
		internal int dwAspect;
		internal int lindex;
		internal int tymed;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct STGMEDIUM 
	{
		internal int tymed;
		internal IntPtr hGlobal;
		internal IntPtr pUnkForRelease;
	}

	//
	// IDataObject interface
	//

	[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000010e-0000-0000-C000-000000000046")]
	internal interface IDataObject 
	{
		void GetData( ref FORMATETC pformatetcIn, ref STGMEDIUM pmedium);
		void GetDataHere( ref FORMATETC pformatetc, ref STGMEDIUM pmedium);
		[PreserveSig] long QueryGetData( ref FORMATETC pformatetc);
		[PreserveSig] long GetCanonicalFormatEtc( ref FORMATETC pformatectIn, ref FORMATETC pformatetcOut);
		[PreserveSig] long SetData( ref FORMATETC pformatetc, ref STGMEDIUM pmedium, bool fRelease);
		[PreserveSig] long EnumFormatEtc( uint dwDirection, ref IntPtr ppenumFormatEtc);
		[PreserveSig] long DAdvise( ref FORMATETC pformatetc, uint advf, IntPtr pAdvSink, out uint pdwConnection);
        [PreserveSig] long DUnadvise( uint dwConnection);
        [PreserveSig] long EnumDAdvise( ref IntPtr ppenumAdvise);
	}
	
	//
	// a specific implementation of IDataObject
	// named MyDataObject
	//

	[ComVisible(false)]
	internal class MyDataObject : MMC.IDataObject 
	{
		
		internal byte[] m_data = null;
		internal short m_cookie = 0;
		internal IntPtr m_pidl = IntPtr.Zero;

		private ushort m_CCFShellIDListArry = MyDataObject.RegisterClipboardFormat("Shell IDList Array");
		
		internal MyDataObject( short cookie, byte[] parentPidl  ) 
		{

			m_data = new byte[parentPidl.Length];

			parentPidl.CopyTo( m_data, 0 );

			m_cookie = cookie;

			m_pidl = Marshal.AllocCoTaskMem(6);

			byte[] b = new Byte[6];

			BitConverter.GetBytes((short) 4).CopyTo(b, 0);
			BitConverter.GetBytes((short) 255).CopyTo(b, 2);
			BitConverter.GetBytes((short) 0).CopyTo(b, 4);

			Marshal.Copy( b, 0, m_pidl, 6 );

		}

		~MyDataObject() 
		{
			Marshal.FreeCoTaskMem(m_pidl);
		}

		void MMC.IDataObject.GetData( ref FORMATETC pformatetcIn, ref STGMEDIUM pmedium) 
		{
			if ( pformatetcIn.cfFormat == this.m_CCFShellIDListArry ) 
			{

				byte[] b = null;

				if ( m_cookie > 0 ) 
				{

					b = new byte[ this.m_data.Length + 18 ];

					BitConverter.GetBytes(1).CopyTo(b, 0);
					BitConverter.GetBytes(12).CopyTo(b, 4);
					BitConverter.GetBytes(12 + m_data.Length).CopyTo(b, 8);

					m_data.CopyTo( b, 12 );

					BitConverter.GetBytes((short) 4).CopyTo(b, 12 + m_data.Length);
					BitConverter.GetBytes((short) 255).CopyTo(b, 14 + m_data.Length);
					BitConverter.GetBytes((short) 0).CopyTo(b, 16 + m_data.Length);

				}
				else
				{
					b = new byte[ this.m_data.Length + 8 ];

					BitConverter.GetBytes(0).CopyTo(b, 0);
					BitConverter.GetBytes(8).CopyTo(b, 4);

					this.m_data.CopyTo( b, 8 );

				}

				pmedium.tymed = 1;
				pmedium.pUnkForRelease = IntPtr.Zero;

				pmedium.hGlobal = Marshal.AllocHGlobal( b.Length );
				Marshal.Copy(b, 0, pmedium.hGlobal, b.Length);

			}
		}

		void MMC.IDataObject.GetDataHere( ref FORMATETC pformatetc, ref STGMEDIUM pmedium) 
		{
		}

		long MMC.IDataObject.QueryGetData( ref FORMATETC pformatetc)
		{
			return 0x80004001L;
		}

		long MMC.IDataObject.GetCanonicalFormatEtc( ref FORMATETC pformatectIn, ref FORMATETC pformatetcOut)
		{
			return 0x80004001L;
		}

		long MMC.IDataObject.SetData( ref FORMATETC pformatetc, ref STGMEDIUM pmedium, bool fRelease)
		{
			return 0x80004001L;
		}

		long MMC.IDataObject.EnumFormatEtc( uint dwDirection, ref IntPtr ppenumFormatEtc)
		{
			return 0x80004001L;
		}

		long MMC.IDataObject.DAdvise( ref FORMATETC pformatetc, uint advf, IntPtr pAdvSink, out uint pdwConnection)
		{
			pdwConnection = 0;

			return 0x80004001L;
		}

		long MMC.IDataObject.DUnadvise( uint dwConnection)
		{
			return 0x80004001L;
		}

		long MMC.IDataObject.EnumDAdvise( ref IntPtr ppenumAdvise)
		{
			return 0x80004001L;
		}


		[DllImport("user32.Dll")]
		private static extern int GetClipboardFormatName( int format, System.Text.StringBuilder buf, int size );

		[DllImport("user32.Dll")]
		private static extern ushort RegisterClipboardFormat( string name );

		[DllImport("kernel32.dll")]
		private static extern IntPtr GlobalAlloc(
			uint uFlags,     // allocation attributes
			int dwBytes   // number of bytes to allocate
		);

		[DllImport("kernel32.dll")]
		private static extern IntPtr GlobalLock(
			IntPtr hMem   // handle to global memory object
		);

		[DllImport("kernel32.dll")]
		private static extern int GlobalUnLock(
			IntPtr hMem   // handle to global memory object
		);



	}
}