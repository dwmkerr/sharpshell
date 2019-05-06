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
        private const char Separator = '\\';
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
            return OpenSubKey(name, RegistryKeyPermissionCheck.ReadSubTree);
        }

        /// <inheritdoc />
        public IRegistryKey OpenSubKey(string name, bool writable)
        {
            return OpenSubKey(name,
                writable ? RegistryKeyPermissionCheck.ReadWriteSubTree : RegistryKeyPermissionCheck.ReadSubTree, RegistryRights.FullControl);
        }

        /// <inheritdoc />
        public IRegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck)
        {
            return OpenSubKey(name, permissionCheck, RegistryRights.FullControl);
        }

        /// <inheritdoc />
        public IRegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck, RegistryRights rights)
        {
            var currentKey = this;
            var subkeyNames = name.Split(Separator);
            foreach (var subkeyName in subkeyNames)
            {
                var subkeyNameLower = subkeyName.ToLower();
                if (currentKey._subkeys.ContainsKey(subkeyNameLower) == false)
                    return null;
                currentKey = currentKey._subkeys[subkeyNameLower];
            }

            return currentKey;
        }

        /// <inheritdoc />
        public object GetValue(string name)
        {
            return GetValue(name, null, RegistryValueOptions.None);
        }

        /// <inheritdoc />
        public object GetValue(string name, object defaultValue)
        {
            return GetValue(name, defaultValue, RegistryValueOptions.None);
        }

        /// <inheritdoc />
        public object GetValue(string name, object defaultValue, RegistryValueOptions options)
        {
            //  Coerce null into the empty string (i.e. '(Default)' value in the registry).
            var valueName = name ?? string.Empty;
            return _values.TryGetValue(valueName, out var value) ? value : defaultValue;
        }

        /// <inheritdoc />
        public IRegistryKey CreateSubKey(string subkey)
        {
            return CreateSubKey(subkey, RegistryKeyPermissionCheck.Default, new RegistrySecurity());
        }

        /// <inheritdoc />
        public IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck)
        {
            return CreateSubKey(subkey, permissionCheck, new RegistrySecurity());
        }

        /// <inheritdoc />
        public IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity)
        {
            var currentKey = this;
            var subkeyNames = subkey.Split(Separator);
            foreach (var subkeyName in subkeyNames)
            {
                var subkeyNameLower = subkeyName.ToLower();
                if (currentKey._subkeys.ContainsKey(subkeyNameLower) == false)
                    currentKey._subkeys[subkeyNameLower] = new InMemoryRegistryKey(_view, subkeyName);
                currentKey = currentKey._subkeys[subkeyNameLower];
            }

            return currentKey;
        }

        /// <inheritdoc />
        public void SetValue(string name, object value)
        {
            //  Coerce null into the empty string (i.e. '(Default)' value in the registry).
            var valueName = name ?? string.Empty;
            _values[valueName] = value;
        }

        /// <inheritdoc />
        public void SetValue(string name, object value, RegistryValueKind valueKind)
        {
            //  Coerce null into the empty string (i.e. '(Default)' value in the registry).
            var valueName = name ?? string.Empty;
            _values[valueName] = value;
        }

        /// <inheritdoc />
        public void DeleteSubKeyTree(string subkey)
        {
            DeleteSubKeyTree(subkey, true);
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
            DeleteValue(name, true);
        }

        /// <inheritdoc />
        public void DeleteValue(string name, bool throwOnMissingValue)
        {
            _values.Remove(name);
        }

        /// <inheritdoc />
        public void DeleteSubKey(string subkey)
        {
            DeleteSubKey(subkey, true);
        }

        /// <inheritdoc />
        public void DeleteSubKey(string subkey, bool throwOnMissingSubKey)
        {
            _subkeys.Remove(subkey);
        }

        /// <inheritdoc />
        public string Name => _name;

        /// <summary>
        /// Gets the view for the key.
        /// </summary>
        public RegistryView View => _view;
    }
}
