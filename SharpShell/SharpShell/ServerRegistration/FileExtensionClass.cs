using System;
using Microsoft.Win32;
using SharpShell.Registry;

namespace SharpShell.ServerRegistration
{
    /// <summary>
    /// Provides methods for manipulating the file extension classes which may be present
    /// in the registry.
    /// </summary>
    public static class FileExtensionClass
    {
        /// <summary>
        /// Gets the class of the given file extension, optionally creating it if it does not exist.
        /// </summary>
        /// <param name="classesKey">The classes key (HKEY_CLASSES_ROOT).</param>
        /// <param name="fileExtension">The file extension, with a leading dot.</param>
        /// <param name="createIfMissing">if set to <c>true</c> then if no class exists, one will be created.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown if the extension is invalid.</exception>
        public static string Get(IRegistryKey classesKey, string fileExtension, bool createIfMissing)
        {
            //  Make sure that we have a file extension, a string which starts with a dot and
            //  has at least one character following it.
            if (string.IsNullOrEmpty(fileExtension) ||
                fileExtension.StartsWith(".") == false ||
                fileExtension.Length < 2)
            {
                throw new InvalidOperationException($@"'{fileExtension}' does not appear to be a valid file extension class.");
            }

            //  We will need the extension with no leading dot later.
            var extension = fileExtension.Substring(1);

            //  Open or create the file extension key.
            using (var fileExtensionClassKey =
                classesKey.CreateSubKey(fileExtension, RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                //  Get the class, which is the 'default' value. If we've got a value, we're done.
                var fileExtensionClassName = fileExtensionClassKey.GetValue(null) as string;
                if (!string.IsNullOrEmpty(fileExtensionClassName)) return fileExtensionClassName;

                //  We don't have a class for the extension. If we are *not* creating missing ones, we're done.
                if (!createIfMissing) return null;

                //  There is no file extension class name, so we'll have to create one. Set the desired class name.
                fileExtensionClassName = $"{extension}.1"; // e.g. 'dllfile', 'mytypefile'

                //  If the desired class name exists, we're going to have to bail out (we could try and find another
                //  name, but that starts to get difficult for users to reason about).
                using (var fileExtensionClassNameKey = classesKey.OpenSubKey(fileExtensionClassName))
                {
                    if (fileExtensionClassNameKey != null)
                    {
                        var applicationName = fileExtensionClassNameKey.GetValue(null) as string;
                        throw new InvalidOperationException(
                            $@"Unable to create a new class '{fileExtensionClassName}' for extension '{fileExtension}'. That class is already in use by application '{applicationName}'.");
                    }
                }

                //  Create the file extension class name key, point the file extension to it, set a sensible name for the application and we're done.
                using (var fileExtensionClassNameKey = classesKey.CreateSubKey(fileExtensionClassName, RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    fileExtensionClassKey.SetValue(null, fileExtensionClassName);
                    fileExtensionClassNameKey.SetValue(null, $"{extension} Application");
                    return fileExtensionClassName;
                }
            }
        }
    }
}
