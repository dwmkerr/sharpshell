using System.Runtime.InteropServices;
using System.Text;

namespace SharpShell.Interop
{
    /// <summary>
    /// The LOGFONT structure defines the attributes of a font.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class LOGFONT
    {
        public int lfHeight;
        public int lfWidth;
        public int lfEscapement;
        public int lfOrientation;
        public FontWeight lfWeight;
        [MarshalAs(UnmanagedType.U1)]
        public bool lfItalic;
        [MarshalAs(UnmanagedType.U1)]
        public bool lfUnderline;
        [MarshalAs(UnmanagedType.U1)]
        public bool lfStrikeOut;
        public FontCharSet lfCharSet;
        public FontPrecision lfOutPrecision;
        public FontClipPrecision lfClipPrecision;
        public FontQuality lfQuality;
        public FontPitchAndFamily lfPitchAndFamily;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string lfFaceName;
    }
}