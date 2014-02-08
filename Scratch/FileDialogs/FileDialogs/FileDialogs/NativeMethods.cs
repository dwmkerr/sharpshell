using System;
using System.Collections;
using System.Runtime.InteropServices;
using ComTypes = System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Drawing;

namespace FileDialogs
{
    internal static class NativeMethods
    {
        #region Guids

        public static Guid IID_IDataObject              = new Guid("0000010E-0000-0000-C000-000000000046");
        public static Guid IID_IShellBrowser            = new Guid("000214e2-0000-0000-c000-000000000046");
        public static Guid IID_IShellView               = new Guid("000214e3-0000-0000-c000-000000000046");
        public static Guid IID_IContextMenu             = new Guid("000214e4-0000-0000-c000-000000000046");
        public static Guid IID_IShellFolder             = new Guid("000214e6-0000-0000-c000-000000000046");
        public static Guid IID_IPersistFolder           = new Guid("000214ea-0000-0000-c000-000000000046");
        public static Guid IID_IFolderView              = new Guid("cde725b0-ccc9-4519-917e-325d72fab4ce");

        public static Guid CLSID_AutoComplete           = new Guid("{00BB2763-6A77-11D0-A535-00C04FD7D062}");
        public static Guid CLSID_ACLHistory             = new Guid("{00BB2764-6A77-11D0-A535-00C04FD7D062}");
        public static Guid CLSID_ACListISF              = new Guid("{03C036F1-A186-11D0-824A-00AA005B4383}");
        public static Guid CLSID_ACLMRU                 = new Guid("{6756A641-dE71-11D0-831B-00AA005B4383}");
        public static Guid CLSID_ACLMulti               = new Guid("{00BB2765-6A77-11D0-A535-00C04FD7D062}");

        public static string CLSID_MyDocuments          = "::{450D8FBA-AD25-11D0-98A8-0800361B1103}";
        public static string CLSID_Internet             = "::{871C5380-42A0-1069-A2EA-08002B30309D}";
        public static string CLSID_RecycleBin           = "::{645FF040-5081-101B-9F08-00AA002F954E}";
        public static string CLSID_ControlPanel         = "::{21EC2020-3AEA-1069-A2DD-08002B30309D}";
        public static string CLSID_Printers             = "::{2227A280-3AEA-1069-A2DE-08002B30309D}";

        #endregion

        #region Constants

        public static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

        public static int cbFileInfo = Marshal.SizeOf(typeof(SHFILEINFO));
        public static int cbComboBoxInfo = Marshal.SizeOf(typeof(COMBOBOXINFO));
        public static int cbInvokeCommand = Marshal.SizeOf(typeof(CMINVOKECOMMANDINFOEX));

        public const int S_OK = 0;
        public const int S_FALSE = 1;

        // Not implemented
        public const int E_NOTIMPL     = unchecked((int)0x80004001);

        // No such interface supported
        public const int E_NOINTERFACE = unchecked((int)0x80004002);

        // Invalid pointer
        public const int E_POINTER     = unchecked((int)0x80004003);

        // Unspecified error
        public const int E_FAIL        = unchecked((int)0x80004005);

        public const int MAX_PATH = 260;

        public const int MIN_SHELL_ID = 1;
        public const int MAX_SHELL_ID = 30000;

        public const int RESOURCETYPE_DISK = 0x00000001;

        #region Constants from CommCtrl.h

        public const int ILD_NORMAL         = 0x00000000;
        public const int ILD_TRANSPARENT    = 0x00000001;
        public const int ILD_MASK           = 0x00000010;
        public const int ILD_IMAGE          = 0x00000020;
        public const int ILD_ROP            = 0x00000040;
        public const int ILD_BLEND25        = 0x00000002;
        public const int ILD_BLEND50        = 0x00000004;
        public const int ILD_OVERLAYMASK    = 0x0000000F;
        public const int ILD_PRESERVEALPHA  = 0x00001000;
        public const int ILD_SCALE          = 0x00002000;
        public const int ILD_DPISCALE       = 0x00004000;

        public const int ILD_SELECTED       = ILD_BLEND50;
        public const int ILD_FOCUS          = ILD_BLEND25;
        public const int ILD_BLEND          = ILD_BLEND50;

        #endregion

        #region Constants from ShellAPI.h

        public const uint SHGFI_ICON              = 0x00000100;     // get icon
        public const uint SHGFI_DISPLAYNAME       = 0x00000200;     // get display name
        public const uint SHGFI_TYPENAME          = 0x00000400;     // get type name
        public const uint SHGFI_ATTRIBUTES        = 0x00000800;     // get attributes
        public const uint SHGFI_ICONLOCATION      = 0x00001000;     // get attributes
        public const uint SHGFI_EXETYPE           = 0x00002000;     // return exe type
        public const uint SHGFI_SYSICONINDEX      = 0x00004000;     // get system icon index
        public const uint SHGFI_LINKOVERLAY       = 0x00008000;     // put a link overlay on icon
        public const uint SHGFI_SELECTED          = 0x00010000;     // show icon in selected state
        public const uint SHGFI_ATTR_SPECIFIED    = 0x00020000;     // get only specified attributes
        public const uint SHGFI_LARGEICON         = 0x00000000;     // get large icon
        public const uint SHGFI_SMALLICON         = 0x00000001;     // get small icon
        public const uint SHGFI_OPENICON          = 0x00000002;     // get open icon
        public const uint SHGFI_SHELLICONSIZE     = 0x00000004;     // get shell size icon
        public const uint SHGFI_PIDL              = 0x00000008;     // pszPath is a pidl
        public const uint SHGFI_USEFILEATTRIBUTES = 0x00000010;     // use passed dwFileAttribute

        public const uint SHGFI_ADDOVERLAYS       = 0x00000020;     // apply the appropriate overlays
        public const uint SHGFI_OVERLAYINDEX      = 0x00000040;     // Get the index of the overlay

        public const int SHIL_LARGE      = 0;   // normally 32x32
        public const int SHIL_SMALL      = 1;   // normally 16x16
        public const int SHIL_EXTRALARGE = 2;
        public const int SHIL_SYSSMALL   = 3;   // like SHIL_SMALL, but tracks system small icon metric correctly

        #endregion

        #region Constants from ShlObj.h

        public const int CDBOSC_SETFOCUS    = 0x00000000;
        public const int CDBOSC_KILLFOCUS   = 0x00000001;
        public const int CDBOSC_SELCHANGE   = 0x00000002;
        public const int CDBOSC_RENAME      = 0x00000003;
        public const int CDBOSC_STATECHANGE = 0x00000004;

        public const string CFSTR_SHELLIDLIST = "Shell IDList Array";

        /*
         * QueryContextMenu uFlags
         */
        public const uint CMF_NORMAL        = 0x00000000;
        public const uint CMF_DEFAULTONLY   = 0x00000001;
        public const uint CMF_VERBSONLY     = 0x00000002;
        public const uint CMF_EXPLORE       = 0x00000004;
        public const uint CMF_NOVERBS       = 0x00000008;
        public const uint CMF_CANRENAME     = 0x00000010;
        public const uint CMF_NODEFAULT     = 0x00000020;
        public const uint CMF_INCLUDESTATIC = 0x00000040;      // rarely used verbs
        public const uint CMF_EXTENDEDVERBS = 0x00000100;
        public const uint CMF_RESERVED      = 0xffff0000;      // View specific

        /*
         * CMINVOKECOMMANDINFO fMask
         */
        public const int CMIC_MASK_ICON           = 0x00000010;
        public const int CMIC_MASK_HOTKEY         = 0x00000020;
        public const int CMIC_MASK_FLAG_NO_UI     = 0x00000400;
        public const int CMIC_MASK_UNICODE        = 0x00004000;
        public const int CMIC_MASK_NO_CONSOLE     = 0x00008000;
        public const int CMIC_MASK_NOZONECHECKS   = 0x00800000;
        public const int CMIC_MASK_FLAG_LOG_USAGE = 0x04000000;
        public const int CMIC_MASK_SHIFT_DOWN     = 0x10000000;
        public const int CMIC_MASK_PTINVOKE       = 0x20000000;
        public const int CMIC_MASK_CONTROL_DOWN   = 0x40000000;

