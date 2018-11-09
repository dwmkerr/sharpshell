using System;
using System.Security.AccessControl;
using Microsoft.Win32;

namespace SharpShell.Registry
{
    /// <summary>
    /// Interface which represents a key-level node in the Windows registry. This class is a registry encapsulation.
    /// </summary>
    public interface IRegistryKey : IDisposable
    {
        /// <summary>Retrieves an array of strings that contains all the subkey names.</summary>
        /// <returns>An array of strings that contains the names of the subkeys for the current key.</returns>
        /// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to read from the key. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being manipulated is closed (closed keys cannot be accessed). </exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
        /// <exception cref="T:System.IO.IOException">A system error occurred, for example the current key has been deleted.</exception>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        string[] GetSubKeyNames();

        /// <summary>Retrieves a subkey as read-only.</summary>
        /// <returns>The subkey requested, or null if the operation failed.</returns>
        /// <param name="name">The name or path of the subkey to open as read-only. </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="name" /> is null</exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> is closed (closed keys cannot be accessed). </exception>
        /// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to read the registry key. </exception>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="\" />
        /// </PermissionSet>
        IRegistryKey OpenSubKey(string name);

        /// <summary>Retrieves a specified subkey, and specifies whether write access is to be applied to the key. </summary>
        /// <returns>The subkey requested, or null if the operation failed.</returns>
        /// <param name="name">Name or path of the subkey to open. </param>
        /// <param name="writable">Set to true if you need write access to the key. </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="name" /> is null. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> is closed (closed keys cannot be accessed). </exception>
        /// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to access the registry key in the specified mode. </exception>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        IRegistryKey OpenSubKey(string name, bool writable);

        /// <summary>Retrieves the specified subkey for read or read/write access.</summary>
        /// <returns>The subkey requested, or null if the operation failed.</returns>
        /// <param name="name">The name or path of the subkey to create or open.</param>
        /// <param name="permissionCheck">One of the enumeration values that specifies whether the key is opened for read or read/write access.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="name" /> is null</exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="permissionCheck" /> contains an invalid value.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> is closed (closed keys cannot be accessed). </exception>
        /// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to read the registry key. </exception>
        IRegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck);

