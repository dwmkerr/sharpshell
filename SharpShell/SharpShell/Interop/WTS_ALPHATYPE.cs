namespace SharpShell.Interop
{
    /// <summary>
    /// Alpha channel type information.
    /// </summary>
    public enum WTS_ALPHATYPE
    {
        /// <summary>
        /// The bitmap is an unknown format. The Shell tries nonetheless to detect whether the image has an alpha channel.
        /// </summary>
        WTSAT_UNKNOWN = 0x0,

        /// <summary>
        /// The bitmap is an RGB image without alpha. The alpha channel is invalid and the Shell ignores it.
        /// </summary>
        WTSAT_RGB = 0x1,

        /// <summary>
        /// The bitmap is an ARGB image with a valid alpha channel.
        /// </summary>
        WTSAT_ARGB = 0x2
    }
}