using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming, UnusedMember.Global, IdentifierTypo, CommentTypo

namespace SharpShell.Interop
{
    /// <summary>
    /// Imports from the win32 kernel32.dll library.
    /// </summary>
    public static class Kernel32
    {
        /// <summary>
        /// Enumerates resource types within a binary module. Starting with Windows Vista, this is typically a language-neutral Portable Executable (LN file), and the enumeration also includes resources from one of the corresponding language-specific resource files (.mui files)—if one exists—that contain localizable language resources. It is also possible to use hModule to specify a .mui file, in which case only that file is searched for resource types.
        /// </summary>
        /// <param name="hModule">A handle to a module to be searched. This handle must be obtained through LoadLibrary or LoadLibraryEx.</param>
        /// <param name="lpEnumFunc">A pointer to the callback function to be called for each enumerated resource type. For more information, see the EnumResTypeProc function.</param>
        /// <param name="lParam">An application-defined value passed to the callback function.</param>
        /// <returns>Returns TRUE if successful; otherwise, FALSE. To get extended error information, call GetLastError.</returns>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool EnumResourceTypes(IntPtr hModule, EnumResTypeProc lpEnumFunc, IntPtr lParam);

        /// <summary>
        /// An application-defined callback function used with the EnumResourceTypes and EnumResourceTypesEx functions. It receives resource types. The ENUMRESTYPEPROC type defines a pointer to this callback function. EnumResTypeProc is a placeholder for the application-defined function name.
        /// </summary>
        /// <param name="hModule">A handle to the module whose executable file contains the resources for which the types are to be enumerated. If this parameter is NULL, the function enumerates the resource types in the module used to create the current process.</param>
        /// <param name="lpszType">The type of resource for which the type is being enumerated. Alternately, rather than a pointer, this parameter can be MAKEINTRESOURCE(ID), where ID is the integer identifier of the given resource type. For standard resource types, see Resource Types. For more information, see the Remarks section below.</param>
        /// <param name="lParam">An application-defined parameter passed to the EnumResourceTypes or EnumResourceTypesEx function. This parameter can be used in error checking.</param>
        /// <returns>Returns TRUE to continue enumeration or FALSE to stop enumeration.</returns>
        public delegate bool EnumResTypeProc(IntPtr hModule, IntPtr lpszType, IntPtr lParam);

        /// <summary>
        /// An application-defined callback function used with the EnumResourceNames and EnumResourceNamesEx functions. It receives the type and name of a resource. The ENUMRESNAMEPROC type defines a pointer to this callback function. EnumResNameProc is a placeholder for the application-defined function name.
        /// </summary>
        /// <param name="hModule">A handle to the module whose executable file contains the resources that are being enumerated. If this parameter is NULL, the function enumerates the resource names in the module used to create the current process.</param>
        /// <param name="type">The type of resource for which the name is being enumerated. Alternately, rather than a pointer, this parameter can be MAKEINTRESOURCE(ID), where ID is an integer value representing a predefined resource type. For standard resource types, see Resource Types. For more information, see the Remarks section below.</param>
        /// <param name="name">The name of a resource of the type being enumerated. Alternately, rather than a pointer, this parameter can be MAKEINTRESOURCE(ID), where ID is the integer identifier of the resource. For more information, see the Remarks section below.</param>
        /// <param name="lParam">An application-defined parameter passed to the EnumResourceNames or EnumResourceNamesEx function. This parameter can be used in error checking.</param>
        /// <returns>Returns TRUE to continue enumeration or FALSE to stop enumeration.</returns>
        public delegate bool EnumResNameProc(IntPtr hModule, IntPtr type, IntPtr name, IntPtr lParam);

