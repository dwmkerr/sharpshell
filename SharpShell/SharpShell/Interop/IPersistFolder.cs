using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Exposes a method that initializes Shell folder objects.
    /// </summary>
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214EA-0000-0000-C000-000000000046")]
    public interface IPersistFolder : IPersist
    {
        #region Overrides for C#/COM compatibility.

        new int GetClassID(out Guid pClassID);

        #endregion

        /// <summary>
        /// Instructs a Shell folder object to initialize itself based on the information passed.
        /// </summary>
        /// <param name="pidl">The address of the ITEMIDLIST (item identifier list) structure that specifies the absolute location of the folder.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int Initialize(IntPtr pidl);
    }
}