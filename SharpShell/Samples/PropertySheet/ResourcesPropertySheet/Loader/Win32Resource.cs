using System;
using System.Drawing;
using System.Runtime.InteropServices;
using SharpShell.Helpers;
using SharpShell.Interop;

namespace ResourcesPropertySheet.Loader
{
    internal class Win32Resource
    {
        public Win32Resource(Win32ResourceName resourceName, Win32ResourceType resourceType)
        {
            ResourceName = resourceName;
            ResourceType = resourceType;
        }

        public void Load(IntPtr hModule, uint resourceSize, IntPtr resourceData)
        {
            if (ResourceType.IsKnownResourceType(ResType.RT_BITMAP))
            {
                //  Bitmap.FromResource only loads string named resources, so we must use LoadBitmap.
                var handle = User32.LoadBitmap(hModule, ResourceName.Resource);
                BitmapData = Image.FromHbitmap(handle);
                Gdi32.DeleteObject(handle);
            }
            else if (ResourceType.IsKnownResourceType(ResType.RT_ICON))
            {

                //  Icon.FromResource only loads string named resources, so we must use LoadIcon.
                var handle = User32.LoadIcon(hModule, ResourceName.Resource);
                IconData = Icon.FromHandle(handle);
                Gdi32.DeleteObject(handle);
            }
        }

        public Icon IconData { get; private set; }
        public Bitmap BitmapData { get; private set; }
        public Win32ResourceName ResourceName { get; }
        public Win32ResourceType ResourceType { get; }
    }
}