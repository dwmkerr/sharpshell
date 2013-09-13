using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Exposes methods that the Shell uses to retrieve flags and info tip information 
    /// for an item that resides in an IShellFolder implementation. Info tips are usually
    /// displayed inside a tooltip control.
    /// </summary>
    [ComImport]
    [Guid("00021500-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IQueryInfo
    {
        /// <summary>
        /// Gets the info tip text for an item.
        /// </summary>
        /// <param name="dwFlags">Flags that direct the handling of the item from which you're retrieving the info tip text. This value is commonly zero.</param>
        /// <param name="ppwszTip">he address of a Unicode string pointer that, when this method returns successfully, receives the tip string pointer. Applications that implement this method must allocate memory for ppwszTip by calling CoTaskMemAlloc.
        /// Calling applications must call CoTaskMemFree to free the memory when it is no longer needed.</param>
        /// <returns>Returns S_OK if the function succeeds. If no info tip text is available, ppwszTip is set to NULL. Otherwise, returns a COM-defined error value.</returns>
        [PreserveSig]
        int GetInfoTip(QITIPF dwFlags, [MarshalAs(UnmanagedType.LPWStr)] out string ppwszTip);

        /// <summary>
        /// Gets the information flags for an item. This method is not currently used.
        /// </summary>
        /// <param name="pdwFlags">A pointer to a value that receives the flags for the item. If no flags are to be returned, this value should be set to zero.</param>
        /// <returns>Returns S_OK if pdwFlags returns any flag values, or a COM-defined error value otherwise.</returns>
        [PreserveSig]
        int GetInfoFlags(out int pdwFlags);
    }
}