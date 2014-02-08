using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
<<<<<<< HEAD
    /// Provides the CLSID of an object that can be stored persistently in the system. Allows the object to specify which object handler to use in the client process, as it is used in the default implementation of marshaling.
    /// </summary>
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("0000010c-0000-0000-C000-000000000046")]
=======
    /// Provides the CLSID of an object that can be stored persistently in the system. 
    /// Allows the object to specify which object handler to use in the client process, as it is used in the default implementation of marshaling.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("0000010c-0000-0000-c000-000000000046")]
>>>>>>> origin/f/DeskBands
    public interface IPersist
    {
        /// <summary>
        /// Retrieves the class identifier (CLSID) of the object.
        /// </summary>
<<<<<<< HEAD
        /// <param name="pClassID">A pointer to the location that receives the CLSID on return. The CLSID is a globally unique identifier (GUID) that uniquely represents an object class that defines the code that can manipulate the object's data.</param>
=======
        /// <param name="pClassID">A pointer to the location that receives the CLSID on return. 
        /// The CLSID is a globally unique identifier (GUID) that uniquely represents an object class that defines the code that can manipulate the object's data.</param>
>>>>>>> origin/f/DeskBands
        /// <returns>If the method succeeds, the return value is S_OK. Otherwise, it is E_FAIL.</returns>
        [PreserveSig]
        int GetClassID(out Guid pClassID);
    }
}