        /// <summary>
        /// Enumerates resources of a specified type within a binary module. For Windows Vista and later, this is typically a language-neutral Portable Executable (LN file), and the enumeration will also include resources from the corresponding language-specific resource files (.mui files) that contain localizable language resources. It is also possible for hModule to specify an .mui file, in which case only that file is searched for resources.
        /// </summary>
        /// <param name="hModule">A handle to a module to be searched. Starting with Windows Vista, if this is an LN file, then appropriate .mui files (if any exist) are included in the search.
        /// If this parameter is NULL, that is equivalent to passing in a handle to the module used to create the current process.</param>
        /// <param name="type">The type of the resource for which the name is being enumerated. Alternately, rather than a pointer, this parameter can be MAKEINTRESOURCE(ID), where ID is an integer value representing a predefined resource type. For a list of predefined resource types, see Resource Types. For more information, see</param>
        /// <param name="lpEnumFunc">A pointer to the callback function to be called for each enumerated resource name or ID. For more information, see EnumResNameProc.</param>
        /// <param name="lParam">An application-defined value passed to the callback function. This parameter can be used in error checking.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool EnumResourceNames(IntPtr hModule, IntPtr type, EnumResNameProc lpEnumFunc, IntPtr lParam);

        /// <summary>
        /// Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded.
        /// For additional load options, use the LoadLibraryEx function.
        /// </summary>
        /// <param name="lpFileName">The name of the module. This can be either a library module (a .dll file) or an executable module (an .exe file). The name specified is the file name of the module and is not related to the name stored in the library module itself, as specified by the LIBRARY keyword in the module-definition (.def) file.
        /// If the string specifies a full path, the function searches only that path for the module.
        /// If the string specifies a relative path or a module name without a path, the function uses a standard search strategy to find the module; for more information, see the Remarks.
        /// If the function cannot find the module, the function fails.When specifying a path, be sure to use backslashes (), not forward slashes (/). For more information about paths, see Naming a File or Directory.
        /// If the string specifies a module name without a path and the file name extension is omitted, the function appends the default library extension .dll to the module name.To prevent the function from appending .dll to the module name, include a trailing point character (.) in the module name string.
        /// </param>
        /// <returns>If the function succeeds, the return value is a handle to the module.
        /// If the function fails, the return value is NULL.To get extended error information, call GetLastError.</returns>
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        /// <summary>
        /// Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded.
        /// </summary>
        /// <param name="lpFileName">Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded.</param>
        /// <param name="hReservedNull">This parameter is reserved for future use. It must be NULL.</param>
        /// <param name="dwFlags">The action to be taken when loading the module. If no flags are specified, the behavior of this function is identical to that of the LoadLibrary function.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hReservedNull, LoadLibraryFlags dwFlags);

        /// <summary>
        /// Loads the specified module into the address space of the calling process. The specified module may cause other modules to be loaded.
        /// </summary>
        /// <param name="hModule">A handle to the DLL module that contains the function or variable. The LoadLibrary, LoadLibraryEx, LoadPackagedLibrary, or GetModuleHandle function returns this handle.
        /// The GetProcAddress function does not retrieve addresses from modules that were loaded using the LOAD_LIBRARY_AS_DATAFILE flag.For more information, see LoadLibraryEx.</param>
        /// <param name="procedureName">The function or variable name, or the function's ordinal value. If this parameter is an ordinal value, it must be in the low-order word; the high-order word must be zero.</param>
        /// <returns>If the function succeeds, the return value is the address of the exported function or variable
        /// If the function fails, the return value is NULL.To get extended error information, call GetLastError.</returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        /// <summary>
        /// Copies a block of memory from one location to another
        /// </summary>
        /// <param name="dest">A pointer to the starting address of the copied block's destination.</param>
        /// <param name="src">A pointer to the starting address of the block of memory to copy.</param>
        /// <param name="count">The size of the block of memory to copy, in bytes.</param>
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        /// <summary>
        /// Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count. When the reference count reaches zero, the module is unloaded from the address space of the calling process and the handle is no longer valid.
        /// </summary>
        /// <param name="hModule">A handle to the loaded library module. The LoadLibrary, LoadLibraryEx, GetModuleHandle, or GetModuleHandleEx function returns this handle.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        ///     If the function fails, the return value is zero.To get extended error information, call the GetLastError function.</returns>
        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);

