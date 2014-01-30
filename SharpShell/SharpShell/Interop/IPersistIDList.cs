using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Exposes methods that are used to persist item identifier lists.
    /// </summary>
    [ComImport, Guid("1079acfc-29bd-11d3-8e0d-00c04f6837d5"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPersistIDList : IPersist
    {
        #region Overriden IPersist Methods
        
        new int GetClassID(out Guid pClassID);

        #endregion

        /// <summary>
        /// Sets a persisted item identifier list.
        /// </summary>
        /// <param name="pidl">A pointer to the item identifier list to set.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        int SetIDList([In] IntPtr pidl);

        /// <summary>
        /// Gets an item identifier list.
        /// </summary>
        /// <param name="pidl">The address of a pointer to the item identifier list to get.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        int GetIDList([Out] out IntPtr pidl);
    }
}