using System;

namespace SharpShell.SharpNamespaceExtension
{
    /// <summary>
    /// ShellNamespaceEnumerationFlags for an enumeration of shell items.
    /// </summary>
    [Flags]
    public enum ShellNamespaceEnumerationFlags
    {
        /// <summary>
        /// The enumeration must include folders.
        /// </summary>
        Folders = 1,

        /// <summary>
        /// The enumeration must include items.
        /// </summary>
        Items = 2
    }
}