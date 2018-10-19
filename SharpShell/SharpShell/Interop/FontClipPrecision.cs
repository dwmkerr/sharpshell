namespace SharpShell.Interop
{
    /// <summary>
    /// The clipping precision. The clipping precision defines how to clip characters that are partially outside the clipping region. It can be one or more of the following values.
    /// </summary>
    public enum FontClipPrecision : byte
    {
        /// <summary>
        /// Specifies default clipping behavior.
        /// </summary>
        CLIP_DEFAULT_PRECIS = 0,

        /// <summary>
        /// Not used.
        /// </summary>
        CLIP_CHARACTER_PRECIS = 1,

        /// <summary>
        /// Not used by the font mapper, but is returned when raster, vector, or TrueType fonts are enumerated.
        /// For compatibility, this value is always returned when enumerating fonts.
        /// </summary>
        CLIP_STROKE_PRECIS = 2,

        /// <summary>
        /// Not used.
        /// </summary>
        CLIP_MASK = 0xf,

        /// <summary>
        /// When this value is used, the rotation for all fonts depends on whether the orientation of the coordinate system is left-handed or right-handed.
        /// If not used, device fonts always rotate counterclockwise, but the rotation of other fonts is dependent on the orientation of the coordinate system.
        /// For more information about the orientation of coordinate systems, see the description of the nOrientation parameter
        /// </summary>
        CLIP_LH_ANGLES = (1 << 4),

        /// <summary>
        /// Not used.
        /// </summary>
        CLIP_TT_ALWAYS = (2 << 4),

        /// <summary>
        /// Windows XP SP1: Turns off font association for the font. Note that this flag is not guaranteed to have any effect on any platform after Windows Server 2003.
        /// </summary>
        CLIP_DFA_DISABLE = (4 << 4),

        /// <summary>
        /// You must specify this flag to use an embedded read-only font.
        /// </summary>
        CLIP_EMBEDDED = (8 << 4),
    }
}