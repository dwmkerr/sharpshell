using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using Microsoft.Win32;

namespace SharpShell.Registry
{
    /// <summary>
    /// An In-Memory registry key. Primarily used for testing scenarios.
    /// </summary>
    /// <seealso cref="SharpShell.Registry.IRegistryKey" />
    public class InMemoryRegistryKey : IRegistryKey
    {
        private readonly string _name;
        private readonly RegistryView _view;
        private readonly Dictionary<string, InMemoryRegistryKey> _subkeys = new Dictionary<string, InMemoryRegistryKey>();
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryRegistryKey"/> class.
        /// </summary>
        /// <param name="view">The registry view.</param>
        /// <param name="name">The name.</param>
        public InMemoryRegistryKey(RegistryView view, string name)
        {
            _view = view;
            _name = name;
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public string[] GetSubKeyNames()
        {
            return _subkeys.Values.Select(v => v.Name).ToArray();
        }

        /// <inheritdoc />
        public IRegistryKey OpenSubKey(string name)
        {
            return _subkeys[name.ToLowerInvariant()];
        }

        /// <inheritdoc />
        public IRegistryKey OpenSubKey(string name, bool writable)
        {
            return _subkeys[name.ToLowerInvariant()];
        }

        /// <inheritdoc />
        public IRegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck)
        {
            return _subkeys[name.ToLowerInvariant()];
        }

        /// <inheritdoc />
        public IRegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck, RegistryRights rights)
        {
            return _subkeys[name.ToLowerInvariant()];
        }

        /// <inheritdoc />
        public object GetValue(string name)
        {
            _values.TryGetValue(name, out var value);
            return value;
        }

        /// <inheritdoc />
        public object GetValue(string name, object defaultValue)
        {
            return _values.TryGetValue(name, out var value) ? value : defaultValue;
        }

        /// <inheritdoc />
        public object GetValue(string name, object defaultValue, RegistryValueOptions options)
        {
            return _values.TryGetValue(name, out var value) ? value : defaultValue;
        }

        /// <inheritdoc />
        public IRegistryKey CreateSubKey(string subkey)
        {
            var subkeyName = subkey.ToLower();
            if (_subkeys.ContainsKey(subkeyName) == false) _subkeys[subkeyName] = new InMemoryRegistryKey(_view, subkey);
            return _subkeys[subkeyName];
        }

        /// <inheritdoc />
        public IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck)
        {
            var subkeyName = subkey.ToLower();
            if (_subkeys.ContainsKey(subkeyName) == false) _subkeys[subkeyName] = new InMemoryRegistryKey(_view, subkey);
            return _subkeys[subkeyName];
        }

        /// <inheritdoc />
        public IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions options)
        {
            var subkeyName = subkey.ToLower();
            if (_subkeys.ContainsKey(subkeyName) == false) _subkeys[subkeyName] = new InMemoryRegistryKey(_view, subkey);
            return _subkeys[subkeyName];
        }

        //        public RegistryKey CreateSubKey(string subkey, bool writable)
        //        {
        //            return _registryKey.CreateSubKey(subkey, writable);
        //        }
        //
        //        public RegistryKey CreateSubKey(string subkey, bool writable, RegistryOptions options)
        //        {
        //            return _registryKey.CreateSubKey(subkey, writable, options);
        //        }

        /// <inheritdoc />
        public IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity)
        {
            var subkeyName = subkey.ToLower();
            if (_subkeys.ContainsKey(subkeyName) == false) _subkeys[subkeyName] = new InMemoryRegistryKey(_view, subkey);
            return _subkeys[subkeyName];
        }

        /// <inheritdoc />
        public IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions registryOptions,
            RegistrySecurity registrySecurity)
        {
            var subkeyName = subkey.ToLower();
            if (_subkeys.ContainsKey(subkeyName) == false) _subkeys[subkeyName] = new InMemoryRegistryKey(_view, subkey);
            return _subkeys[subkeyName];
        }

        /// <inheritdoc />
        public void SetValue(string name, object value)
        {
            _values[name] = value;
        }

        /// <inheritdoc />
        public void SetValue(string name, object value, RegistryValueKind valueKind)
        {
            _values[name] = value;
        }

        /// <inheritdoc />
        public void DeleteSubKeyTree(string subkey)
        {
            _subkeys.Remove(subkey);
        }

        /// <inheritdoc />
        public void DeleteSubKeyTree(string subkey, bool throwOnMissingSubKey)
        {
            _subkeys.Remove(subkey);
        }

        /// <inheritdoc />
        public string[] GetValueNames()
        {
            return _values.Keys.ToArray();
        }

        /// <inheritdoc />
        public void DeleteValue(string name)
        {
            _values.Remove(name);
        }

        /// <inheritdoc />
        public void DeleteValue(string name, bool throwOnMissingValue)
        {
            _values.Remove(name);
        }

        /// <inheritdoc />
        public void DeleteSubKey(string subkey)
        {
            _subkeys.Remove(subkey);
        }

        /// <inheritdoc />
        public void DeleteSubKey(string subkey, bool throwOnMissingSubKey)
        {
            _subkeys.Remove(subkey);
        }

        /// <inheritdoc />
        public string Name => _name;

        public RegistryView View => _view;
    }
}
