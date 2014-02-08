namespace SharpShell.Interop
{
// ReSharper disable InconsistentNaming

    /// <summary>
    /// Specifies column format flags.
    /// </summary>
    public enum LVCFMT
    {
        /// <summary>
        /// Left aligned.
        /// </summary>
        LVCFMT_LEFT = 0x0000,

        /// <summary>
        /// Right aligned.
        /// </summary>
        LVCFMT_RIGHT = 0x0001,

        /// <summary>
        /// Centered
        /// </summary>
        LVCFMT_CENTER = 0x0002,

        /// <summary>
        /// The column has an icon.
        /// </summary>
        LVCFMT_COL_HAS_IMAGES = 0x8000
    }

// ReSharper restore InconsistentNaming
}