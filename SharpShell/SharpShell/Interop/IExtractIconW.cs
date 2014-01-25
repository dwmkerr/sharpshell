using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpShell.Interop
{
    /// <summary>
    /// Exposes methods that allow a client to retrieve the icon that is associated with one of the objects in a folder.
    /// </summary>
    [ComImport]
    [Guid("000214FA-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IExtractIconW
    {
        /// <summary>
        /// Gets the location and index of an icon.
        /// </summary>
        /// <param name="uFlags">One or more of the following values. This parameter can also be NULL.</param>
        /// <param name="szIconFile">A pointer to a buffer that receives the icon location. The icon location is a null-terminated string that identifies the file that contains the icon.</param>
        /// <param name="cchMax">The size of the buffer, in characters, pointed to by pszIconFile.</param>
        /// <param name="piIndex">A pointer to an int that receives the index of the icon in the file pointed to by pszIconFile.</param>
        /// <param name="pwFlags">A pointer to a UINT value that receives zero or a combination of the following value</param>
        /// <returns></returns>
        [PreserveSig]
        int GetIconLocation(GILInFlags uFlags, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder szIconFile, int cchMax, out int piIndex, out GILOutFlags pwFlags);

        /// <summary>
        /// Extracts an icon image from the specified location.
        /// </summary>
        /// <param name="pszFile">A pointer to a null-terminated string that specifies the icon location.</param>
        /// <param name="nIconIndex">The index of the icon in the file pointed to by pszFile.</param>
        /// <param name="phiconLarge">A pointer to an HICON value that receives the handle to the large icon. This parameter may be NULL.</param>
        /// <param name="phiconSmall">A pointer to an HICON value that receives the handle to the small icon. This parameter may be NULL.</param>
        /// <param name="nIconSize">The desired size of the icon, in pixels. The low word contains the size of the large icon, and the high word contains the size of the small icon. The size specified can be the width or height. The width of an icon always equals its height.</param>
        /// <returns>
        /// Returns S_OK if the function extracted the icon, or S_FALSE if the calling application should extract the icon.
        /// </returns>
        [PreserveSig]
        int Extract([In, MarshalAs(UnmanagedType.LPWStr)] string pszFile, uint nIconIndex, out IntPtr phiconLarge, out IntPtr phiconSmall, uint nIconSize);
    }
}