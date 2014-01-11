using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using SharpShell.Attributes;
using SharpShell.Interop;
using SharpShell.Pidl;

namespace SharpShell.SharpNamespaceExtension
{
    [ServerType(ServerType.ShellNamespaceExtension)]
    public abstract class SharpNamespaceExtension : SharpShellServer, IPersistFolder, IShellFolder
    {
        #region Implementation of IPersistFolder.

        /// <summary>
        /// Retrieves the class identifier (CLSID) of the object.
        /// </summary>
        /// <param name="pClassID">A pointer to the location that receives the CLSID on return.
        /// The CLSID is a globally unique identifier (GUID) that uniquely represents an object 
        /// class that defines the code that can manipulate the object's data.</param>
        /// <returns>
        /// If the method succeeds, the return value is S_OK. Otherwise, it is E_FAIL.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        int IPersist.GetClassID(out Guid pClassID)
        {
            //  In our case, we can provide our SharpShell server class ID.
            pClassID = ServerClsid;

            //  We're done.
            return WinError.S_OK;
        }

        /// <summary>
        /// Instructs a Shell folder object to initialize itself based on the information passed.
        /// </summary>
        /// <param name="pidl">The address of the ITEMIDLIST (item identifier list) structure 
        /// that specifies the absolute location of the folder.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        int IPersistFolder.Initialize(IntPtr pidl)
        {
            //  The shell has initialised the extension and provided an absolute PIDL
            //  from the root (desktop) to the extension folder. We can store this
            //  pidl in our own format.
            extensionAbsolutePidl = PidlManager.PidlToIdlist(pidl);

            //  We're good, we've got the ID list.
            return WinError.S_OK;
        }

        #endregion

        #region Implmentation of IShellFolder

