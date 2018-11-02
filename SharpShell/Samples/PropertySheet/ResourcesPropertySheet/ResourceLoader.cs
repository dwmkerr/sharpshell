using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SharpShell.Diagnostics;
using SharpShell.Extensions;
using SharpShell.Helpers;
using SharpShell.Interop;

namespace ResourcesPropertySheet
{
    class Win32Resource
    {
        public Win32Resource(IntPtr resource)
        {
            Resource = resource;
            IsInt = Win32Helper.IS_INTRESOURCE(resource);
        }
    
        public bool IsInt { get; }
        public IntPtr Resource { get; }

        public uint IntValue
        {
            get
            {
                if (IsInt)
                    return (uint)Resource;
                throw new NotSupportedException($"Resource with value {Resource} cannot be safely converted into an ID");
            }
        }

        public string StringValue
        {
            get
            {
                if (IsInt == false)
                    return Marshal.PtrToStringUni(Resource);
                throw new NotSupportedException($"Resource with value {Resource} cannot be safely converted into a string");
            }
        }


        public override string ToString()
        {
            //  Resources are written as an int value, or a quoted string.
            return IsInt ? $"{IntValue}" : $"\"{StringValue}\"";

        }
    }

    class Win32Resources
    {
        public Win32ResourceType ResourceType { get; }
        public Win32Resource[] ResourceNames { get; }

        public Win32Resources(Win32ResourceType resourceType, Win32Resource[] resourceNames)
        {
            ResourceType = resourceType;
            ResourceNames = resourceNames;
        }
    }

    class Win32ResourceType : Win32Resource
    {

        public Win32ResourceType(IntPtr resource) : base(resource)
        {
        }

        public override string ToString()
        {
            //  For int resources, we return the known name if possible.
            if (IsInt)
            {
                //  If the resource is a known type...
                if (Enum.IsDefined(typeof(ResType), IntValue))
                {
                    //  ...then return its display name or just the value name.
                    var resType = (ResType)IntValue;
                    var displayName = resType.GetAttribute<DisplayAttribute>();
                    return displayName != null ? displayName.Name : resType.ToString();
                }

                //  If it's not a known type, we just return the int value.
                return $"{IntValue}";
            }

            //  For string resources, if the name starts with a Hash, we use that. Otherwise we quote it. This
            //  is *reasonably* consistent with the visual studio resource viewer.
            return StringValue.StartsWith(@"#") ? StringValue : $"\"{StringValue}\"";
        }

        public bool IsKnownResourceType(ResType resType)
        {
            if (!IsInt) return false;
            return Enum.IsDefined(typeof(ResType), IntValue);
        }
    }

    enum ResType
    {
        [Display(Name = "Cursor")]
        CURSOR = 1,

        [Display(Name = "Bitmap")]
        BITMAP = 2,

        [Display(Name = "Icon")]
        ICON = 3,

        [Display(Name = "Menu")]
        MENU = 4,

        [Display(Name = "Dialog")]
        DIALOG = 5,

        [Display(Name = "String Table")]
        STRING = 6,

        [Display(Name = "Font Directory")]
        FONTDIR = 7,

        [Display(Name = "Font")]
        FONT = 8,

        [Display(Name = "Accelerator")]
        ACCELERATOR = 9,

        [Display(Name = "Application Defined Resource")]
        RCDATA = 10,

        [Display(Name = "Message Table")]
        MESSAGETABLE = 11,

        [Display(Name = "Group Cursor")]
        GROUP_CURSOR = 12,

        [Display(Name = "Group Icon")]
        GROUP_ICON = 14,

        [Display(Name = "Version")]
        VERSION = 16,

        [Display(Name = "Dialog Include")]
        DLGINCLUDE = 17,

        [Display(Name = "Plug and Play")]
        PLUGPLAY = 19,

        [Display(Name = "VXD")]
        VXD = 20,

        [Display(Name = "Animated Cursor")]
        ANICURSOR = 21,

        [Display(Name = "Animated Icon")]
        ANIICON = 22,

        [Display(Name = "HTML")]
        HTML = 23,

        [Display(Name = "RT_MANIFEST")]
        MANIFEST = 24
    }

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
                    resourceNames.Add(new Win32Resource(name));
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
