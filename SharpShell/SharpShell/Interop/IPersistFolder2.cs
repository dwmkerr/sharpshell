using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Exposes methods that obtain information from Shell folder objects.
    /// </summary>
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("1AC3D9F0-175C-11d1-95BE-00609797EA4F")]
    public interface IPersistFolder2 : IPersistFolder
    {
        #region Overrides for C#/COM compatibility.

        [PreserveSig]
        new int GetClassID(out Guid pClassID);
        [PreserveSig]
        new int Initialize(IntPtr pidl);

        #endregion

        /// <summary>
        /// Gets the ITEMIDLIST for the folder object.
        /// </summary>
        /// <param name="ppidl">The address of an ITEMIDLIST pointer. This PIDL represents the absolute location of the folder and must be relative to the desktop. This is typically a copy of the PIDL passed to Initialize.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <remarks>If the folder object has not been initialized, this method returns S_FALSE and ppidl is set to NULL.</remarks>
        [PreserveSig]
        int GetCurFolder(out IntPtr ppidl);
    }
}