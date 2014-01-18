using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.EnterpriseServices;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;
using SharpShell.Attributes;
using SharpShell.Extensions;
using SharpShell.Interop;
using SharpShell.Pidl;
using SharpShell.ServerRegistration;

namespace SharpShell.SharpNamespaceExtension
{
    //  More info:
    //      Virtual Junction Points: http://msdn.microsoft.com/en-us/library/windows/desktop/cc144096(v=vs.85).aspx

    /// <summary>
    /// A <see cref="SharpNamespaceExtension"/> is a SharpShell implemented Shell Namespace Extension.
    /// This is the base class for all Shell Namespace Extensions.
    /// </summary>
    [ServerType(ServerType.ShellNamespaceExtension)]
    public abstract class SharpNamespaceExtension : 
        SharpShellServer, 
        IPersistFolder2,
        IShellFolder2

    {
        protected SharpNamespaceExtension()
        {
            Log("Instatiated Namespace Extension");
        }

        #region Implementation of IPersistFolder2.

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
        int IPersistFolder.GetClassID(out Guid pClassId) {return ((IPersist)this).GetClassID(out pClassId); }
        int IPersistFolder2.GetClassID(out Guid pClassId) { return ((IPersist)this).GetClassID(out pClassId); }

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
        int IPersistFolder2.Initialize(IntPtr pidl) { return ((IPersistFolder)this).Initialize(pidl); }

        /// <summary>
        /// Gets the ITEMIDLIST for the folder object.
        /// </summary>
        /// <param name="ppidl">The address of an ITEMIDLIST pointer. This PIDL represents the absolute location of the folder and must be relative to the desktop. This is typically a copy of the PIDL passed to Initialize.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        /// <remarks>
        /// If the folder object has not been initialized, this method returns S_FALSE and ppidl is set to NULL.
        /// </remarks>
        int IPersistFolder2.GetCurFolder(out IntPtr ppidl)
        {
            //  If we haven't been initialised set a null pidl and return false.
            if (this.extensionAbsolutePidl == null)
            {
                ppidl = IntPtr.Zero;
                return WinError.S_FALSE;
            }

            //  Otherwise, set the pidl and return.
            ppidl = PidlManager.IdListToPidl(extensionAbsolutePidl);
            return WinError.S_OK;
        }

        #endregion


        #region Implmentation of IShellFolder

        /// <summary>
        /// Translates the display name of a file object or a folder into an item identifier list.
        /// </summary>
        /// <param name="hwnd">A window handle. The client should provide a window handle if it displays a dialog or message box. Otherwise set hwnd to NULL.</param>
        /// <param name="pbc">Optional. A pointer to a bind context used to pass parameters as inputs and outputs to the parsing function.</param>
        /// <param name="pszDisplayName">A null-terminated Unicode string with the display name.</param>
        /// <param name="pchEaten">A pointer to a ULONG value that receives the number of characters of the display name that was parsed. If your application does not need this information, set pchEaten to NULL, and no value will be returned.</param>
        /// <param name="ppidl">When this method returns, contains a pointer to the PIDL for the object.</param>
        /// <param name="pdwAttributes">The value used to query for file attributes. If not used, it should be set to NULL.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        int IShellFolder.ParseDisplayName(IntPtr hwnd, IntPtr pbc, string pszDisplayName, ref uint pchEaten, out IntPtr ppidl, ref SFGAO pdwAttributes)
        {
            //  There's not much to this due to the way we handle PIDLs - we can use the pidl manager to 
            //  decode out pidl.
            ppidl = PidlManager.IdListToPidl(IdList.FromParsingString(pszDisplayName));
            pchEaten = (uint)pszDisplayName.Length;

            //  Get the attributes while we're here.
            //  TODO: get the attributes.

            return WinError.S_OK;
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

        /// <summary>
        /// Requests a pointer to an object's storage interface.
        /// Return value: error code, if any
        /// </summary>
        /// <param name="pidl">Address of an ITEMIDLIST structure that identifies the subfolder relative to its parent folder.</param>
        /// <param name="pbc">Optional address of an IBindCtx interface on a bind context object to be  used during this operation.</param>
        /// <param name="riid">Interface identifier (IID) of the requested storage interface.</param>
        /// <param name="ppv">Address that receives the interface pointer specified by riid.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        int IShellFolder.BindToStorage(IntPtr pidl, IntPtr pbc, ref Guid riid, out IntPtr ppv)
        {
            //  TODO: this will need to be implemented at some stage.
            ppv = IntPtr.Zero;
            return WinError.E_NOTIMPL;
        }

        /// <summary>
        /// Determines the relative order of two file objects or folders, given
        /// their item identifier lists. Return value: If this method is
        /// successful, the CODE field of the HRESULT contains one of the
        /// following values (the code can be retrived using the helper function
        /// GetHResultCode): Negative A negative return value indicates that the first item should precede the second (pidl1 &lt; pidl2).
        /// Positive A positive return value indicates that the first item should
        /// follow the second (pidl1 &gt; pidl2).  Zero A return value of zero
        /// indicates that the two items are the same (pidl1 = pidl2).
        /// </summary>
        /// <param name="lParam">Value that specifies how the comparison  should be performed. The lower Sixteen bits of lParam define the sorting  rule.
        /// The upper sixteen bits of lParam are used for flags that modify the sorting rule. values can be from  the SHCIDS enum</param>
        /// <param name="pidl1">Pointer to the first item's ITEMIDLIST structure.</param>
        /// <param name="pidl2">Pointer to the second item's ITEMIDLIST structure.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        int IShellFolder.CompareIDs(IntPtr lParam, IntPtr pidl1, IntPtr pidl2)
        {
            //  Get the low short from the lParam, this is the sorting option.
            short sortingRule = (short)(lParam.ToInt64() & 0x000000FF);
            SCHIDS modifiers = (SCHIDS) ((lParam.ToInt64() >> 16) & 0x000000FF);

            //  TODO: build an HRESULT with a CODE that contains a negative/positive/zero indicator.

            return WinError.S_OK;
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
                //  If we can create a view object, we'll use that.
                var customView = CreateView();
                if (customView != null)
                {
                    //  TODO: we need to split the classes - one custom view folder, one def view folder.
                    var host = new ShellViewHost(customView);
                    ppv = Marshal.GetComInterfaceForObject(host, typeof(IShellView));
                    return WinError.S_OK;
                }

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
            else if (riid == typeof (Interop.IDropTarget).GUID)
            {
                ppv = IntPtr.Zero;
                return WinError.E_NOINTERFACE;
            }
            else if (riid == typeof (IContextMenu).GUID)
            {
                ppv = IntPtr.Zero;
                return WinError.E_NOINTERFACE;
            }
            else if (riid == typeof(IExtractIcon).GUID)
            {
                ppv = IntPtr.Zero;
                return WinError.E_NOINTERFACE;
            }
            else if (riid == typeof(IQueryInfo).GUID)
            {
                ppv = IntPtr.Zero;
                return WinError.E_NOINTERFACE;
            }
            else if (riid == typeof(IShellDetails).GUID)
            {
                ppv = IntPtr.Zero;
                return WinError.E_NOINTERFACE;
            }
            //  TODO: we have to deal with others later.
            else
            {
                //  We've been asked for a com inteface we cannot handle.
                ppv = IntPtr.Zero;

                //  Importantly in this case, we MUST return E_NOINTERFACE.
                return WinError.E_NOINTERFACE;
            }
        }

        /// <summary>
        /// Retrieves the attributes of one or more file objects or subfolders.
        /// Return value: error code, if any
        /// </summary>
        /// <param name="cidl">Number of file objects from which to retrieve attributes.</param>
        /// <param name="apidl">Address of an array of pointers to ITEMIDLIST structures, each of which  uniquely identifies a file object relative to the parent folder.</param>
        /// <param name="rgfInOut">Address of a single ULONG value that, on entry contains the attributes that the caller is
        /// requesting. On exit, this value contains the requested attributes that are common to all of the specified objects. this value can be from the SFGAO enum</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        int IShellFolder.GetAttributesOf(uint cidl, IntPtr[] apidl, ref SFGAO rgfInOut)
        {
            //  Get each pidl as an idlist.
            var idlists = (cidl > 0 && apidl != null)
                ? from pidl in apidl select PidlManager.PidlToIdlist(pidl)
                : new List<IdList>();

            //  Now we can ask for the attributes of each item. We only ask for attributes that
            //  are set in the flags - clearing them if they don't apply to every item.
            var allItems = idlists.Select(GetChildItem);
            var allAttributes = allItems.Select(sni => sni.GetAttributes()).ToList();
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_BROWSABLE, allAttributes.All(a => a.HasFlag(AttributeFlags.IsBrowsable)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_CANCOPY, allAttributes.All(a => a.HasFlag(AttributeFlags.CanByCopied)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_CANDELETE, allAttributes.All(a => a.HasFlag(AttributeFlags.CanBeDeleted)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_CANLINK, allAttributes.All(a => a.HasFlag(AttributeFlags.CanBeLinked)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_CANMOVE, allAttributes.All(a => a.HasFlag(AttributeFlags.CanBeMoved)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_CANRENAME, allAttributes.All(a => a.HasFlag(AttributeFlags.CanBeRenamed)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_COMPRESSED, allAttributes.All(a => a.HasFlag(AttributeFlags.IsCompressed)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_DROPTARGET, allAttributes.All(a => a.HasFlag(AttributeFlags.IsDropTarget)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_ENCRYPTED, allAttributes.All(a => a.HasFlag(AttributeFlags.IsEncrypted)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_FILESYSANCESTOR, allAttributes.All(a => a.HasFlag(AttributeFlags.IsFileSystemAncestor)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_FILESYSTEM, allAttributes.All(a => a.HasFlag(AttributeFlags.IsFileSystem)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_FOLDER, allAttributes.All(a => a.HasFlag(AttributeFlags.IsFolder)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_GHOSTED, allAttributes.All(a => a.HasFlag(AttributeFlags.IsBrowsable)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_HASPROPSHEET, allAttributes.All(a => a.HasFlag(AttributeFlags.HasPropertySheets)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_HASSUBFOLDER, allAttributes.All(a => a.HasFlag(AttributeFlags.MayContainSubFolders)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_HIDDEN, allAttributes.All(a => a.HasFlag(AttributeFlags.IsHidden)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_ISSLOW, allAttributes.All(a => a.HasFlag(AttributeFlags.IsSlow)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_LINK, allAttributes.All(a => a.HasFlag(AttributeFlags.IsLink)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_NEWCONTENT, allAttributes.All(a => a.HasFlag(AttributeFlags.HasOrIsNewContent)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_READONLY, allAttributes.All(a => a.HasFlag(AttributeFlags.IsReadOnly)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_REMOVABLE, allAttributes.All(a => a.HasFlag(AttributeFlags.IsRemovableMedia)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_SHARE, allAttributes.All(a => a.HasFlag(AttributeFlags.IsShared)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_STORAGE, allAttributes.All(a => a.HasFlag(AttributeFlags.IsStorage)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_STORAGEANCESTOR, allAttributes.All(a => a.HasFlag(AttributeFlags.IsStorageAncestor)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_STREAM, allAttributes.All(a => a.HasFlag(AttributeFlags.IsStream)));
            UpdateFlagIfSet(ref rgfInOut, SFGAO.SFGAO_VALIDATE, allAttributes.All(a => a.HasFlag(AttributeFlags.IsVolatile)));

            //  And we're done.
            return WinError.S_OK;
        }

        private static void UpdateFlagIfSet(ref SFGAO sfgao, SFGAO flag, bool set)
        {
            if (sfgao.HasFlag(flag))
            {
                if (set == false)
                    sfgao ^= flag;
            }
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

        /// <summary>
        /// Retrieves the display name for the specified file object or subfolder.
        /// Return value: error code, if any
        /// </summary>
        /// <param name="pidl">Address of an ITEMIDLIST structure (PIDL)  that uniquely identifies the file  object or subfolder relative to the parent  folder.</param>
        /// <param name="uFlags">Flags used to request the type of display name to return. For a list of possible values.</param>
        /// <param name="pName">Address of a STRRET structure in which to return the display name.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
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

            //  If we have not worked out what the caller needs, warn about this in the log.
            LogError("An unexpected set of flags were passed to IShellFolder.GetDisplayNameOf (" + uFlags + ").");
            pName = new STRRET {uType = STRRET.STRRETTYPE.STRRET_WSTR};
            return WinError.E_NOTIMPL;
        }


        /// <summary>
        /// Sets the display name of a file object or subfolder, changing the item
        /// identifier in the process.
        /// Return value: error code, if any
        /// </summary>
        /// <param name="hwnd">Handle to the owner window of any dialog or message boxes that the client displays.</param>
        /// <param name="pidl">Pointer to an ITEMIDLIST structure that uniquely identifies the file object or subfolder relative to the parent folder.</param>
        /// <param name="pszName">Pointer to a null-terminated string that specifies the new display name.</param>
        /// <param name="uFlags">Flags indicating the type of name specified by  the lpszName parameter. For a list of possible values, see the description of the SHGNO enum.</param>
        /// <param name="ppidlOut"></param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        int IShellFolder.SetNameOf(IntPtr hwnd, IntPtr pidl, string pszName, SHGDNF uFlags, out IntPtr ppidlOut)
        {
            //  TODO this needs to be implemented.
            ppidlOut = IntPtr.Zero;
            return WinError.E_NOTIMPL;
        }
        
        #endregion

        #region IShellFolder2 Implementation

        int IShellFolder2.ParseDisplayName(IntPtr hwnd, IntPtr pbc, string pszDisplayName, ref uint pchEaten,
            out IntPtr ppidl, ref SFGAO pdwAttributes)
        {
            return ((IShellFolder)this).ParseDisplayName(hwnd, pbc, pszDisplayName, pchEaten, out ppidl,
                ref pdwAttributes);
        }

        int IShellFolder2.EnumObjects(IntPtr hwnd, SHCONTF grfFlags, out IEnumIDList ppenumIDList)
        {
            return ((IShellFolder)this).EnumObjects(hwnd, grfFlags, out ppenumIDList);

        }

        int IShellFolder2.BindToObject(IntPtr pidl, IntPtr pbc, ref Guid riid, out IntPtr ppv)
        {
            return ((IShellFolder)this).BindToObject(pidl, pbc, ref riid, out ppv);
        }

        int IShellFolder2.BindToStorage(IntPtr pidl, IntPtr pbc, ref Guid riid, out IntPtr ppv)
        {
            return ((IShellFolder)this).BindToStorage(pidl, pbc, ref riid, out ppv);
        }

        int IShellFolder2.CompareIDs(IntPtr lParam, IntPtr pidl1, IntPtr pidl2)
        {
            return ((IShellFolder)this).CompareIDs(lParam, pidl1, pidl2);
        }

        int IShellFolder2.CreateViewObject(IntPtr hwndOwner, ref Guid riid, out IntPtr ppv)
        {
            return ((IShellFolder)this).CreateViewObject(hwndOwner, ref riid, out ppv);
        }

        int IShellFolder2.GetAttributesOf(uint cidl, IntPtr[] apidl, ref SFGAO rgfInOut)
        {
            return ((IShellFolder)this).GetAttributesOf(cidl, apidl, ref rgfInOut);
        }

        int IShellFolder2.GetUIObjectOf(IntPtr hwndOwner, uint cidl, IntPtr[] apidl, ref Guid riid, uint rgfReserved,
            out IntPtr ppv)
        {
            return ((IShellFolder)this).GetUIObjectOf(hwndOwner, cidl, apidl, ref riid, rgfReserved, out ppv);
        }

        int IShellFolder2.GetDisplayNameOf(IntPtr pidl, SHGDNF uFlags, out STRRET pName)
        {
            return ((IShellFolder)this).GetDisplayNameOf(pidl, uFlags, out pName);
        }

        int IShellFolder2.SetNameOf(IntPtr hwnd, IntPtr pidl, string pszName, SHGDNF uFlags, out IntPtr ppidlOut)
        {
            return ((IShellFolder)this).SetNameOf(hwnd, pidl, pszName, uFlags, out ppidlOut);
        }
        
        int IShellFolder2.GetDefaultSearchGUID(out Guid pguid)
        {
            pguid = Guid.Empty;
            return WinError.E_NOTIMPL;
        }

        int IShellFolder2.EnumSearches(out IEnumExtraSearch ppenum)
        {
            ppenum = null;
            return WinError.E_NOTIMPL;
        }

        int IShellFolder2.GetDefaultColumn(uint dwRes, out uint pSort, out uint pDisplay)
        {
            pSort = 0;
            pDisplay = 0;
            return WinError.E_NOTIMPL;
        }

        int IShellFolder2.GetDefaultColumnState(uint iColumn, out SHCOLSTATEF pcsFlags)
        {
            pcsFlags = 0;
            return WinError.E_NOTIMPL;
        }

        int IShellFolder2.GetDetailsEx(IntPtr pidl, SHCOLUMNID pscid, out IntPtr pv)
        {
            pv = IntPtr.Zero;
            return WinError.E_NOTIMPL;
        }

        int IShellFolder2.GetDetailsOf(IntPtr pidl, uint iColumn, out IntPtr psd)
        {
            if (iColumn == 0)
            {


                var sd =  new SHELLDETAILS
                {
                    fmt = 0,
                    cxChar = 5,
                    str = STRRET.CreateUnicode("Name")
                };
                IntPtr buf = Marshal.AllocHGlobal(
    	            Marshal.SizeOf(sd));
                Marshal.StructureToPtr(sd,
    	            buf, false);
                psd = buf;
                return WinError.S_OK;
            }
            else
            {
                psd = IntPtr.Zero;
                return WinError.E_FAIL;
            }
        }

        int IShellFolder2.MapColumnToSCID(uint iColumn, out SHCOLUMNID pscid)
        {
            //  TODO: see http://msdn.microsoft.com/en-us/library/windows/desktop/bb759748(v=vs.85).aspx
            //  TODO: see http://msdn.microsoft.com/en-us/library/windows/desktop/bb773381(v=vs.85).aspx

            //  Unique PROPERTYID value.
            pscid = new SHCOLUMNID
            {
                fmtid = Guid.NewGuid(),
                pid = 2
            };

            /*
            pscid = new SHCOLUMNID
            {
                fmtid = new Guid("{B725F130-47EF-101A-A5F1-02608C9EEBAC}"),
                pid = 10 // displayname.
            }; */

            return WinError.S_OK;
        }

        #endregion

        #region Custom Registration and Unregistration

        /// <summary>
        /// The custom registration function.
        /// </summary>
        /// <param name="serverType">Type of the server.</param>
        /// <param name="registrationType">Type of the registration.</param>
        [CustomRegisterFunction]
        internal static void CustomRegisterFunction(Type serverType, RegistrationType registrationType)
        {
            //  TODO: currently, we will only support virtual junction points.

            //  Get the junction point.
            var junctionPoint = NamespaceExtensionJunctionPointAttribute.GetJunctionPoint(serverType);

            //  If the junction point is not defined, we must fail.
            if (junctionPoint == null)
                throw new InvalidOperationException("Unable to register a SharpNamespaceExtension as it is missing it's junction point definition.");

            //  Now we have the junction point, we can build the key as below:
            /* HKEY_LOCAL_MACHINE or HKEY_CURRENT_USER
                   Software
                      Microsoft
                         Windows
                            CurrentVersion
                               Explorer
                                  Virtual Folder Name
                                     NameSpace
                                        {Extension CLSID}
                                           (Default) = Junction Point Name
            */

            //  Work out the hive and view to use, based on the junction point availability
            //  and the registration mode.
            var hive = junctionPoint.Availablity == NamespaceExtensionAvailability.CurrentUser
                ? RegistryHive.CurrentUser
                : RegistryHive.LocalMachine;
            var view = registrationType == RegistrationType.OS64Bit ? RegistryView.Registry64 : RegistryView.Registry32;

            //  Now open the base key.
            using (var baseKey = RegistryKey.OpenBaseKey(hive, view))
            {
                //  Create the path to the virtual folder namespace key.
                var virtualFolderNamespacePath =
                    string.Format(@"Software\Microsoft\Windows\CurrentVersion\Explorer\{0}\NameSpace",
                        RegistryKeyAttribute.GetRegistryKey(junctionPoint.Location));

                //  Open the virtual folder namespace key,
                using (var namespaceKey = baseKey.OpenSubKey(virtualFolderNamespacePath,
                    RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.WriteKey))
                {
                    //  If we don't have the key, we've got a problem.
                    if (namespaceKey == null)
                        throw new InvalidOperationException("Cannot open the Virtual Folder NameSpace key.");

                    //  Write the server guid as a key, then the Junction Point Name as it's default value.
                    var serverKey = namespaceKey.CreateSubKey(serverType.GUID.ToRegistryString());
                    if(serverKey == null)
                        throw new InvalidOperationException("Failed to create the Virtual Folder NameSpace extension.");
                    serverKey.SetValue(null, junctionPoint.Name, RegistryValueKind.String);
                }
            }

            //  We can now customise the class registration as needed.
            //  The class is already registered by the Installation of the server, we're only
            //  adapting it here.

            //  Open the classes root.
            using (var classesBaseKey = registrationType == RegistrationType.OS64Bit
                ? RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64) :
                  RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry32))
            {
                //  Our server guid.
                var serverGuid = serverType.GUID.ToRegistryString();

                //  Open the Class Key.
                using (var classKey = classesBaseKey
                    .OpenSubKey(string.Format(@"CLSID\{0}", serverGuid),
                    RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.WriteKey))
                {
                    //  If we don't have the key, we've got a problem.
                    if (classKey == null)
                        throw new InvalidOperationException("Cannot open the class key.");

                    //  Create an instance of the server to get it's registration settings.
                    var serverInstance = (SharpNamespaceExtension)Activator.CreateInstance(serverType);
                    var registrationSettings = serverInstance.GetRegistrationSettings();

                    //  Apply basic settings.
                    if(registrationSettings.HideFolderVerbs) classKey.SetValue("HideFolderVerbs", 1, RegistryValueKind.DWord);

                    //  TODO: at some stage, we may handle WantsFORPARSING
                    //  TODO: at some stage, we must handle HideAsDelete
                    //  TODO: at some stage, we must handle HideAsDeletePerUser
                    //  TODO: at some stage, we must handle QueryForOverlay

                    //  The default value is the junction point name.
                    classKey.SetValue(null, junctionPoint.Name, RegistryValueKind.String);

                    //  Set the infotip. TODO
                    // classKey.SetValue(@"InfoTip", infoTip, RegistryValueKind.String);

                    //  Set the default icon. TODO key not attribute
                    //  classKey.SetValue(@"DefaultIcon", "File.dll,index", RegistryValueKind.String);
                    
                    //  TODO support custom verbs with a 'Shell' subkey.
                    //  TODO support custom shortcut menu handler with ShellEx.
                    //  TODO tie in support for a property sheet handler.
                    
                    //  Set the attributes.
                    using (var shellFolderKey = classKey.CreateSubKey("ShellFolder"))
                    {
                        if(shellFolderKey == null)
                            throw new InvalidOperationException("An exception occured creating the ShellFolder key.");
                        shellFolderKey.SetValue("Attributes", (int)registrationSettings.ExtensionAttributes, RegistryValueKind.DWord);
                    }
                    //  TODO Critical, as we don't set SGFAO_FOLDER in the above currently, we can't display child items.
                    //  See documentation at: http://msdn.microsoft.com/en-us/library/windows/desktop/cc144093.aspx#ishellfolder
                }
            }
        }

        /// <summary>
        /// Customs the unregister function.
        /// </summary>
        /// <param name="serverType">Type of the server.</param>
        /// <param name="registrationType">Type of the registration.</param>
        [CustomUnregisterFunction]
        internal static void CustomUnregisterFunction(Type serverType, RegistrationType registrationType)
        {
            //  Get the junction point.
            var junctionPoint = NamespaceExtensionJunctionPointAttribute.GetJunctionPoint(serverType);

            //  If the junction point is not defined, we must fail.
            if (junctionPoint == null)
                throw new InvalidOperationException("Unable to register a SharpNamespaceExtension as it is missing it's junction point definition.");
            
            //  Work out the hive and view to use, based on the junction point availability
            //  and the registration mode.
            var hive = junctionPoint.Availablity == NamespaceExtensionAvailability.CurrentUser
                ? RegistryHive.CurrentUser
                : RegistryHive.LocalMachine;
            var view = registrationType == RegistrationType.OS64Bit ? RegistryView.Registry64 : RegistryView.Registry32;

            //  Now open the base key.
            using (var baseKey = RegistryKey.OpenBaseKey(hive, view))
            {
                //  Create the path to the virtual folder namespace key.
                var virtualFolderNamespacePath =
                    string.Format(@"Software\Microsoft\Windows\CurrentVersion\Explorer\{0}\NameSpace",
                        RegistryKeyAttribute.GetRegistryKey(junctionPoint.Location));
                
                //  Open the virtual folder namespace key,
                using (var namespaceKey = baseKey.OpenSubKey(virtualFolderNamespacePath,
                    RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.WriteKey))
                {
                    //  If we don't have the key, we've got a problem.
                    if (namespaceKey == null)
                        throw new InvalidOperationException("Cannot open the Virtual Folder NameSpace key.");

                    //  Delete the shell extension key, which is just it's CLSID.
                    namespaceKey.DeleteSubKeyTree(serverType.GUID.ToRegistryString());
                }
            }
        }

        #endregion

        private IShellNamespaceItem GetChildItem(IdList idList)
        {
            //  todo optimise this heavily to allow the folder to find it's own child.
            var childItems = new List<IShellNamespaceItem>();
            uint index = 0, count = 10;
            while (true)
            {
                var taken = EnumerateChildren(index, count, new Targets()).ToList();
                childItems.AddRange(taken);
                if (taken.Count < count)
                    break;
                index += (uint)taken.Count;
            }
            return childItems.SingleOrDefault(ci => idList.Matches(ci.GetUniqueId()));
        }


        /// <summary>
        /// Gets the attributes for the namespace extension. These attributes can be used
        /// to identify that a shell extension is a folder, contains folders, is part of the
        /// file system and so on and so on.
        /// </summary>
        /// <returns>The attributes for the shell item</returns>
        public abstract AttributeFlags GetAttributes();

        /// <summary>
        /// This function is called by SharpShell to get the children of a Shell Folder. For performance reasons,
        /// children will often be loaded in batches, so they must be returned as a list.
        /// </summary>
        /// <param name="index">The index of the first item in the set to load.</param>
        /// <param name="count">The number of items to load.</param>
        /// <param name="flags">The enumeration flags.</param>
        /// <returns>A set of child items that corresponds to the requested range.</returns>
        public abstract IEnumerable<IShellNamespaceItem> EnumerateChildren(uint index, uint count,
            Targets flags);


        /// <summary>
        /// Gets the registration settings. This function is called only during the initial
        /// registration of a shell namespace extension to provide core configuration.
        /// </summary>
        /// <returns>Registration settings for the server.</returns>
        public abstract NamespaceExtensionRegistrationSettings GetRegistrationSettings();

        public abstract Control CreateView();

        private IdList extensionAbsolutePidl;

    }

    /// <summary>
    /// Targets for an enumeration of shell items.
    /// </summary>
    [Flags]
    public enum Targets
    {
        /// <summary>
        /// The enumeration must include folders.
        /// </summary>
        Folders = 1,

        /// <summary>
        /// The enumeration must include items.
        /// </summary>
        Items = 2
    }

    public class DetailsViewColumn
    {
        public string Name { get; private set; }
    }
}
