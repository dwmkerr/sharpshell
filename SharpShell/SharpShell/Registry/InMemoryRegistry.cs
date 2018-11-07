using System;
using Microsoft.Win32;

namespace SharpShell.Registry
{
    /// <summary>
    /// The In-Memory registry implements <see cref="IRegistry"/> with a simple in-memory structure.
    /// It is designed to support testing scenarios, for example, asserting the ServerRegistrationManager
    /// can correctly register differnt types of servers.
    /// </summary>
    /// <seealso cref="SharpShell.Registry.IRegistry" />
    public class InMemoryRegistry : IRegistry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryRegistry"/> class.
        /// </summary>
        public InMemoryRegistry()
        {
            CurrentUser = new InMemoryRegistryKey("HKEY_CURRENT_USER");
            LocalMachine = new InMemoryRegistryKey("HKEY_LOCAL_MACHINE");
            ClassesRoot = new InMemoryRegistryKey("HKEY_CLASSES_ROOT");
            Users = new InMemoryRegistryKey("HKEY_USERS");
            PerformanceData = new InMemoryRegistryKey("HKEY_PERFORMANCE_DATA");
            CurrentConfig = new InMemoryRegistryKey("HKEY_CURRENT_CONFIG");
            CurrentConfig = new InMemoryRegistryKey("HKEY_CURRENT_CONFIG");
        }

        /// <inheritdoc />
        public IRegistryKey OpenBaseKey(RegistryHive hKey, RegistryView view)
        {
            switch (hKey)
            {
                case RegistryHive.ClassesRoot:
                    return ClassesRoot;
                case RegistryHive.CurrentUser:
                    return CurrentUser;
                case RegistryHive.LocalMachine:
                    return LocalMachine;
                case RegistryHive.Users:
                    return Users;
                case RegistryHive.PerformanceData:
                    return PerformanceData;
                case RegistryHive.CurrentConfig:
                    return CurrentConfig;
                default:
                    throw new ArgumentOutOfRangeException(nameof(hKey), hKey, null);
            }
        }

        /// <inheritdoc />
        public IRegistryKey CurrentUser { get; }

        /// <inheritdoc />
        public IRegistryKey LocalMachine { get; }

        /// <inheritdoc />
        public IRegistryKey ClassesRoot { get; }

        /// <inheritdoc />
        public IRegistryKey Users { get; }

        /// <inheritdoc />
        public IRegistryKey PerformanceData { get; }

        /// <inheritdoc />
        public IRegistryKey CurrentConfig { get; }
    }
}