using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpShell.Interop;
using SharpShell.Pidl;
using SharpShell.SharpContextMenu;

namespace GitHubNamespaceExtension
{
    public class GithubContextMenu : SharpContextMenu
    {
        private IdList _folderIdList;
        private IdList[] _folderItemIdLists;

        public GithubContextMenu(IdList folderIdList, IdList[] folderItemIdLists)
        {
            _folderItemIdLists = folderItemIdLists;
            _folderIdList = folderIdList;
        }

        protected override bool CanShowMenu()
        {
            return true;
        }

        protected override ContextMenuStrip CreateMenu()
        {
            var menu = new ContextMenuStrip();
            menu.Items.Add("Open", null, OnOpen);
            return menu;
        }

        private void OnOpen(object sender, EventArgs eventArgs)
        {
            SHELLEXECUTEINFO sei = new SHELLEXECUTEINFO();
            sei.cbSize = Marshal.SizeOf(sei);
            sei.fMask = SEE.SEE_MASK_IDLIST | SEE.SEE_MASK_CLASSNAME;
            var fullPidl = PidlManager.IdListToPidl(PidlManager.Combine(_folderIdList, _folderItemIdLists[0]));
            sei.lpIDList = fullPidl;
            sei.lpClass = "folder";
            sei.hwnd = CurrentInvokeCommandInfo.WindowHandle;
            sei.nShow = CurrentInvokeCommandInfo.ShowCommand;
            sei.lpVerb = "open"; // todo parameter open.
            Shell32.ShellExecuteEx(ref sei);
            PidlManager.DeletePidl(fullPidl);
        }

    }

    /*
        SHELLEXECUTEINFO sei = { 0 };
 sei.cbSize = sizeof(sei);
 sei.fMask = SEE_MASK_IDLIST | SEE_MASK_CLASSNAME;
 sei.lpIDList = pidl; // the fully qualified PIDL
 sei.lpClass = TEXT("folder");
 sei.hwnd = pcmi->hwnd;
 sei.nShow = pcmi->nShow;
 sei.lpVerb = cmd.verb.w_str();
 BOOL bRes = ::ShellExecuteEx(&sei);
 DeletePidl(pidl);
 return bRes?S_OK:HR(HRESULT_FROM_WIN32(GetLastError()));
    }*/
}