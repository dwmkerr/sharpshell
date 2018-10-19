using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpShell.Interop
{
    /// <summary>
    /// Exposes methods that either create or merge a shortcut (context) menu associated with a Shell object. Extends IContextMenu by adding a method that allows client objects to handle messages associated with owner-drawn menu items.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214f4-0000-0000-c000-000000000046")]
    public interface IContextMenu2 : IContextMenu
    {
        #region IContextMenu overrides

        /// <summary>
        /// Adds commands to a shortcut menu.
        /// </summary>
        /// <param name="hMenu">A handle to the shortcut menu. The handler should specify this handle when adding menu items.</param>
        /// <param name="indexMenu">The zero-based position at which to insert the first new menu item.</param>
        /// <param name="idCmdFirst">The minimum value that the handler can specify for a menu item identifier.</param>
        /// <param name="idCmdLast">The maximum value that the handler can specify for a menu item identifier.</param>
        /// <param name="uFlags">Optional flags that specify how the shortcut menu can be changed. This parameter can be set to a combination of the following values. The remaining bits of the low-order word are reserved by the system. The high-order word can be used for context-specific communications. The CMF_RESERVED value can be used to mask the low-order word.</param>
        /// <returns>
        /// If successful, returns an HRESULT value that has its severity value set to SEVERITY_SUCCESS and its code value set to the offset of the largest command identifier that was assigned, plus one. For example, if idCmdFirst is set to 5 and you add three items to the menu with command identifiers of 5, 7, and 8, the return value should be MAKE_HRESULT(SEVERITY_SUCCESS, 0, 8 - 5 + 1). Otherwise, it returns a COM error value.
        /// </returns>
        new int QueryContextMenu(IntPtr hMenu, uint indexMenu, int idCmdFirst, int idCmdLast, CMF uFlags);

        /// <summary>
        /// Carries out the command associated with a shortcut menu item.
        /// </summary>
        /// <param name="pici">A pointer to a CMINVOKECOMMANDINFO or CMINVOKECOMMANDINFOEX structure containing information about the command. For further details, see the Remarks section.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        new int InvokeCommand(IntPtr pici);

        /// <summary>
        /// Gets information about a shortcut menu command, including the help string and the language-independent, or canonical, name for the command.
        /// </summary>
        /// <param name="idcmd">Menu command identifier offset.</param>
        /// <param name="uflags">Flags specifying the information to return. This parameter can have one of the following values.</param>
        /// <param name="reserved">Reserved. Applications must specify NULL when calling this method and handlers must ignore this parameter when called.</param>
        /// <param name="commandstring">The address of the buffer to receive the null-terminated string being retrieved.</param>
        /// <param name="cch">Size of the buffer, in characters, to receive the null-terminated string.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        new int GetCommandString(int idcmd, GCS uflags, int reserved, StringBuilder commandstring, int cch);

        #endregion

        /// <summary>
        /// Enables client objects of the IContextMenu interface to handle messages associated with owner-drawn menu items.
        /// </summary>
        /// <param name="uMsg">The message to be processed. In the case of some messages, such as WM_INITMENUPOPUP, WM_DRAWITEM, WM_MENUCHAR, or WM_MEASUREITEM, the client object being called may provide owner-drawn menu items.</param>
        /// <param name="wParam">Additional message information. The value of this parameter depends on the value of the uMsg parameter.</param>
        /// <param name="lParam">Additional message information. The value of this parameter depends on the value of the uMsg parameter.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int HandleMenuMsg(uint uMsg, IntPtr wParam, IntPtr lParam);
    }
}