        public const uint SVSI_DESELECT       = 0x00000000;
        public const uint SVSI_SELECT         = 0x00000001;
        public const uint SVSI_EDIT           = 0x00000003;  // includes select
        public const uint SVSI_DESELECTOTHERS = 0x00000004;
        public const uint SVSI_ENSUREVISIBLE  = 0x00000008;
        public const uint SVSI_FOCUSED        = 0x00000010;
        public const uint SVSI_TRANSLATEPT    = 0x00000020;
        public const uint SVSI_SELECTIONMARK  = 0x00000040;
        public const uint SVSI_POSITIONITEM   = 0x00000080;
        public const uint SVSI_CHECK          = 0x00000100;
        public const uint SVSI_NOSTATECHANGE  = 0x80000000;

        #endregion

        #region Constants from ShObjIdl.h

        public const uint SBSP_DEFBROWSER         = 0x0000;
        public const uint SBSP_SAMEBROWSER        = 0x0001;
        public const uint SBSP_NEWBROWSER         = 0x0002;
        public const uint SBSP_DEFMODE            = 0x0000;
        public const uint SBSP_OPENMODE           = 0x0010;
        public const uint SBSP_EXPLOREMODE        = 0x0020;
        public const uint SBSP_HELPMODE           = 0x0040; // IEUNIX : Help window uses this.
        public const uint SBSP_NOTRANSFERHIST     = 0x0080;
        public const uint SBSP_ABSOLUTE           = 0x0000;
        public const uint SBSP_RELATIVE           = 0x1000;
        public const uint SBSP_PARENT             = 0x2000;
        public const uint SBSP_NAVIGATEBACK       = 0x4000;
        public const uint SBSP_NAVIGATEFORWARD    = 0x8000;
        public const uint SBSP_ALLOW_AUTONAVIGATE = 0x10000;
        public const uint SBSP_CALLERUNTRUSTED      = 0x00800000;
        public const uint SBSP_TRUSTFIRSTDOWNLOAD   = 0x01000000;
        public const uint SBSP_UNTRUSTEDFORDOWNLOAD = 0x02000000;
        public const uint SBSP_NOAUTOSELECT         = 0x04000000;
        public const uint SBSP_WRITENOHISTORY       = 0x08000000;
        public const uint SBSP_TRUSTEDFORACTIVEX    = 0x10000000;
        public const uint SBSP_REDIRECT                     = 0x40000000;
        public const uint SBSP_INITIATEDBYHLINKFRAME        = 0x80000000;

        #endregion

        #region Constants from WinError.h

        public const int NOERROR = 0;

        #endregion

        #region Constants from WinNT.h

        /*
         * Define access rights to files and directories
         */
        public const int FILE_SHARE_READ = 0x00000001;
        public const int FILE_SHARE_WRITE = 0x00000002;
        public const int FILE_SHARE_DELETE = 0x00000004;
        public const int FILE_ATTRIBUTE_READONLY = 0x00000001;
        public const int FILE_ATTRIBUTE_HIDDEN = 0x00000002;
        public const int FILE_ATTRIBUTE_SYSTEM = 0x00000004;
        public const int FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
        public const int FILE_ATTRIBUTE_ARCHIVE = 0x00000020;
        public const int FILE_ATTRIBUTE_DEVICE = 0x00000040;
        public const int FILE_ATTRIBUTE_NORMAL = 0x00000080;
        public const int FILE_ATTRIBUTE_TEMPORARY = 0x00000100;
        public const int FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200;
        public const int FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400;
        public const int FILE_ATTRIBUTE_COMPRESSED = 0x00000800;
        public const int FILE_ATTRIBUTE_OFFLINE = 0x00001000;
        public const int FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000;
        public const int FILE_ATTRIBUTE_ENCRYPTED = 0x00004000;
        public const int FILE_NOTIFY_CHANGE_FILE_NAME = 0x00000001;
        public const int FILE_NOTIFY_CHANGE_DIR_NAME = 0x00000002;
        public const int FILE_NOTIFY_CHANGE_ATTRIBUTES = 0x00000004;
        public const int FILE_NOTIFY_CHANGE_SIZE = 0x00000008;
        public const int FILE_NOTIFY_CHANGE_LAST_WRITE = 0x00000010;
        public const int FILE_NOTIFY_CHANGE_LAST_ACCESS = 0x00000020;
        public const int FILE_NOTIFY_CHANGE_CREATION = 0x00000040;
        public const int FILE_NOTIFY_CHANGE_SECURITY = 0x00000100;
        public const int FILE_ACTION_ADDED = 0x00000001;
        public const int FILE_ACTION_REMOVED = 0x00000002;
        public const int FILE_ACTION_MODIFIED = 0x00000003;
        public const int FILE_ACTION_RENAMED_OLD_NAME = 0x00000004;
        public const int FILE_ACTION_RENAMED_NEW_NAME = 0x00000005;
        public const int FILE_CASE_SENSITIVE_SEARCH = 0x00000001;
        public const int FILE_CASE_PRESERVED_NAMES = 0x00000002;
        public const int FILE_UNICODE_ON_DISK = 0x00000004;
        public const int FILE_PERSISTENT_ACLS = 0x00000008;
        public const int FILE_FILE_COMPRESSION = 0x00000010;
        public const int FILE_VOLUME_QUOTAS = 0x00000020;
        public const int FILE_SUPPORTS_SPARSE_FILES = 0x00000040;
        public const int FILE_SUPPORTS_REPARSE_POINTS = 0x00000080;
        public const int FILE_SUPPORTS_REMOTE_STORAGE = 0x00000100;
        public const int FILE_VOLUME_IS_COMPRESSED = 0x00008000;
        public const int FILE_SUPPORTS_OBJECT_IDS = 0x00010000;
        public const int FILE_SUPPORTS_ENCRYPTION = 0x00020000;
        public const int FILE_NAMED_STREAMS = 0x00040000;
        public const int FILE_READ_ONLY_VOLUME = 0x00080000;

        #endregion

        #region Constants from WinUser.h

        /*
         * Combo Box Notification Codes
         */
        public const int CBN_DROPDOWN = 7;
        public const int CBN_CLOSEUP = 8;

        /*
         * ShowWindow() Commands
         */
        public const int SW_HIDE = 0;
        public const int SW_SHOWNORMAL = 1;
        public const int SW_NORMAL = 1;
        public const int SW_SHOWMINIMIZED = 2;
        public const int SW_SHOWMAXIMIZED = 3;
        public const int SW_MAXIMIZE = 3;
        public const int SW_SHOWNOACTIVATE = 4;
        public const int SW_SHOW = 5;
        public const int SW_MINIMIZE = 6;
        public const int SW_SHOWMINNOACTIVE = 7;
        public const int SW_SHOWNA = 8;
        public const int SW_RESTORE = 9;
        public const int SW_SHOWDEFAULT = 10;
        public const int SW_FORCEMINIMIZE = 11;
        public const int SW_MAX = 11;

        /*
         * Window Messages
         */
        public const int WM_CONTEXTMENU         = 0x007B;
        public const int WM_NCLBUTTONDBLCLK 	= 0x00A3;
        public const int WM_CTLCOLORLISTBOX 	= 0x0134;
        public const int WM_USER                = 0x0400;
        public const int WM_GETISHELLBROWSER    = (WM_USER + 7);

        /*
         * SetWindowPos Flags
         */
        public const int SWP_NOSIZE         = 0x0001;
        public const int SWP_NOMOVE         = 0x0002;
        public const int SWP_NOZORDER       = 0x0004;
        public const int SWP_NOREDRAW       = 0x0008;
        public const int SWP_NOACTIVATE     = 0x0010;
        public const int SWP_FRAMECHANGED   = 0x0020;
        public const int SWP_SHOWWINDOW     = 0x0040;
        public const int SWP_HIDEWINDOW     = 0x0080;
        public const int SWP_NOCOPYBITS     = 0x0100;
        public const int SWP_NOOWNERZORDER  = 0x0200;
        public const int SWP_NOSENDCHANGING = 0x0400;

