using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Exposes a method to initialize a handler, such as a property handler, thumbnail handler, or preview handler, with a file path.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("b7d14566-0509-4cce-a71f-0a554233bd9b")]
    public interface IInitializeWithFile
    {
        /// <summary>
        /// Initializes a handler with a file path.
        /// </summary>
        /// <param name="pszFilePath">A pointer to a buffer that contains the file path as a null-terminated Unicode string.</param>
        /// <param name="grfMode">One of the following STGM values that indicates the access mode for pszFilePath. STGM_READ or STGM_READWRITE.</param>
        [PreserveSig]
        int Initialize([MarshalAs(UnmanagedType.LPWStr)] string pszFilePath, STGM grfMode);
    }
}