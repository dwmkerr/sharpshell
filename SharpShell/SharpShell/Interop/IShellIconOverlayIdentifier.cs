using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpShell.Interop
{
    /// <summary>
    /// Exposes methods that handle all communication between icon overlay handlers and the Shell.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("0c6c4200-c589-11d0-999a-00c04fd655e1")]
    public interface IShellIconOverlayIdentifier
    {
        /// <summary>
        /// Specifies whether an icon overlay should be added to a Shell object's icon.
        /// </summary>
        /// <param name="pwszPath">A Unicode string that contains the fully qualified path of the Shell object.</param>
        /// <param name="dwAttrib">The object's attributes. For a complete list of file attributes and their associated flags, see IShellFolder::GetAttributesOf.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int IsMemberOf([MarshalAs(UnmanagedType.LPWStr)] string pwszPath, FILE_ATTRIBUTE dwAttrib);

        /// <summary>
        /// Provides the location of the icon overlay's bitmap.
        /// </summary>
        /// <param name="pwszIconFile">A null-terminated Unicode string that contains the fully qualified path of the file containing the icon. The .dll, .exe, and .ico file types are all acceptable. You must set the ISIOI_ICONFILE flag in pdwFlags if you return a file name.</param>
        /// <param name="cchMax">The size of the pwszIconFile buffer, in Unicode characters.</param>
        /// <param name="pIndex">Pointer to an index value used to identify the icon in a file that contains multiple icons. You must set the ISIOI_ICONINDEX flag in pdwFlags if you return an index.</param>
        /// <param name="pdwFlags">Pointer to a bitmap that specifies the information that is being returned by the method. This parameter can be one or both of the following values: ISIOI_ICONFILE, ISIOI_ICONINDEX.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int GetOverlayInfo(IntPtr pwszIconFile, int cchMax, out int pIndex, out ISIOI pdwFlags);

        /// <summary>
        /// Specifies the priority of an icon overlay.
        /// </summary>
        /// <param name="pPriority">The address of a value that indicates the priority of the overlay identifier. Possible values range from zero to 100, with zero the highest priority.</param>
        /// <returns>Returns S_OK if successful, or a COM error code otherwise.</returns>
        [PreserveSig]
        int GetPriority(out int pPriority);
    }
}