using Microsoft.Win32;

namespace SharpShell.Registry
{
    /// <summary>
    /// An interface for any type which can provide access to the registry. In most circumstances, the implementation
    /// will be <see cref="WindowsRegistry"/>. This service exists to support effective testing of registry access. 
    /// </summary>
    public interface IRegistry
    {
        /// <summary>Opens a new <see cref="IRegistryKey"/> that represents the requested key on the local machine with the specified view.</summary>
        /// <returns>The requested registry key.</returns>
        /// <param name="hKey">The HKEY to open.</param>
        /// <param name="view">The registry view to use.</param>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="hKey" /> or <paramref name="view" /> is invalid.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
        /// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to perform this action.</exception>
        IRegistryKey OpenBaseKey(RegistryHive hKey, RegistryView view);

        /// <summary>Contains information about the current user preferences. This field reads the Windows registry base key HKEY_CURRENT_USER </summary>
        IRegistryKey CurrentUser { get; }

        /// <summary>Contains the configuration data for the local machine. This field reads the Windows registry base key HKEY_LOCAL_MACHINE.</summary>
        IRegistryKey LocalMachine { get; }

        /// <summary>Defines the types (or classes) of documents and the properties associated with those types. This field reads the Windows registry base key HKEY_CLASSES_ROOT.</summary>
        IRegistryKey ClassesRoot { get; }

        /// <summary>Contains information about the default user configuration. This field reads the Windows registry base key HKEY_USERS.</summary>
        IRegistryKey Users { get; }

        /// <summary>Contains performance information for software components. This field reads the Windows registry base key HKEY_PERFORMANCE_DATA.</summary>
        IRegistryKey PerformanceData { get; }

        /// <summary>Contains configuration information pertaining to the hardware that is not specific to the user. This field reads the Windows registry base key HKEY_CURRENT_CONFIG.</summary>
        IRegistryKey CurrentConfig { get; }

    }
}