using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using SharpShell.Interop;
using SharpShell.Pidl;

namespace SharpShell.SharpNamespaceExtension
{
    internal class ShellFolderImpl
    {
        private IShellNamespaceFolder folder;
        private readonly Lazy<ShellNamespaceFolderView> lazyView;
        private readonly Guid serverGuid;

        private ShellNamespaceFolderView folderView
        {
            get { return lazyView.Value; }
        }

        public ShellFolderImpl(IShellNamespaceFolder folder, Guid serverGuid)
        {
            this.folder = folder;
            this.lazyView = new Lazy<ShellNamespaceFolderView>(() => folder.GetView());
            this.serverGuid = serverGuid;
        }

        internal int ParseDisplayName(IntPtr hwnd, IntPtr pbc, string pszDisplayName, ref uint pchEaten, out IntPtr ppidl,
            ref SFGAO pdwAttributes)
        {
            //  First we can decode the pidl from the display name.
            var idList = IdList.FromParsingString(pszDisplayName);
            ppidl = PidlManager.IdListToPidl(idList);

            //  We always eat the entire display string for SharpShell PIDL/DisplayName parsing.
            pchEaten = (uint) pszDisplayName.Length;

            if (pdwAttributes != 0)
            {
                int i = 0;
            }

            //  In theory, we should understand the pidl.
            var item = GetChildItem(idList);
            var name = item.GetDisplayName(DisplayNameContext.Normal);

            //  TODO: We may be asked to get the attributes at the same time. If so, we must set them here.
            return WinError.S_OK;
        }

        internal int EnumObjects(IntPtr hwnd, SHCONTF grfFlags, out IEnumIDList ppenumIdList)
        {
            //  Create an object that will enumerate the contents of this shell folder (that implements
            //  IEnumIdList). This can be returned to the shell.
            ppenumIdList = new ShellNamespaceFolderIdListEnumerator(folder, grfFlags, 0);

            //  TODO we should also store the window handle for user interaction.

            //  We're done.
            return WinError.S_OK;
        }

        internal int BindToObject(IntPtr pidl, IntPtr pbc, ref Guid riid, out IntPtr ppv)
        {
            var name = string.Join(Environment.NewLine, PidlManager.PidlToIdlist(pidl).Ids.Select(id => id.ToString()));

            //  Have we been asked to bind to a folder?
            if (riid == typeof(IShellFolder).GUID || riid == typeof(IShellFolder2).GUID)
            {
                //  Get the child item.
                var childItem = GetChildItem(PidlManager.PidlToIdlist(pidl));

                //  If the item is a folder, we can create a proxy for it and return the proxy.
                var subFolder = childItem as IShellNamespaceFolder;
                if (subFolder != null)
                {
                    var folderProxy = new ShellFolderProxy(subFolder, serverGuid);
                    ppv = Marshal.GetComInterfaceForObject(folderProxy, typeof(IShellFolder2));
                    return WinError.S_OK;
                }
            }

            //  Note: We are also asked to bind to IPropertyStore IPropertyStoreFactory and IPropertyStoreCache.

            //  If we cannot return the required interface, we must return no interface.
            ppv = IntPtr.Zero;
            return WinError.E_NOINTERFACE;
        }

        internal int BindToStorage(IntPtr pidl, IntPtr pbc, ref Guid riid, out IntPtr ppv)
        {
            //  TODO: this will need to be implemented at some stage.
            ppv = IntPtr.Zero;
            return WinError.E_NOTIMPL;
        }

        internal int CompareIDs(IntPtr lParam, IntPtr pidl1, IntPtr pidl2)
        {
            //  Get the low short from the lParam, this is the sorting option.
            short sortingRule = (short)(lParam.ToInt64() & 0x000000FF);
            SCHIDS modifiers = (SCHIDS)((lParam.ToInt64() >> 16) & 0x000000FF);

            //  TODO: build an HRESULT with a CODE that contains a negative/positive/zero indicator.

            return WinError.S_OK;
        }

        internal int CreateViewObject(IShellFolder shellFolder, IntPtr hwndOwner, ref Guid riid, out IntPtr ppv)
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
                    var shellFolderView = folderView.CreateShellView(shellFolder);
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

        internal int GetAttributesOf(uint cidl, IntPtr apidl, ref SFGAO rgfInOut)
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

