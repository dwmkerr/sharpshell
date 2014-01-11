using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// This structure is used with the SHCreateShellFolderView function.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SFV_CREATE
    {
        /// <summary>
        /// The size of the SFV_CREATE structure, in bytes.
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// The IShellFolder interface of the folder for which to create the view.
        /// </summary>
        public IShellFolder pshf;

        /// <summary>
        /// A pointer to the parent IShellView interface. This parameter may be NULL. This parameter is used only when the view created by SHCreateShellFolderView is hosted in a common dialog box.
        /// </summary>
        public IShellView psvOuter;

        /// <summary>
        /// A pointer to the IShellFolderViewCB interface that handles the view's callbacks when various events occur. This parameter may be NULL.
        /// </summary>
        public IShellFolderViewCB psfvcb;
    }
}