        public const int SWP_DRAWFRAME      = SWP_FRAMECHANGED;
        public const int SWP_NOREPOSITION   = SWP_NOOWNERZORDER;

        /*
         * Menu flags for Add/Check/EnableMenuItem()
         */
        public const int MF_INSERT          = 0x00000000;
        public const int MF_CHANGE          = 0x00000080;
        public const int MF_APPEND          = 0x00000100;
        public const int MF_DELETE          = 0x00000200;
        public const int MF_REMOVE          = 0x00001000;

        public const int MF_BYCOMMAND       = 0x00000000;
        public const int MF_BYPOSITION      = 0x00000400;

        public const int MF_SEPARATOR       = 0x00000800;

        public const int MF_ENABLED         = 0x00000000;
        public const int MF_GRAYED          = 0x00000001;
        public const int MF_DISABLED        = 0x00000002;

        public const int MF_UNCHECKED       = 0x00000000;
        public const int MF_CHECKED         = 0x00000008;
        public const int MF_USECHECKBITMAPS = 0x00000200;

        public const int MF_STRING          = 0x00000000;
        public const int MF_BITMAP          = 0x00000004;
        public const int MF_OWNERDRAW       = 0x00000100;

        public const int MF_POPUP           = 0x00000010;
        public const int MF_MENUBARBREAK    = 0x00000020;
        public const int MF_MENUBREAK       = 0x00000040;

        public const int MF_UNHILITE        = 0x00000000;
        public const int MF_HILITE          = 0x00000080;

        /*
         * System Menu Command Values
         */
        public const int SC_SIZE         = 0xF000;
        public const int SC_MOVE         = 0xF010;
        public const int SC_MINIMIZE     = 0xF020;
        public const int SC_MAXIMIZE     = 0xF030;
        public const int SC_NEXTWINDOW   = 0xF040;
        public const int SC_PREVWINDOW   = 0xF050;
        public const int SC_CLOSE        = 0xF060;
        public const int SC_VSCROLL      = 0xF070;
        public const int SC_HSCROLL      = 0xF080;
        public const int SC_MOUSEMENU    = 0xF090;
        public const int SC_KEYMENU      = 0xF100;
        public const int SC_ARRANGE      = 0xF110;
        public const int SC_RESTORE      = 0xF120;
        public const int SC_TASKLIST     = 0xF130;
        public const int SC_SCREENSAVE   = 0xF140;
        public const int SC_HOTKEY       = 0xF150;
        public const int SC_DEFAULT      = 0xF160;
        public const int SC_MONITORPOWER = 0xF170;
        public const int SC_CONTEXTHELP  = 0xF180;
        public const int SC_SEPARATOR    = 0xF00F;

        #endregion

        #endregion

        #region Classes

        [SuppressUnmanagedCodeSecurity, ComVisible(false)]
        public class ComCtl32
        {
            [DllImport("comctl32.dll")]
            public static extern IntPtr ImageList_GetIcon(IntPtr himl, int i, int flags);

            [DllImport("comctl32.dll")]
            public static extern int ImageList_ReplaceIcon(IntPtr himl, int index, IntPtr hicon);
        }

        [SuppressUnmanagedCodeSecurity, ComVisible(false)]
        public class Kernel32
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern IntPtr GlobalLock(IntPtr hMem);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern bool GlobalUnlock(IntPtr hMem);
        }

        [SuppressUnmanagedCodeSecurity, ComVisible(false)]
        public class Mpr
        {
            [DllImport("mpr.dll")]
            public static extern int WNetConnectionDialog(IntPtr phWnd, int piType);
        }