        int IShellFolder.ParseDisplayName(IntPtr hwnd, IntPtr pbc, string pszDisplayName, uint pchEaten, out IntPtr ppidl, uint pdwAttributes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Allows a client to determine the contents of a folder by creating an item identifier enumeration object and returning its IEnumIDList interface.
        /// Return value: error code, if any
        /// </summary>
        /// <param name="hwnd">If user input is required to perform the enumeration, this window handle should be used by the enumeration object as the parent window to take user input.</param>
        /// <param name="grfFlags">Flags indicating which items to include in the  enumeration. For a list of possible values, see the SHCONTF enum.</param>
        /// <param name="ppenumIDList">Address that receives a pointer to the IEnumIDList interface of the enumeration object created by this method.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        int IShellFolder.EnumObjects(IntPtr hwnd, SHCONTF grfFlags, out IEnumIDList ppenumIDList)
        {
            //  Create an object that will enumerate the contents of this shell folder (that implements
            //  IEnumIdList). This can be returned to the shell.
            ppenumIDList = new SharpNamespaceExtensionIdListEnumerator(this, grfFlags, 0);

            //  TODO we should also store the window handle for user interaction.

            //  We're done.
            return WinError.S_OK;
        }

        /// <summary>
        /// Retrieves an IShellFolder object for a subfolder.
        //  Return value: error code, if any
        /// </summary>
        /// <param name="pidl">Address of an ITEMIDLIST structure (PIDL) that identifies the subfolder.</param>
        /// <param name="pbc">Optional address of an IBindCtx interface on a bind context object to be used during this operation.</param>
        /// <param name="riid">Identifier of the interface to return. </param>
        /// <param name="ppv">Address that receives the interface pointer.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        int IShellFolder.BindToObject(IntPtr pidl, IntPtr pbc, ref Guid riid, out IntPtr ppv)
        {
            //  We can only return IShellFolder interfaces.
            if (riid != typeof (IShellFolder).GUID)
            {
                ppv = IntPtr.Zero;
                return WinError.E_NOINTERFACE;
            }

            //  We've been asked for a shell folder, so we must get the shell folder for the given
            //  pidl which is relative to the root of the extension but can be nested.
            var folderIdlist = PidlManager.PidlToIdlist(pidl);

            //  We have a folder ID list, this means we can ask the extension to get a folder.
            //  TODO: we must now create an IShellFolder for that specific folder.
            ppv = IntPtr.Zero;
            return WinError.S_OK;
        }

        int IShellFolder.BindToStorage(IntPtr pidl, IntPtr pbc, ref Guid riid, out IntPtr ppv)
        {
            throw new NotImplementedException();
        }

        int IShellFolder.CompareIDs(int lParam, IntPtr pidl1, IntPtr pidl2)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Requests an object that can be used to obtain information from or interact
        /// with a folder object.
        /// Return value: error code, if any
        /// </summary>
        /// <param name="hwndOwner">Handle to the owner window.</param>
        /// <param name="riid">Identifier of the requested interface.</param>
        /// <param name="ppv">Address of a pointer to the requested interface.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        int IShellFolder.CreateViewObject(IntPtr hwndOwner, ref Guid riid, out IntPtr ppv)
        {
            //  Before the contents of the folder are displayed, the shell asks for an IShellView.
            //  This function is also called to get other shell interfaces for interacting with the
            //  folder itself.

            if (riid == typeof (IShellView).GUID)
            {

                //  TODO: Currently we are only support the default shell view. By allowing 
                //  clients to implement IShellView via a SharpShell wrapper we can take this further.

                //  Create a default folder view.
                SFV_CREATE createInfo = new SFV_CREATE
                {
                    cbSize = (uint) Marshal.SizeOf(typeof (SFV_CREATE)),
                    pshf = this, //  We are the IShellFolder.
                    psvOuter = null, //  We have no outer view.
                    psfvcb = null //  No callback provided.
                };
                IShellView view;
                if (Shell32.SHCreateShellFolderView(createInfo, out view) != WinError.S_OK)
                {
                    LogError("An error occured creating the default folder view for this shell folder.");
                    ppv = IntPtr.Zero;
                    return WinError.E_FAIL;
                }

                //  We've created the view, return it.
                ppv = Marshal.GetComInterfaceForObject(view, typeof (IShellView));
                return WinError.S_OK;
            }
            //  TODO: we have to deal with others later.
            else
            {
                //  We've been asked for a com inteface we cannot handle.
                ppv = IntPtr.Zero;
                return WinError.E_NOTIMPL;
            }
        }

        int IShellFolder.GetAttributesOf(uint cidl, IntPtr[] apidl, ref SFGAO rgfInOut)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves an OLE interface that can be used to carry out actions on the
        /// specified file objects or folders. Return value: error code, if any
        /// </summary>
        /// <param name="hwndOwner">Handle to the owner window that the client should specify if it displays a dialog box or message box.</param>
        /// <param name="cidl">Number of file objects or subfolders specified in the apidl parameter.</param>
        /// <param name="apidl">Address of an array of pointers to ITEMIDLIST  structures, each of which  uniquely identifies a file object or subfolder relative to the parent folder.</param>
        /// <param name="riid">Identifier of the COM interface object to return.</param>
        /// <param name="rgfReserved">Reserved.</param>
        /// <param name="ppv">Pointer to the requested interface.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        int IShellFolder.GetUIObjectOf(IntPtr hwndOwner, uint cidl, IntPtr[] apidl, ref Guid riid, uint rgfReserved, out IntPtr ppv)
        {
            //  We have a set of child pidls (i.e. length one). We can now offer interfaces such as:
            /*
             * IContextMenu	The cidl parameter can be greater than or equal to one.
IContextMenu2	The cidl parameter can be greater than or equal to one.
IDataObject	The cidl parameter can be greater than or equal to one.
IDropTarget	The cidl parameter can only be one.
IExtractIcon	The cidl parameter can only be one.
IQueryInfo	The cidl parameter can only be one.
             * */

            //  Currently, we don't offer any extra child item UI objects.
            ppv = IntPtr.Zero;
            return WinError.E_NOTIMPL;
        }

        int IShellFolder.GetDisplayNameOf(IntPtr pidl, SHGDNF uFlags, out STRRET pName)
        {
            //  Create an idlist from the pidl.
            var idlist = PidlManager.PidlToIdlist(pidl);

            //  Get the shell item.
            //  TODO; handle errors
            var shellItem = GetChildItem(idlist);

            //  If the flags are normal only, we're asking for the display name only.
            if (uFlags == SHGDNF.SHGDN_NORMAL)
            {
                //  We only need the display name.
                pName = STRRET.CreateUnicode(shellItem.GetDisplayName(DisplayNameContext.OutOfFolder));
                return WinError.S_OK;
            }

            //  If the flags are in folder, we're asking for the standard display name.
            if (uFlags == SHGDNF.SHGDN_INFOLDER || uFlags == SHGDNF.SHGDN_FORADDRESSBAR)
            {
                pName = STRRET.CreateUnicode(shellItem.GetDisplayName(DisplayNameContext.Normal));
                return WinError.S_OK;
            }

            //  If the flags indicate parsing mode, we need to construct a name
            //  that'll let us bounce from PIDL <-> name. We do this, rather than
            //  the implementor.
            if (uFlags.HasFlag(SHGDNF.SHGDN_FORPARSING))
            {
                //  It's either relative (INFOLDER) or fully qualified.
                var str = uFlags.HasFlag(SHGDNF.SHGDN_INFOLDER)
                    ? idlist.ToParsingString()
                    : /* TODO start with my id list */ idlist.ToParsingString();
                pName = STRRET.CreateUnicode(str);
                return WinError.S_OK;
            }

            //  If we have in folder and for editing, we're asking for the editing name.
            if (uFlags == (SHGDNF.SHGDN_INFOLDER & SHGDNF.SHGDN_FOREDITING))
            {
                pName = STRRET.CreateUnicode(shellItem.GetDisplayName(DisplayNameContext.Editing));
                return WinError.S_OK;
            }
        }

        int IShellFolder.SetNameOf(IntPtr hwnd, IntPtr pidl, string pszName, SHCONTF uFlags, out IntPtr ppidlOut)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// This function is called by SharpShell to get the children of a Shell Folder. For performance reasons,
        /// children will often be loaded in batches, so they must be returned as a list.
        /// </summary>
        /// <param name="index">The index of the first item in the set to load.</param>
        /// <param name="count">The number of items to load.</param>
        /// <param name="flags">The enumeration flags.</param>
        /// <returns>A set of child items that corresponds to the requested range.</returns>
        public abstract IEnumerable<IShellNamespaceItem> EnumerateChildren(uint index, uint count,
            EnumerateChildrenFlags flags);

        public abstract IShellNamespaceItem GetChildItem(IdList idList);
    }

    public enum EnumerateChildrenFlags
    {
    }

    public class GitHubExtension
    {
        
    }
}
