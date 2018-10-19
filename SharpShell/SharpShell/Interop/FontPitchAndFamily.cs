using System;

namespace SharpShell.Interop
{
    /// <summary>
    /// The pitch and family of the font. The two low-order bits specify the pitch of the font, the four high-order bits specify the font family.
    /// </summary>
    [Flags]
    public enum FontPitchAndFamily : byte
    {
        /// <summary>
        /// The default pitch.
        /// </summary>
        DEFAULT_PITCH = 0,

        /// <summary>
        /// The fixed pitch.
        /// </summary>
        FIXED_PITCH = 1,

        /// <summary>
        /// The variable pitch.
        /// </summary>
        VARIABLE_PITCH = 2,

        /// <summary>
        /// Use default font.
        /// </summary>
        FF_DONTCARE = (0 << 4),

        /// <summary>
        /// Fonts with variable stroke width and with serifs. MS Serif is an example.
        /// </summary>
        FF_ROMAN = (1 << 4),

        /// <summary>
        /// Fonts with variable stroke width and without serifs. MS?Sans Serif is an example.
        /// </summary>
        FF_SWISS = (2 << 4),

        /// <summary>
        /// Fonts with constant stroke width, with or without serifs. Pica, Elite, and Courier New are examples.
        /// </summary>
        FF_MODERN = (3 << 4),

        /// <summary>
        /// Fonts designed to look like handwriting. Script and Cursive are examples.
        /// </summary>
        FF_SCRIPT = (4 << 4),

        /// <summary>
        /// Novelty fonts. Old English is an example.
        /// </summary>
        FF_DECORATIVE = (5 << 4),
    }
}