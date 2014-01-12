using System;

namespace SharpShell.Interop
{
    /// <summary>
    /// TODO: document this.
    /// </summary>
    [Flags]
    public enum SBSP : uint
    {
        SBSP_DEFBROWSER         = 0x0000,
        SBSP_SAMEBROWSER        = 0x0001,
        SBSP_NEWBROWSER         = 0x0002,
        SBSP_DEFMODE            = 0x0000,
        SBSP_OPENMODE           = 0x0010,
        SBSP_EXPLOREMODE        = 0x0020,
        SBSP_HELPMODE           = 0x0040,
        SBSP_NOTRANSFERHIST     = 0x0080,
        SBSP_ABSOLUTE           = 0x0000,
        SBSP_RELATIVE           = 0x1000,
        SBSP_PARENT             = 0x2000,
        SBSP_NAVIGATEBACK       = 0x4000,
        SBSP_NAVIGATEFORWARD    = 0x8000,
        SBSP_ALLOW_AUTONAVIGATE   = 0x00010000,
        SBSP_KEEPSAMETEMPLATE     = 0x00020000,
        SBSP_KEEPWORDWHEELTEXT    = 0x00040000,
        SBSP_ACTIVATE_NOFOCUS     = 0x00080000,
        SBSP_CREATENOHISTORY      = 0x00100000,
        SBSP_PLAYNOSOUND          = 0x00200000,
        SBSP_CALLERUNTRUSTED      = 0x00800000,
        SBSP_TRUSTFIRSTDOWNLOAD   = 0x01000000,
        SBSP_UNTRUSTEDFORDOWNLOAD = 0x02000000,
        SBSP_NOAUTOSELECT         = 0x04000000,
        SBSP_WRITENOHISTORY       = 0x08000000,
        SBSP_TRUSTEDFORACTIVEX    = 0x10000000,
        SBSP_FEEDNAVIGATION       = 0x20000000,
        SBSP_REDIRECT                     = 0x40000000,
        SBSP_INITIATEDBYHLINKFRAME        = 0x80000000
    }
}