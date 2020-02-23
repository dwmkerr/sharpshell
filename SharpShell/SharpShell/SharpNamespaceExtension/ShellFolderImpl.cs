using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using SharpShell.Interop;
using SharpShell.Pidl;

namespace SharpShell.SharpNamespaceExtension
{
    /// <summary>
    /// The ShellFolderImpl is used to provide a single location of functionality for the 
    /// Shell Folder parts of a namespace extension, as well as a namespace extension's 
    /// child folder (if any).
    /// </summary>
    internal class ShellFolderImpl : IShellFolder2, IPersistFolder2, IPersistIDList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellFolderImpl"/> class.
        /// This class must be initialised with a reference to the Shell Namespace Extension
        /// that is being used and the target object (which will be an <see cref="IShellNamespaceFolder" />.
        /// </summary>
        /// <param name="namespaceExtension">The namespace extension.</param>
        /// <param name="proxyFolder">The folder that we are acting as an implementation for.</param>
        public ShellFolderImpl(SharpNamespaceExtension namespaceExtension, IShellNamespaceFolder proxyFolder)
        {
            //  Store the namespace extension and folder.
            this.namespaceExtension = namespaceExtension;
            this.proxyFolder = proxyFolder;

            //  Create the lazy folder view.
            lazyFolderView = new Lazy<ShellNamespaceFolderView>(proxyFolder.GetView);
        }
        
        #region Implementation of IShellFolder and IShellFolder2

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
            //  First we can decode the pidl from the display name.
            var idList = IdList.FromParsingString(pszDisplayName);
            ppidl = PidlManager.IdListToPidl(idList);

            //  We always eat the entire display string for SharpShell PIDL/DisplayName parsing.
            pchEaten = (uint)pszDisplayName.Length;


            //  In theory, we should understand the pidl.
            var item = GetChildItem(idList);
            var name = item.GetDisplayName(DisplayNameContext.Normal);

            //  TODO: We may be asked to get the attributes at the same time. If so, we must set them here.
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
            ppenumIDList = new ShellNamespaceFolderIdListEnumerator(proxyFolder, grfFlags, 0);

            //  TODO we should also store the window handle for user interaction.

