using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace SharpShell.Interop
{
    /// <summary>
    /// Ole32 imports.
    /// </summary>
    public static class Ole32
    {
        /// <summary>
        /// Releases the STG medium.
        /// </summary>
        /// <param name="pmedium">The pmedium.</param>
        [DllImport("ole32.dll")]
        public static extern void ReleaseStgMedium([In] ref STGMEDIUM pmedium);

        /// <summary>
        /// Coes the create instance.
        /// </summary>
        /// <param name="rclsid">The rclsid.</param>
        /// <param name="pUnkOuter">The p unk outer.</param>
        /// <param name="dwClsContext">The dw CLS context.</param>
        /// <param name="riid">The riid.</param>
        /// <param name="rReturnedComObject">The r returned COM object.</param>
        /// <returns></returns>
        [DllImport("ole32.dll", ExactSpelling = true, PreserveSig = false)]
        [return: MarshalAs(UnmanagedType.Interface)]
        public static extern object CoCreateInstance(
           [In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid,
           [MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter,
           CLSCTX dwClsContext,
           [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid,
           [MarshalAs(UnmanagedType.IUnknown)] out object rReturnedComObject);
    }
}
