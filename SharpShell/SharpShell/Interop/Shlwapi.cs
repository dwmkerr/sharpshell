using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace SharpShell.Interop
{
    [SuppressUnmanagedCodeSecurity, ComVisible(false)]
    public class Shlwapi
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Int32 StrRetToBuf(IntPtr pstr, IntPtr pidl, StringBuilder pszBuf, int cchBuf);
    }
}