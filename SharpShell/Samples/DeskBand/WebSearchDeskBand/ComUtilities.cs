using System;
using System.Linq;
using Microsoft.Win32;
using SharpShell.Attributes;

namespace WebSearchDeskBand
{
    internal static class ComUtilities
    {
        ///	<summary>
        ///	Called to register the control.
        /// Key format: "HKEY_CLASSES_ROOT\CLSID\{guid}"
        ///	</summary>
        ///	<param name="type">The type of the desk band class</param>
        public static void RegisterDeskBandClass(Type type)
        {
            var name = (type
                .GetCustomAttributes(typeof(DisplayNameAttribute), true)
                .FirstOrDefault() as DisplayNameAttribute)
                ?.DisplayName ?? "Default Desk Band";

            var keyName = @"CLSID\" + type.GUID.ToString("B");
            using (var key = Registry.ClassesRoot.CreateSubKey(keyName))
            {
                if (key == null)
                {
                    throw new Exception("Cannot register deskband. Key is null");
                }

                key.SetValue(null, name);

                using (var subkey = key.CreateSubKey("Implemented Categories"))
                {
                    if (subkey == null)
                    {
                        throw new Exception("Cannot register deskband. Sub key is null");
                    }

                    subkey.CreateSubKey("{00021492-0000-0000-C000-000000000046}");
                }
            }
        }

        ///	<summary>
        ///	Called to unregister the control.
        /// Key format: "HKEY_CLASSES_ROOT\CLSID\{guid}"
        ///	</summary>
        ///	<param name="type">The type of the desk band class</param>
        public static void UnregisterClass(Type type)
        {
            Registry.ClassesRoot.DeleteSubKeyTree(@"CLSID\" + type.GUID.ToString("B"), false);
        }
    }
}