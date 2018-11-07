using System;
using Microsoft.Win32;
using SharpShell.Registry;

namespace SharpShell.Tests
{
    public class InMemoryRegistry : IRegistry
    {
        public InMemoryRegistry()
        {
            CurrentUser = new InMemoryRegistryKey("HKEY_CURRENT_USER");
            CurrentUser = new InMemoryRegistryKey("HKEY_LOCAL_MACHINE");
            CurrentUser = new InMemoryRegistryKey("HKEY_CLASSES_ROOT");
            CurrentUser = new InMemoryRegistryKey("HKEY_USERS");
            CurrentUser = new InMemoryRegistryKey("HKEY_PERFORMANCE_DATA");
            CurrentUser = new InMemoryRegistryKey("HKEY_CURRENT_CONFIG");
        }

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
                case RegistryHive.DynData:
                    return DynamicData;
                default:
                    throw new ArgumentOutOfRangeException(nameof(hKey), hKey, null);
            }
        }

        public IRegistryKey CurrentUser { get; }
        public IRegistryKey LocalMachine { get; }
        public IRegistryKey ClassesRoot { get; }
        public IRegistryKey Users { get; }
        public IRegistryKey PerformanceData { get; }
        public IRegistryKey CurrentConfig { get; }
        public IRegistryKey DynamicData { get; }
    }
}