        /// <summary>
        /// Determines the location of a resource with the specified type and name in the specified module.
        /// To specify a language, use the FindResourceEx function.
        /// </summary>
        /// <param name="hModule">A handle to the module whose portable executable file or an accompanying MUI file contains the resource. If this parameter is NULL, the function searches the module used to create the current process.</param>
        /// <param name="lpName">The name of the resource. Alternately, rather than a pointer, this parameter can be MAKEINTRESOURCE(ID), where ID is the integer identifier of the resource. For more information, see the Remarks section below.</param>
        /// <param name="lpType">The resource type. Alternately, rather than a pointer, this parameter can be MAKEINTRESOURCE(ID), where ID is the integer identifier of the given resource type. For standard resource types, see Resource Types.For more information, see the Remarks section below.</param>
        /// <returns>If the function succeeds, the return value is a handle to the specified resource's information block. To obtain a handle to the resource, pass this handle to the LoadResource function.
        /// If the function fails, the return value is NULL.To get extended error information, call GetLastError.</returns>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr FindResource(IntPtr hModule, IntPtr lpName, IntPtr lpType);

        /// <summary>
        /// Retrieves a handle that can be used to obtain a pointer to the first byte of the specified resource in memory.
        /// </summary>
        /// <param name="hModule">A handle to the module whose executable file contains the resource.If hModule is NULL, the system loads the resource from the module that was used to create the current process.</param>
        /// <param name="hResInfo">A handle to the resource to be loaded. This handle is returned by the FindResource or FindResourceEx function.</param>
        /// <returns>If the function succeeds, the return value is a handle to the data associated with the resource. If the function fails, the return value is NULL.To get extended error information, call GetLastError.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        /// <summary>
        /// Retrieves a pointer to the specified resource in memory.
        /// </summary>
        /// <param name="hResData">A handle to the resource to be accessed. The LoadResource function returns this handle. Note that this parameter is listed as an HGLOBAL variable only for backward compatibility. Do not pass any value as a parameter other than a successful return value from the LoadResource function.</param>
        /// <returns>If the loaded resource is available, the return value is a pointer to the first byte of the resource; otherwise, it is NULL.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LockResource(IntPtr hResData);

        /// <summary>
        /// Retrieves a handle to the default heap of the calling process. This handle can then be used in subsequent calls to the heap functions.
        /// </summary>
        /// <returns>If the function succeeds, the return value is a handle to the calling process's heap.
        /// If the function fails, the return value is NULL.To get extended error information, call GetLastError.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetProcessHeap();

        /// <summary>
        /// Allocates a block of memory from a heap. The allocated memory is not movable.
        /// </summary>
        /// <param name="hHeap">A handle to the heap from which the memory will be allocated. This handle is returned by the HeapCreate or GetProcessHeap function.</param>
        /// <param name="dwFlags">The heap allocation options. Specifying any of these values will override the corresponding value specified when the heap was created with HeapCreate.</param>
        /// <param name="dwBytes">The number of bytes to be allocated.
        /// If the heap specified by the hHeap parameter is a "non-growable" heap, dwBytes must be less than 0x7FFF8. You create a non-growable heap by calling the HeapCreate function with a nonzero value.</param>
        /// <returns>If the function succeeds, the return value is a pointer to the allocated memory block.
        /// If the function fails and you have not specified HEAP_GENERATE_EXCEPTIONS, the return value is NULL.</returns>
        [DllImport("kernel32.dll", SetLastError = false)]
        public static extern IntPtr HeapAlloc(IntPtr hHeap, uint dwFlags, UIntPtr dwBytes);

