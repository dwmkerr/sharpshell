using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace SharpShell.Interop
{
    /// <summary>
    /// Shlwapi shlwapi.dll imports.
    /// </summary>
    [SuppressUnmanagedCodeSecurity, ComVisible(false)]
    public class Shlwapi
    {
        /// <summary>
        /// Converts an STRRET structure returned by IShellFolder::GetDisplayNameOf to a string, and places the result in a buffer.
        /// </summary>
        /// <param name="pstr">A pointer to the STRRET structure. When the function returns, this pointer will no longer be valid.</param>
        /// <param name="pidl">A pointer to the item's ITEMIDLIST structure.</param>
        /// <param name="pszBuf">A buffer to hold the display name. It will be returned as a null-terminated string. If cchBuf is too small, the name will be truncated to fit.</param>
        /// <param name="cchBuf">The size of pszBuf, in characters. If cchBuf is too small, the string will be truncated to fit.</param>
        /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [DllImport("shlwapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Int32 StrRetToBuf(IntPtr pstr, IntPtr pidl, StringBuilder pszBuf, int cchBuf);
    }
}