using System;
using SharpShell.Extensions;

namespace SharpShell.SharpNamespaceExtension
{
    /// <summary>
    /// Specifies the registry key for an enumeration member.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class RegistryKeyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryKeyAttribute"/> class.
        /// </summary>
        /// <param name="registryKey">The registry key.</param>
        public RegistryKeyAttribute(string registryKey)
        {
            RegistryKey = registryKey;
        }

        /// <summary>
        /// Gets the registry key for an enumeration member, or null if it is not set.
        /// </summary>
        /// <param name="enum">The enumeration member.</param>
        /// <returns>The registry key attribute for the member, or null if it is not set.</returns>
        public static string GetRegistryKey(Enum @enum)
        {
            var registryKeyAttribute = @enum.GetAttribute<RegistryKeyAttribute>();
            return registryKeyAttribute != null ? registryKeyAttribute.RegistryKey : null;
        }

        /// <summary>
        /// Gets the registry key.
        /// </summary>
        /// <value>
        /// The registry key.
        /// </value>
        public string RegistryKey { get; private set; }
    }
}