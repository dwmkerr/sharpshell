using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Informs the browser that the focus has changed.
    /// </summary>
    [ComImport]
    [Guid("f1db8392-7331-11d0-8c99-00a0c92dbfe8")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInputObjectSite
    {
        /// <summary>
        /// Informs the browser that the focus has changed.
        /// </summary>
        /// <param name="punkObj">The address of the IUnknown interface of the object gaining or losing the focus.</param>
        /// <param name="fSetFocus">Indicates if the object has gained or lost the focus. If this value is nonzero, the object has gained the focus. If this value is zero, the object has lost the focus.</param>
        /// <returns></returns>
        [PreserveSig]
        int OnFocusChangeIS(
            [MarshalAs(UnmanagedType.IUnknown)] object punkObj,
            [MarshalAs(UnmanagedType.Bool)] bool fSetFocus);
    }
}