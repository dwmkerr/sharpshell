using SharpShell.Interop;

namespace SharpShell
{
    /// <summary>
    /// InitializeWithFileServer provides a base for SharpShell Servers that must implement
    /// IInitializeWithItem (thumbnail handlers which need a file path for initialisation etc).
    /// Note that if possible, <see cref="InitializeWithStreamServer"/> should be used, as it is far more
    /// performant.
    /// Note that in Windows Vista onwards, implementations of this interface may not behave as expected as
    /// it has been essentially deprecated for security reasons. For example, a Shell Thumbnail Handler which
    /// implements this interface will *not* be called.
    /// </summary>
    public abstract class InitializeWithFileServer : SharpShellServer, IInitializeWithFile
    {
        #region Implementation of IInitializeWithFile

        public int Initialize(string pszFilePath, STGM grfMode)
        {
            Log($"Intiailising a file based server for '{pszFilePath}' with mode '{grfMode}'.");

            //  Store the path and mode.
            SelectedItemPath = pszFilePath;
            SelectedItemStorageMedium = grfMode;

            //  Return success.
            return WinError.S_OK;
        }

        #endregion

        /// <summary>
        /// Gets the selected item stream.
        /// </summary>
        /// <value>
        /// The selected item stream.
        /// </value>
        public string SelectedItemPath { get; private set; }

        /// <summary>Gets the selected item storage medium.</summary>
        /// <value>The selected item storage medium.</value>
        public STGM SelectedItemStorageMedium { get; private set; }
    }
}