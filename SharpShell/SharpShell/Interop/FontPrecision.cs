namespace SharpShell.Interop
{
    /// <summary>
    /// The output precision. The output precision defines how closely the output must match the requested font's height, width, character orientation, escapement, pitch, and font type. It can be one of the following values.
    /// </summary>
    public enum FontPrecision : byte
    {
        /// <summary>
        /// The default font mapper behavior.
        /// </summary>
        OUT_DEFAULT_PRECIS = 0,

        /// <summary>
        /// This value is not used by the font mapper, but it is returned when raster fonts are enumerated.
        /// </summary>
        OUT_STRING_PRECIS = 1,

        /// <summary>
        /// Not used.
        /// </summary>
        OUT_CHARACTER_PRECIS = 2,

        /// <summary>
        /// This value is not used by the font mapper, but it is returned when TrueType, other outline-based fonts, and vector fonts are enumerated.
        /// </summary>
        OUT_STROKE_PRECIS = 3,

        /// <summary>
        /// Instructs the font mapper to choose a TrueType font when the system contains multiple fonts with the same name.
        /// </summary>
        OUT_TT_PRECIS = 4,

        /// <summary>
        /// Instructs the font mapper to choose a Device font when the system contains multiple fonts with the same name.
        /// </summary>
        OUT_DEVICE_PRECIS = 5,

        /// <summary>
        /// Instructs the font mapper to choose a raster font when the system contains multiple fonts with the same name.
        /// </summary>
        OUT_RASTER_PRECIS = 6,

        /// <summary>
        /// Instructs the font mapper to choose from only TrueType fonts. If there are no TrueType fonts installed in the system, the font mapper returns to default behavior.
        /// </summary>
        OUT_TT_ONLY_PRECIS = 7,

        /// <summary>
        /// This value instructs the font mapper to choose from TrueType and other outline-based fonts.
        /// </summary>
        OUT_OUTLINE_PRECIS = 8,

        /// <summary>
        /// A value that specifies a preference for TrueType and other outline fonts.
        /// </summary>
        OUT_SCREEN_OUTLINE_PRECIS = 9,

        /// <summary>
        /// Instructs the font mapper to choose from only PostScript fonts. If there are no PostScript fonts installed in the system, the font mapper returns to default behavior.
        /// </summary>
        OUT_PS_ONLY_PRECIS = 10,
    }
}