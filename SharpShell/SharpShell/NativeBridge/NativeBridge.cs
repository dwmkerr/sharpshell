using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using SharpShell.Diagnostics;
using SharpShell.Interop;

namespace SharpShell.NativeBridge
{
    /// <summary>
    /// The NativeBridge is an object that wraps the functionality of the SharpShellNativeBridge
    /// library. It also automatically extracts the DLL from the manifest resources.
    /// </summary>
    public class NativeBridge
    {
        /// <summary>
        /// Initialises the Native Bridge.
        /// </summary>
        /// <returns>True if the initialisation succeeded, otherwise false.</returns>
        public bool Initialise()
        {
            //  Get the manifest resource name.
            var resouceName = GetBridgeManifestResourceName();

            //  We'll now try and get the bridge library path.
            string bridgeLibraryPath = string.Empty;

            try
            {
                //  Get the manifest resource stream.
                using (var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resouceName))
                {
                    //  Set the temporary path..
                    bridgeLibraryPath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".dll";
                    using (var tempStream = File.Create(bridgeLibraryPath))
                    {
                        resourceStream.CopyTo(tempStream);
                    }
                }
            }
            catch (Exception exception)
            {
                //  Log the exception.
                Logging.Error("NativeBridge: Failed to extract the bridge library. The manifest path is '" +
                              bridgeLibraryPath + "'", exception);
                return false;
            }

            //  Load the bridge library.
            try
            {
                libraryHandle = Kernel32.LoadLibrary(bridgeLibraryPath);
            }
            catch (Exception exception)
            {
                //  Log the exception.
                Logging.Error("NativeBridge: Exception loading the bridge library.", exception);
            }

            //  If the library hasn't been loaded, log the last windows error.
            if (libraryHandle == IntPtr.Zero)
            {
                Logging.Error("NativeBridge: Failure to load the brige library.",
                              new Win32Exception(Marshal.GetLastWin32Error()));
                return false;
            }

            Logging.Log("Bridge Initialised");

            //  We've successfully loaded the bridge library.
            return true;
        }

        /// <summary>
        /// Calls the add prop sheet page.
        /// </summary>
        /// <param name="pAddPropSheet">The p add prop sheet.</param>
        /// <param name="hProp">The h prop.</param>
        /// <param name="lParam">The l param.</param>
        /// <returns></returns>
        public int CallAddPropSheetPage(IntPtr pAddPropSheet, IntPtr hProp, IntPtr lParam)
        {
            //  Get the proc address.
            var procAddress = Kernel32.GetProcAddress(libraryHandle, "CallAddPropSheetPage");

            //  Try and cast the proc, then call it.
            try
            {
                var callAddPropSheet = (CallAddPropSheetPageDelegate) Marshal.GetDelegateForFunctionPointer(
                    procAddress, typeof (CallAddPropSheetPageDelegate));
                return callAddPropSheet(pAddPropSheet, hProp, lParam);
            }
            catch (Exception exception)
            {
                //  Log the exception and fail.
                Logging.Error(
                    "NativeBridge: Failed to either load the proc (address is '" + procAddress.ToString("x8") +
                    "'), marshal it or call it.", exception);
                return 0;
            }
        }

        /// <summary>
        /// Gets the proxy host template.
        /// </summary>
        /// <returns>The pointer to the proxy host template.</returns>
        public IntPtr GetProxyHostTemplate()
        {
            //  Get the proc address.
            var procAddress = Kernel32.GetProcAddress(libraryHandle, "GetProxyHostTemplate");

            //  Try and cast the proc, then call it.
            try
            {
                var proc = (GetProxyHostTemplateDelegate)Marshal.GetDelegateForFunctionPointer(
                    procAddress, typeof(GetProxyHostTemplateDelegate));
                return proc();
            }
            catch (Exception exception)
            {
                //  Log the exception and fail.
                Logging.Error(
                    "NativeBridge: Failed to either load the proc (address is '" + procAddress.ToString("x8") +
                    "'), marshal it or call it.", exception);
                return IntPtr.Zero;
            }
        }


        /// <summary>
        /// Deinitialises this instance.
        /// </summary>
        public void Deinitialise()
        {
            //  Free the library.
            Kernel32.FreeLibrary(libraryHandle);
        }

        /// <summary>
        /// Gets the name of the bridge manifest resource.
        /// </summary>
        /// <returns>The name of the bridge manifest resource.</returns>
        private static string GetBridgeManifestResourceName()
        {
            //  Create the name of the bridge manifest.
            return string.Format("{0}.NativeBridge.SharpShellNativeBridge{1}.dll",
                                 Assembly.GetExecutingAssembly().GetName().Name,
                                 Environment.Is64BitProcess ? "64" : "32");
        }

        /// <summary>
        /// Gets the instance handle.
        /// </summary>
        /// <returns>The Instance Handle.</returns>
        public IntPtr GetInstanceHandle()
        {
            return libraryHandle;
        }
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int CallAddPropSheetPageDelegate(IntPtr lpfnAddPage, IntPtr hProp, IntPtr lParam);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr GetProxyHostTemplateDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void CreatePropertySheetDelegate(ref PROPSHEETHEADER header);

        private IntPtr libraryHandle;

        internal void CreatePropertySheet(ref PROPSHEETHEADER psh)
        { //  Try and cast the proc, then call it.
            var procAddress = Kernel32.GetProcAddress(libraryHandle, "CreatePropertySheet");
            try
            {
                var proc = (CreatePropertySheetDelegate)Marshal.GetDelegateForFunctionPointer(
                    procAddress, typeof(CreatePropertySheetDelegate));
                proc(ref psh);
            }
            catch (Exception exception)
            {
                //  Log the exception and fail.
                Logging.Error(
                    "NativeBridge: Failed to either load the proc (address is '" + procAddress.ToString("x8") +
                    "'), marshal it or call it.", exception);
            }
        }
    }
}
