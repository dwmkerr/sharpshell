using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using SharpShell.Interop;

namespace ResourcesPropertySheet.Loader
{
    internal static class ResourceLoader
    {
        public static List<Win32Resources> LoadResources(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new NullReferenceException("'path' cannot be null or empty");

            //  Load the module for reflection types.
            var hModule = Kernel32.LoadLibraryEx(path, IntPtr.Zero,
                LoadLibraryFlags.LOAD_LIBRARY_AS_DATAFILE | LoadLibraryFlags.LOAD_LIBRARY_AS_IMAGE_RESOURCE);
            if (hModule == IntPtr.Zero)
            {
                throw new InvalidOperationException($"Failed to load resources from {path}", new Win32Exception(Marshal.GetLastWin32Error()));
            }

            //  Enumerate the resources.
            var resourceTypes = new List<Win32ResourceType>();

            bool EnumResourceTypesProc(IntPtr hMod, IntPtr type, IntPtr param)
            {
                resourceTypes.Add(new Win32ResourceType(type));
                return true;
            }

            Kernel32.EnumResourceTypes(hModule, EnumResourceTypesProc, IntPtr.Zero);

            //  Now that we have the resource types, load resources for each type.
            var resources = new List<Win32Resources>();
            foreach (var resourceType in resourceTypes)
            {
                var resourceNames = new List<Win32Resource>();

                //  Enumerate the resource names for the given type.
                bool EnumResourceNamesProc(IntPtr _, IntPtr type, IntPtr name, IntPtr lParam)
                {
                    //  Create a managed resource name.
                    var resourceName = new Win32ResourceName(name);

                    //  Get the resource handle.
                    var hResourceInfo = Kernel32.FindResource(hModule, name, resourceType.Resource);
                    var resourceSize = Kernel32.SizeofResource(hModule, hResourceInfo);
                    var hResourceData = Kernel32.LoadResource(hModule, hResourceInfo);
                    var resourceData = Kernel32.LockResource(hResourceData);

                    //  Load resource data.
                    var resource = new Win32Resource(resourceName, resourceType);
                    resource.Load(hModule, resourceSize, resourceData);
                
                    resourceNames.Add(resource);
                    return true;
                }

                Kernel32.EnumResourceNames(hModule, resourceType.Resource, EnumResourceNamesProc, IntPtr.Zero);

                resources.Add(new Win32Resources(resourceType, resourceNames.ToArray()));
            }

            Kernel32.FreeLibrary(hModule);

            return resources;
        }
    }
}
