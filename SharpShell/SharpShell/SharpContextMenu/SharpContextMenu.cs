using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using SharpShell.Attributes;
using SharpShell.Helpers;
using SharpShell.Interop;

namespace SharpShell.SharpContextMenu
{
    /// <summary>
    /// SharpContextMenu is the base class for Shell Context Menu Extensions supported
    /// by SharpShell. By providing the implementation of 'CanShowMenu' and 'CreateMenu',
    /// derived classes can provide all of the functionality required to create a fully
    /// functional shell context menu.
    /// </summary>
    [ServerType(ServerType.ShellContextMenu)]
    public abstract class SharpContextMenu : ShellExtInitServer, IContextMenu3
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpContextMenu"/> class.
        /// </summary>
        protected SharpContextMenu()
        {
            //  The abstract CreateMenu function will provide the value for the lazy.
            contextMenuStrip = new Lazy<ContextMenuStrip>(CreateMenu);
        }

        #region Implementation of IContextMenu

        /// <summary>
        /// Called to query the context menu.
        /// </summary>
        /// <param name="hMenu">The handle to the parent menu.</param>
        /// <param name="indexMenu">The index of the menu.</param>
        /// <param name="idCmdFirst">The first command ID.</param>
        /// <param name="idCmdLast">The last command ID.</param>
        /// <param name="uFlags">The flags.</param>
        /// <returns>An HRESULT indicating success.</returns>
        int IContextMenu.QueryContextMenu(IntPtr hMenu, uint indexMenu, int idCmdFirst, int idCmdLast, CMF uFlags)
        {
            //  Log this key event.
            Log(string.Format("Query Context Menu for items: {0}{1}", Environment.NewLine, string.Join(Environment.NewLine, SelectedItemPaths)));
            
            //  If we've got the defaultonly flag, we're done.
            if (uFlags.HasFlag(CMF.CMF_DEFAULTONLY))
                return WinError.MAKE_HRESULT(WinError.SEVERITY_SUCCESS, 0, 0);

            //  Before we build the context menu, determine whether we need to show it.
            try
            {
                //  If we can't show the menu, return false.
                if (!CanShowMenu())
                    return WinError.S_FALSE;
            }
            catch (Exception exception)
            {
                //  We can't show the menu.
                LogError("An exception ocurred determining whether the context menu should be shown.", exception);

                //  Return the failure.
                return WinError.E_FAIL;
            }

            //  Set the first item id.
            var firstItemId = (uint)idCmdFirst;
            
            //  Use the native context menu wrapper to build the context menu.
            uint lastItemId = 0;
            try
            {
                nativeContextMenuWrapper.ResetNativeContextMenu();
                lastItemId = nativeContextMenuWrapper.BuildNativeContextMenu(hMenu, firstItemId, contextMenuStrip.Value.Items);
            }
            catch (Exception exception)
            {
                //  DebugLog the exception.
                LogError("An exception occured building the context menu.", exception);

                //  Return the failure.
                return WinError.E_FAIL;
            }
            
            //  Return success, passing the the last item ID plus one (which will be the next command id).
            //  MSDN documentation is flakey here - to be explicit we need to return the count of the items added plus one.
            return WinError.MAKE_HRESULT(WinError.SEVERITY_SUCCESS, 0, (lastItemId - firstItemId) + 1);
        }
        int IContextMenu2.QueryContextMenu(IntPtr hMenu, uint indexMenu, int idCmdFirst, int idCmdLast, CMF uFlags)
        {
            return ((IContextMenu) this).QueryContextMenu(hMenu, indexMenu, idCmdFirst, idCmdLast, uFlags);
        }
        int IContextMenu3.QueryContextMenu(IntPtr hMenu, uint indexMenu, int idCmdFirst, int idCmdLast, CMF uFlags)
        {
            return ((IContextMenu)this).QueryContextMenu(hMenu, indexMenu, idCmdFirst, idCmdLast, uFlags);
        }

