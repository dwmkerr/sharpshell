using System.Runtime.InteropServices;
using System.Text;
#pragma warning disable 1591

namespace SharpShell.Interop
{
    /// <summary>
    /// The LOGFONT structure defines the attributes of a font.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class LOGFONT
    {
        /// <summary>
        /// The height, in logical units, of the font's character cell or character. The character height value (also known as the em height) is the character cell height value minus the internal-leading value.
        /// </summary>
        public int lfHeight;

        /// <summary>
        /// The average width, in logical units, of characters in the font. If lfWidth is zero, the aspect ratio of the device is matched against the digitization aspect ratio of the available fonts to find the closest match, determined by the absolute value of the difference.
        /// </summary>
        public int lfWidth;

        /// <summary>
        /// The angle, in tenths of degrees, between the escapement vector and the x-axis of the device. The escapement vector is parallel to the base line of a row of text.
        /// </summary>
        public int lfEscapement;

        /// <summary>
        /// The angle, in tenths of degrees, between each character's base line and the x-axis of the device.
        /// </summary>
        public int lfOrientation;

        /// <summary>
        /// The weight of the font in the range 0 through 1000. For example, 400 is normal and 700 is bold. If this value is zero, a default weight is used.
        /// </summary>
        public FontWeight lfWeight;

        /// <summary>
        /// An italic font if set to TRUE.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool lfItalic;

        /// <summary>
        /// An underlined font if set to TRUE.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool lfUnderline;

        /// <summary>
        /// A strikeout font if set to TRUE.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool lfStrikeOut;

        /// <summary>
        /// The character set.
        /// </summary>
        public FontCharSet lfCharSet;

        /// <summary>
        /// The output precision. The output precision defines how closely the output must match the requested font's height, width, character orientation, escapement, pitch, and font type.
        /// </summary>
        public FontPrecision lfOutPrecision;

        /// <summary>
        /// The clipping precision. The clipping precision defines how to clip characters that are partially outside the clipping region.
        /// </summary>
        public FontClipPrecision lfClipPrecision;

        /// <summary>
        /// The output quality. The output quality defines how carefully the graphics device interface (GDI) must attempt to match the logical-font attributes to those of an actual physical font
        /// </summary>
        public FontQuality lfQuality;

        /// <summary>
        /// The pitch and family of the font.
        /// </summary>
        public FontPitchAndFamily lfPitchAndFamily;

        /// <summary>
        /// A null-terminated string that specifies the typeface name of the font. The length of this string must not exceed 32 TCHAR values, including the terminating NULL. The EnumFontFamiliesEx function can be used to enumerate the typeface names of all currently available fonts. If lfFaceName is an empty string, GDI uses the first font that matches the other specified attributes.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string lfFaceName;
    }
}