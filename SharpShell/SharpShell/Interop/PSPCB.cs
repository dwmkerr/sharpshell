namespace SharpShell.Interop
{
    /// <summary>
    /// Action flag.
    /// </summary>
    public enum PSPCB : uint
    {
        /// <summary>
        /// Version 5.80 or later. A page is being created. The return value is not used.
        /// </summary>
        PSPCB_ADDREF = 0,

        /// <summary>
        /// A page is being destroyed. The return value is ignored.
        /// </summary>
        PSPCB_RELEASE = 1,

        /// <summary>
        /// A dialog box for a page is being created. Return nonzero to allow it to be created, or zero to prevent it.
        /// </summary>
        PSPCB_CREATE = 2
    }
}