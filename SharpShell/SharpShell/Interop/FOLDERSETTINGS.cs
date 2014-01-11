using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Contains folder view information.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FOLDERSETTINGS
    {
        /// <summary>
        /// Folder view mode. One of the FOLDERVIEWMODE values.
        /// </summary>
        private FOLDERVIEWMODE ViewMode;

        /// <summary>
        /// A set of flags that indicate the options for the folder. This can be zero or a combination of the FOLDERFLAGS values.
        /// </summary>
        private FOLDERFLAGS fFlags;
    }
}