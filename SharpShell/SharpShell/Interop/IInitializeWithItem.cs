using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Initializes a handler with an IShellItem.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("7f73be3f-fb79-493c-a6c7-7ee14e245841")]
    public interface IInitializeWithItem
    {
        /// <summary>
        /// Exposes a method used to initialize a handler, such as a property handler, thumbnail handler, or preview handler, with an <see cref="IShellItem"/>.
        /// </summary>
        /// <param name="shellItem">A pointer to an <see cref="IShellItem"/>.</param>
        /// <param name="accessMode">One of the following <see cref="STGM"/> values that indicate the access mode for psi.
        /// STGM_READ: The IShellItem is read-only.
        /// STGM_READWRITE: The IShellItem is read/write accessible.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int Initialize(IShellItem shellItem, STGM accessMode);
    }
}