        /// <summary>Retrieves the specified subkey for read or read/write access, requesting the specified access rights.</summary>
        /// <returns>The subkey requested, or null if the operation failed.</returns>
        /// <param name="name">The name or path of the subkey to create or open.</param>
        /// <param name="permissionCheck">One of the enumeration values that specifies whether the key is opened for read or read/write access.</param>
        /// <param name="rights">A bitwise combination of enumeration values that specifies the desired security access.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="name" /> is null</exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="permissionCheck" /> contains an invalid value.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> is closed (closed keys cannot be accessed). </exception>
        /// <exception cref="T:System.Security.SecurityException">
        /// <paramref name="rights" /> includes invalid registry rights values.-or-The user does not have the requested permissions. </exception>
        IRegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck, RegistryRights rights);

        /// <summary>Retrieves the value associated with the specified name. Returns null if the name/value pair does not exist in the registry.</summary>
        /// <returns>The value associated with <paramref name="name" />, or null if <paramref name="name" /> is not found.</returns>
        /// <param name="name">The name of the value to retrieve. This string is not case-sensitive.</param>
        /// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to read from the registry key. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> that contains the specified value is closed (closed keys cannot be accessed). </exception>
        /// <exception cref="T:System.IO.IOException">The <see cref="T:Microsoft.Win32.RegistryKey" /> that contains the specified value has been marked for deletion. </exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="\" />
        /// </PermissionSet>
        object GetValue(string name);

        /// <summary>Retrieves the value associated with the specified name. If the name is not found, returns the default value that you provide.</summary>
        /// <returns>The value associated with <paramref name="name" />, with any embedded environment variables left unexpanded, or <paramref name="defaultValue" /> if <paramref name="name" /> is not found.</returns>
        /// <param name="name">The name of the value to retrieve. This string is not case-sensitive.</param>
        /// <param name="defaultValue">The value to return if <paramref name="name" /> does not exist. </param>
        /// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to read from the registry key. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> that contains the specified value is closed (closed keys cannot be accessed). </exception>
        /// <exception cref="T:System.IO.IOException">The <see cref="T:Microsoft.Win32.RegistryKey" /> that contains the specified value has been marked for deletion. </exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="\" />
        /// </PermissionSet>
        object GetValue(string name, object defaultValue);

        /// <summary>Retrieves the value associated with the specified name and retrieval options. If the name is not found, returns the default value that you provide.</summary>
        /// <returns>The value associated with <paramref name="name" />, processed according to the specified <paramref name="options" />, or <paramref name="defaultValue" /> if <paramref name="name" /> is not found.</returns>
        /// <param name="name">The name of the value to retrieve. This string is not case-sensitive.</param>
        /// <param name="defaultValue">The value to return if <paramref name="name" /> does not exist. </param>
        /// <param name="options">One of the enumeration values that specifies optional processing of the retrieved value.</param>
        /// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to read from the registry key. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> that contains the specified value is closed (closed keys cannot be accessed). </exception>
        /// <exception cref="T:System.IO.IOException">The <see cref="T:Microsoft.Win32.RegistryKey" /> that contains the specified value has been marked for deletion. </exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="options" /> is not a valid <see cref="T:Microsoft.Win32.RegistryValueOptions" /> value; for example, an invalid value is cast to <see cref="T:Microsoft.Win32.RegistryValueOptions" />.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="\" />
        /// </PermissionSet>
        object GetValue(string name, object defaultValue, RegistryValueOptions options);

        /// <summary>Creates a new subkey or opens an existing subkey for write access.  </summary>
        /// <returns>The newly created subkey, or null if the operation failed. If a zero-length string is specified for <paramref name="subkey" />, the current <see cref="T:Microsoft.Win32.RegistryKey" /> object is returned.</returns>
        /// <param name="subkey">The name or path of the subkey to create or open. This string is not case-sensitive.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="subkey" /> is null. </exception>
        /// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to create or open the registry key. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> on which this method is being invoked is closed (closed keys cannot be accessed). </exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The <see cref="T:Microsoft.Win32.RegistryKey" /> cannot be written to; for example, it was not opened as a writable key , or the user does not have the necessary access rights. </exception>
        /// <exception cref="T:System.IO.IOException">The nesting level exceeds 510.-or-A system error occurred, such as deletion of the key, or an attempt to create a key in the <see cref="F:Microsoft.Win32.Registry.LocalMachine" /> root.</exception>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        IRegistryKey CreateSubKey(string subkey);

        /// <summary>Creates a new subkey or opens an existing subkey for write access, using the specified permission check option. </summary>
        /// <returns>The newly created subkey, or null if the operation failed. If a zero-length string is specified for <paramref name="subkey" />, the current <see cref="T:Microsoft.Win32.RegistryKey" /> object is returned.</returns>
        /// <param name="subkey">The name or path of the subkey to create or open. This string is not case-sensitive.</param>
        /// <param name="permissionCheck">One of the enumeration values that specifies whether the key is opened for read or read/write access.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="subkey" /> is null. </exception>
        /// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to create or open the registry key. </exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="permissionCheck" /> contains an invalid value.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> on which this method is being invoked is closed (closed keys cannot be accessed). </exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The <see cref="T:Microsoft.Win32.RegistryKey" /> cannot be written to; for example, it was not opened as a writable key, or the user does not have the necessary access rights. </exception>
        /// <exception cref="T:System.IO.IOException">The nesting level exceeds 510.-or-A system error occurred, such as deletion of the key, or an attempt to create a key in the <see cref="F:Microsoft.Win32.Registry.LocalMachine" /> root.</exception>
        IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck);

        /// <summary>Creates a new subkey or opens an existing subkey for write access, using the specified permission check option and registry security. </summary>
        /// <returns>The newly created subkey, or null if the operation failed. If a zero-length string is specified for <paramref name="subkey" />, the current <see cref="T:Microsoft.Win32.RegistryKey" /> object is returned.</returns>
        /// <param name="subkey">The name or path of the subkey to create or open. This string is not case-sensitive.</param>
        /// <param name="permissionCheck">One of the enumeration values that specifies whether the key is opened for read or read/write access.</param>
        /// <param name="registrySecurity">The access control security for the new key.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="subkey" /> is null. </exception>
        /// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to create or open the registry key. </exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="permissionCheck" /> contains an invalid value.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> on which this method is being invoked is closed (closed keys cannot be accessed). </exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The current <see cref="T:Microsoft.Win32.RegistryKey" /> cannot be written to; for example, it was not opened as a writable key, or the user does not have the necessary access rights.</exception>
        /// <exception cref="T:System.IO.IOException">The nesting level exceeds 510.-or-A system error occurred, such as deletion of the key, or an attempt to create a key in the <see cref="F:Microsoft.Win32.Registry.LocalMachine" /> root.</exception>
        IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck,
            RegistrySecurity registrySecurity);

        /// <summary>Sets the specified name/value pair.</summary>
        /// <param name="name">The name of the value to store. </param>
        /// <param name="value">The data to be stored. </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="value" /> is null. </exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="value" /> is an unsupported data type. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> that contains the specified value is closed (closed keys cannot be accessed). </exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The <see cref="T:Microsoft.Win32.RegistryKey" /> is read-only, and cannot be written to; for example, the key has not been opened with write access. -or-The <see cref="T:Microsoft.Win32.RegistryKey" /> object represents a root-level node, and the operating system is Windows Millennium Edition or Windows 98.</exception>
        /// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to create or modify registry keys. </exception>
        /// <exception cref="T:System.IO.IOException">The <see cref="T:Microsoft.Win32.RegistryKey" /> object represents a root-level node, and the operating system is Windows 2000, Windows XP, or Windows Server 2003.</exception>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        void SetValue(string name, object value);

        /// <summary>Sets the value of a name/value pair in the registry key, using the specified registry data type.</summary>
        /// <param name="name">The name of the value to be stored. </param>
        /// <param name="value">The data to be stored. </param>
        /// <param name="valueKind">The registry data type to use when storing the data. </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="value" /> is null. </exception>
        /// <exception cref="T:System.ArgumentException">The type of <paramref name="value" /> did not match the registry data type specified by <paramref name="valueKind" />, therefore the data could not be converted properly. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> that contains the specified value is closed (closed keys cannot be accessed). </exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The <see cref="T:Microsoft.Win32.RegistryKey" /> is read-only, and cannot be written to; for example, the key has not been opened with write access.-or-The <see cref="T:Microsoft.Win32.RegistryKey" /> object represents a root-level node, and the operating system is Windows Millennium Edition or Windows 98. </exception>
        /// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to create or modify registry keys. </exception>
        /// <exception cref="T:System.IO.IOException">The <see cref="T:Microsoft.Win32.RegistryKey" /> object represents a root-level node, and the operating system is Windows 2000, Windows XP, or Windows Server 2003.</exception>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        void SetValue(string name, object value, RegistryValueKind valueKind);

        /// <summary>Deletes a subkey and any child subkeys recursively. </summary>
        /// <param name="subkey">The subkey to delete. This string is not case-sensitive.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="subkey" /> is null. </exception>
        /// <exception cref="T:System.ArgumentException">Deletion of a root hive is attempted.-or-<paramref name="subkey" /> does not specify a valid registry subkey. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error has occurred.</exception>
        /// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to delete the key. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being manipulated is closed (closed keys cannot be accessed). </exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        void DeleteSubKeyTree(string subkey);

        /// <summary>Deletes the specified subkey and any child subkeys recursively, and specifies whether an exception is raised if the subkey is not found. </summary>
        /// <param name="subkey">The name of the subkey to delete. This string is not case-sensitive.</param>
        /// <param name="throwOnMissingSubKey">Indicates whether an exception should be raised if the specified subkey cannot be found. If this argument is true and the specified subkey does not exist, an exception is raised. If this argument is false and the specified subkey does not exist, no action is taken.</param>
        /// <exception cref="T:System.ArgumentException">An attempt was made to delete the root hive of the tree.-or-<paramref name="subkey" /> does not specify a valid registry subkey, and <paramref name="throwOnMissingSubKey" /> is true.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="subkey" /> is null.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> is closed (closed keys cannot be accessed).</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
        /// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to delete the key.</exception>
        void DeleteSubKeyTree(string subkey, bool throwOnMissingSubKey);

        /// <summary>Retrieves an array of strings that contains all the value names associated with this key.</summary>
        /// <returns>An array of strings that contains the value names for the current key.</returns>
        /// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to read from the registry key. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" />  being manipulated is closed (closed keys cannot be accessed). </exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
        /// <exception cref="T:System.IO.IOException">A system error occurred; for example, the current key has been deleted.</exception>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        string[] GetValueNames();

        /// <summary>Deletes the specified value from this key.</summary>
        /// <param name="name">The name of the value to delete. </param>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="name" /> is not a valid reference to a value. </exception>
        /// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to delete the value. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being manipulated is closed (closed keys cannot be accessed). </exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being manipulated is read-only. </exception>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        void DeleteValue(string name);

        /// <summary>Deletes the specified value from this key, and specifies whether an exception is raised if the value is not found.</summary>
        /// <param name="name">The name of the value to delete. </param>
        /// <param name="throwOnMissingValue">Indicates whether an exception should be raised if the specified value cannot be found. If this argument is true and the specified value does not exist, an exception is raised. If this argument is false and the specified value does not exist, no action is taken. </param>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="name" /> is not a valid reference to a value and <paramref name="throwOnMissingValue" /> is true. -or- <paramref name="name" /> is null.</exception>
        /// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to delete the value. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being manipulated is closed (closed keys cannot be accessed). </exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being manipulated is read-only. </exception>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        void DeleteValue(string name, bool throwOnMissingValue);

        /// <summary>Deletes the specified subkey. </summary>
        /// <param name="subkey">The name of the subkey to delete. This string is not case-sensitive.</param>
        /// <exception cref="T:System.InvalidOperationException">The <paramref name="subkey" /> has child subkeys </exception>
        /// <exception cref="T:System.ArgumentException">The <paramref name="subkey" /> parameter does not specify a valid registry key </exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="subkey" /> is null</exception>
        /// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to delete the key. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being manipulated is closed (closed keys cannot be accessed). </exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        void DeleteSubKey(string subkey);

        /// <summary>Deletes the specified subkey, and specifies whether an exception is raised if the subkey is not found. </summary>
        /// <param name="subkey">The name of the subkey to delete. This string is not case-sensitive.</param>
        /// <param name="throwOnMissingSubKey">Indicates whether an exception should be raised if the specified subkey cannot be found. If this argument is true and the specified subkey does not exist, an exception is raised. If this argument is false and the specified subkey does not exist, no action is taken. </param>
        /// <exception cref="T:System.InvalidOperationException">
        /// <paramref name="subkey" /> has child subkeys. </exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="subkey" /> does not specify a valid registry key, and <paramref name="throwOnMissingSubKey" /> is true. </exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="subkey" /> is null.</exception>
        /// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to delete the key. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> being manipulated is closed (closed keys cannot be accessed). </exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The user does not have the necessary registry rights.</exception>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
        /// </PermissionSet>
        void DeleteSubKey(string subkey, bool throwOnMissingSubKey);
        
        /// <summary>Retrieves the name of the key.</summary>
        /// <returns>The absolute (qualified) name of the key.</returns>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Microsoft.Win32.RegistryKey" /> is closed (closed keys cannot be accessed). </exception>
        string Name { get; }
    }
}
