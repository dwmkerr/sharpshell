using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using SharpShell.Interop;
using SharpShell.Pidl;

namespace SharpShell.SharpNamespaceExtension
{
    internal static class ShellFolderImpl
    {
        internal static int ParseDisplayName(IShellNamespaceFolder folder, IntPtr hwnd, IntPtr pbc, string pszDisplayName, ref uint pchEaten, out IntPtr ppidl,
            ref SFGAO pdwAttributes)
        {
            //  First we can decode the pidl from the display name.
            ppidl = PidlManager.IdListToPidl(IdList.FromParsingString(pszDisplayName));

            //  We always eat the entire display string for SharpShell PIDL/DisplayName parsing.
            pchEaten = (uint) pszDisplayName.Length;

            //  TODO: We may be asked to get the attributes at the same time. If so, we must set them here.
            return WinError.S_OK;
        }

        internal static int EnumObjects(IShellNamespaceFolder folder, IntPtr hwnd, SHCONTF grfFlags, out IEnumIDList ppenumIdList)
        {
            //  Create an object that will enumerate the contents of this shell folder (that implements
            //  IEnumIdList). This can be returned to the shell.
            ppenumIdList = new ShellNamespaceFolderIdListEnumerator(folder, grfFlags, 0);

            //  TODO we should also store the window handle for user interaction.

            //  We're done.
            return WinError.S_OK;
        }

        internal static int BindToObject(IShellNamespaceFolder folder, IntPtr pidl, IntPtr pbc, ref Guid riid, out IntPtr ppv)
        {
            //  Have we been asked to bind to a folder?
            if (riid == typeof(IShellFolder).GUID || riid == typeof(IShellFolder2).GUID)
            {
                //  Get the child item.
                var childItem = GetChildItem(folder, PidlManager.PidlToIdlist(pidl));

                //  If the item is a folder, we can create a proxy for it and return the proxy.
                var subFolder = childItem as IShellNamespaceFolder;
                if (subFolder != null)
                {
                    var folderProxy = new ShellFolderProxy(subFolder);
                    ppv = Marshal.GetComInterfaceForObject(folderProxy, typeof(IShellFolder2));
                    return WinError.S_OK;
                }
            }

            //  If we cannot return the required interface, we must return no interface.
            ppv = IntPtr.Zero;
            return WinError.E_NOINTERFACE;
        }

        internal static int BindToStorage(IShellNamespaceFolder folder, IntPtr pidl, IntPtr pbc, ref Guid riid, out IntPtr ppv)
        {
            //  TODO: this will need to be implemented at some stage.
            ppv = IntPtr.Zero;
            return WinError.E_NOTIMPL;
        }

        internal static int CompareIDs(IShellNamespaceFolder folder, IntPtr lParam, IntPtr pidl1, IntPtr pidl2)
        {
            //  Get the low short from the lParam, this is the sorting option.
            short sortingRule = (short)(lParam.ToInt64() & 0x000000FF);
            SCHIDS modifiers = (SCHIDS)((lParam.ToInt64() >> 16) & 0x000000FF);

            //  TODO: build an HRESULT with a CODE that contains a negative/positive/zero indicator.

            return WinError.S_OK;
        }

