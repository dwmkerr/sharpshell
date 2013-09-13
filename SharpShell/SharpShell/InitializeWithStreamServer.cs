using System.Runtime.InteropServices.ComTypes;
using SharpShell.Helpers;
using SharpShell.Interop;

namespace SharpShell
{
    /// <summary>
    /// InitializeWithStreamServer provides a base for SharpShell Servers that must implement
    /// IInitializeWithStream (thumbnail handlers, etc).
    /// </summary>
    public abstract class InitializeWithStreamServer : SharpShellServer, IInitializeWithStream
    {
        #region Implementation of IInitializeWithStream

        /// <summary>
        /// Initializes a handler with a stream.
        /// </summary>
        /// <param name="pstream">A pointer to an IStream interface that represents the stream source.</param>
        /// <param name="grfMode">One of the following STGM values that indicates the access mode for pstream. STGM_READ or STGM_READWRITE.</param>
        public int Initialize(IStream pstream, uint grfMode)
        {
            Log("Intiailising a stream based server.");

            //  Set the selected item stream.
            SelectedItemStream = new ComStream(pstream);

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
        public ComStream SelectedItemStream { get; private set; }
    }
}