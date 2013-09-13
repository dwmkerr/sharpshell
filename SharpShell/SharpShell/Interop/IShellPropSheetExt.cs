using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{

    /// <summary>
    /// Exposes methods that allow a property sheet handler to add or replace pages in the property sheet displayed for a file object.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214e9-0000-0000-c000-000000000046")]
    internal interface IShellPropSheetExt
    {
        /// <summary>
        /// Adds one or more pages to a property sheet that the Shell displays for a file object. The Shell calls this method for each property sheet handler registered to the file type.
        /// </summary>
        /// <param name="pfnAddPage">A pointer to a function that the property sheet handler calls to add a page to the property sheet. The function takes a property sheet handle returned by the CreatePropertySheetPage function and the lParam parameter passed to this method.</param>
        /// <param name="lParam">Handler-specific data to pass to the function pointed to by pfnAddPage.</param>
        /// <returns>If successful, returns a one-based index to specify the page that should be initially displayed. See Remarks for more information.</returns>
        [PreserveSig]
        int AddPages(IntPtr pfnAddPage, IntPtr lParam);

        /// <summary>
        /// Replaces a page in a property sheet for a Control Panel object.
        /// </summary>
        /// <param name="uPageID">Not used.
        ///  Microsoft Windows XP and earlier: A type EXPPS identifier of the page to replace. The values for this parameter for Control Panels can be found in the Cplext.h header file.</param>
        /// <param name="lpfnReplacePage">A pointer to a function that the property sheet handler calls to replace a page to the property sheet. The function takes a property sheet handle returned by the CreatePropertySheetPage function and the lParam parameter passed to the ReplacePage method.</param>
        /// <param name="lParam">The parameter to pass to the function specified by the pfnReplacePage parameter.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int ReplacePage(uint uPageID, AddPropertySheetPageDelegate lpfnReplacePage, IntPtr lParam);
    }


    internal delegate bool AddPropertySheetPageDelegate(IntPtr hPropSheetPage, IntPtr lParam);
    
    /// <summary>
    /// Property sheet callback proc.
    /// </summary>
    /// <param name="hwnd">The window handle.</param>
    /// <param name="uMsg">The message.</param>
    /// <param name="ppsp">The property sheet page.</param>
    /// <returns>The return code.</returns>
    public delegate uint PropSheetCallback(IntPtr hwnd, PSPCB uMsg, ref PROPSHEETPAGE ppsp);

    /// <summary>
    /// Dialog proc.
    /// </summary>
    /// <param name="hwndDlg">The window handle.</param>
    /// <param name="uMsg">The message.</param>
    /// <param name="wParam">The w param.</param>
    /// <param name="lParam">The l param.</param>
    /// <returns>True if the message was handled.</returns>
    public delegate IntPtr DialogProc(IntPtr hwndDlg, uint uMsg, IntPtr wParam, IntPtr lParam);
}