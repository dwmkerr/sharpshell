using System;
using System.Runtime.InteropServices;
using MSDNMagazine.Shell;

namespace MSDNMagazineSampleNamespaceExtension
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>

	[ProgId("MSDN Magazine Sample Shell Namespace Extension")]
	[Guid("6B49E580-186E-4f8c-AB6A-E55D6F0F171D")]
	public class Class1 : MSDNMagazine.Shell.ShellFolder
	{
		public Class1()
		{
			//
			// TODO: Add constructor logic here
			//

			Form1 m = new Form1();
			
			m.m_folder = this;

			
			this.Setup( "c:\\temp\\test", new MSDNMagazine.Shell.OnDefaultAction(m.OnAction), m );
			

		}

	}
}
