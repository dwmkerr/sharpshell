using Microsoft.Win32;

namespace SharpShell.Registry
{
    /// <summary>
    /// This class implements <see cref="IRegistry"/>, providing test-able access to the registry. SharpShell should
    /// never use the registry directly, it should use IRegistry so that we can test these interactions.
    /// </summary>
    /// <seealso cref="SharpShell.Registry.IRegistry" />
    public class WindowsRegistry : IRegistry
    {
        /// <inheritdoc />
        public IRegistryKey OpenBaseKey(RegistryHive hKey, RegistryView view)
        {
            //  Proxy directly to the windows registry.
            var key = RegistryKey.OpenBaseKey(hKey, view);
            return new WindowsRegistryKey(key);
        }
    }
}