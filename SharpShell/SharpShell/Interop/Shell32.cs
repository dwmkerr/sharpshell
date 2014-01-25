using System;
using System.Runtime.InteropServices;
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
        [DllImport("shell32.dll", SetLastError = true)]
        public static extern uint DragQueryFile(IntPtr hDrop, uint iFile, [Out] StringBuilder lpszFile, uint cch);

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
        public static extern int SHCreateShellFolderView(SFV_CREATE pcsfv, out IShellView ppsv);

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
        /// Shes the get folder location.
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
        /// TODO: document this.
        /// </summary>
        /// <param name="pidl1"></param>
        /// <param name="pidl2"></param>
        /// <returns></returns>
        [DllImport("shell32.dll")]
        public static extern bool ILIsEqual(IntPtr pidl1, IntPtr pidl2);
        /// <summary>
        /// TODO: document this.
        /// </summary>
        [DllImport("shell32.dll")]
        public static extern IntPtr ILCombine(IntPtr pidl1, IntPtr pidl2);
        /// <summary>
        /// TODO: document this.
        /// </summary>
        [DllImport("shell32.dll")]
        public static extern IntPtr ILClone(IntPtr pidl);


        /// <summary>
        /// TODO: document this.
        /// </summary>
        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttribs, out SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags);

        /// <summary>
        /// TODO: document this.
        /// </summary>
        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(IntPtr pIDL, uint dwFileAttributes, out SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags);
        
        /// <summary>
        /// todo: document
        /// </summary>
        [DllImport("shell32.dll", EntryPoint = "#727")]
        public extern static int SHGetImageList(int iImageList, ref Guid riid, ref IImageList ppv);

        [DllImport("shell32.dll")]
        public static extern Int32 SHGetSpecialFolderLocation(IntPtr hwndOwner, CSIDL nFolder, ref IntPtr ppidl);

        public static Guid IID_IShellFolder = new Guid("000214E6-0000-0000-C000-000000000046");

        public static Guid IID_IImageList = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");

        /// <summary>
        /// Creates a standard icon extractor, whose defaults can be further configured via the IDefaultExtractIconInit interface.
        /// </summary>
        /// <param name="guid">A reference to interface ID.</param>
        /// <param name="pdxi">The address of IDefaultExtractIconInit interface pointer.</param>
        /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [DllImport("shell32.dll")]
        public static extern int SHCreateDefaultExtractIcon(Guid guid, out IDefaultExtractIconInit pdxi);
    }
}