        /// <summary>
        /// Called to invoke the comamand.
        /// </summary>
        /// <param name="pici">The command info pointer.</param>
        int IContextMenu.InvokeCommand(IntPtr pici)
        {
            //  We'll work out whether the commandis unicode or not...
            var isUnicode = false;

            //  We could have been provided with a CMINVOKECOMMANDINFO or a 
            //  CMINVOKECOMMANDINFOEX - cast to the small and then check the size.
            var ici = (CMINVOKECOMMANDINFO)Marshal.PtrToStructure(pici, typeof(CMINVOKECOMMANDINFO));
            var iciex = new CMINVOKECOMMANDINFOEX();

            //  Is it a CMINVOKECOMMANDINFOEX?
            if (ici.cbSize == Marshal.SizeOf(typeof(CMINVOKECOMMANDINFOEX)))
            {
                //  Check the unicode flag, get the extended command info.
                if ((ici.fMask & CMIC.CMIC_MASK_UNICODE) != 0)
                {
                    isUnicode = true;
                    iciex = (CMINVOKECOMMANDINFOEX)Marshal.PtrToStructure(pici,
                        typeof(CMINVOKECOMMANDINFOEX));
                }
            }

            //  The derived class MAY need some of the extended command data,
            //  so we store it now. It can be retrieved and used in the handler
            //  of the menu item.
            SaveInvokeCommandInfo(isUnicode, ici, iciex);

            //  If we're not unicode and the verb hiword is not zero,
            //  we've got an ANSI verb string.
            if (!isUnicode && User32.HighWord(ici.verb.ToInt32()) != 0)
            {
                //  Get the verb.
                var verb = Marshal.PtrToStringAnsi(ici.verb);

                //  DebugLog this key event.
                Log(string.Format("Invoke ANSI verb {0}", verb));
                
                //  Try and invoke the command. If we don't invoke it, throw
                //  E_FAIL so that other handlers can try.
                if (!nativeContextMenuWrapper.TryInvokeCommand(verb))
                    Marshal.ThrowExceptionForHR(WinError.E_FAIL);
            }
            //  If we're unicode, and the verb hiword is not zero,
            //  we've got a unicode command string.
            else if (isUnicode && User32.HighWord(iciex.verbW.ToInt32()) != 0)
            {
                //  Get the verb.
                var verb = Marshal.PtrToStringAnsi(ici.verb);

                //  DebugLog this key event.
                Log(string.Format("Invoke Unicode verb {0}", verb));
                
                //  Try and invoke the command. If we don't invoke it, throw
                //  E_FAIL so that other handlers can try.
                if (!nativeContextMenuWrapper.TryInvokeCommand(verb))
                    Marshal.ThrowExceptionForHR(WinError.E_FAIL);
            }
            //  The verb pointer isn't a string at all, it's an index.
            else
            {
                //  Get the command index. Logically, we don't actually need to
                //  loword it, as the hiword is zero, but we're following the
                //  documentation rigourously.
                var index = User32.LowWord(ici.verb.ToInt32());
                
                //  DebugLog this key event.
                Log(string.Format("Invoke command index {0}", index));
                
                //  Try and invoke the command. If we don't invoke it, throw
                //  E_FAIL so that other handlers can try.
                if (!nativeContextMenuWrapper.TryInvokeCommand(index))
                    Marshal.ThrowExceptionForHR(WinError.E_FAIL);
            }

            //  Return success.
            return WinError.S_OK;
        }
        int IContextMenu2.InvokeCommand(IntPtr pici)
        {
            return ((IContextMenu)this).InvokeCommand(pici);
        }
        int IContextMenu3.InvokeCommand(IntPtr pici)
        {
            return ((IContextMenu)this).InvokeCommand(pici);
        }

        /// <summary>
        /// Saves the invoke command information.
        /// </summary>
        /// <param name="isUnicode">if set to <c>true</c> the unicode structure is used.</param>
        /// <param name="ici">The ici.</param>
        /// <param name="iciex">The iciex.</param>
        private void SaveInvokeCommandInfo(bool isUnicode, CMINVOKECOMMANDINFO ici, CMINVOKECOMMANDINFOEX iciex)
        {
            if (isUnicode)
            {
                //  Create command info from the Unicode structure.
                currentInvokeCommandInfo = new InvokeCommandInfo
                {
                    WindowHandle = iciex.hwnd,
                    ShowCommand = iciex.nShow
                };
            }
            else
            {
                //  Create command info from the ANSI structure.
                currentInvokeCommandInfo = new InvokeCommandInfo
                {
                    WindowHandle = ici.hwnd,
                    ShowCommand = ici.nShow
                };
            }
        }

        /// <summary>
        /// Gets the command string.
        /// </summary>
        /// <param name="idcmd">The idcmd.</param>
        /// <param name="uflags">The uflags.</param>
        /// <param name="reserved">The reserved.</param>
        /// <param name="commandstring">The commandstring.</param>
        /// <param name="cch">The CCH.</param>
        int IContextMenu.GetCommandString(int idcmd, GCS uflags, int reserved, StringBuilder commandstring, int cch)
        {
            //  Get the item.
            if (idcmd >= contextMenuStrip.Value.Items.Count)
                return WinError.E_FAIL;
            var item = contextMenuStrip.Value.Items[idcmd];
            
            //  Based on the flags, choose a string to set.
            var stringData = string.Empty;
            switch (uflags)
            {
                case GCS.GCS_VERBW:
                    //  We need to provide a verb. Use the item name, as the Context Menu Builder will 
                    //  make sure it is unique.
                    stringData = item.Name;
                    break;

                case GCS.GCS_HELPTEXTW:
                    //  We need to provide the tooltip text.
                    stringData = item.ToolTipText ?? string.Empty;
                    break;
            }

            //  If we have not been given sufficient space for the string, throw an insufficient buffer exception.
            if(stringData.Length > cch - 1)
            {
                Marshal.ThrowExceptionForHR(WinError.STRSAFE_E_INSUFFICIENT_BUFFER);
                return WinError.E_FAIL;
            }

            //  Append the string data.
            commandstring.Clear();
            commandstring.Append(stringData);

            //  Return success.
            return WinError.S_OK;
        }
        int IContextMenu2.GetCommandString(int idcmd, GCS uflags, int reserved, StringBuilder commandstring, int cch)
        {
            return ((IContextMenu)this).GetCommandString(idcmd, uflags, reserved, commandstring, cch);
        }
        int IContextMenu3.GetCommandString(int idcmd, GCS uflags, int reserved, StringBuilder commandstring, int cch)
        {
            return ((IContextMenu)this).GetCommandString(idcmd, uflags, reserved, commandstring, cch);
        }

