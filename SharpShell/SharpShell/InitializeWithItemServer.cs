using System;
using SharpShell.Interop;

namespace SharpShell
{
    /// <summary>
    /// InitializeWithItemServer provides a base for SharpShell Servers that must implement
    /// IInitializeWithItem (thumbnail handlers which need a display name or path for initialisation etc).
    /// Note that if possible, <see cref="InitializeWithStreamServer"/> should be used, as it is far more
    /// performant.
    /// </summary>
    public abstract class InitializeWithItemServer : SharpShellServer, IInitializeWithItem
    {
        #region Implementation of IInitializeWithItem

        public int Initialize(IShellItem shellItem, STGM accessMode)
        {
            Log($"Intiailising a shell item server with mode '{accessMode}'.");

            //  Store the item and mode.
            SelectedShellItem = shellItem;
            SelectedShellItemAccessMode = accessMode;

            //  Return success.
            return WinError.S_OK;
        }

        #endregion

        /// <summary>
        /// Gets the selected shell item.
        /// </summary>
        /// <value>
        /// The selected shell item.
        /// </value>
        public IShellItem SelectedShellItem { get; private set; }

        /// <summary>
        ///  Gets the selected shell item access mode.
        /// </summary>
        /// <value>The selected shell item access mode.</value>
        public STGM SelectedShellItemAccessMode { get; private set; }

    }
}