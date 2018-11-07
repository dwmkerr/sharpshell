using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using Microsoft.Win32;
using SharpShell.Registry;

namespace SharpShell.Tests
{
    public class InMemoryRegistryKey : IRegistryKey
    {
        private readonly string _name;
        private readonly Dictionary<string, InMemoryRegistryKey> _subkeys = new Dictionary<string, InMemoryRegistryKey>();
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        public InMemoryRegistryKey(string name)
        {
            _name = name;
        }

        public void Dispose()
        {
        }

        public string[] GetSubKeyNames()
        {
            return _subkeys.Keys.ToArray();
        }

        public IRegistryKey OpenSubKey(string name)
        {
            return _subkeys[name.ToLowerInvariant()];
        }

        public IRegistryKey OpenSubKey(string name, bool writable)
        {
            return _subkeys[name.ToLowerInvariant()];
        }

        public IRegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck)
        {
            return _subkeys[name.ToLowerInvariant()];
        }

        public IRegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck, RegistryRights rights)
        {
            return _subkeys[name.ToLowerInvariant()];
        }

        public object GetValue(string name)
        {
            _values.TryGetValue(name.ToLowerInvariant(), out object value);
            return value;
        }

        public object GetValue(string name, object defaultValue)
        {
            return _values.TryGetValue(name.ToLowerInvariant(), out object value) ? value : defaultValue;
        }

        public object GetValue(string name, object defaultValue, RegistryValueOptions options)
        {
            return _values.TryGetValue(name.ToLowerInvariant(), out object value) ? value : defaultValue;
        }

        public IRegistryKey CreateSubKey(string subkey)
        {
            var key = new InMemoryRegistryKey(subkey);
            _subkeys[subkey] = key;
            return key;
        }

        public IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck)
        {
            var key = new InMemoryRegistryKey(subkey);
            _subkeys[subkey] = key;
            return key;
        }

        public IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions options)
        {
            var key = new InMemoryRegistryKey(subkey);
            _subkeys[subkey] = key;
            return key;
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

        public IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity)
        {
            var key = new InMemoryRegistryKey(subkey);
            _subkeys[subkey] = key;
            return key;
        }

        public IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions registryOptions,
            RegistrySecurity registrySecurity)
        {
            var key = new InMemoryRegistryKey(subkey);
            _subkeys[subkey] = key;
            return key;
        }

        public void SetValue(string name, object value)
        {
            _values[name] = value;
        }

        public void SetValue(string name, object value, RegistryValueKind valueKind)
        {
            _values[name] = value;
        }

        public void DeleteSubKeyTree(string subkey)
        {
            _subkeys.Remove(subkey);
        }

        public void DeleteSubKeyTree(string subkey, bool throwOnMissingSubKey)
        {
            _subkeys.Remove(subkey);
        }

        public string[] GetValueNames()
        {
            return _values.Keys.ToArray();
        }

        public void DeleteValue(string name)
        {
            _values.Remove(name);
        }

        public void DeleteValue(string name, bool throwOnMissingValue)
        {
            _values.Remove(name);
        }

        public void DeleteSubKey(string subkey)
        {
            _subkeys.Remove(subkey);
        }

        public void DeleteSubKey(string subkey, bool throwOnMissingSubKey)
        {
            _subkeys.Remove(subkey);
        }

        public string Name => _name;
    }
}
