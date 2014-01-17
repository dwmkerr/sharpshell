using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Extends the capabilities of IShellFolder. Its methods provide a variety of information about the contents of a Shell folder.
    /// </summary>
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("93F2F68C-1D1B-11d3-A30E-00C04F79ABD1")]
    public interface IShellFolder2 : IShellFolder
    {
        #region IShellFolder overrides for COM/C# compatibility.
        
        new int ParseDisplayName(IntPtr hwnd, IntPtr pbc, string pszDisplayName, ref uint pchEaten, out IntPtr ppidl,
                             ref SFGAO pdwAttributes);
        new int EnumObjects(IntPtr hwnd, SHCONTF grfFlags, out IEnumIDList ppenumIDList);
        new int BindToObject(IntPtr pidl, IntPtr pbc, [In] ref Guid riid, out IntPtr ppv);
        new int BindToStorage(IntPtr pidl, IntPtr pbc, [In] ref Guid riid, out IntPtr ppv);
        new int CompareIDs(IntPtr lParam, IntPtr pidl1, IntPtr pidl2);
        new int CreateViewObject(IntPtr hwndOwner, [In] ref Guid riid, out IntPtr ppv);

        new int GetAttributesOf(UInt32 cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] IntPtr[] apidl,
                            ref SFGAO rgfInOut);
        new int GetUIObjectOf(IntPtr hwndOwner, UInt32 cidl,
                          [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IntPtr[] apidl, [In] ref Guid riid,
                          UInt32 rgfReserved, out IntPtr ppv);
        new int GetDisplayNameOf(IntPtr pidl, SHGDNF uFlags, out STRRET pName);
        new int SetNameOf(IntPtr hwnd, IntPtr pidl, String pszName, SHGDNF uFlags, out IntPtr ppidlOut);

        #endregion

        /// <summary>
        /// Returns the globally unique identifier (GUID) of the default search object for the folder.
        /// </summary>
        /// <param name="pguid">The GUID of the default search object.</param>
        /// <returns>Returns S_OK if successful, or a COM error value otherwise.</returns>
        [PreserveSig]
        int GetDefaultSearchGUID(out Guid pguid);

        /// <summary>
        /// Requests a pointer to an interface that allows a client to enumerate the available search objects.
        /// </summary>
        /// <param name="ppenum">The address of a pointer to an enumerator object's IEnumExtraSearch interface.</param>
        /// <returns>Returns S_OK if successful, or a COM error value otherwise.</returns>
        [PreserveSig]
        int EnumSearches(out IEnumExtraSearch ppenum);

        /// <summary>
        /// Gets the default sorting and display columns.
        /// </summary>
        /// <param name="dwRes">Reserved. Set to zero.</param>
        /// <param name="pSort">A pointer to a value that receives the index of the default sorted column.</param>
        /// <param name="pDisplay">A pointer to a value that receives the index of the default display column.</param>
        /// <returns>Returns S_OK if successful, or a COM error value otherwise.</returns>
        [PreserveSig]
        int GetDefaultColumn(uint dwRes, out uint pSort, out uint pDisplay);

        /// <summary>
        /// Gets the default state for a specified column.
        /// </summary>
        /// <param name="iColumn">An integer that specifies the column number.</param>
        /// <param name="pcsFlags">A pointer to a value that contains flags that indicate the default column state. This parameter can include a combination of the following flags.</param>
        /// <returns>Returns S_OK if successful, or a COM error value otherwise.</returns>
        [PreserveSig]
        int GetDefaultColumnState(uint iColumn, out SHCOLSTATEF pcsFlags);

        /// <summary>
        /// Gets detailed information, identified by a property set identifier (FMTID) and a property identifier (PID), on an item in a Shell folder.
        /// </summary>
        /// <param name="pidl">A PIDL of the item, relative to the parent folder. This method accepts only single-level PIDLs. The structure must contain exactly one SHITEMID structure followed by a terminating zero. This value cannot be NULL.</param>
        /// <param name="pscid">A pointer to an SHCOLUMNID structure that identifies the column.</param>
        /// <param name="pv">A pointer to a VARIANT with the requested information. The value is fully typed. The value returned for properties from the property system must conform to the type specified in that property definition's typeInfo as the legacyType attribute.</param>
        /// <returns>Returns S_OK if successful, or a COM error value otherwise.</returns>
        [PreserveSig]
        int GetDetailsEx(IntPtr pidl, SHCOLUMNID pscid, out IntPtr pv);

        /// <summary>
        /// Gets detailed information, identified by a column index, on an item in a Shell folder.
        /// </summary>
        /// <param name="pidl">PIDL of the item for which you are requesting information. This method accepts only single-level PIDLs. The structure must contain exactly one SHITEMID structure followed by a terminating zero. If this parameter is set to NULL, the title of the information field specified by iColumn is returned.</param>
        /// <param name="iColumn">The zero-based index of the desired information field. It is identical to the column number of the information as it is displayed in a Windows Explorer Details view.</param>
        /// <param name="psd">The zero-based index of the desired information field. It is identical to the column number of the information as it is displayed in a Windows Explorer Details view.</param>
        /// <returns>Returns S_OK if successful, or a COM error value otherwise.</returns>
        [PreserveSig]
        int GetDetailsOf(IntPtr pidl, uint iColumn, out IntPtr psd);

        /// <summary>
        /// Converts a column to the appropriate property set ID (FMTID) and property ID (PID).
        /// </summary>
        /// <param name="iColumn">The column ID.</param>
        /// <param name="pscid">A pointer to an SHCOLUMNID structure containing the FMTID and PID.</param>
        /// <returns>Returns S_OK if successful, or a COM error value otherwise.</returns>
        [PreserveSig]
        int MapColumnToSCID(uint iColumn, out SHCOLUMNID pscid);
    };
}