        #endregion
        
        #region Implementation of IContextMenu2

        /// <summary>
        /// Enables client objects of the IContextMenu interface to handle messages associated with owner-drawn menu items.
        /// </summary>
        /// <param name="uMsg">The message to be processed. In the case of some messages, such as WM_INITMENUPOPUP, WM_DRAWITEM, WM_MENUCHAR, or WM_MEASUREITEM, the client object being called may provide owner-drawn menu items.</param>
        /// <param name="wParam">Additional message information. The value of this parameter depends on the value of the uMsg parameter.</param>
        /// <param name="lParam">Additional message information. The value of this parameter depends on the value of the uMsg parameter.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        int IContextMenu2.HandleMenuMsg(uint uMsg, IntPtr wParam, IntPtr lParam)
        {
            //  Always delegate to the IContextMenu3 version.
            return ((IContextMenu3) this).HandleMenuMsg2(uMsg, wParam, lParam, IntPtr.Zero);
        }
        int IContextMenu3.HandleMenuMsg(uint uMsg, IntPtr wParam, IntPtr lParam)
        {
            //  Always delegate to the IContextMenu3 version.
            return ((IContextMenu3)this).HandleMenuMsg2(uMsg, wParam, lParam, IntPtr.Zero);
        }

        #endregion
        
        #region Implementation of IContextMenu3

        /// <summary>
        /// Allows client objects of the IContextMenu3 interface to handle messages associated with owner-drawn menu items.
        /// </summary>
        /// <param name="uMsg">The message to be processed. In the case of some messages, such as WM_INITMENUPOPUP, WM_DRAWITEM, WM_MENUCHAR, or WM_MEASUREITEM, the client object being called may provide owner-drawn menu items.</param>
        /// <param name="wParam">Additional message information. The value of this parameter depends on the value of the uMsg parameter.</param>
        /// <param name="lParam">Additional message information. The value of this parameter depends on the value of the uMsg parameter.</param>
        /// <param name="plResult">The address of an LRESULT value that the owner of the menu will return from the message. This parameter can be NULL.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        int IContextMenu3.HandleMenuMsg2(uint uMsg, IntPtr wParam, IntPtr lParam, ref IntPtr plResult)
        {
            if (uMsg == (uint)WM.INITMENUPOPUP)
            {
                //  TODO IMPORTANT: What we have here is not quite right, this is only called when a popup item 
                //  is being opened, not when we initialise the whole menu.

                var menuHandle = wParam;
                var parentIndex = Win32Helper.LoWord(lParam);
                var isWindowMenu = Win32Helper.HiWord(lParam) != 0;


                //  Call the virtual function allowing derived classes to customise the menu.
                OnInitialiseMenu(parentIndex);
            }

            //  Return success.
            return WinError.S_OK;
        }

        #endregion

        private Dictionary<uint, SIZE[]> idsToPopupSizes = new Dictionary<uint, SIZE[]>();
        
        /// <summary>
        /// Gets the current invoke command information. This will only be set when a command
        /// is invoked, and will be replaced when the next command is invoked.
        /// </summary>
        protected InvokeCommandInfo CurrentInvokeCommandInfo { get { return currentInvokeCommandInfo; }}

        /// <summary>
        /// Determines whether this instance can a shell context show menu, given the specified selected file list.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance should show a shell context menu for the specified file list; otherwise, <c>false</c>.
        /// </returns>
        protected abstract bool CanShowMenu();

        /// <summary>
        /// Creates the context menu. This can be a single menu item or a tree of them.
        /// </summary>
        /// <returns>The context menu for the shell context menu.</returns>
        protected abstract ContextMenuStrip CreateMenu();

        /// <summary>
        /// Called when the context menu is about to be displayed.
        /// This function can be used to customise the context menu based
        /// on the items selected.
        /// </summary>
        /// <param name="parentItemIndex">The index of the parent menu item.</param>
        protected virtual void OnInitialiseMenu(int parentItemIndex)
        {
        }

        /// <summary>
        /// The lazy context menu strip, only created when we actually need it.
        /// </summary>
        private readonly Lazy<ContextMenuStrip> contextMenuStrip;

        /// <summary>
        /// The native context menu wrapper.
        /// </summary>
        private readonly NativeContextMenuWrapper nativeContextMenuWrapper = new NativeContextMenuWrapper();

        /// <summary>
        /// The current invoke command information.
        /// </summary>
        private InvokeCommandInfo currentInvokeCommandInfo;
    }
}
