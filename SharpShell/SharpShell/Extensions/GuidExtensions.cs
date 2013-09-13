using System;

namespace SharpShell.Extensions
{
    /// <summary>
    /// Extensions for the Guid type.
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        /// Returns the GUID as a string suitable for the registry.
        /// </summary>
        /// <param name="this">The Guid.</param>
        /// <returns>The GUID as a string suitable for the registry</returns>
        public static string ToRegistryString(this Guid @this)
        {
            //  Return formatted for the registry.
            return @this.ToString("B");
        }
    }
}
