using System;
using System.Runtime.InteropServices;
#pragma warning disable 1591

namespace SharpShell.Interop
{
    /// <summary>
    /// Exposes a method that allows communication between Windows Explorer and a folder view implemented using the system folder view object (the IShellView object returned through SHCreateShellFolderView) so that the folder view can be notified of events and modify its view accordingly.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("2047E320-F2A9-11CE-AE65-08002B2E1262")]
    public interface IShellFolderViewCB
    {
        /// <summary>
        /// Allows communication between the system folder view object and a system folder view callback object.
        /// </summary>
        /// <param name="uMsg">One of the following notifications.</param>
        /// <param name="wParam">Additional information.</param>
        /// <param name="lParam">Additional information.</param>
        /// <param name="plResult">Additional information.</param>
        /// <returns>S_OK if the message was handled, E_NOTIMPL if the shell should perform default processing.</returns>
        [PreserveSig]
        int MessageSFVCB(SFVM uMsg, IntPtr wParam, IntPtr lParam, ref IntPtr plResult);
    }

    public enum SFVM
    {
        SFVM_MERGEMENU         =    1, // -                  LPQCMINFO
        SFVM_INVOKECOMMAND     =    2, // idCmd              -
        SFVM_GETHELPTEXT       =    3, // idCmd,cchMax       pszText
        SFVM_GETTOOLTIPTEXT    =    4, // idCmd,cchMax       pszText
        SFVM_GETBUTTONINFO     =    5, // -                  LPTBINFO
        SFVM_GETBUTTONS        =    6, // idCmdFirst,cbtnMax LPTBBUTTON
        SFVM_INITMENUPOPUP     =    7, // idCmdFirst,nIndex  hmenu
        SFVM_FSNOTIFY          =   14, // LPCITEMIDLIST*     lEvent
        SFVM_WINDOWCREATED     =   15, // hwnd               -
        SFVM_GETDETAILSOF      =   23, // iColumn            DETAILSINFO*
        SFVM_COLUMNCLICK       =   24, // iColumn            -
        SFVM_QUERYFSNOTIFY     =   25, // -                  SHChangeNotifyEntry *
        SFVM_DEFITEMCOUNT      =   26, // -                  UINT*
        SFVM_DEFVIEWMODE       =   27, // -                  FOLDERVIEWMODE*
        SFVM_UNMERGEMENU       =   28, // -                  hmenu
        SFVM_UPDATESTATUSBAR   =   31, // fInitialize        -
        SFVM_BACKGROUNDENUM    =   32, // -                  -
        SFVM_DIDDRAGDROP       =   36, // dwEffect           IDataObject *
        SFVM_SETISFV           =   39, // -                  IShellFolderView*
        SFVM_THISIDLIST        =   41, // -                  LPITMIDLIST*
        SFVM_ADDPROPERTYPAGES  =   47, // -                  SFVM_PROPPAGE_DATA *
        SFVM_BACKGROUNDENUMDONE=   48, // -                  -
        SFVM_GETNOTIFY         =   49, // LPITEMIDLIST*      LONG*
        SFVM_GETSORTDEFAULTS   =   53, // iDirection         iParamSort
        SFVM_SIZE              =   57, // -                  -
        SFVM_GETZONE           =   58, // -                  DWORD*
        SFVM_GETPANE           =   59, // Pane ID            DWORD*
        SFVM_GETHELPTOPIC      =   63, // -                  SFVM_HELPTOPIC_DATA *
        SFVM_GETANIMATION      =   68, // HINSTANCE *        WCHAR *
    }
}