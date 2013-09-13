using System;

namespace SharpShell.Interop
{
    /// <summary>
    /// Flags specifying the information to return. This parameter can have one of the following values.
    /// </summary>
    [Flags]
    public enum GCS : uint
    {
        /// <summary>
        /// Sets pszName to an ANSI string containing the language-independent command name for the menu item.
        /// </summary>
        GCS_VERBA = 0x00000000,

        /// <summary>
        /// Sets pszName to an ANSI string containing the help text for the command.
        /// </summary>
        GCS_HELPTEXTA = 0x00000001,

        /// <summary>
        /// Returns S_OK if the menu item exists, or S_FALSE otherwise.
        /// </summary>
        GCS_VALIDATEA = 0x00000002,

        /// <summary>
        /// Sets pszName to a Unicode string containing the language-independent command name for the menu item.
        /// </summary>
        GCS_VERBW = 0x00000004,

        /// <summary>
        /// Sets pszName to a Unicode string containing the help text for the command.
        /// </summary>
        GCS_HELPTEXTW = 0x00000005,

        /// <summary>
        /// Returns S_OK if the menu item exists, or S_FALSE otherwise.
        /// </summary>
        GCS_VALIDATEW = 0x00000006,

        /// <summary>
        /// Not documented.
        /// </summary>
        GCS_VERBICONW = 0x00000014,

        /// <summary>
        /// Not documented.
        /// </summary>
        GCS_UNICODE = 0x00000004
    }
}