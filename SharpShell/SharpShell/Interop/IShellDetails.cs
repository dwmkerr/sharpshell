using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Exposed by Shell folders to provide detailed information about the items in a folder. This is the same information that is displayed by the Windows Explorer when the view of the folder is set to Details. For Windows 2000 and later systems, IShellDetails is superseded by IShellFolder2.
    /// </summary>
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214EC-0000-0000-c000-000000000046")]
    public interface IShellDetails
    {
        /// <summary>
        /// Gets detailed information on an item in a Shell folder.
        /// </summary>
        /// <param name="pidl">The PIDL of the item that you are requesting information for. If this parameter is set to NULL, the title of the information field specified by iColumn will be returned in the SHELLDETAILS structure pointed to by pDetails.</param>
        /// <param name="iColumn">The zero-based index of the desired information field. It is identical to column number of the information as it is displayed in a Windows Explorer Details view.</param>
        /// <param name="pDetails">A pointer to a SHELLDETAILS structure with the detail information.</param>
        /// <returns>Returns S_OK if successful. Returns E_FAIL if iColumn exceeds the number of columns supported by the folder. Otherwise, returns a standard COM error code.</returns>
        [PreserveSig]
        int GetDetailsOf(IntPtr pidl, uint iColumn, SHELLDETAILS pDetails);

        /// <summary>
        /// Rearranges a column.
        /// </summary>
        /// <param name="iColumn">The index of the column to be rearranged.</param>
        /// <returns>Returns S_FALSE to tell the calling application to sort the selected column. Otherwise, returns S_OK if successful, a COM error code otherwise.</returns>
        [PreserveSig]
        int ColumnClick(uint iColumn);
    }
}