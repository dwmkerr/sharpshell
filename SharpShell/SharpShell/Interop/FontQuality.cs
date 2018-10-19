namespace SharpShell.Interop
{
    /// <summary>
    /// The output quality. The output quality defines how carefully GDI must attempt to match the logical-font attributes to those of an actual physical font. It can be one of the following values.
    /// </summary>
    public enum FontQuality : byte
    {
        /// <summary>
        /// Appearance of the font does not matter.
        /// </summary>
        DEFAULT_QUALITY = 0,

        /// <summary>
        /// Appearance of the font is less important than when the PROOF_QUALITY value is used. For GDI raster fonts, scaling is enabled, which means that more font sizes are available, but the quality may be lower. Bold, italic, underline, and strikeout fonts are synthesized, if necessary.
        /// </summary>
        DRAFT_QUALITY = 1,

        /// <summary>
        /// Character quality of the font is more important than exact matching of the logical-font attributes. For GDI raster fonts, scaling is disabled and the font closest in size is chosen. Although the chosen font size may not be mapped exactly when PROOF_QUALITY is used, the quality of the font is high and there is no distortion of appearance. Bold, italic, underline, and strikeout fonts are synthesized, if necessary.
        /// </summary>
        PROOF_QUALITY = 2,

        /// <summary>
        /// Font is never antialiased, that is, font smoothing is not done.
        /// </summary>
        NONANTIALIASED_QUALITY = 3,

        /// <summary>
        /// Font is antialiased, or smoothed, if the font supports it and the size of the font is not too small or too large.
        /// </summary>
        ANTIALIASED_QUALITY = 4,

        /// <summary>
        /// If set, text is rendered (when possible) using ClearType antialiasing method. See Remarks for more information.
        /// </summary>
        CLEARTYPE_QUALITY = 5,

        /// <summary>
        /// The cleartype natural quality value.
        /// </summary>
        CLEARTYPE_NATURAL_QUALITY = 6,
    }
}