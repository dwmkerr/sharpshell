using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Provides a simple way to support communication between an object and its site in the container.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("FC4801A3-2BA9-11CF-A229-00AA003D7352")]
    public interface IObjectWithSite
    {
        /// <summary>
        /// Retrieves the latest site passed using SetSite.
        /// </summary>
        /// <param name="riid">The IID of the interface pointer that should be returned in ppvSite.</param>
        /// <param name="ppvSite">Address of pointer variable that receives the interface pointer requested in riid. Upon successful return, *ppvSite contains the requested interface pointer to the site last seen in SetSite. The specific interface returned depends on the riid argument—in essence, the two arguments act identically to those in QueryInterface. If the appropriate interface pointer is available, the object must call AddRef on that pointer before returning successfully. If no site is available, or the requested interface is not supported, this method must *ppvSite to NULL and return a failure code.</param>
        /// <returns>This method returns S_OK on success.</returns>
        [PreserveSig]
        int GetSite(ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppvSite);

        /// <summary>
        /// Enables a container to pass an object a pointer to the interface for its site.
        /// </summary>
        /// <param name="pUnkSite">A pointer to the IUnknown interface pointer of the site managing this object. If NULL, the object should call Release on any existing site at which point the object no longer knows its site.</param>
        /// <returns>This method returns S_OK on success.</returns>
        [PreserveSig]
        int SetSite([MarshalAs(UnmanagedType.IUnknown)] object pUnkSite);
    }

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