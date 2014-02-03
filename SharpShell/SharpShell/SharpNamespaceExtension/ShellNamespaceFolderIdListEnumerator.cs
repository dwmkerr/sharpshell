using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using SharpShell.Interop;
using SharpShell.Pidl;

namespace SharpShell.SharpNamespaceExtension
{
    /// <summary>
    /// Internally used class to enumerate the contents of a Sharp Namespace Extension folder.
    /// </summary>
    internal class ShellNamespaceFolderIdListEnumerator : IEnumIDList
    {
        public ShellNamespaceFolderIdListEnumerator(IShellNamespaceFolder shellNamespaceFolder, SHCONTF grfFlags, uint index)
        {
            //  todo: The flags should be a type in the sharpshell domain, not the shell.
            //  todo the flags might change how we have to do this.
            //  Store the extension for the folder we're enuerating.
            this.shellNamespaceFolder = shellNamespaceFolder;

            this.currentIndex = index;
            this.flags = grfFlags;
            //  Map the flags.
            //  TODO: more to be done here.
            _shellNamespaceEnumerationFlags = 0;
            if (grfFlags.HasFlag(SHCONTF.SHCONTF_FOLDERS))
                _shellNamespaceEnumerationFlags |= ShellNamespaceEnumerationFlags.Folders;
            if (grfFlags.HasFlag(SHCONTF.SHCONTF_NONFOLDERS))
                _shellNamespaceEnumerationFlags |= ShellNamespaceEnumerationFlags.Items;
        }

        /// <summary>
        /// Retrieves the specified number of item identifiers in the
        /// enumeration sequence and advances the current position by
        /// the number of items retrieved.
        /// </summary>
        /// <param name="celt">Number of elements in the array pointed to by the rgelt parameter.</param>
        /// <param name="rgelt">Address of an array of ITEMIDLIST pointers that receives the item identifiers. The implementation must allocate these item identifiers
        /// using the Shell's allocator (retrieved by the SHGetMalloc function). The calling application is responsible for freeing the item
        /// identifiers using the Shell's allocator.</param>
        /// <param name="pceltFetched">Address of a value that receives a count of the item identifiers actually returned in rgelt. The count can be smaller than the value
        /// specified in the celt parameter. This parameter can be NULL only if celt is one.</param>
        /// <returns></returns>
        public int Next(uint celt, IntPtr rgelt, out uint pceltFetched)
        {
            //  Request the children from the extension. As this is an abstract call, we always 
            //  use an exception handler.
            var items = new List<IShellNamespaceItem>();
            try
            {
                //  TODO: We may want to improve on the public api here so that we don't have to enumerate
                //  everything to get certain items.

                //  Enumerate the children, adding them to the items collection and moving the index forwards
                //  by the number of items we've enumerated.
                items.AddRange(
                    shellNamespaceFolder
                        .GetChildren(_shellNamespaceEnumerationFlags)
                        .Skip((int)currentIndex)
                        .Take((int)celt));
                currentIndex += (uint)items.Count;
            }
            catch (Exception exception)
            {
                //  Log the exception, but continue as if we've enumerated nothing.
                Diagnostics.Logging.Error(string.Format("An unhandled exception occured enumerating {0} items from the {1} namespace extension.", celt, shellNamespaceFolder.GetDisplayName(DisplayNameContext.Normal)),
                    exception);
            }

            //  If we've not enumerated anything, we can return now.
            if (items.Any() == false && celt > 0)
            {
                pceltFetched = 0;
                return WinError.S_FALSE;
            }

            //  For every item enumerated, use the PIDL manager to create a shell allocated PIDL.
            //  These PIDLs must not be relative, so using the value returned by the GetShellId
            //  function is enough.
            var pidlArray = items.Select(
                iid => PidlManager.IdListToPidl(IdList.Create(new List<ShellId> { iid.GetShellId()} ))).ToArray();

            //  Copy the data to the provided array.
            Marshal.Copy(pidlArray, 0, rgelt, pidlArray.Length);
            pceltFetched = (uint)items.Count;

            //  We're done. We return OK if we've got more to enumerate and false otherwise.
            return pceltFetched == celt ? WinError.S_OK : WinError.S_FALSE;
        }

        /// <summary>
        /// Skips over the specified number of elements in the enumeration sequence.
        /// </summary>
        /// <param name="celt">Number of item identifiers to skip.</param>
        public int Skip(uint celt)
        {
            //  Move the index forwards.
            currentIndex += celt;

            //  We're done.
            return WinError.S_OK;
        }

        /// <summary>
        /// Returns to the beginning of the enumeration sequence.
        /// </summary>
        public int Reset()
        {
            //  Move the enumeration index to the beginning.
            currentIndex = 0;

            //  We're done.
            return WinError.S_OK;
        }

        /// <summary>
        /// Creates a new item enumeration object with the same contents and state as the current one.
        /// </summary>
        /// <param name="ppenum">Address of a pointer to the new enumeration object. The calling application must
        /// eventually free the new object by calling its Release member function.</param>
        public int Clone(out IEnumIDList ppenum)
        {
            //  Create a brand new enumerator, pointing at the same object
            //  and move it's index to the same position.
            ppenum = new ShellNamespaceFolderIdListEnumerator(shellNamespaceFolder, flags, currentIndex);

            //  We're done.
            return WinError.S_OK;
        }

        /// <summary>
        /// The namespace extension that we're enumerating.
        /// </summary>
        private readonly IShellNamespaceFolder shellNamespaceFolder;

        /// <summary>
        /// The enumeration flags.
        /// </summary>
        private readonly ShellNamespaceEnumerationFlags _shellNamespaceEnumerationFlags;

        /// <summary>
        /// The current index of the enumerator.
        /// </summary>
        private uint currentIndex;

        private readonly SHCONTF flags;
    }
}