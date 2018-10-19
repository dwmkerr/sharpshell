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

        /// <summary>
        /// Translates the display name of a file object or a folder into an item identifier list.
        /// </summary>
        /// <param name="hwnd">A window handle. The client should provide a window handle if it displays a dialog or message box. Otherwise set hwnd to NULL.</param>
        /// <param name="pbc">Optional. A pointer to a bind context used to pass parameters as inputs and outputs to the parsing function.</param>
        /// <param name="pszDisplayName">A null-terminated Unicode string with the display name.</param>
        /// <param name="pchEaten">A pointer to a ULONG value that receives the number of characters of the display name that was parsed. If your application does not need this information, set pchEaten to NULL, and no value will be returned.</param>
        /// <param name="ppidl">When this method returns, contains a pointer to the PIDL for the object.</param>
        /// <param name="pdwAttributes">The value used to query for file attributes. If not used, it should be set to NULL.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        [PreserveSig]
        new int ParseDisplayName(IntPtr hwnd, IntPtr pbc, string pszDisplayName, ref uint pchEaten, out IntPtr ppidl,
                             ref SFGAO pdwAttributes);

        /// <summary>
        /// Allows a client to determine the contents of a folder by creating an item identifier enumeration object and returning its IEnumIDList interface.
        /// Return value: error code, if any
        /// </summary>
        /// <param name="hwnd">If user input is required to perform the enumeration, this window handle should be used by the enumeration object as the parent window to take user input.</param>
        /// <param name="grfFlags">Flags indicating which items to include in the  enumeration. For a list of possible values, see the SHCONTF enum.</param>
        /// <param name="ppenumIDList">Address that receives a pointer to the IEnumIDList interface of the enumeration object created by this method.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        [PreserveSig]
        new int EnumObjects(IntPtr hwnd, SHCONTF grfFlags, out IEnumIDList ppenumIDList);

        /// <summary>
        /// Retrieves an IShellFolder object for a subfolder.
        /// Return value: error code, if any
        /// </summary>
        /// <param name="pidl">Address of an ITEMIDLIST structure (PIDL) that identifies the subfolder.</param>
        /// <param name="pbc">Optional address of an IBindCtx interface on a bind context object to be used during this operation.</param>
        /// <param name="riid">Identifier of the interface to return.</param>
        /// <param name="ppv">Address that receives the interface pointer.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        [PreserveSig]
        new int BindToObject(IntPtr pidl, IntPtr pbc, [In] ref Guid riid, out IntPtr ppv);

        /// <summary>
        /// Requests a pointer to an object's storage interface.
        /// Return value: error code, if any
        /// </summary>
        /// <param name="pidl">Address of an ITEMIDLIST structure that identifies the subfolder relative to its parent folder.</param>
        /// <param name="pbc">Optional address of an IBindCtx interface on a bind context object to be  used during this operation.</param>
        /// <param name="riid">Interface identifier (IID) of the requested storage interface.</param>
        /// <param name="ppv">Address that receives the interface pointer specified by riid.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        [PreserveSig]
        new int BindToStorage(IntPtr pidl, IntPtr pbc, [In] ref Guid riid, out IntPtr ppv);

        /// <summary>
        /// Determines the relative order of two file objects or folders, given
        /// their item identifier lists. Return value: If this method is
        /// successful, the CODE field of the HRESULT contains one of the
        /// following values (the code can be retrived using the helper function
        /// GetHResultCode): Negative A negative return value indicates that the first item should precede the second (pidl1 &lt; pidl2).
        /// Positive A positive return value indicates that the first item should
        /// follow the second (pidl1 &gt; pidl2).  Zero A return value of zero
        /// indicates that the two items are the same (pidl1 = pidl2).
        /// </summary>
        /// <param name="lParam">Value that specifies how the comparison  should be performed. The lower Sixteen bits of lParam define the sorting  rule.
        /// The upper sixteen bits of lParam are used for flags that modify the sorting rule. values can be from  the SHCIDS enum</param>
        /// <param name="pidl1">Pointer to the first item's ITEMIDLIST structure.</param>
        /// <param name="pidl2">Pointer to the second item's ITEMIDLIST structure.</param>
        /// <returns></returns>
        [PreserveSig]
        new int CompareIDs(IntPtr lParam, IntPtr pidl1, IntPtr pidl2);

        /// <summary>
        /// Requests an object that can be used to obtain information from or interact
        /// with a folder object.
        /// Return value: error code, if any
        /// </summary>
        /// <param name="hwndOwner">Handle to the owner window.</param>
        /// <param name="riid">Identifier of the requested interface.</param>
        /// <param name="ppv">Address of a pointer to the requested interface.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        [PreserveSig]
        new int CreateViewObject(IntPtr hwndOwner, [In] ref Guid riid, out IntPtr ppv);

        /// <summary>
        /// Retrieves the attributes of one or more file objects or subfolders.
        /// Return value: error code, if any
        /// </summary>
        /// <param name="cidl">Number of file objects from which to retrieve attributes.</param>
        /// <param name="apidl">Address of an array of pointers to ITEMIDLIST structures, each of which  uniquely identifies a file object relative to the parent folder.</param>
        /// <param name="rgfInOut">Address of a single ULONG value that, on entry contains the attributes that the caller is
        /// requesting. On exit, this value contains the requested attributes that are common to all of the specified objects. this value can be from the SFGAO enum</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        [PreserveSig]
        new int GetAttributesOf(UInt32 cidl, IntPtr apidl,
                            ref SFGAO rgfInOut);

        /// <summary>
        /// Retrieves an OLE interface that can be used to carry out actions on the
        /// specified file objects or folders. Return value: error code, if any
        /// </summary>
        /// <param name="hwndOwner">Handle to the owner window that the client should specify if it displays a dialog box or message box.</param>
        /// <param name="cidl">Number of file objects or subfolders specified in the apidl parameter.</param>
        /// <param name="apidl">Address of an array of pointers to ITEMIDLIST  structures, each of which  uniquely identifies a file object or subfolder relative to the parent folder.</param>
        /// <param name="riid">Identifier of the COM interface object to return.</param>
        /// <param name="rgfReserved">Reserved.</param>
        /// <param name="ppv">Pointer to the requested interface.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        [PreserveSig]
        new int GetUIObjectOf(IntPtr hwndOwner, UInt32 cidl,
                          IntPtr apidl, [In] ref Guid riid,
                          UInt32 rgfReserved, out IntPtr ppv);

        /// <summary>
        /// Retrieves the display name for the specified file object or subfolder.
        /// Return value: error code, if any
        /// </summary>
        /// <param name="pidl">Address of an ITEMIDLIST structure (PIDL)  that uniquely identifies the file  object or subfolder relative to the parent  folder.</param>
        /// <param name="uFlags">Flags used to request the type of display name to return. For a list of possible values.</param>
        /// <param name="pName">Address of a STRRET structure in which to return the display name.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        [PreserveSig]
        new int GetDisplayNameOf(IntPtr pidl, SHGDNF uFlags, out STRRET pName);

        /// <summary>
        /// Sets the display name of a file object or subfolder, changing the item
        /// identifier in the process.
        /// Return value: error code, if any
        /// </summary>
        /// <param name="hwnd">Handle to the owner window of any dialog or message boxes that the client displays.</param>
        /// <param name="pidl">Pointer to an ITEMIDLIST structure that uniquely identifies the file object or subfolder relative to the parent folder.</param>
        /// <param name="pszName">Pointer to a null-terminated string that specifies the new display name.</param>
        /// <param name="uFlags">Flags indicating the type of name specified by  the lpszName parameter. For a list of possible values, see the description of the SHGNO enum.</param>
        /// <param name="ppidlOut"></param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        [PreserveSig]
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
        int GetDetailsEx(IntPtr pidl, PROPERTYKEY pscid, out object pv);

        /// <summary>
        /// Gets detailed information, identified by a column index, on an item in a Shell folder.
        /// </summary>
        /// <param name="pidl">PIDL of the item for which you are requesting information. This method accepts only single-level PIDLs. The structure must contain exactly one SHITEMID structure followed by a terminating zero. If this parameter is set to NULL, the title of the information field specified by iColumn is returned.</param>
        /// <param name="iColumn">The zero-based index of the desired information field. It is identical to the column number of the information as it is displayed in a Windows Explorer Details view.</param>
        /// <param name="psd">The zero-based index of the desired information field. It is identical to the column number of the information as it is displayed in a Windows Explorer Details view.</param>
        /// <returns>Returns S_OK if successful, or a COM error value otherwise.</returns>
        [PreserveSig]
        int GetDetailsOf(IntPtr pidl, uint iColumn, out SHELLDETAILS psd);

        /// <summary>
        /// Converts a column to the appropriate property set ID (FMTID) and property ID (PID).
        /// </summary>
        /// <param name="iColumn">The column ID.</param>
        /// <param name="pscid">A pointer to an SHCOLUMNID structure containing the FMTID and PID.</param>
        /// <returns>Returns S_OK if successful, or a COM error value otherwise.</returns>
        [PreserveSig]
        int MapColumnToSCID(uint iColumn, out PROPERTYKEY pscid);
    };
}