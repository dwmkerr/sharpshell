using System;
using System.Collections.Generic;
using System.Linq;
using SharpShell.Interop;
using SharpShell.Pidl;

namespace SharpShell.SharpNamespaceExtension
{
    /// <summary>
    /// Internally used class to enumerate the contents of a Sharp Namespace Extension folder.
    /// </summary>
    internal class SharpNamespaceExtensionIdListEnumerator : IEnumIDList
    {
        public SharpNamespaceExtensionIdListEnumerator(SharpNamespaceExtension sharpNamespaceExtension, SHCONTF grfFlags, uint index)
        {
            //  todo: The flags should be a type in the sharpshell domain, not the shell.
            //  Store the extension for the folder we're enuerating.
            this.namespaceExtension = sharpNamespaceExtension;
            this.flags = grfFlags;
            this.currentIndex = index;
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
        public int Next(uint celt, out IntPtr[] rgelt, out uint pceltFetched)
        {
            //  Request the children from the extension. As this is an abstract call, we always 
            //  use an exception handler.
            var items = new List<IShellNamespaceItem>();
            try
            {
                //  Enumerate the children, adding them to the items collection and moving the index forwards
                //  by the number of items we've enumerated.
                items.AddRange(namespaceExtension.EnumerateChildren(currentIndex, celt, new EnumerateChildrenFlags()));
                currentIndex += (uint)items.Count;
            }
            catch (Exception exception)
            {
                //  Log the exception, but continue as if we've enumerated nothing.
                Diagnostics.Logging.Error(string.Format("An unhandled exception occured enumerating {0} items from the {1} namespace extension.", celt, namespaceExtension.DisplayName),
                    exception);
            }

            //  If we've not enumerated anything, we can return now.
            if (items.Any() == false)
            {
                rgelt = null;
                pceltFetched = 0;
                return WinError.S_OK;
            }

            //  For every item enumerated, use the PIDL manager to create a shell allocated PIDL.
            //  These PIDLs must not be relative, so using the value returned by the GetUniqueId
            //  function is enough.
            var pidlArray = items.Select(
                iid => PidlManager.IdListToPidl(IdList.Create(IdListType.Relative, new List<byte[]> { iid.GetUniqueId()} ))).ToArray();

            //  We can now return the pidl array.
            rgelt = pidlArray;
            pceltFetched = (uint)items.Count;

            //  We're done.
            return WinError.S_OK;
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
            ppenum = new SharpNamespaceExtensionIdListEnumerator(namespaceExtension, flags, currentIndex);

            //  We're done.
            return WinError.S_OK;
        }

        /// <summary>
        /// The namespace extension that we're enumerating.
        /// </summary>
        private readonly SharpNamespaceExtension namespaceExtension;

        /// <summary>
        /// The enumeration flags.
        /// </summary>
        private readonly SHCONTF flags;

        /// <summary>
        /// The current index of the enumerator.
        /// </summary>
        private uint currentIndex;
    }
}