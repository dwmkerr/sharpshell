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

        /// <summary>
        /// Retrieves the class identifier (CLSID) of the object.
        /// </summary>
        /// <param name="pClassID">A pointer to the location that receives the CLSID on return.
        /// The CLSID is a globally unique identifier (GUID) that uniquely represents an object class that defines the code that can manipulate the object's data.</param>
        /// <returns>
        /// If the method succeeds, the return value is S_OK. Otherwise, it is E_FAIL.
        /// </returns>
        [PreserveSig]
        new int GetClassID(out Guid pClassID);

        /// <summary>
        /// Instructs a Shell folder object to initialize itself based on the information passed.
        /// </summary>
        /// <param name="pidl">The address of the ITEMIDLIST (item identifier list) structure that specifies the absolute location of the folder.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
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