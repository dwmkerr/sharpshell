using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Exposes methods to set default icons associated with an object.
    /// </summary>
    [ComImport]
    [Guid("41ded17d-d6b3-4261-997d-88c60e4b1d58")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDefaultExtractIconInit
    {
        /// <summary>
        /// Sets GIL_XXX flags. See GetIconLocation
        /// </summary>
        /// <param name="uFlags">Specifies return flags to get icon location.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int SetFlags(uint uFlags);

        /// <summary>
        /// Sets the registry key from which to load the "DefaultIcon" value.
        /// </summary>
        /// <param name="hkey">A handle to the registry key.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int SetKey(IntPtr hkey);

        /// <summary>
        /// Sets the normal icon.
        /// </summary>
        /// <param name="pszFile">A pointer to a buffer that contains the full icon path, including the file name and extension, as a Unicode string. This pointer can be NULL.</param>
        /// <param name="iIcon">A Shell icon ID.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int SetNormalIcon([In, MarshalAs(UnmanagedType.LPWStr)] string pszFile, int iIcon);

        /// <summary>
        /// Sets the icon that allows containers to specify an "open" look.
        /// </summary>
        /// <param name="pszFile">A pointer to a buffer that contains the full icon path, including the file name and extension, as a Unicode string. This pointer can be NULL.</param>
        /// <param name="iIcon">Shell icon ID.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int SetOpenIcon([In, MarshalAs(UnmanagedType.LPWStr)] string pszFile, int iIcon);

        /// <summary>
        /// Sets the icon for a shortcut to the object.
        /// </summary>
        /// <param name="pszFile">A pointer to a buffer that contains the full icon path, including the file name and extension, as a Unicode string. This pointer can be NULL.</param>
        /// <param name="iIcon">Shell icon ID.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int SetShortcutIcon([In, MarshalAs(UnmanagedType.LPWStr)] string pszFile, int iIcon);

        /// <summary>
        /// Sets the default icon.
        /// </summary>
        /// <param name="pszFile">A pointer to a buffer that contains the full icon path, including the file name and extension, as a Unicode string. This pointer can be NULL.</param>
        /// <param name="iIcon">The Shell icon ID.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int SetDefaultIcon([In, MarshalAs(UnmanagedType.LPWStr)] string pszFile,int iIcon);
    }
}