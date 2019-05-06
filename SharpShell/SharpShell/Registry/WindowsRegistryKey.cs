using System;
using System.Security.AccessControl;
using Microsoft.Win32;

namespace SharpShell.Registry
{
    /// <summary>
    /// A Windows Registry Key. Essentially a wrapper around <see cref="RegistryKey"/>.
    /// </summary>
    /// <seealso cref="SharpShell.Registry.IRegistryKey" />
    public class WindowsRegistryKey : IRegistryKey
    {
        private readonly RegistryKey _registryKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsRegistryKey"/> class.
        /// </summary>
        /// <param name="registryKey">The registry key.</param>
        /// <exception cref="ArgumentNullException">registryKey</exception>
        public WindowsRegistryKey(RegistryKey registryKey)
        {
            _registryKey = registryKey ?? throw new ArgumentNullException(nameof(registryKey));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _registryKey.Dispose();
        }

        /// <inheritdoc />
        public string[] GetSubKeyNames()
        {
            return _registryKey.GetSubKeyNames();
        }

        /// <inheritdoc />
        public IRegistryKey OpenSubKey(string name)
        {
            var subkey = _registryKey.OpenSubKey(name);
            return subkey != null ? new WindowsRegistryKey(subkey) : null;
        }

        /// <inheritdoc />
        public IRegistryKey OpenSubKey(string name, bool writable)
        {
            var subkey = _registryKey.OpenSubKey(name, writable);
            return subkey != null ? new WindowsRegistryKey(subkey) : null;
        }

        /// <inheritdoc />
        public IRegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck)
        {
            var subkey = _registryKey.OpenSubKey(name, permissionCheck);
            return subkey != null ? new WindowsRegistryKey(subkey) : null;
        }

        /// <inheritdoc />
        public IRegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck, RegistryRights rights)
        {
            var subkey = _registryKey.OpenSubKey(name, permissionCheck, rights);
            return subkey != null ? new WindowsRegistryKey(subkey) : null;
        }

        /// <inheritdoc />
        public object GetValue(string name)
        {
            return _registryKey.GetValue(name);
        }

        /// <inheritdoc />
        public object GetValue(string name, object defaultValue)
        {
            return _registryKey.GetValue(name, defaultValue);
        }

        /// <inheritdoc />
        public object GetValue(string name, object defaultValue, RegistryValueOptions options)
        {
            return _registryKey.GetValue(name, defaultValue, options);
        }

        /// <inheritdoc />
        public IRegistryKey CreateSubKey(string subkey)
        {
            return new WindowsRegistryKey(_registryKey.CreateSubKey(subkey));
        }

        /// <inheritdoc />
        public IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck)
        {
            return new WindowsRegistryKey(_registryKey.CreateSubKey(subkey, permissionCheck));
        }
        
        /// <inheritdoc />
        public IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity)
        {
            return new WindowsRegistryKey(_registryKey.CreateSubKey(subkey, permissionCheck, registrySecurity));
        }

        /// <inheritdoc />
        public void SetValue(string name, object value)
        {
            _registryKey.SetValue(name, value);
        }

        /// <inheritdoc />
        public void SetValue(string name, object value, RegistryValueKind valueKind)
        {
            _registryKey.SetValue(name, value, valueKind);
        }

        /// <inheritdoc />
        public void DeleteSubKeyTree(string subkey)
        {
            _registryKey.DeleteSubKeyTree(subkey);
        }

        /// <inheritdoc />
        public void DeleteSubKeyTree(string subkey, bool throwOnMissingSubKey)
        {
            _registryKey.DeleteSubKeyTree(subkey, throwOnMissingSubKey);
        }

        /// <inheritdoc />
        public string[] GetValueNames()
        {
            return _registryKey.GetValueNames();
        }

        /// <inheritdoc />
        public void DeleteValue(string name)
        {
            _registryKey.DeleteValue(name);
        }

        /// <inheritdoc />
        public void DeleteValue(string name, bool throwOnMissingValue)
        {
            _registryKey.DeleteValue(name, throwOnMissingValue);
        }

        /// <inheritdoc />
        public void DeleteSubKey(string subkey)
        {
            _registryKey.DeleteSubKey(subkey);
        }

        /// <inheritdoc />
        public void DeleteSubKey(string subkey, bool throwOnMissingSubKey)
        {
            _registryKey.DeleteSubKey(subkey, throwOnMissingSubKey);
        }

        /// <inheritdoc />
        public string Name => _registryKey.Name;
    }
}
