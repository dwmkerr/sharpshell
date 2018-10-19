using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Contains context menu information used by SHCreateDefaultContextMenu.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DEFCONTEXTMENU
    {
        /// <summary>
        /// A handle to the context menu. Set this member to the handle returned from CreateMenu.
        /// </summary>
        public IntPtr hwnd;

        /// <summary>
        /// A pointer to the IContextMenuCB interface supported by the callback object. This value is optional and can be NULL.
        /// </summary>
        public IContextMenuCB pcmcb;

        /// <summary>
        /// The PIDL of the folder that contains the selected file object(s) or the folder of the context menu if no file objects are selected. This value is optional and can be NULL, in which case the PIDL is computed from the psf member.
        /// </summary>
        public IntPtr pidlFolder;

        /// <summary>
        /// A pointer to the IShellFolder interface of the folder object that contains the selected file objects, or the folder that contains the context menu if no file objects are selected.
        /// </summary>
        public IShellFolder psf;

        /// <summary>
        /// The count of items in member apidl.
        /// </summary>
        public uint cidl;

        /// <summary>
        /// A pointer to a constant array of ITEMIDLIST structures. Each entry in the array describes a child item to which the context menu applies, for instance, a selected file the user wants to Open.
        /// </summary>
        public IntPtr apidl;

        /// <summary>
        /// A pointer to the IQueryAssociations interface on the object from which to load extensions. This parameter is optional and thus can be NULL. If this value is NULL and members aKeys and cKeys are also NULL (see Remarks), punkAssociationInfo is computed from the apidl member and cidl via a request for IQueryAssociations through IShellFolder::GetUIObjectOf.
        /// If IShellFolder::GetUIObjectOf returns E_NOTIMPL, a default implementation is provided based on the SFGAO_FOLDER and SFGAO_FILESYSTEM attributes returned from IShellFolder::GetAttributesOf.
        /// </summary>
        public IntPtr punkAssociationInfo;

        /// <summary>
        /// The count of items in member aKeys. This value can be zero. If the value is zero, the extensions are loaded based on the object that supports interface IQueryAssociations as specified by member punkAssociationInfo. If the value is non-NULL, the extensions are loaded based only on member aKeys and not member punkAssociationInfo.
        ///  Note: The maximum number of keys is 16. Callers must enforce this limit as the API does not. Failing to do so can result in memory corruption.
        /// </summary>
        public uint cKeys;

        /// <summary>
        /// A pointer to an HKEY that specifies the registry key from which to load extensions. This parameter is optional and can be NULL. If the value is NULL, the extensions are loaded based on the object that supports interface IQueryAssociations as specified in punkAssociationInfo.
        /// </summary>
        public IntPtr[] aKeys;
    }
    
    // ReSharper restore InconsistentNaming
}