        internal int GetUIObjectOf(IntPtr hwndOwner, uint cidl, IntPtr apidl, ref Guid riid, uint rgfReserved, out IntPtr ppv)
        {
            //  Get the ID lists from the array of PIDLs provided.
            var idLists = PidlManager.APidlToIdListArray(apidl, (int) cidl);

            if (riid == typeof(IExtractIconW).GUID)
            {
                //  If we've been asked for an icon, it should only be for a single PIDL.
                if(idLists.Length != 1)
                {
                    Diagnostics.Logging.Error(string.Format("The Shell Folder Impl for folder {0} has been asked for icons for multiple files at once, this is not supportedd.", 
                        folder.GetDisplayName(DisplayNameContext.Normal)));
                    ppv = IntPtr.Zero;
                    return WinError.E_FAIL;
                }

                //  Get the idlist and item.
                var idList = idLists[0];
                var item = GetChildItem(idList);

                //  Now get the icon. If we don't provide one we'll use the defaults.
                var icon = item.GetIcon();
                if(icon == null)
                {
                    ProvideDefaultIExtractIcon(item is IShellFolder, out ppv);
                    return WinError.S_OK;
                }
            }


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
            interfacePointer = Marshal.GetComInterfaceForObject(pdxi, typeof (IExtractIconW));
        }

        internal int GetDisplayNameOf(IntPtr pidl, SHGDNF uFlags, out STRRET pName)
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

        internal int SetNameOf(IntPtr hwnd, IntPtr pidl, string pszName, SHGDNF uFlags, out IntPtr ppidlOut)
        {
            //  TODO this needs to be implemented.
            ppidlOut = IntPtr.Zero;
            return WinError.E_NOTIMPL;
        }

        internal int GetDefaultSearchGUID(out Guid pguid)
        {
            pguid = Guid.Empty;
            return WinError.E_NOTIMPL;
        }

        internal int EnumSearches(out IEnumExtraSearch ppenum)
        {
            ppenum = null;
            return WinError.E_NOTIMPL;
        }

        internal int GetDefaultColumn(uint dwRes, out uint pSort, out uint pDisplay)
        {
            //  TODO: expose this to the API in the future. For now, default column is the first.
            pSort = 0;
            pDisplay = 0;
            return WinError.E_NOTIMPL;
        }

        internal int GetDefaultColumnState(uint iColumn, out SHCOLSTATEF pcsFlags)
        {
            //  TODO: expose this to the API via properties on the column. For now, the default state is text.
            pcsFlags = SHCOLSTATEF.SHCOLSTATE_ONBYDEFAULT | SHCOLSTATEF.SHCOLSTATE_TYPE_STR;
            
            //  We've successfully set the column state.
            return WinError.S_OK;
        }

        internal int GetDetailsEx(IntPtr pidl, PROPERTYKEY pkey, out object pv)
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

        internal int GetDetailsOf(IntPtr pidl, uint iColumn, out SHELLDETAILS psd)
        {
            //  Get the folder view columns.
            var columns = ((DefaultNamespaceFolderView)folderView).Columns;

            //  If details are being requested for a column we don't have, we must fail.
            if (iColumn >= columns.Count)
            {
                psd = new SHELLDETAILS { cxChar = 0, fmt = 0, str = new STRRET { uType = STRRET.STRRETTYPE.STRRET_WSTR, data = IntPtr.Zero}};
                return WinError.E_FAIL;
            }

            //  Whatever shell details we create we store here.
            SHELLDETAILS shellDetails;

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
                MapColumnToSCID(iColumn, out propertyKey);

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

        private string GetItemColumnValue(IntPtr pidl, PROPERTYKEY propertyKey)
        {
            //  Get the value for the property key.
            var item = GetChildItem(PidlManager.PidlToIdlist(pidl));
            var column = ((DefaultNamespaceFolderView) folderView).Columns.FirstOrDefault(c =>
                {
                    var key = c.PropertyKey.CreateShellPropertyKey();
                    return key.fmtid == propertyKey.fmtid && key.pid == propertyKey.pid;
                });
            var detail = ((DefaultNamespaceFolderView) folderView).GetItemDetail(item, column);
            return detail.ToString();
        }

        internal int MapColumnToSCID(uint iColumn, out PROPERTYKEY pscid)
        {
            //  Get the detail columns.
            var columns = ((DefaultNamespaceFolderView) folderView).Columns;
            
            //  If we've been asked for a column we don't have, return failure.
            if (iColumn >= columns.Count)
            {
                pscid = new PROPERTYKEY();
                return WinError.E_FAIL;
            }

            //  Get the column property id.
            pscid = columns[(int) iColumn].PropertyKey.CreateShellPropertyKey();

            //  We've mapped the column.
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

        private IShellNamespaceItem GetChildItem(IdList idList)
        {
            //  TODO: Get child item can actually be asked for a deeper child item, if there
            //  are multiple items in the ID list.

            //  Go through each item in the list.
            var currentFolder = folder;
            for (int depth = 0; depth < idList.Ids.Count; depth++)
            {
                //  If we are NOT on the last item, we're looking for a folder.
                if (depth != idList.Ids.Count - 1)
                {
                    currentFolder = GetChildFolder(folder, idList.Ids[depth]);
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
    }
}