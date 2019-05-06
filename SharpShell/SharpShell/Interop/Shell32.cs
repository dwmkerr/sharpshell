using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace SharpShell.Interop
{
    /// <summary>
    /// Methods imported from Shell32.dll.
    /// </summary>
    public static class Shell32
    {
        /// <summary>
        /// Retrieves the names of dropped files that result from a successful drag-and-drop operation.
        /// </summary>
        /// <param name="hDrop">Identifier of the structure that contains the file names of the dropped files.</param>
        /// <param name="iFile">Index of the file to query. If the value of this parameter is 0xFFFFFFFF, DragQueryFile returns a count of the files dropped. If the value of this parameter is between zero and the total number of files dropped, DragQueryFile copies the file name with the corresponding value to the buffer pointed to by the lpszFile parameter.</param>
        /// <param name="lpszFile">The address of a buffer that receives the file name of a dropped file when the function returns. This file name is a null-terminated string. If this parameter is NULL, DragQueryFile returns the required size, in characters, of this buffer.</param>
        /// <param name="cch">The size, in characters, of the lpszFile buffer.</param>
        /// <returns>A nonzero value indicates a successful call.</returns>
        [DllImport("shell32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint DragQueryFile(IntPtr hDrop, uint iFile, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszFile, uint cch);

        /// <summary>
        /// Performs an operation on a specified file.
        /// </summary>
        /// <param name="pExecInfo">A pointer to a SHELLEXECUTEINFO structure that contains and receives information about the application being executed.</param>
        /// <returns>Returns TRUE if successful; otherwise, FALSE. Call GetLastError for extended error information.</returns>
        [DllImport("shell32.dll", EntryPoint = "ShellExecuteEx", SetLastError = true)]
        public static extern int ShellExecuteEx(ref SHELLEXECUTEINFO pExecInfo);

        /// <summary>
        /// Retrieves the path of a known folder as an ITEMIDLIST structure.
        /// </summary>
        /// <param name="rfid">A reference to the KNOWNFOLDERID that identifies the folder. The folders associated with the known folder IDs might not exist on a particular system.</param>
        /// <param name="dwFlags">Flags that specify special retrieval options. This value can be 0; otherwise, it is one or more of the KNOWN_FOLDER_FLAG values.</param>
        /// <param name="hToken">An access token used to represent a particular user. This parameter is usually set to NULL, in which case the function tries to access the current user's instance of the folder.</param>
        /// <param name="ppidl">When this method returns, contains a pointer to the PIDL of the folder. This parameter is passed uninitialized. The caller is responsible for freeing the returned PIDL when it is no longer needed by calling ILFree.</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise, including the following:</returns>
        [DllImport("shell32.dll", SetLastError = true)]
        public static extern int SHGetKnownFolderIDList([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, KNOWN_FOLDER_FLAG dwFlags, IntPtr hToken, out IntPtr ppidl);

        /// <summary>
        /// Retrieves the full path of a known folder identified by the folder's KNOWNFOLDERID.
        /// </summary>
        /// <param name="rfid">A reference to the KNOWNFOLDERID that identifies the folder. The folders associated with the known folder IDs might not exist on a particular system.</param>
        /// <param name="dwFlags">Flags that specify special retrieval options. This value can be 0; otherwise, it is one or more of the KNOWN_FOLDER_FLAG values.</param>
        /// <param name="hToken">An access token used to represent a particular user. This parameter is usually set to NULL, in which case the function tries to access the current user's instance of the folder.</param>
        /// <param name="pszPath">When this method returns, contains the address of a pointer to a null-terminated Unicode string that specifies the path of the known folder. The calling process is responsible for freeing this resource once it is no longer needed by calling CoTaskMemFree. The returned path does not include a trailing backslash. For example, "C:\Users" is returned rather than "C:\Users\".</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise, including the following:</returns>
        [DllImport("shell32.dll")]
        public static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, KNOWN_FOLDER_FLAG dwFlags, IntPtr hToken, out string pszPath);

        /// <summary>
        /// Frees an ITEMIDLIST structure allocated by the Shell.
        /// </summary>
        /// <param name="pidl">A pointer to the ITEMIDLIST structure to be freed. This parameter can be NULL.</param>
        [DllImport("shell32.dll")]
        public static extern void ILFree(IntPtr pidl);

        /// <summary>
        /// Creates a new instance of the default Shell folder view object (DefView).
        /// </summary>
        /// <param name="pcsfv">Pointer to a SFV_CREATE structure that describes the particulars used in creating this instance of the Shell folder view object.</param>
        /// <param name="ppsv">When this function returns successfully, contains an interface pointer to the new IShellView object. On failure, this value is NULL.</param>
        /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [DllImport("shell32.dll")]
        public static extern int SHCreateShellFolderView(ref SFV_CREATE pcsfv, out IShellView ppsv);

        /// <summary>
        /// Creates a new instance of the default Shell folder view object. It is recommended that you use SHCreateShellFolderView rather than this function.
        /// </summary>
        /// <param name="pcsfv">Pointer to a structure that describes the details used in creating this instance of the Shell folder view object.</param>
        /// <param name="ppsv">The address of an IShellView interface pointer that, when this function returns successfully, points to the new view object. On failure, this value is NULL.</param>
        /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [DllImport("shell32.dll")]
        public static extern int SHCreateShellFolderViewEx(CSFV pcsfv, out IShellView ppsv);

        /// <summary>
        /// Retrieves the IShellFolder interface for the desktop folder, which is the root of the Shell's namespace.
        /// </summary>
        /// <param name="ppshf">When this method returns, receives an IShellFolder interface pointer for the desktop folder. The calling application is responsible for eventually freeing the interface by calling its IUnknown::Release method.</param>
        /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [DllImport("shell32.dll")]
        public static extern int SHGetDesktopFolder(out IShellFolder ppshf);

        /// <summary>
        /// Deprecated. Retrieves the path of a folder as an ITEMIDLIST structure
        /// </summary>
        /// <param name="hwndOwner">Reserved.</param>
        /// <param name="nFolder">A CSIDL value that identifies the folder to be located. The folders associated with the CSIDLs might not exist on a particular system.</param>
        /// <param name="hToken">An access token that can be used to represent a particular user.</param>
        /// <param name="dwReserved">Reserved.</param>
        /// <param name="ppidl">The address of a pointer to an item identifier list structure that specifies the folder's location relative to the root of the namespace (the desktop). The ppidl parameter is set to NULL on failure. The calling application is responsible for freeing this resource by calling ILFree.</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise.</returns>
        [DllImport("shell32.dll")]
        public static extern int SHGetFolderLocation(IntPtr hwndOwner, CSIDL nFolder, IntPtr hToken, uint dwReserved, out IntPtr ppidl);

        /// <summary>
        /// Converts an item identifier list to a file system path.
        /// </summary>
        /// <param name="pidl">The address of an item identifier list that specifies a file or directory location relative to the root of the namespace (the desktop).</param>
        /// <param name="pszPath">The address of a buffer to receive the file system path. This buffer must be at least MAX_PATH characters in size.</param>
        /// <returns>Returns TRUE if successful; otherwise, FALSE.</returns>
        [DllImport("shell32.dll", EntryPoint = "SHGetPathFromIDListW")]
        public static extern bool SHGetPathFromIDList(IntPtr pidl, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder pszPath);

        /// <summary>
        /// Makes a copy of a string in newly allocated memory.
        /// </summary>
        /// <param name="pszSource">A pointer to the null-terminated string to be copied.</param>
        /// <param name="ppwsz">A pointer to an allocated Unicode string that contains the result. SHStrDup allocates memory for this string with CoTaskMemAlloc. You should free the string with CoTaskMemFree when it is no longer needed. In the case of failure, this value is NULL.</param>
        /// <returns>Returns S_OK if successful, or a COM error value otherwise.</returns>
        [DllImport("shlwapi.dll", EntryPoint = "SHStrDupW")]
        public static extern int SHStrDup([MarshalAs(UnmanagedType.LPWStr)] string pszSource, out IntPtr ppwsz);

        /// <summary>
        /// Creates an object that represents the Shell's default context menu implementation.
        /// </summary>
        /// <param name="pdcm">A pointer to a constant DEFCONTEXTMENU structure.</param>
        /// <param name="riid">Reference to the interface ID of the interface on which to base the object. This is typically the IID of IContextMenu, IContextMenu2, or IContextMenu3.</param>
        /// <param name="ppv">When this method returns, contains the interface pointer requested in riid.</param>
        /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [DllImport("shell32.dll")]
        public static extern int SHCreateDefaultContextMenu(DEFCONTEXTMENU pdcm, Guid riid, out IntPtr ppv);

        /// <summary>
        /// Retrieves an object that implements an IQueryAssociations interface.
        /// </summary>
        /// <param name="rgClasses">A pointer to an array of ASSOCIATIONELEMENT structures.</param>
        /// <param name="cClasses">The number of elements in the array pointed to by rgClasses.</param>
        /// <param name="riid">Reference to the desired IID, normally IID_IQueryAssociations.</param>
        /// <param name="ppv">When this method returns, contains the interface pointer requested in riid. This is normally IQueryAssociations.</param>
        /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [DllImport("shell32.dll")]
        public static extern int AssocCreateForClasses(ASSOCIATIONELEMENT[] rgClasses, uint cClasses, Guid riid, out IntPtr ppv);


        /// <summary>
        /// Tests whether two ITEMIDLIST structures are equal in a binary comparison.
        /// </summary>
        /// <param name="pidl1">The first ITEMIDLIST structure.</param>
        /// <param name="pidl2">The second ITEMIDLIST structure.</param>
        /// <returns>Returns TRUE if the two structures are equal, FALSE otherwise.</returns>
        [DllImport("shell32.dll")]
        public static extern bool ILIsEqual(IntPtr pidl1, IntPtr pidl2);

        /// <summary>
        /// Combines two ITEMIDLIST structures.
        /// </summary>
        /// <param name="pidl1">A pointer to the first ITEMIDLIST structure.</param>
        /// <param name="pidl2">A pointer to the second ITEMIDLIST structure. This structure is appended to the structure pointed to by pidl1.</param>
        /// <returns>Returns an ITEMIDLIST containing the combined structures. If you set either pidl1 or pidl2 to NULL, the returned ITEMIDLIST structure is a clone of the non-NULL parameter. Returns NULL if pidl1 and pidl2 are both set to NULL.</returns>
        [DllImport("shell32.dll")]
        public static extern IntPtr ILCombine(IntPtr pidl1, IntPtr pidl2);

        /// <summary>
        /// Clones an ITEMIDLIST structure.
        /// </summary>
        /// <param name="pidl">A pointer to the ITEMIDLIST structure to be cloned.</param>
        /// <returns>Returns a pointer to a copy of the ITEMIDLIST structure pointed to by pidl.</returns>
        [DllImport("shell32.dll")]
        public static extern IntPtr ILClone(IntPtr pidl);

        /// <summary>
        /// Retrieves information about an object in the file system, such as a file, folder, directory, or drive root.
        /// </summary>
        /// <param name="pszPath">A pointer to a null-terminated string of maximum length MAX_PATH that contains the path and file name. Both absolute and relative paths are valid.
        /// If the uFlags parameter includes the SHGFI_PIDL flag, this parameter must be the address of an ITEMIDLIST (PIDL) structure that contains the list of item identifiers that uniquely identifies the file within the Shell's namespace. The PIDL must be a fully qualified PIDL. Relative PIDLs are not allowed.
        /// If the uFlags parameter includes the SHGFI_USEFILEATTRIBUTES flag, this parameter does not have to be a valid file name. The function will proceed as if the file exists with the specified name and with the file attributes passed in the dwFileAttributes parameter. This allows you to obtain information about a file type by passing just the extension for pszPath and passing FILE_ATTRIBUTE_NORMAL in dwFileAttributes.
        /// This string can use either short (the 8.3 form) or long file names.</param>
        /// <param name="dwFileAttribs">A combination of one or more file attribute flags (FILE_ATTRIBUTE_ values as defined in Winnt.h). If uFlags does not include the SHGFI_USEFILEATTRIBUTES flag, this parameter is ignored.</param>
        /// <param name="psfi">A combination of one or more file attribute flags (FILE_ATTRIBUTE_ values as defined in Winnt.h). If uFlags does not include the SHGFI_USEFILEATTRIBUTES flag, this parameter is ignored.</param>
        /// <param name="cbFileInfo">A combination of one or more file attribute flags (FILE_ATTRIBUTE_ values as defined in Winnt.h). If uFlags does not include the SHGFI_USEFILEATTRIBUTES flag, this parameter is ignored.</param>
        /// <param name="uFlags">A combination of one or more file attribute flags (FILE_ATTRIBUTE_ values as defined in Winnt.h). If uFlags does not include the SHGFI_USEFILEATTRIBUTES flag, this parameter is ignored.</param>
        /// <returns>A combination of one or more file attribute flags (FILE_ATTRIBUTE_ values as defined in Winnt.h). If uFlags does not include the SHGFI_USEFILEATTRIBUTES flag, this parameter is ignored.</returns>
        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttribs, out SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags);

        /// <summary>
        /// Retrieves information about an object in the file system, such as a file, folder, directory, or drive root.
        /// </summary>
        /// <param name="pszPath">A pointer to a null-terminated string of maximum length MAX_PATH that contains the path and file name. Both absolute and relative paths are valid.
        /// If the uFlags parameter includes the SHGFI_PIDL flag, this parameter must be the address of an ITEMIDLIST (PIDL) structure that contains the list of item identifiers that uniquely identifies the file within the Shell's namespace. The PIDL must be a fully qualified PIDL. Relative PIDLs are not allowed.
        /// If the uFlags parameter includes the SHGFI_USEFILEATTRIBUTES flag, this parameter does not have to be a valid file name. The function will proceed as if the file exists with the specified name and with the file attributes passed in the dwFileAttributes parameter. This allows you to obtain information about a file type by passing just the extension for pszPath and passing FILE_ATTRIBUTE_NORMAL in dwFileAttributes.
        /// This string can use either short (the 8.3 form) or long file names.</param>
        /// <param name="dwFileAttribs">A combination of one or more file attribute flags (FILE_ATTRIBUTE_ values as defined in Winnt.h). If uFlags does not include the SHGFI_USEFILEATTRIBUTES flag, this parameter is ignored.</param>
        /// <param name="psfi">A combination of one or more file attribute flags (FILE_ATTRIBUTE_ values as defined in Winnt.h). If uFlags does not include the SHGFI_USEFILEATTRIBUTES flag, this parameter is ignored.</param>
        /// <param name="cbFileInfo">A combination of one or more file attribute flags (FILE_ATTRIBUTE_ values as defined in Winnt.h). If uFlags does not include the SHGFI_USEFILEATTRIBUTES flag, this parameter is ignored.</param>
        /// <param name="uFlags">A combination of one or more file attribute flags (FILE_ATTRIBUTE_ values as defined in Winnt.h). If uFlags does not include the SHGFI_USEFILEATTRIBUTES flag, this parameter is ignored.</param>
        /// <returns>A combination of one or more file attribute flags (FILE_ATTRIBUTE_ values as defined in Winnt.h). If uFlags does not include the SHGFI_USEFILEATTRIBUTES flag, this parameter is ignored.</returns>
        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(IntPtr pszPath, uint dwFileAttribs, out SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags);

        /// <summary>
        /// Retrieves an image list.
        /// </summary>
        /// <param name="iImageList">The image type contained in the list. One of the following values:</param>
        /// <param name="riid">Reference to the image list interface identifier, normally IID_IImageList.</param>
        /// <param name="ppv">When this method returns, contains the interface pointer requested in riid. This is typically IImageList.</param>
        /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [DllImport("shell32.dll", EntryPoint = "#727")]
        public extern static int SHGetImageList(int iImageList, ref Guid riid, ref IImageList ppv);

        /// <summary>
        /// Retrieves a pointer to the ITEMIDLIST structure of a special folder.
        /// </summary>
        /// <param name="hwndOwner">Reserved.</param>
        /// <param name="nFolder">A CSIDL value that identifies the folder of interest.</param>
        /// <param name="ppidl">A PIDL specifying the folder's location relative to the root of the namespace (the desktop). It is the responsibility of the calling application to free the returned IDList by using CoTaskMemFree.</param>
        /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [Obsolete("SHGetSpecialFolderLocation is not supported and may be altered or unavailable in the future. Instead, use SHGetFolderLocation")]
        [DllImport("shell32.dll")]
        public static extern Int32 SHGetSpecialFolderLocation(IntPtr hwndOwner, CSIDL nFolder, ref IntPtr ppidl);

        /// <summary>
        /// Creates a standard icon extractor, whose defaults can be further configured via the IDefaultExtractIconInit interface.
        /// </summary>
        /// <param name="guid">A reference to interface ID.</param>
        /// <param name="pdxi">The address of IDefaultExtractIconInit interface pointer.</param>
        /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [DllImport("shell32.dll")]
        public static extern int SHCreateDefaultExtractIcon(Guid guid, out IDefaultExtractIconInit pdxi);

        /// <summary>
        /// Creates a data object in a parent folder.
        /// </summary>
        /// <param name="pidlFolder">A pointer to an ITEMIDLIST (PIDL) of the parent folder that contains the data object.</param>
        /// <param name="cidl">The number of file objects or subfolders specified in the apidl parameter.</param>
        /// <param name="apidl">An array of pointers to constant ITEMIDLIST structures, each of which uniquely identifies a file object or subfolder relative to the parent folder. Each item identifier list must contain exactly one SHITEMID structure followed by a terminating zero.</param>
        /// <param name="pdtInner">A pointer to interface IDataObject. This parameter can be NULL. Specify pdtInner only if the data object created needs to support additional FORMATETC clipboard formats beyond the default formats it is assigned at creation. Alternatively, provide support for populating the created data object using non-default clipboard formats by calling method IDataObject::SetData and specifying the format in the FORMATETC structure passed in parameter pFormatetc.</param>
        /// <param name="riid">A reference to the IID of the interface to retrieve through ppv. This must be IID_IDataObject.</param>
        /// <param name="ppv">When this method returns successfully, contains the IDataObject interface pointer requested in riid.</param>
        /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [DllImport("shell32.dll")]
        public static extern int SHCreateDataObject(IntPtr pidlFolder, uint cidl, IntPtr apidl, IDataObject pdtInner, Guid riid,
            out IntPtr ppv);

        /// <summary>
        /// Notifies the system of an event that an application has performed. An application should use this function if it performs an action that may affect the Shell.
        /// </summary>
        /// <param name="wEventId">Describes the event that has occurred. Typically, only one event is specified at a time. If more than one event is specified, the values contained in the dwItem1 and dwItem2 parameters must be the same, respectively, for all specified events. This parameter can be one or more of the following values:</param>
        /// <param name="uFlags">Flags that, when combined bitwise with SHCNF_TYPE, indicate the meaning of the dwItem1 and dwItem2 parameters. The uFlags parameter must be one of the following values.</param>
        /// <param name="dwItem1">Optional. First event-dependent value.</param>
        /// <param name="dwItem2">Optional. Second event-dependent value.</param>
        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(Int32 wEventId, UInt32 uFlags, IntPtr dwItem1, IntPtr dwItem2);

        /// <summary>
        /// A file type association has changed. SHCNF_IDLIST must be specified in the uFlags parameter. dwItem1 and dwItem2 are not used and must be NULL. This event should also be sent for registered protocols.
        /// </summary>
        public static Int32 SHCNE_ASSOCCHANGED = 0x08000000;

        /// <summary>
        /// Guid for IID_IQueryAssociations.
        /// </summary>
        public static Guid IID_IQueryAssociations = new Guid("{c46ca590-3c3f-11d2-bee6-0000f805ca57}");

        /// <summary>
        /// Guid for IID_IShellFolder.
        /// </summary>
        public static Guid IID_IShellFolder = new Guid("000214E6-0000-0000-C000-000000000046");

        /// <summary>
        /// Guid for IID_IImageList.
        /// </summary>
        public static Guid IID_IImageList = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");

        /// <summary>
        /// Guid for IID_ExtractIconW.
        /// </summary>
        public static Guid IID_ExtractIconW = new Guid("{000214FA-0000-0000-C000-000000000046}");

        /// <summary>
        /// Guid for IID_IDataObject.
        /// </summary>
        public static Guid IID_IDataObject = new Guid("{0000010E-0000-0000-C000-000000000046}");

        /// <summary>
        /// Guid for IID_IContextMenu.
        /// </summary>
        public static Guid IID_IContextMenu = new Guid("000214e4-0000-0000-c000-000000000046");

        /// <summary>
        /// Guid for IID_IShellBrowser.
        /// </summary>
        public static Guid IID_IShellBrowser = new Guid("000214E2-0000-0000-C000-000000000046");

        /// <summary>
        /// Guid for IID_IFolderView.
        /// </summary>
        public static Guid IID_IFolderView = new Guid("cde725b0-ccc9-4519-917e-325d72fab4ce");

    }
}