        [SuppressUnmanagedCodeSecurity, ComVisible(false)]
        public class Ole32
        {
            [DllImport("ole32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern void ReleaseStgMedium(ref ComTypes.STGMEDIUM pmedium);
        }

        [SuppressUnmanagedCodeSecurity, ComVisible(false)]
        public class Shlwapi
        {
            [DllImport("shlwapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern Int32 StrRetToBuf(IntPtr pstr, IntPtr pidl, StringBuilder pszBuf, int cchBuf);
        }

        [SuppressUnmanagedCodeSecurity, ComVisible(false)]
        public class Shell32
        {
            // Tests whether two ITEMIDLIST structures are equal in a binary comparison
            [DllImport("shell32.dll")]
            public static extern bool ILIsEqual(IntPtr pidl1, IntPtr pidl2);

            // Clones an ITEMIDLIST structure
            [DllImport("shell32.dll")]
            public static extern IntPtr ILClone(IntPtr fullPidl);

            // Combines two ITEMIDLIST structures
            [DllImport("shell32.dll")]
            public static extern IntPtr ILCombine(IntPtr fullPidl, IntPtr relPidl);

            // Gets the next SHITEMID structure in an ITEMIDLIST structure
            [DllImport("shell32.dll")]
            public static extern IntPtr ILGetNext(IntPtr relPidl);

            // Tests whether an ITEMIDLIST structure is the parent of another ITEMIDLIST structure
            [DllImport("shell32.dll")]
            public static extern bool ILIsParent(IntPtr fullPidl1, IntPtr fullPidl2, bool fImmediate);

            [DllImport("shell32.dll")]
            public static extern void ILFree(IntPtr pidl); 

            // Clones the first SHITEMID structure in an ITEMIDLIST structure
            [DllImport("shell32.dll")]
            public static extern IntPtr ILCloneFirst(IntPtr relPidl);

            // Returns a pointer to the last SHITEMID structure in an ITEMIDLIST structure
            [DllImport("shell32.dll")]
            public static extern IntPtr ILFindLastID(IntPtr fullPidl);

            // Removes the last SHITEMID structure from an ITEMIDLIST structure
            [DllImport("shell32.dll")]
            public static extern bool ILRemoveLastID(IntPtr fullPidl);

            //  Helper function which returns a IShellFolder interface to the desktop
            // folder. This is equivalent to call CoCreateInstance with CLSID_ShellDesktop.
            [DllImport("shell32.dll")]
            public static extern void SHGetDesktopFolder(out IntPtr ppshf);

            // The SHGetFileInfo API provides an easy way to get attributes
            // for a file given a pathname.
            [DllImport("shell32.dll")]
            public static extern IntPtr SHGetFileInfo(IntPtr pszPath, int dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, uint uFlags);

            [DllImport("shell32.dll")]
            public static extern IntPtr SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, uint uFlags);

            [DllImport("shell32.dll")]
            public static extern int SHGetFolderLocation(IntPtr hwndOwner, int nFolder, IntPtr hToken, uint dwReserved, out IntPtr ppidl);

            [DllImport("shell32.dll", EntryPoint = "#727")]
            public static extern int SHGetImageListHandle(int iImageList, ref Guid riid, ref IntPtr handle);

            [DllImport("shell32.dll")]
            public static extern void SHGetSpecialFolderLocation(IntPtr hwndOwner, int nFolder, out IntPtr ppidl);
        }

        [SuppressUnmanagedCodeSecurity, ComVisible(false)]
        public class User32
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr CreatePopupMenu();

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
            public static extern bool DestroyIcon(IntPtr hIcon);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern bool DestroyMenu(IntPtr hMenu);

            [DllImport("user32.dll")]
            public static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, int nMaxCount);

            [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern bool GetComboBoxInfo(IntPtr hwndCombo, ref COMBOBOXINFO info);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern IntPtr GetFocus();

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int RegisterClipboardFormat(string format);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern IntPtr SetFocus(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern bool SetWindowPos(HandleRef hWnd, HandleRef hWndInsertAfter, int x, int y, int cx, int cy, int flags);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern bool ShowWindow(HandleRef hWnd, int nCmdShow);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern IntPtr GetSystemMenu(HandleRef hWnd, bool bRevert);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern bool RemoveMenu(HandleRef hMenu, int uPosition, int uFlags);
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public class Util
        {
            public static int HIWORD(int n)
            {
                return ((n >> 0x10) & 0xFFFF);
            }

            public static int HIWORD(IntPtr n)
            {
                return HIWORD((int)((long)n));
            }

            public static int SignedHIWORD(int n)
            {
                return (short)((n >> 0x10) & 0xffff);
            }

            public static int SignedHIWORD(IntPtr n)
            {
                return SignedHIWORD((int)((long)n));
            }

            public static int LOWORD(int n)
            {
                return (n & 0xFFFF);
            }

            public static int LOWORD(IntPtr n)
            {
                return LOWORD((int)n);
            }

            public static int SignedLOWORD(int n)
            {
                return (short)(n & 0xffff);
            }

            public static int SignedLOWORD(IntPtr n)
            {
                return SignedLOWORD((int)((long)n));
            }

            public static int MAKELONG(int low, int high)
            {
                return ((high << 0x10) | (low & 0xffff));
            }

            public static IntPtr MAKELPARAM(int low, int high)
            {
                return (IntPtr)((high << 0x10) | (low & 0xffff));
            }
        }

        #endregion

        #region Methods

        public static int MAKEINTRESOURCE(int i)
        {
            return 0x0000FFFF & i;
        }

        public static bool SUCCEEDED(int hr)
        {
            return (hr >= 0);
        }

        #endregion

        #region Interfaces

        [ComImport, Guid("00000101-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IEnumString
        {
            [PreserveSig]
            int Next(int celt, [Out, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] rgelt, IntPtr pceltFetched);
            [PreserveSig]
            int Skip(int celt);
            void Reset();
            void Clone(out IEnumString ppenum);
        }

        //-------------------------------------------------------------------------
        //
        // IACList interface
        //
        //
        // [Member functions]
        //
        // IObjMgr::Expand(LPCOLESTR)
        //   This function tells an autocomplete list to expand a specific string.
        //
        // If the user enters a multi-level path, AutoComplete (CLSID_AutoComplete)
        // will use this interface to tell the "AutoComplete Lists" where to expand
        // the results.
        //
        // For Example, if the user enters "C:\Program Files\Micros", AutoComplete
        // first completely enumerate the "AutoComplete Lists" via IEnumString.  Then it
        // will call the "AutoComplete Lists" with IACList::Expand(L"C:\Program Files").
        // It will then enumerate the IEnumString interface again to get results in
        // that directory.
        //-------------------------------------------------------------------------

        [ComImport, Guid("77A130B0-94FD-11D0-A544-00C04FD7D062")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IACList
        {
            //[PreserveSig]
            //int Next(int celt, [Out, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] string[] rgelt, IntPtr pceltFetched);
            //[PreserveSig]
            //int Skip(int celt);
            //void Reset();
            //void Clone(out IEnumString ppenum);

            [PreserveSig]
            int Expand([MarshalAs(UnmanagedType.LPWStr)] string pszExpand);
        }

        //-------------------------------------------------------------------------
        //
        // IACList2 interface
        //
        // [Description]
        //              This interface exists to allow the caller to set filter criteria
        // for an AutoComplete List.  AutoComplete Lists generates the list of
        // possible AutoComplete completions.  CLSID_ACListISF is one AutoComplete
        // List COM object that implements this interface.
        //-------------------------------------------------------------------------

        [ComImport, Guid("470141A0-5186-11D2-BBB6-0060977B464C")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IACList2
        {
            [PreserveSig]
            int Expand([MarshalAs(UnmanagedType.LPWStr)] string pszExpand);
            [PreserveSig]
            int SetOptions(uint dwFlag);
            [PreserveSig]
            int GetOptions(out uint pdwFlag);
        }

        [ComImport, Guid("00BB2762-6A77-11D0-A535-00C04FD7D062")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IAutoComplete
        {
            [PreserveSig]
            int Init(IntPtr hwndEdit, [MarshalAs(UnmanagedType.IUnknown)] object punkACL, [MarshalAs(UnmanagedType.LPWStr)] string pwszRegKeyPath, [MarshalAs(UnmanagedType.LPWStr)] string pwszQuickComplete);				// Pointer to an optional string that specifies the format to be
            [PreserveSig]
            int Enable(int fEnable);
        }

        [ComImport, Guid("EAC04BC0-3791-11D2-BB95-0060977B464C")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IAutoComplete2
        {
            [PreserveSig]
            int Init(IntPtr hwndEdit, [MarshalAs(UnmanagedType.IUnknown)] object punkACL, [MarshalAs(UnmanagedType.LPWStr)] string pwszRegKeyPath, [MarshalAs(UnmanagedType.LPWStr)] string pwszQuickComplete);				// Pointer to an optional string that specifies the format to be
            [PreserveSig]
            int Enable(int fEnable);
            [PreserveSig]
            int SetOptions(uint dwFlag);
            [PreserveSig]
            int GetOptions(out uint pdwFlag);
        }

        /// <summary>
        /// The IContextMenu interface is called by the Shell to either create
        /// or merge a shortcut menu associated with a Shell object.
        /// </summary>
        [ComImport, Guid("000214e4-0000-0000-c000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IContextMenu
        {
            [PreserveSig]
            int QueryContextMenu(IntPtr hMenu, uint indexMenu, uint idCmdFirst, uint idCmdLast, uint uFlags);
            [PreserveSig]
            int InvokeCommand(ref CMINVOKECOMMANDINFOEX ici);
            void GetCommandString(uint idCmd, uint uFlags, IntPtr pwReserved, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, uint cchMax);
        }

        [ComImport, Guid("000214f2-0000-0000-c000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IEnumIDList
        {
            [PreserveSig]
            int Next(uint celt, out IntPtr rgelt, out int pceltFetched);
            void Skip(uint celt);
            void Reset();
            void Clone(out IEnumIDList ppenum);
        }

        [ComImport, Guid("0000010b-0000-0000-c000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IPersistFile
        {
            void GetClassID(out Guid pClassID);
            [PreserveSig]
            int IsDirty();
            void Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, int dwMode);
            void Save([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, [MarshalAs(UnmanagedType.Bool)] bool fRemember);
            void SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);
            void GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out IntPtr ppszFileName);
        }

        //-------------------------------------------------------------------------
        // ICommDlgBrowser interface
        //
        //  ICommDlgBrowser interface is the interface that is provided by the new
        // common dialog window to hook and modify the behavior of IShellView.  When
        // a default view is created, it queries its parent IShellBrowser for the
        // ICommDlgBrowser interface.  If supported, it calls out to that interface
        // in several cases that need to behave differently in a dialog.
        //
        // Member functions:
        //
        //  ICommDlgBrowser::OnDefaultCommand()
        //    Called when the user double-clicks in the view or presses Enter.  The
        //   browser should return S_OK if it processed the action itself, S_FALSE
        //   to let the view perform the default action.
        //
        //  ICommDlgBrowser::OnStateChange(ULONG uChange)
        //    Called when some states in the view change.  'uChange' is one of the
        //   CDBOSC_* values.  This call is made after the state (selection, focus,
        //   etc) has changed.  There is no return value.
        //
        //  ICommDlgBrowser::IncludeObject(LPCITEMIDLIST pidl)
        //    Called when the view is enumerating objects.  'pidl' is a relative
        //   IDLIST.  The browser should return S_OK to include the object in the
        //   view, S_FALSE to hide it
        //
        //-------------------------------------------------------------------------

        [ComImport, Guid("000214f1-0000-0000-c000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface ICommDlgBrowser
        {
            [PreserveSig]
            int OnDefaultCommand([In] IntPtr ppshv);
            [PreserveSig]
            int OnStateChange([In] IntPtr ppshv, IntPtr uChange);
            [PreserveSig]
            int IncludeObject([In] IntPtr ppshv, IntPtr pidl);
        }

        //--------------------------------------------------------------------------
        //
        // Interface:   IShellBrowser
        //
        //  IShellBrowser interface is the interface that is provided by the shell
        // explorer/folder frame window. When it creates the 'contents pane' of
        // a shell folder (which provides IShellFolder interface), it calls its
        // CreateViewObject member function to create an IShellView object. Then,
        // it calls its CreateViewWindow member to create the 'contents pane'
        // window. The pointer to the IShellBrowser interface is passed to
        // the IShellView object as a parameter to this CreateViewWindow member
        // function call.
        //
        //    +--------------------------+  <-- Explorer window
        //    | [] Explorer              |
        //    |--------------------------+       IShellBrowser
        //    | File Edit View ..        |
        //    |--------------------------|
        //    |        |                 |
        //    |        |              <-------- Content pane
        //    |        |                 |
        //    |        |                 |       IShellView
        //    |        |                 |
        //    |        |                 |
        //    +--------------------------+
        //
        //
        //
        // [Member functions]
        //
        //
        // IShellBrowser::GetWindow(phwnd)
        //
        //   Inherited from IOleWindow::GetWindow.
        //
        //
        // IShellBrowser::ContextSensitiveHelp(fEnterMode)
        //
        //   Inherited from IOleWindow::ContextSensitiveHelp.
        //
        //
        // IShellBrowser::InsertMenusSB(hmenuShared, lpMenuWidths)
        //
        //   Similar to the IOleInPlaceFrame::InsertMenus. The explorer will put
        //  'File' and 'Edit' pulldown in the File menu group, 'View' and 'Tools'
        //  in the Container menu group and 'Help' in the Window menu group. Each
        //  pulldown menu will have a uniqu ID, FCIDM_MENU_FILE/EDIT/VIEW/TOOLS/HELP
        //  The view is allowed to insert menuitems into those sub-menus by those
        //  IDs must be between FCIDM_SHVIEWFIRST and FCIDM_SHVIEWLAST.
        //
        //
        // IShellBrowser::SetMenuSB(hmenuShared, holemenu, hwndActiveObject)
        //
        //   Similar to the IOleInPlaceFrame::SetMenu. The explorer ignores the
        //  holemenu parameter (reserved for future enhancement)  and performs
        //  menu-dispatch based on the menuitem IDs (see the description above).
        //  It is important to note that the explorer will add different
        //  set of menuitems depending on whether the view has a focus or not.
        //  Therefore, it is very important to call ISB::OnViewWindowActivate
        //  whenever the view window (or its children) gets the focus.
        //
        //
        // IShellBrowser::RemoveMenusSB(hmenuShared)
        //
        //   Same as the IOleInPlaceFrame::RemoveMenus.
        //
        //
        // IShellBrowser::SetStatusTextSB(pszStatusText)
        //
        //   Same as the IOleInPlaceFrame::SetStatusText. It is also possible to
        //  send messages directly to the status window via SendControlMsg.
        //
        //
        // IShellBrowser::EnableModelessSB(fEnable)
        //
        //   Same as the IOleInPlaceFrame::EnableModeless.
        //
        //
        // IShellBrowser::TranslateAcceleratorSB(lpmsg, wID)
        //
        //   Same as the IOleInPlaceFrame::TranslateAccelerator, but will be
        //  never called because we don't support EXEs (i.e., the explorer has
        //  the message loop). This member function is defined here for possible
        //  future enhancement.
        //
        //
        // IShellBrowser::BrowseObject(pidl, wFlags)")
        //
        //   The view calls this member to let shell explorer browse to another")
        //  folder. The pidl and wFlags specifies the folder to be browsed.")
        //
        //  Following three flags specifies whether it creates another window or not.
        //   SBSP_SAMEBROWSER  -- Browse to another folder with the same window.
        //   SBSP_NEWBROWSER   -- Creates another window for the specified folder.
        //   SBSP_DEFBROWSER   -- Default behavior (respects the view option).
        //
        //  Following three flags specifies open, explore, or default mode. These   .
        //  are ignored if SBSP_SAMEBROWSER or (SBSP_DEFBROWSER && (single window   .
        //  browser || explorer)).                                                  .
        //   SBSP_OPENMODE     -- Use a normal folder window
        //   SBSP_EXPLOREMODE  -- Use an explorer window
        //   SBSP_DEFMODE      -- Use the same as the current window
        //
        //  Following three flags specifies the pidl.
        //   SBSP_ABSOLUTE -- pidl is an absolute pidl (relative from desktop)
        //   SBSP_RELATIVE -- pidl is relative from the current folder.
        //   SBSP_PARENT   -- Browse the parent folder (ignores the pidl)
        //   SBSP_NAVIGATEBACK    -- Navigate back (ignores the pidl)
        //   SBSP_NAVIGATEFORWARD -- Navigate forward (ignores the pidl)
        //
        //  Following two flags control history manipulation as result of navigate
        //   SBSP_WRITENOHISTORY -- write no history (shell folder) entry
        //   SBSP_NOAUTOSELECT -- suppress selection in history pane
        //
        // IShellBrowser::GetViewStateStream(grfMode, ppstm)
        //
        //   The browser returns an IStream interface as the storage for view
        //  specific state information.
        //
        //   grfMode -- Specifies the read/write access (STGM_READ/WRITE/READWRITE)
        //   ppstm   -- Specifies the IStream *variable to be filled.
        //
        //
        // IShellBrowser::GetControlWindow(id, phwnd)
        //
        //   The shell view may call this member function to get the window handle
        //  of Explorer controls (toolbar or status winodw -- FCW_TOOLBAR or
        //  FCW_STATUS).
        //
        //
        // IShellBrowser::SendControlMsg(id, uMsg, wParam, lParam, pret)
        //
        //   The shell view calls this member function to send control messages to
        //  one of Explorer controls (toolbar or status window -- FCW_TOOLBAR or
        //  FCW_STATUS).
        //
        //
        // IShellBrowser::QueryActiveShellView(IShellView * ppshv)
        //
        //   This member returns currently activated (displayed) shellview object.
        //  A shellview never need to call this member function.
        //
        //
        // IShellBrowser::OnViewWindowActive(pshv)
        //
        //   The shell view window calls this member function when the view window
        //  (or one of its children) got the focus. It MUST call this member before
        //  calling IShellBrowser::InsertMenus, because it will insert different
        //  set of menu items depending on whether the view has the focus or not.
        //
        //
        // IShellBrowser::SetToolbarItems(lpButtons, nButtons, uFlags)
        //
        //   The view calls this function to add toolbar items to the exporer's
        //  toolbar. 'lpButtons' and 'nButtons' specifies the array of toolbar
        //  items. 'uFlags' must be one of FCT_MERGE, FCT_CONFIGABLE, FCT_ADDTOEND.
        //
        //-------------------------------------------------------------------------

        [ComImport, Guid("000214e2-0000-0000-c000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IShellBrowser
        {
            [PreserveSig]
            int GetWindow(out IntPtr hwnd);
            [PreserveSig]
            int ContextSensitiveHelp(int fEnterMode);

            [PreserveSig]
            int InsertMenusSB(IntPtr hmenuShared, IntPtr lpMenuWidths);
            [PreserveSig]
            int SetMenuSB(IntPtr hmenuShared, IntPtr holemenuRes, IntPtr hwndActiveObject);
            [PreserveSig]
            int RemoveMenusSB(IntPtr hmenuShared);
            [PreserveSig]
            int SetStatusTextSB(IntPtr pszStatusText);
            [PreserveSig]
            int EnableModelessSB(bool fEnable);
            [PreserveSig]
            int TranslateAcceleratorSB(IntPtr pmsg, short wID);
            [PreserveSig]
            int BrowseObject(IntPtr pidl, [MarshalAs(UnmanagedType.U4)] uint wFlags);
            [PreserveSig]
            int GetViewStateStream(uint grfMode, IntPtr ppStrm);
            [PreserveSig]
            int GetControlWindow(uint id, out IntPtr phwnd);
            [PreserveSig]
            int SendControlMsg(uint id, uint uMsg, uint wParam, uint lParam, IntPtr pret);
            [PreserveSig]
            int QueryActiveShellView([MarshalAs(UnmanagedType.Interface)] ref IShellView ppshv);
            [PreserveSig]
            int OnViewWindowActive([MarshalAs(UnmanagedType.Interface)] IShellView pshv);
            [PreserveSig]
            int SetToolbarItems(IntPtr lpButtons, uint nButtons, uint uFlags);
        }

        //==========================================================================
        //
        // Interface:   IShellView
        //
        // IShellView::GetWindow(phwnd)
        //
        //   Inherited from IOleWindow::GetWindow.
        //
        //
        // IShellView::ContextSensitiveHelp(fEnterMode)
        //
        //   Inherited from IOleWindow::ContextSensitiveHelp.
        //
        //
        // IShellView::TranslateAccelerator(lpmsg)
        //
        //   Similar to IOleInPlaceActiveObject::TranlateAccelerator. The explorer
        //  calls this function BEFORE any other translation. Returning S_OK
        //  indicates that the message was translated (eaten) and should not be
        //  translated or dispatched by the explorer.
        //
        //
        // IShellView::EnableModeless(fEnable)
        //   Similar to IOleInPlaceActiveObject::EnableModeless.
        //
        //
        // IShellView::UIActivate(uState)
        //
        //   The explorer calls this member function whenever the activation
        //  state of the view window is changed by a certain event that is
        //  NOT caused by the shell view itself.
        //
        //   SVUIA_DEACTIVATE will be passed when the explorer is about to
        //  destroy the shell view window; the shell view is supposed to remove
        //  all the extended UIs (typically merged menu and modeless popup windows).
        //
        //   SVUIA_ACTIVATE_NOFOCUS will be passsed when the shell view is losing
        //  the input focus or the shell view has been just created without the
        //  input focus; the shell view is supposed to set menuitems appropriate
        //  for non-focused state (no selection specific items should be added).
        //
        //   SVUIA_ACTIVATE_FOCUS will be passed when the explorer has just
        //  created the view window with the input focus; the shell view is
        //  supposed to set menuitems appropriate for focused state.
        //
        //   SVUIA_INPLACEACTIVATE(new) will be passed when the shell view is opened
        //  within an ActiveX control, which is not a UI active. In this case,
        //  the shell view should not merge menus or put toolbas. To be compatible
        //  with Win95 client, we don't pass this value unless the view supports
        //  IShellView2.
        //
        //   The shell view should not change focus within this member function.
        //  The shell view should not hook the WM_KILLFOCUS message to remerge
        //  menuitems. However, the shell view typically hook the WM_SETFOCUS
        //  message, and re-merge the menu after calling IShellBrowser::
        //  OnViewWindowActivated.
        //
        //   One of the ACTIVATE / INPLACEACTIVATE messages will be sent when
        //  the view window becomes the currently displayed view.  On Win95 systems,
        //  this will happen immediately after the CreateViewWindow call.  On IE4, Win98,
        //  and NT5 systems this may happen when the view reports it is ready (if the
        //  IShellView supports async creation).  This can be used as a hint as to when
        //  to make your view window visible.  Note: the Win95/Win98/NT4 common dialogs
        //  do not send either of these on creation.
        //
        //
        // IShellView::Refresh()
        //
        //   The explorer calls this member when the view needs to refresh its
        //  contents (such as when the user hits F5 key).
        //
        //
        // IShellView::CreateViewWindow
        //
        //   This member creates the view window (right-pane of the explorer or the
        //  client window of the folder window).
        //
        //
        // IShellView::DestroyViewWindow
        //
        //   This member destroys the view window.
        //
        //
        // IShellView::GetCurrentInfo
        //
        //   This member returns the folder settings.
        //
        //
        // IShellView::AddPropertySHeetPages
        //
        //   The explorer calls this member when it is opening the option property
        //  sheet. This allows the view to add additional pages to it.
        //
        //
        // IShellView::SaveViewState()
        //
        //   The explorer calls this member when the shell view is supposed to
        //  store its view settings. The shell view is supposed to get a view
        //  stream by calling IShellBrowser::GetViewStateStream and store the
        //  current view state into that stream.
        //
        //
        // IShellView::SelectItem(pidlItem, uFlags)
        //
        //   The explorer calls this member to change the selection state of
        //  item(s) within the shell view window.  If pidlItem is NULL and uFlags
        //  is SVSI_DESELECTOTHERS, all items should be deselected.
        //
        //-------------------------------------------------------------------------

        [ComImport, Guid("000214e3-0000-0000-c000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IShellView
        {
            [PreserveSig]
            int GetWindow(out IntPtr hwnd);
            [PreserveSig]
            int ContextSensitiveHelp(int fEnterMode);

            [PreserveSig]
            long TranslateAccelerator(ref System.Windows.Forms.Message lpmsg);

            void EnableModeless(bool fEnable);
            void UIActivate([MarshalAs(UnmanagedType.U4)] uint uState);
            void Refresh();
            [PreserveSig]
            int CreateViewWindow([In, MarshalAs(UnmanagedType.Interface)] IShellView psvPrevious, [In] ref FOLDERSETTINGS pfs, [In, MarshalAs(UnmanagedType.Interface)] IShellBrowser psb, [In] ref RECT prcVie, [In, Out] ref IntPtr phWnd);
            void DestroyViewWindow();
            void GetCurrentInfo(ref FOLDERSETTINGS lpfs);
            void AddPropertySheetPages([In, MarshalAs(UnmanagedType.U4)] uint dwReserved, [In] ref IntPtr lpfn, [In] IntPtr lparam);
            void SaveViewState();
            void SelectItem(IntPtr pidlItem, [MarshalAs(UnmanagedType.U4)] uint uFlags);

            [PreserveSig]
            int GetItemObject(uint uItem, ref Guid riid, out IntPtr ppv);
        }

        [ComImport, Guid("000214e6-0000-0000-c000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IShellFolder
        {
            [PreserveSig]
            int ParseDisplayName(IntPtr hwnd, IntPtr pbc, [MarshalAs(UnmanagedType.LPWStr)] string pwszDisplayName, IntPtr pchEaten, out IntPtr ppidl, SHGFAO pdwAttributes);
            [PreserveSig]
            int EnumObjects(IntPtr hwndOwner, SHCONTF grfFlags, [MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenumIDList);
            [PreserveSig]
            int BindToObject(IntPtr pidl, IntPtr pbc, ref Guid riid, out IntPtr ppv);
            void BindToStorage(IntPtr pidl, IntPtr pbc, ref Guid riid, out IntPtr ppv);
            [PreserveSig]
            int CompareIDs(int lParam, IntPtr pidl1, IntPtr pidl2);
            [PreserveSig]
            int CreateViewObject(IntPtr hwndOwner, ref Guid riid, out IntPtr ppv);
            void GetAttributesOf(uint cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] IntPtr[] apidl, ref SHGFAO rgfInOut);
            [PreserveSig]
            int GetUIObjectOf(IntPtr hwndOwner, uint cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IntPtr[] apidl, ref Guid riid, IntPtr rgfReserved, out IntPtr ppv);
            [PreserveSig]
            int GetDisplayNameOf(IntPtr pidl, SHGNO uFlags, IntPtr lpName);
            [PreserveSig]
            int SetNameOf(IntPtr hwndOwner, IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] string lpszName, SHGNO uFlags, ref IntPtr ppidlOut);
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214ee-0000-0000-c000-000000000046")]
        public interface IShellLink
        {
            void GetPath([Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder pszFile, int cchMaxPath, out WIN32_FIND_DATA pfd, SLGP_FLAGS fFlags);
            void GetIDList(out IntPtr ppidl);
            void SetIDList(IntPtr pidl);
            void GetDescription([Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder pszName, int cchMaxName);
            void SetDescription([MarshalAs(UnmanagedType.LPStr)] string pszName);
            void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder pszDir, int cchMaxPath);
            void SetWorkingDirectory([MarshalAs(UnmanagedType.LPStr)] string pszDir);
            void GetArguments([Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder pszArgs, int cchMaxPath);
            void SetArguments([MarshalAs(UnmanagedType.LPStr)] string pszArgs);
            void GetHotkey(out short pwHotkey);
            void SetHotkey(short wHotkey);
            void GetShowCmd(out int piShowCmd);
            void SetShowCmd(int iShowCmd);
            void GetIconLocation([Out, MarshalAs(UnmanagedType.LPStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
            void SetIconLocation([MarshalAs(UnmanagedType.LPStr)] string pszIconPath, int iIcon);
            void SetRelativePath([MarshalAs(UnmanagedType.LPStr)] string pszPathRel, int dwReserved);
            void Resolve(IntPtr hwnd, SLR_FLAGS fFlags);
            void SetPath([MarshalAs(UnmanagedType.LPStr)] string pszFile);
        }

        [ComImport]
        [ClassInterface(ClassInterfaceType.None)]
        [Guid("00021401-0000-0000-c000-000000000046")]
        public class ShellLink  // : IPersistFile, IShellLinkA, IShellLinkW 
        {
        }

        [ComImport, Guid("6d5140c1-7436-11ce-8034-00aa006009fa")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IServiceProvider
        {
            [PreserveSig]
            int QueryService(ref Guid guidService, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IShellBrowser ppvObject);
        }

        [ComImport, Guid("000214ea-0000-0000-c000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IPersistFolder
        {
            void GetClassID(out Guid pClassID);
            void Initialize(IntPtr pidl);
        }

        [ComImport, Guid("cde725b0-ccc9-4519-917e-325d72fab4ce")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IFolderView
        {
            void GetCurrentViewMode(out uint pViewMode);
            void SetCurrentViewMode(uint ViewMode);
            void GetFolder(ref Guid riid, ref IntPtr ppv);
            void Item(int iItemIndex, out IntPtr ppidl);
            void ItemCount(uint uFlags, out int pcItems);
            void Items(uint uFlags, ref Guid riid, ref IntPtr ppv);
            void GetSelectionMarkedItem(out int piItem);
            void GetFocusedItem(out int piItem);
            void GetItemPosition(IntPtr pidl, out POINT ppt);
            void GetSpacing(out POINT ppt);
            void GetDefaultSpacing(out POINT ppt);
            [PreserveSig]
            int GetAutoArrange();
            void SelectItem(int iItem, uint dwFlags);
            void SelectAndPositionItems(uint cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] IntPtr[] apidl, ref POINT apt, uint dwFlags);
        }

        #endregion

        #region Structures

        /// <summary>
        /// Contains extended information about a shortcut menu command.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct CMINVOKECOMMANDINFOEX
        {
            public int cbSize;              // must be sizeof(CMINVOKECOMMANDINFOEX)
            public int fMask;               // any combination of CMIC_MASK_*
            public IntPtr hwnd;             // might be NULL (indicating no owner window)
            public IntPtr lpVerb;           // either a string or MAKEINTRESOURCE(idOffset)
            public IntPtr lpParameters;     // might be NULL (indicating no parameter)
            public IntPtr lpDirectory;      // might be NULL (indicating no specific directory)
            public int nShow;               // one of SW_ values for ShowWindow() API
            public int dwHotKey;
            public IntPtr hIcon;
            public IntPtr lpTitle;          // For CreateProcess-StartupInfo.lpTitle
            public IntPtr lpVerbW;          // Unicode verb (for those who can use it)
            public IntPtr lpParametersW;    // Unicode parameters (for those who can use it)
            public IntPtr lpDirectoryW;     // Unicode directory (for those who can use it)
            public IntPtr lpTitleW;         // Unicode title (for those who can use it)
            public POINT ptInvoke;          // Point where it's invoked
        }

        /// <summary>
        /// Contains combo box status information.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct COMBOBOXINFO
        {
            public int cbSize;
            public RECT rcItem;
            public RECT rcButton;
            public IntPtr stateButton;
            public IntPtr hwndCombo;
            public IntPtr hwndEdit;
            public IntPtr hwndList;
        }

        /// <summary>
        /// The POINT structure defines the x- and y- coordinates of a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;

            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        /// <summary>
        /// The RECT structure defines the coordinates of the upper-left
        /// and lower-right corners of a rectangle.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public RECT(Rectangle r)
            {
                this.left = r.Left;
                this.top = r.Top;
                this.right = r.Right;
                this.bottom = r.Bottom;
            }

            public static RECT FromXYWH(int x, int y, int width, int height)
            {
                return new RECT(x, y, x + width, y + height);
            }

            public Rectangle ToRectangle()
            {
                return new Rectangle(this.left, this.top, this.right - this.left, this.bottom - this.top);
            }
        }

        /// <summary>
        /// Contains information about a file object.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public int dwAttributes;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string szDisplayName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct WIN32_FIND_DATA
        {
            public int dwFileAttributes;
            public int ftCreationTime;
            public int ftLastAccessTime;
            public int ftLastWriteTime;
            public int nFileSizeHigh;
            public int nFileSizeLow;
            public int dwReserved0;
            public int dwReserved1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string cFileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName;
        }

        #endregion

        #region Enumerators

        [Flags]
        public enum AUTOCOMPLETEOPTIONS : uint
        {
            ACO_NONE           = 0x00,      // No AutoComplete
            ACO_AUTOSUGGEST    = 0x01,      // enable autosuggest dropdown
            ACO_AUTOAPPEND     = 0x02,      // enable autoappend
            ACO_SEARCH         = 0x04,      // add search entry to completion list
            ACO_FILTERPREFIXES = 0x08,      // don't match common prefixes (www., http://, etc)
            ACO_USETAB         = 0x10,      // use tab to select autosuggest entries
            ACO_UPDOWNKEYDROPSLIST = 0x20,  // up/down arrow key invokes autosuggest dropdown (if enabled)
            ACO_RTLREADING     = 0x40,      // enable RTL reading order for dropdown
        }

        [Flags]
        public enum AUTOCOMPLETELISTOPTIONS : uint
        {
            ACLO_NONE            = 0,    // don't enumerate anything
            ACLO_CURRENTDIR      = 1,    // enumerate current directory
            ACLO_MYCOMPUTER      = 2,    // enumerate MyComputer
            ACLO_DESKTOP         = 4,    // enumerate Desktop Folder
            ACLO_FAVORITES       = 8,    // enumerate Favorites Folder
            ACLO_FILESYSONLY     = 16,   // enumerate only the file system
            ACLO_FILESYSDIRS     = 32,   // enumerate only the file system dirs, UNC shares, and UNC servers.
        }

        //--------------------------------------------------------------------------
        //
        // FOLDERSETTINGS
        //
        //  FOLDERSETTINGS is a data structure that explorer passes from one folder
        // view to another, when the user is browsing. It calls ISV::GetCurrentInfo
        // member to get the current settings and pass it to ISV::CreateViewWindow
        // to allow the next folder view 'inherit' it. These settings assumes a
        // particular UI (which the shell's folder view has), and shell extensions
        // may or may not use those settings.
        //
        //--------------------------------------------------------------------------

        // FWF_DESKTOP implies FWF_TRANSPARENT/NOCLIENTEDGE/NOSCROLL

        [Flags]
        public enum FOLDERFLAGS
        {
            FWF_AUTOARRANGE =       0x00000001,
            FWF_ABBREVIATEDNAMES =  0x00000002,
            FWF_SNAPTOGRID =        0x00000004,
            FWF_OWNERDATA =         0x00000008,
            FWF_BESTFITWINDOW =     0x00000010,
            FWF_DESKTOP =           0x00000020,
            FWF_SINGLESEL =         0x00000040,
            FWF_NOSUBFOLDERS =      0x00000080,
            FWF_TRANSPARENT  =      0x00000100,
            FWF_NOCLIENTEDGE =      0x00000200,
            FWF_NOSCROLL     =      0x00000400,
            FWF_ALIGNLEFT    =      0x00000800,
            FWF_NOICONS      =      0x00001000,
            FWF_SHOWSELALWAYS =     0x00002000,
            FWF_NOVISIBLE    =      0X00004000,
            FWF_SINGLECLICKACTIVATE=0x00008000,     // TEMPORARY -- NO UI FOR THIS
            FWF_NOWEBVIEW =         0x00010000,
            FWF_HIDEFILENAMES =     0x00020000,
            FWF_CHECKSELECT =       0x00040000,
        }

        [Flags]
        public enum FOLDERVIEWMODE
        {
            FVM_FIRST =             1,
            FVM_ICON =              1,
            FVM_SMALLICON =         2,
            FVM_LIST =              3,
            FVM_DETAILS =           4,
            FVM_THUMBNAIL =         5,
            FVM_TILE =              6,
            FVM_THUMBSTRIP =        7,
            FVM_LAST =              7,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FOLDERSETTINGS
        {
            public uint ViewMode;       // View mode (FOLDERVIEWMODE values)
            public uint fFlags;         // View options (FOLDERFLAGS bits)
        }

        // IShellFolder::EnumObjects grfFlags bits
        [Flags]
        public enum SHCONTF
        {
            SHCONTF_FOLDERS             = 0x0020,   // only want folders enumerated (SFGAO_FOLDER)
            SHCONTF_NONFOLDERS          = 0x0040,   // include non folders
            SHCONTF_INCLUDEHIDDEN       = 0x0080,   // show items normally hidden
            SHCONTF_INIT_ON_FIRST_NEXT  = 0x0100,   // allow EnumObject() to return before validating enum
            SHCONTF_NETPRINTERSRCH      = 0x0200,   // hint that client is looking for printers
            SHCONTF_SHAREABLE           = 0x0400,   // hint that client is looking sharable resources (remote shares)
            SHCONTF_STORAGE             = 0x0800,   // include all items with accessible storage and their ancestors
        }

        // IShellFolder::GetAttributesOf flags
        // SFGAO_CANLINK: If this bit is set on an item in the shell folder, a
        //            'Create Shortcut' menu item will be added to the File
        //            menu and context menus for the item.  If the user selects
        //            that command, your IContextMenu::InvokeCommand() will be called
        //            with 'link'.
        //                 That flag will also be used to determine if 'Create Shortcut'
        //            should be added when the item in your folder is dragged to another
        //            folder.
        [Flags]
        public enum SHGFAO : uint
        {
            SFGAO_CANCOPY           = 0x00000001, // Objects can be copied
            SFGAO_CANMOVE           = 0x00000002, // Objects can be moved
            SFGAO_CANLINK           = 0x00000004, // Objects can be linked
            SFGAO_STORAGE           = 0x00000008, // supports BindToObject(IID_IStorage)
            SFGAO_CANRENAME         = 0x00000010, // Objects can be renamed
            SFGAO_CANDELETE         = 0x00000020, // Objects can be deleted
            SFGAO_HASPROPSHEET      = 0x00000040, // Objects have property sheets
            SFGAO_DROPTARGET        = 0x00000100, // Objects are drop target
            SFGAO_CAPABILITYMASK    = 0x00000177,
            SFGAO_ENCRYPTED         = 0x00002000, // object is encrypted (use alt color)
            SFGAO_ISSLOW            = 0x00004000, // 'slow' object
            SFGAO_GHOSTED           = 0x00008000, // ghosted icon
            SFGAO_LINK              = 0x00010000, // Shortcut (link)
            SFGAO_SHARE             = 0x00020000, // shared
            SFGAO_READONLY          = 0x00040000, // read-only
            SFGAO_HIDDEN            = 0x00080000, // hidden object
            SFGAO_DISPLAYATTRMASK   = 0x000FC000,
            SFGAO_FILESYSANCESTOR   = 0x10000000, // may contain children with SFGAO_FILESYSTEM
            SFGAO_FOLDER            = 0x20000000, // support BindToObject(IID_IShellFolder)
            SFGAO_FILESYSTEM        = 0x40000000, // is a win32 file system object (file/folder/root)
            SFGAO_HASSUBFOLDER      = 0x80000000, // may contain children with SFGAO_FOLDER
            SFGAO_CONTENTSMASK      = 0x80000000,
            SFGAO_VALIDATE          = 0x01000000, // invalidate cached information
            SFGAO_REMOVABLE         = 0x02000000, // is this removeable media?
            SFGAO_COMPRESSED        = 0x04000000, // Object is compressed (use alt color)
            SFGAO_BROWSABLE         = 0x08000000, // supports IShellFolder, but only implements CreateViewObject() (non-folder view)
            SFGAO_NONENUMERATED     = 0x00100000, // is a non-enumerated object
            SFGAO_NEWCONTENT        = 0x00200000, // should show bold in explorer tree
            SFGAO_CANMONIKER        = 0x00400000, // defunct
            SFGAO_HASSTORAGE        = 0x00400000, // defunct
            SFGAO_STREAM            = 0x00400000, // supports BindToObject(IID_IStream)
            SFGAO_STORAGEANCESTOR   = 0x00800000, // may contain children with SFGAO_STORAGE or SFGAO_STREAM
            SFGAO_STORAGECAPMASK    = 0x70C50008, // for determining storage capabilities, ie for open/save semantics
        }

        // IShellFolder::GetDisplayNameOf/SetNameOf uFlags 
        [Flags]
        public enum SHGNO
        {
            SHGDN_NORMAL             = 0x0000,		// default (display purpose)
            SHGDN_INFOLDER           = 0x0001,		// displayed under a folder (relative)
            SHGDN_FOREDITING         = 0x1000,		// for in-place editing
            SHGDN_FORADDRESSBAR      = 0x4000,		// UI friendly parsing name (remove ugly stuff)
            SHGDN_FORPARSING         = 0x8000,		// parsing name for ParseDisplayName()
        }

        // IShellLink::Resolve fFlags
        [Flags]
        public enum SLR_FLAGS
        {
            SLR_NO_UI               = 0x0001,   // don't post any UI durring the resolve operation, not msgs are pumped
            SLR_ANY_MATCH           = 0x0002,   // no longer used
            SLR_UPDATE              = 0x0004,   // save the link back to it's file if the track made it dirty
            SLR_NOUPDATE            = 0x0008,
            SLR_NOSEARCH            = 0x0010,   // don't execute the search heuristics
            SLR_NOTRACK             = 0x0020,   // don't use NT5 object ID to track the link
            SLR_NOLINKINFO          = 0x0040,   // don't use the net and volume relative info
            SLR_INVOKE_MSI          = 0x0080,   // if we have a darwin link, then call msi to fault in the applicaion
            SLR_NO_UI_WITH_MSG_PUMP = 0x0101,   // SLR_NO_UI + requires an enable modeless site or HWND
        }

        // IShellLink::GetPath fFlags
        [Flags]
        public enum SLGP_FLAGS
        {
            SLGP_SHORTPATH      = 0x0001,
            SLGP_UNCPRIORITY    = 0x0002,
            SLGP_RAWPATH        = 0x0004,
        }

        //
        // uState values for IShellView::UIActivate
        //
        [Flags]
        public enum SVUIA_STATUS
        {
            SVUIA_DEACTIVATE       = 0,
            SVUIA_ACTIVATE_NOFOCUS = 1,
            SVUIA_ACTIVATE_FOCUS   = 2,
            SVUIA_INPLACEACTIVATE  = 3          // new flag for IShellView2
        }

        //
        // shellview get item object flags
        //
        [Flags]
        public enum SVGIO : uint
        {
            SVGIO_BACKGROUND        = 0x00000000,
            SVGIO_SELECTION         = 0x00000001,
            SVGIO_ALLVIEW           = 0x00000002,
            SVGIO_CHECKED           = 0x00000003,
            SVGIO_TYPE_MASK         = 0x0000000F,
            SVGIO_FLAG_VIEWORDER    = 0x80000000,
        }

        #endregion
    }
}
