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
    }
}