            //  We're done.
            return WinError.S_OK;
        }

        /// <summary>
        /// Retrieves an IShellFolder object for a subfolder.
        /// Return value: error code, if any
        /// </summary>
        /// <param name="pidl">Address of an ITEMIDLIST structure (PIDL) that identifies the subfolder.</param>
        /// <param name="pbc">Optional address of an IBindCtx interface on a bind context object to be used during this operation.</param>
        /// <param name="riid">Identifier of the interface to return. </param>
        /// <param name="ppv">Address that receives the interface pointer.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        int IShellFolder.BindToObject(IntPtr pidl, IntPtr pbc, ref Guid riid, out IntPtr ppv)
        {
            //  Have we been asked to bind to a folder?
            if (riid == typeof(IShellFolder).GUID || riid == typeof(IShellFolder2).GUID)
            {
                //  Get the child item.
                var idList = PidlManager.PidlToIdlist(pidl);
                var childItem = GetChildItem(idList);

                //  If the item is a folder, we can create a proxy for it and return the proxy.
                var subFolder = childItem as IShellNamespaceFolder;
                if (subFolder != null)
                {
                    var folderProxy = new ShellFolderImpl(namespaceExtension, subFolder);
                    ppv = Marshal.GetComInterfaceForObject(folderProxy, typeof(IShellFolder2));
                    return WinError.S_OK;
                }
            }

            //  Note: We are also asked to bind to IPropertyStore IPropertyStoreFactory and IPropertyStoreCache.

            //  If we cannot return the required interface, we must return no interface.
            ppv = IntPtr.Zero;
            return WinError.E_NOINTERFACE;
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
            SCHIDS modifiers = (SCHIDS)((lParam.ToInt64() >> 16) & 0x000000FF);

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

            if (riid == typeof(IShellView).GUID)
            {
                //  Now create the actual shell view.
                try
                {
                    //  Create the view, get its pointer and return success.
                    var shellFolderView = lazyFolderView.Value.CreateShellView(this);
                    ppv = Marshal.GetComInterfaceForObject(shellFolderView, typeof(IShellView));
                    return WinError.S_OK;
                }
                catch (Exception exception)
                {
                    //  Log the exception, set the view to null and fail.
                    Diagnostics.Logging.Error("An unhandled exception occured createing the folder view.", exception);
                    ppv = IntPtr.Zero;
                    return WinError.E_FAIL;
                }
            }
            else if (riid == typeof(Interop.IDropTarget).GUID)
            {
                ppv = IntPtr.Zero;
                return WinError.E_NOINTERFACE;
            }
            else if (riid == typeof(IContextMenu).GUID)
            {
                ppv = IntPtr.Zero;
                return WinError.E_NOINTERFACE;
            }
            else if (riid == typeof(IExtractIconA).GUID)
            {
                ppv = IntPtr.Zero;
                return WinError.E_NOINTERFACE;
            }
            else if (riid == typeof(IExtractIconW).GUID)
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
            //  IID_ICategoryProvider
            //  IID_IExplorerCommandProvider
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
        int IShellFolder.GetAttributesOf(uint cidl, IntPtr apidl, ref SFGAO rgfInOut)
        {
            //  Get each id list.
            var idlists = PidlManager.APidlToIdListArray(apidl, (int)cidl);

            //  Now we can ask for the attributes of each item. We only ask for attributes that
            //  are set in the flags - clearing them if they don't apply to every item.
            var allItems = idlists.Select(GetChildItem).ToList();
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
        int IShellFolder.GetUIObjectOf(IntPtr hwndOwner, uint cidl, IntPtr apidl, ref Guid riid, uint rgfReserved, out IntPtr ppv)
        {
            //  Get the ID lists from the array of PIDLs provided.
            var idLists = PidlManager.APidlToIdListArray(apidl, (int)cidl);

            if (riid == typeof(IContextMenu).GUID || riid == typeof(IContextMenu2).GUID || riid == typeof(IContextMenu3).GUID)
            {
                //  If the folder implments the context menu provider, we can use that.
                var contextMenuProvider = proxyFolder as IShellNamespaceFolderContextMenuProvider;
                if (contextMenuProvider != null)
                {
                    ppv = Marshal.GetComInterfaceForObject(contextMenuProvider.CreateContextMenu(idListAbsolute, idLists),
                        typeof(IContextMenu));
                    return WinError.S_OK;
                }
                var dcm = new DEFCONTEXTMENU
                {
                    hwnd = hwndOwner,
                    pcmcb = null,
                    pidlFolder = PidlManager.IdListToPidl(idListAbsolute),
                    psf = this,
                    cidl = cidl,
                    apidl = apidl,
                    punkAssociationInfo = IntPtr.Zero,
                    cKeys = 0,
                    aKeys = null
                };

                //  Create the default context menu.
                var result = Shell32.SHCreateDefaultContextMenu(dcm, riid, out ppv);
            }
            else if (riid == Shell32.IID_ExtractIconW)
            {
                //  If we've been asked for an icon, it should only be for a single PIDL.
                if (idLists.Length != 1)
                {
                    Diagnostics.Logging.Error(string.Format("The Shell Folder Impl for folder {0} has been asked for icons for multiple files at once, this is not supportedd.",
                        proxyFolder.GetDisplayName(DisplayNameContext.Normal)));
                    ppv = IntPtr.Zero;
                    return WinError.E_FAIL;
                }

                //  Get the idlist and item.
                var idList = idLists[0];
                var item = GetChildItem(idList);

                //  Now get the icon. If we don't provide one we'll use the defaults.
                var icon = item.GetIcon();
                if (icon == null)
                {
                    ProvideDefaultIExtractIcon(item is IShellNamespaceFolder, out ppv);
                    return WinError.S_OK;
                }
                else
                {
                    //  Create an icon provider.
                    var provider = new Components.ExtractIconImpl() { DoNotCacheIcons = false, Icon = icon };
                    ppv = Marshal.GetComInterfaceForObject(provider, typeof(IExtractIconW));
                    return WinError.S_OK;
                }
            }
            else if (riid == Shell32.IID_IDataObject)
            {
                //  Create the data object.
                Shell32.SHCreateDataObject(PidlManager.IdListToPidl(idListAbsolute), cidl, apidl, null, riid, out ppv);
            }
            else if (riid == Shell32.IID_IQueryAssociations)
            {
                //  If we've been asked for a query associations, it should only be for a single PIDL.
                if (idLists.Length != 1)
                {
                    Diagnostics.Logging.Error(string.Format("The Shell Folder Impl for folder {0} has been asked for query associations for multiple files at once, this is not supportedd.",
                        proxyFolder.GetDisplayName(DisplayNameContext.Normal)));
                    ppv = IntPtr.Zero;
                    return WinError.E_FAIL;
                }
                var item = GetChildItem(idLists[0]);
                var isFolder = item is IShellNamespaceFolder;

                if (isFolder)
                {
                    //  todo perhaps a good class name would simply be the 
                    //  name of the item type? or an attribute that uses the classname as a 
                    //  fallback.
                    var associations = new ASSOCIATIONELEMENT[]
                    {
                        new ASSOCIATIONELEMENT
                            {
                                ac = ASSOCCLASS.ASSOCCLASS_PROGID_STR,
                                hkClass = IntPtr.Zero,
                                pszClass = "FolderViewSampleType"
                            },
                        new ASSOCIATIONELEMENT
                            {
                                ac = ASSOCCLASS.ASSOCCLASS_FOLDER,
                                hkClass = IntPtr.Zero,
                                pszClass = "FolderViewSampleType"
                            }
                    };
                    Shell32.AssocCreateForClasses(associations, (uint)associations.Length, riid, out ppv);

                }
                else
                {
                    var associations = new ASSOCIATIONELEMENT[]
                    {
                        new ASSOCIATIONELEMENT
                            {
                                ac = ASSOCCLASS.ASSOCCLASS_PROGID_STR,
                                hkClass = IntPtr.Zero,
                                pszClass = "FolderViewSampleType"
                            }
                    };
                    Shell32.AssocCreateForClasses(associations, (uint)associations.Length, riid, out ppv);
                }
            }

            /*   } */


            //  We have a set of child pidls (i.e. length one). We can now offer interfaces such as:
            /*
             * IContextMenu	The cidl parameter can be greater than or equal to one.
IContextMenu2	The cidl parameter can be greater than or equal to one.
IDataObject	The cidl parameter can be greater than or equal to one.
IDropTarget	The cidl parameter can only be one.
IExtractIcon	The cidl parameter can only be one.
IQueryInfo	The cidl parameter can only be one.
             * */

            //  IID_IExtractIconW
            //  IID_IDataObject
            //  IID_IQueryAssociations
            //  Currently, we don't offer any extra child item UI objects.
            ppv = IntPtr.Zero;
            return WinError.E_NOINTERFACE;
        }


        /// <summary>
        /// Provides a default IExtractIcon implementation.
        /// </summary>
        /// <param name="folderIcon">if set to <c>true</c> use a folder icon, otherwise use an item icon.</param>
        /// <param name="interfacePointer">The interface pointer.</param>
        private void ProvideDefaultIExtractIcon(bool folderIcon, out IntPtr interfacePointer)
        {
            //  Create a default extract icon init interface.
            IDefaultExtractIconInit pdxi;
            Shell32.SHCreateDefaultExtractIcon(typeof(IDefaultExtractIconInit).GUID, out pdxi);

            //  Set the normal icon.
            pdxi.SetNormalIcon("shell32.dll", folderIcon ? 4 : 1);

            //  Get the IExtractIconW interface.
            interfacePointer = Marshal.GetComInterfaceForObject(pdxi, typeof(IExtractIconW));
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
            //  If we have an invalid PIDL, we must fail.
            if (pidl == IntPtr.Zero)
            {
                pName = new STRRET();
                return WinError.E_INVALIDARG;
            }

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

            pName = STRRET.CreateUnicode(string.Empty);
            return WinError.S_OK;
        }

        /// <summary>
        /// Sets the display name of a file object or subchanging the item
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

        int IShellFolder2.GetAttributesOf(uint cidl, IntPtr apidl, ref SFGAO rgfInOut)
        {
            return ((IShellFolder)this).GetAttributesOf(cidl, apidl, ref rgfInOut);
        }

        int IShellFolder2.GetUIObjectOf(IntPtr hwndOwner, uint cidl, IntPtr apidl, ref Guid riid, uint rgfReserved,
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
            //  TODO: expose this to the API in the future. For now, default column is the first.
            pSort = 0;
            pDisplay = 0;
            return WinError.E_NOTIMPL;
        }

        int IShellFolder2.GetDefaultColumnState(uint iColumn, out SHCOLSTATEF pcsFlags)
        {
            //  TODO: expose this to the API via properties on the column. For now, the default state is text.
            pcsFlags = SHCOLSTATEF.SHCOLSTATE_ONBYDEFAULT | SHCOLSTATEF.SHCOLSTATE_TYPE_STR;

            //  We've successfully set the column state.
            return WinError.S_OK;
        }

        int IShellFolder2.GetDetailsEx(IntPtr pidl, PROPERTYKEY pkey, out object pv)
        {
            //  If we have no pidl, we cannot get details.
            if (pidl == IntPtr.Zero)
            {
                pv = null;
                return WinError.E_INVALIDARG;
            }

            // todo If the item is not a folder and the property key is PKEY_PropList_PreviewDetails
            //  we need to return a string like:
            //  "prop:Microsoft.SDKSample.AreaSize;Microsoft.SDKSample.NumberOfSides;Microsoft.SDKSample.DirectoryLevel");
            //  to indicate what to show in the details pane.

            /*
            if (!pfIsFolder && IsEqualPropertyKey(*pkey, PKEY_PropList_PreviewDetails))
        {
            // This proplist indicates what properties are shown in the details pane at the bottom of the explorer browser.
            pv->vt = VT_BSTR;
            pv->bstrVal = SysAllocString(L"prop:Microsoft.SDKSample.AreaSize;Microsoft.SDKSample.NumberOfSides;Microsoft.SDKSample.DirectoryLevel");
            hr = pv->bstrVal ? S_OK : E_OUTOFMEMORY;
        }*/

            //  Get the detail todo rename
            pv = GetItemColumnValue(pidl, pkey);

            //  We're done.
            return WinError.S_OK;
        }

        int IShellFolder2.GetDetailsOf(IntPtr pidl, uint iColumn, out SHELLDETAILS psd)
        {
            //  Get the folder view columns.
            var columns = ((DefaultNamespaceFolderView)lazyFolderView.Value).Columns;

            //  If details are being requested for a column we don't have, we must fail.
            if (iColumn >= columns.Count)
            {
                psd = new SHELLDETAILS { cxChar = 0, fmt = 0, str = new STRRET { uType = STRRET.STRRETTYPE.STRRET_WSTR, data = IntPtr.Zero } };
                return WinError.E_FAIL;
            }
            
            //  If we have no pidl, we need the details of the column itself.
            if (pidl == IntPtr.Zero)
            {
                var column = columns[(int)iColumn];

                //  Create the column format.
                int format = 0;
                switch (column.ColumnAlignment)
                {
                    case ColumnAlignment.Left:
                        format = (int)LVCFMT.LVCFMT_LEFT;
                        break;
                    case ColumnAlignment.Centre:
                        format = (int)LVCFMT.LVCFMT_CENTER;
                        break;
                    case ColumnAlignment.Right:
                        format = (int)LVCFMT.LVCFMT_RIGHT;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                //  Set the column icon flag (if we have one).
                if (column.HasImage)
                    format |= (int)LVCFMT.LVCFMT_COL_HAS_IMAGES;


                //  TODO I have no idea why the shell details are not correctly respecting the cxChar..


                //  Create shell details for the column.
                psd = new SHELLDETAILS
                {
                    fmt = format,
                    cxChar = column.Name.Length,
                    str = STRRET.CreateUnicode(column.Name)
                };
            }
            else
            {
                //  We've been asked for the details of an item.
                //  Get the column ID.
                PROPERTYKEY propertyKey;
                ((IShellFolder2)this).MapColumnToSCID(iColumn, out propertyKey);

                //  Get the value of an item at a column.
                var valueText = GetItemColumnValue(pidl, propertyKey);
                psd = new SHELLDETAILS
                {
                    fmt = 0, // todo, currently set to 'left'.
                    cxChar = valueText.Length,
                    str = STRRET.CreateUnicode(valueText)
                };
            }

            return WinError.S_OK;
        }

        int IShellFolder2.MapColumnToSCID(uint iColumn, out PROPERTYKEY pscid)
        {
            //  Get the detail columns.
            var columns = ((DefaultNamespaceFolderView)lazyFolderView.Value).Columns;

            //  If we've been asked for a column we don't have, return failure.
            if (iColumn >= columns.Count)
            {
                pscid = new PROPERTYKEY();
                return WinError.E_FAIL;
            }

            //  Get the column property id.
            pscid = columns[(int)iColumn].PropertyKey.CreateShellPropertyKey();

            //  We've mapped the column.
            return WinError.S_OK;
        }

        #endregion

        #region Implementation IPersist, IPersistFolder, IPersistFolder2, IPersistIDList

        /// <summary>
        /// Gets the class identifier.
        /// </summary>
        /// <param name="pClassID">The p class identifier.</param>
        /// <returns></returns>
        int IPersist.GetClassID(out Guid pClassID)
        {
            //  Set the class ID to the server id.
            pClassID = namespaceExtension.ServerClsid;
            return WinError.S_OK;
        }
        int IPersistFolder.GetClassID(out Guid pClassID) { return ((IPersist)this).GetClassID(out pClassID); }
        int IPersistFolder2.GetClassID(out Guid pClassID) { return ((IPersist)this).GetClassID(out pClassID); }
        int IPersistIDList.GetClassID(out Guid pClassID) {return ((IPersist)this).GetClassID(out pClassID); }

        int IPersistFolder.Initialize(IntPtr pidl)
        {
            //  Store the folder absolute pidl.
            idListAbsolute = PidlManager.PidlToIdlist(pidl);
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
            //  Return null if we're not initialised.
            if (idListAbsolute == null)
            {
                ppidl = IntPtr.Zero;
                return WinError.S_FALSE;
            }
            //  Return the pidl.
            ppidl = PidlManager.IdListToPidl(idListAbsolute);
            return WinError.S_OK;
        }

        int IPersistIDList.SetIDList(IntPtr pidl)
        {
            return ((IPersistFolder2) this).Initialize(pidl);
        }

        int IPersistIDList.GetIDList([Out] out IntPtr pidl)
        {
            return ((IPersistFolder2) this).GetCurFolder(out pidl);
        }

        #endregion

        private static IShellNamespaceFolder GetChildFolder(IShellNamespaceFolder folder, ShellId itemId)
        {
            //  Get the item that is represented by the shell id.
            var childFolder = folder
                .GetChildren(ShellNamespaceEnumerationFlags.Folders)
                .OfType<IShellNamespaceFolder>()
                .SingleOrDefault(i => i.GetShellId().Equals(itemId));

            //  If we don't find the item, we've got a problem.
            if (childFolder == null)
            {
                //  TODO how will we handle this error?
                var me = folder.GetDisplayName(DisplayNameContext.Normal);
                var you = itemId.ToString();
                return null;
            }
            return childFolder;
        }

        private IShellNamespaceItem GetChildItem(IdList idList)
        {
            //  Go through each item in the list.
            var currentFolder = proxyFolder;
            for (int depth = 0; depth < idList.Ids.Count; depth++)
            {
                //  If we are NOT on the last item, we're looking for a folder.
                if (depth != idList.Ids.Count - 1)
                {
                    currentFolder = GetChildFolder(currentFolder, idList.Ids[depth]);
                    continue;
                }

                //  We ARE looking for an item, so get it.
                var item =
                    currentFolder
                        .GetChildren(ShellNamespaceEnumerationFlags.Folders | ShellNamespaceEnumerationFlags.Items)
                        .SingleOrDefault(i => i.GetShellId().Equals(idList.Ids[depth]));
                if (item == null)
                {
                    var me = currentFolder.GetDisplayName(DisplayNameContext.Normal);
                    var you = idList.Ids[depth].ToString();
                    return null;
                }
                return item;
            }
            return null;
        }
        private static void UpdateFlagIfSet(ref SFGAO sfgao, SFGAO flag, bool set)
        {
            if (sfgao.HasFlag(flag))
            {
                if (set == false)
                    sfgao ^= flag;
            }
        }

        private string GetItemColumnValue(IntPtr pidl, PROPERTYKEY propertyKey)
        {
            //  Get the value for the property key.
            var item = GetChildItem(PidlManager.PidlToIdlist(pidl));
            var column = ((DefaultNamespaceFolderView)lazyFolderView.Value).Columns.FirstOrDefault(c =>
            {
                var key = c.PropertyKey.CreateShellPropertyKey();
                return key.fmtid == propertyKey.fmtid && key.pid == propertyKey.pid;
            });
            var detail = ((DefaultNamespaceFolderView)lazyFolderView.Value).GetItemDetail(item, column);
            return detail.ToString();
        }

        /// <summary>
        /// The namespace extension that we are either a proxy for or that is that parent of a 
        /// folder we are a proxy for.
        /// </summary>
        private SharpNamespaceExtension namespaceExtension;

        /// <summary>
        /// The shell folder that we are providing an implementation for.
        /// </summary>
        private readonly IShellNamespaceFolder proxyFolder;

        /// <summary>
        /// The lazy folder view. Initialised when required from the IShellNamespaceFolder object.
        /// </summary>
        private readonly Lazy<ShellNamespaceFolderView> lazyFolderView; 

        /// <summary>
        /// The absolute ID list of the folder. This is provided by IPersistFolder.
        /// </summary>
        private IdList idListAbsolute;
    }
}