        /// <summary>
        /// Frees a memory block allocated from a heap by the HeapAlloc or HeapReAlloc function.
        /// </summary>
        /// <param name="hHeap">A handle to the heap whose memory block is to be freed. This handle is returned by either the HeapCreate or GetProcessHeap function.</param>
        /// <param name="dwFlags">The heap free options. Specifying the following value overrides the corresponding value specified in the flOptions parameter when the heap was created by using the HeapCreate function.</param>
        /// <param name="lpMem">A pointer to the memory block to be freed. This pointer is returned by the HeapAlloc or HeapReAlloc function. If this pointer is NULL, the behavior is undefined.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.An application can call GetLastError for extended error information.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool HeapFree(IntPtr hHeap, uint dwFlags, IntPtr lpMem);

        /// <summary>
        /// Retrieves the size, in bytes, of the specified resource
        /// </summary>
        /// <param name="hModule">A handle to the module whose executable file contains the resource.</param>
        /// <param name="hResInfo">A handle to the resource. This handle must be created by using the FindResource or FindResourceEx function.</param>
        /// <returns>If the function succeeds, the return value is the number of bytes in the resource.
        /// If the function fails, the return value is zero.To get extended error information, call GetLastError.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint SizeofResource(IntPtr hModule, IntPtr hResInfo);
    }

    /// <summary>
    /// This parameter can be one of the following values.
    /// </summary>
    [Flags]
    public enum LoadLibraryFlags : uint
    {
        /// <summary>
        /// No values.
        /// </summary>
        None = 0,

        /// <summary>
        /// If this value is used, and the executable module is a DLL, the system does not call DllMain for process and thread initialization and termination. Also, the system does not load additional executable modules that are referenced by the specified module.
        /// Note  Do not use this value; it is provided only for backward compatibility. If you are planning to access only data or resources in the DLL, use LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE or LOAD_LIBRARY_AS_IMAGE_RESOURCE or both. Otherwise, load the library as a DLL or executable module using the LoadLibrary function.
        /// </summary>
        DONT_RESOLVE_DLL_REFERENCES = 0x00000001,

        /// <summary>
        /// If this value is used, the system does not check AppLocker rules or apply Software Restriction Policies for the DLL. This action applies only to the DLL being loaded and not to its dependencies. This value is recommended for use in setup programs that must run extracted DLLs during installation.
        /// Windows Server 2008 R2 and Windows 7:  On systems with KB2532445 installed, the caller must be running as "LocalSystem" or "TrustedInstaller"; otherwise the system ignores this flag. For more information, see "You can circumvent AppLocker rules by using an Office macro on a computer that is running Windows 7 or Windows Server 2008 R2" in the Help and Support Knowledge Base at http://support.microsoft.com/kb/2532445.
        /// Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP:  AppLocker was introduced in Windows 7 and Windows Server 2008 R2.
        /// </summary>
        LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010,

        /// <summary>
        /// If this value is used, the system maps the file into the calling process's virtual address space as if it were a data file. Nothing is done to execute or prepare to execute the mapped file. Therefore, you cannot call functions like GetModuleFileName, GetModuleHandle or GetProcAddress with this DLL. Using this value causes writes to read-only memory to raise an access violation. Use this flag when you want to load a DLL only to extract messages or resources from it.
        /// This value can be used with LOAD_LIBRARY_AS_IMAGE_RESOURCE. For more information, see Remarks.
        /// </summary>
        LOAD_LIBRARY_AS_DATAFILE = 0x00000002,

        /// <summary>
        /// Similar to LOAD_LIBRARY_AS_DATAFILE, except that the DLL file is opened with exclusive write access for the calling process. Other processes cannot open the DLL file for write access while it is in use. However, the DLL can still be opened by other processes.
        /// This value can be used with LOAD_LIBRARY_AS_IMAGE_RESOURCE. For more information, see Remarks.
        /// Windows Server 2003 and Windows XP:  This value is not supported until Windows Vista.
        /// </summary>
        LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x00000040,

        /// <summary>
        /// If this value is used, the system maps the file into the process's virtual address space as an image file. However, the loader does not load the static imports or perform the other usual initialization steps. Use this flag when you want to load a DLL only to extract messages or resources from it.
        /// Unless the application depends on the file having the in-memory layout of an image, this value should be used with either LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE or LOAD_LIBRARY_AS_DATAFILE. For more information, see the Remarks section.
        /// Windows Server 2003 and Windows XP:  This value is not supported until Windows Vista.
        /// </summary>
        LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020,

        /// <summary>
        /// If this value is used, the application's installation directory is searched for the DLL and its dependencies. Directories in the standard search path are not searched. This value cannot be combined with LOAD_WITH_ALTERED_SEARCH_PATH.
        /// Windows 7, Windows Server 2008 R2, Windows Vista and Windows Server 2008:  This value requires KB2533623 to be installed.
        /// Windows Server 2003 and Windows XP:  This value is not supported.
        /// </summary>
        LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 0x00000200,

        /// <summary>
        /// This value is a combination of LOAD_LIBRARY_SEARCH_APPLICATION_DIR, LOAD_LIBRARY_SEARCH_SYSTEM32, and LOAD_LIBRARY_SEARCH_USER_DIRS. Directories in the standard search path are not searched. This value cannot be combined with LOAD_WITH_ALTERED_SEARCH_PATH.
        /// This value represents the recommended maximum number of directories an application should include in its DLL search path.
        /// Windows 7, Windows Server 2008 R2, Windows Vista and Windows Server 2008:  This value requires KB2533623 to be installed.
        /// Windows Server 2003 and Windows XP:  This value is not supported.
        /// </summary>
        LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000,

        /// <summary>
        /// If this value is used, the directory that contains the DLL is temporarily added to the beginning of the list of directories that are searched for the DLL's dependencies. Directories in the standard search path are not searched.
        /// The lpFileName parameter must specify a fully qualified path. This value cannot be combined with LOAD_WITH_ALTERED_SEARCH_PATH.
        /// For example, if Lib2.dll is a dependency of C:\Dir1\Lib1.dll, loading Lib1.dll with this value causes the system to search for Lib2.dll only in C:\Dir1. To search for Lib2.dll in C:\Dir1 and all of the directories in the DLL search path, combine this value with LOAD_LIBRARY_DEFAULT_DIRS
        /// Windows 7, Windows Server 2008 R2, Windows Vista and Windows Server 2008:  This value requires KB2533623 to be installed.
        /// Windows Server 2003 and Windows XP:  This value is not supported.
        /// </summary>
        LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 0x00000100,

        /// <summary>
        /// If this value is used, %windows%\system32 is searched for the DLL and its dependencies. Directories in the standard search path are not searched. This value cannot be combined with LOAD_WITH_ALTERED_SEARCH_PATH.
        /// Windows 7, Windows Server 2008 R2, Windows Vista and Windows Server 2008:  This value requires KB2533623 to be installed.
        /// Windows Server 2003 and Windows XP:  This value is not supported.
        /// </summary>
        LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x00000800,

        /// <summary>
        /// If this value is used, directories added using the AddDllDirectory or the SetDllDirectory function are searched for the DLL and its dependencies. If more than one directory has been added, the order in which the directories are searched is unspecified. Directories in the standard search path are not searched. This value cannot be combined with LOAD_WITH_ALTERED_SEARCH_PATH.
        /// Windows 7, Windows Server 2008 R2, Windows Vista and Windows Server 2008:  This value requires KB2533623 to be installed.
        /// Windows Server 2003 and Windows XP:  This value is not supported.
        /// </summary>
        LOAD_LIBRARY_SEARCH_USER_DIRS = 0x00000400,

        /// <summary>
        /// If this value is used and lpFileName specifies an absolute path, the system uses the alternate file search strategy discussed in the Remarks section to find associated executable modules that the specified module causes to be loaded. If this value is used and lpFileName specifies a relative path, the behavior is undefined.
        /// If this value is not used, or if lpFileName does not specify a path, the system uses the standard search strategy discussed in the Remarks section to find associated executable modules that the specified module causes to be loaded.
        /// This value cannot be combined with any LOAD_LIBRARY_SEARCH flag.
        /// </summary>
        LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008
    }
}
