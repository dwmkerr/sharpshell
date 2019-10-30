using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Exposes methods that retrieve information about a Shell item. <see cref="IShellItem"/> and IShellItem2 are the preferred representations of items in any new code.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
    public interface IShellItem
    {
        /// <summary>Binds to a handler for an item as specified by the handler ID value (BHID).</summary>
        /// <param name="pbc">
        /// A pointer to an IBindCtx interface on a bind context object. Used to pass optional parameters to the handler. The contents of the bind context are handler-specific. For example, when binding to BHID_Stream, the STGM flags in the bind context indicate the mode of access desired (read or read/write).
        /// </param>
        /// <param name="bhid">Reference to a GUID that specifies which handler will be created. One of the following values defined in Shlguid.h:</param>
        /// <param name="riid">IID of the object type to retrieve.</param>
        /// <param name="ppv">When this method returns, contains a pointer of type riid that is returned by the handler specified by rbhid.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int BindToHandler(IntPtr pbc,
            [MarshalAs(UnmanagedType.LPStruct)]Guid bhid,
            [MarshalAs(UnmanagedType.LPStruct)]Guid riid,
            out IntPtr ppv);

        /// <summary>Gets the parent of an IShellItem object.</summary>
        /// <param name="ppsi">The address of a pointer to the parent of an IShellItem interface.</param>
        /// <returns>Returns S_OK if successful, or an error value otherwise.</returns>
        int GetParent(out IShellItem ppsi);

        /// <summary>Gets the display name of the IShellItem object.</summary>
        /// <param name="sigdnName">One of the SIGDN values that indicates how the name should look.</param>
        /// <param name="ppszName">A value that, when this function returns successfully, receives the address of a pointer to the retrieved display name.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        int GetDisplayName(SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);

        /// <summary>Gets a requested set of attributes of the IShellItem object.</summary>
        /// <param name="sfgaoMask">Specifies the attributes to retrieve. One or more of the SFGAO values. Use a bitwise OR operator to determine the attributes to retrieve.</param>
        /// <param name="psfgaoAttribs">A pointer to a value that, when this method returns successfully, contains the requested attributes. One or more of the SFGAO values. Only those attributes specified by sfgaoMask are returned; other attribute values are undefined.</param>
        /// <returns>Returns S_OK if the attributes returned exactly match those requested in sfgaoMask, S_FALSE if the attributes do not exactly match, or a standard COM error value otherwise.</returns>
        int GetAttributes(uint sfgaoMask, out uint psfgaoAttribs);

        /// <summary>Compares two IShellItem objects.</summary>
        /// <param name="psi">A pointer to an IShellItem object to compare with the existing IShellItem object.</param>
        /// <param name="hint">One of the SICHINTF values that determines how to perform the comparison. See SICHINTF for the list of possible values for this parameter.</param>
        /// <param name="piOrder">This parameter receives the result of the comparison. If the two items are the same this parameter equals zero; if they are different the parameter is nonzero.</param>
        /// <returns>Returns S_OK if the items are the same, S_FALSE if they are different, or an error value otherwise.</returns>
        int Compare(IShellItem psi, uint hint, out int piOrder);
    }
}