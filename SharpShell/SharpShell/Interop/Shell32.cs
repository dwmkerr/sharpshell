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
    }
}