        internal static int CreateViewObject(IShellNamespaceFolder folder, IShellFolder shellFolder, IntPtr hwndOwner, ref Guid riid, out IntPtr ppv)
        {
            //  Before the contents of the folder are displayed, the shell asks for an IShellView.
            //  This function is also called to get other shell interfaces for interacting with the
            //  folder itself.

            if (riid == typeof(IShellView).GUID)
            {
                //  Create the folder view.
                var folderView = folder.GetView();

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

        internal static int GetAttributesOf(IShellNamespaceFolder folder, uint cidl, IntPtr[] apidl, ref SFGAO rgfInOut)
        {
            //  Get each pidl as an idlist.
            var idlists = (cidl > 0 && apidl != null)
                ? from pidl in apidl select PidlManager.PidlToIdlist(pidl)
                : new List<IdList>();

            //  Now we can ask for the attributes of each item. We only ask for attributes that
            //  are set in the flags - clearing them if they don't apply to every item.
            var allItems = idlists.Select(idl => GetChildItem(folder, idl)).ToList();
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

        internal static int GetUIObjectOf(IShellNamespaceFolder folder, IntPtr hwndOwner, uint cidl, IntPtr[] apidl, ref Guid riid, uint rgfReserved, out IntPtr ppv)
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

        internal static int GetDisplayNameOf(IShellNamespaceFolder folder, IntPtr pidl, SHGDNF uFlags, out STRRET pName)
        {
            //  Create an idlist from the pidl.
            var idlist = PidlManager.PidlToIdlist(pidl);

            //  Get the shell item.
            //  TODO; handle errors
            var shellItem = GetChildItem(folder, idlist);

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

        internal static int SetNameOf(IShellNamespaceFolder folder, IntPtr hwnd, IntPtr pidl, string pszName, SHGDNF uFlags, out IntPtr ppidlOut)
        {
            //  TODO this needs to be implemented.
            ppidlOut = IntPtr.Zero;
            return WinError.E_NOTIMPL;
        }

        internal static int GetDefaultSearchGUID(IShellNamespaceFolder folder, out Guid pguid)
        {
            pguid = Guid.Empty;
            return WinError.E_NOTIMPL;
        }

        internal static int EnumSearches(IShellNamespaceFolder folder, out IEnumExtraSearch ppenum)
        {
            ppenum = null;
            return WinError.E_NOTIMPL;
        }

        internal static int GetDefaultColumn(IShellNamespaceFolder folder, uint dwRes, out uint pSort, out uint pDisplay)
        {
            //  TODO: expose this to the API in the future. For now, default column is the first.
            pSort = 0;
            pDisplay = 0;
            return WinError.S_OK;
        }

        internal static int GetDefaultColumnState(IShellNamespaceFolder folder, uint iColumn, out SHCOLSTATEF pcsFlags)
        {
            //  TODO: expose this to the API via properties on the column. For now, the default state is text.
            pcsFlags = SHCOLSTATEF.SHCOLSTATE_TYPE_STR;
            
            //  We've successfully set the column state.
            return WinError.S_OK;
        }

        internal static int GetDetailsEx(IShellNamespaceFolder folder, IntPtr pidl, SHCOLUMNID pscid, out object pv)
        {
            //  Get the column.
            var detailView = (DefaultNamespaceFolderView) folder.GetView();
            var column = detailView.Columns.Single(c => c.UniqueId == pscid.fmtid);

            //  Get the item.
            var item = GetChildItem(folder, PidlManager.PidlToIdlist(pidl));

            //  Return the detail for the item.
            var detail = detailView.GetItemDetail(item, column);

            //  Todo marshal variant
            pv = IntPtr.Zero;
            return WinError.E_NOTIMPL;
        }

        internal static int GetDetailsOf(IShellNamespaceFolder folder, IntPtr pidl, uint iColumn, out IntPtr psd)
        {
            //  Get the folder view columns.
            var columns = ((DefaultNamespaceFolderView) folder.GetView()).Columns;

            //  If details are being requested for a column we don't have, we must fail.
            if (iColumn >= columns.Count)
            {
                psd = IntPtr.Zero;
                return WinError.E_FAIL;
            }

            //  Create shell details for the column.
            var column = columns[(int) iColumn];
            var details = new SHELLDETAILS
            {
                fmt = 0, // todo, currently set to 'left'.
                cxChar = column.Name.Length,
                str = STRRET.CreateUnicode(column.Name)
            };

            //  Allocate enough space for the structure and copy it to the allocated memory.
            var buffer = Marshal.AllocHGlobal(Marshal.SizeOf(details));
            Marshal.StructureToPtr(details, buffer, false);
            psd = buffer;

            //  Return the pointer to the buffer and we're done.
            psd = buffer;
            return WinError.S_OK;
        }

        internal static int MapColumnToSCID(IShellNamespaceFolder folder, uint iColumn, out SHCOLUMNID pscid)
        {
            //  TODO: see http://msdn.microsoft.com/en-us/library/windows/desktop/bb759748(v=vs.85).aspx
            //  TODO: see http://msdn.microsoft.com/en-us/library/windows/desktop/bb773381(v=vs.85).aspx

            //  Get the column.
            var column = ((DefaultNamespaceFolderView)folder.GetView()).Columns[(int)iColumn];

            //  TODO: there are 'default' columns such as 'Name' we can map to, see notes above.
            //  Set the column id for it.
            pscid = new SHCOLUMNID
            {
                fmtid = column.UniqueId,
                pid = 2
            };

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

        private static IShellNamespaceItem GetChildItem(IShellNamespaceFolder folder, IdList idList)
        {
            return folder
                .GetChildren(ShellNamespaceEnumerationFlags.Folders | ShellNamespaceEnumerationFlags.Items)
                .SingleOrDefault(i => idList.Matches(i.GetShellId()));

        }
    }
}