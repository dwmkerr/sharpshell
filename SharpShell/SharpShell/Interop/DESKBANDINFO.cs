using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Receives information about a band object. This structure is used with the deprecated IDeskBand::GetBandInfo method.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct DESKBANDINFO
    {
        /// <summary>
        /// Set of flags that determine which members of this structure are being requested. 
        /// </summary>
        /// <remarks>
        /// This will be a combination of the following values:
        ///     DBIM_MINSIZE    ptMinSize is being requested.
        ///     DBIM_MAXSIZE    ptMaxSize is being requested.
        ///     DBIM_INTEGRAL   ptIntegral is being requested.
        ///     DBIM_ACTUAL     ptActual is being requested.
        ///     DBIM_TITLE      wszTitle is being requested.
        ///     DBIM_MODEFLAGS  dwModeFlags is being requested.
        ///     DBIM_BKCOLOR    crBkgnd is being requested.
        /// </remarks>
        public DBIM dwMask;

        /// <summary>
        /// Point structure that receives the minimum size of the band object. 
        /// The minimum width is placed in the x member and the minimum height 
        /// is placed in the y member. 
        /// </summary>
        public POINT ptMinSize;

        /// <summary>
        /// Point structure that receives the maximum size of the band object. 
        /// The maximum height is placed in the y member and the x member is ignored. 
        /// If there is no limit for the maximum height, (LONG)-1 should be used. 
        /// </summary>
        public POINT ptMaxSize;

        /// <summary>
        /// Point structure that receives the sizing step value of the band object. 
        /// The vertical step value is placed in the y member, and the x member is ignored. 
        /// The step value determines in what increments the band will be resized. 
        /// </summary>
        /// <remarks>
        /// This member is ignored if dwModeFlags does not contain DBIMF_VARIABLEHEIGHT. 
        /// </remarks>
        public POINT ptIntegral;

        /// <summary>
        /// Point structure that receives the ideal size of the band object. 
        /// The ideal width is placed in the x member and the ideal height is placed in the y member. 
        /// The band container will attempt to use these values, but the band is not guaranteed to be this size.
        /// </summary>
        public POINT ptActual;

        /// <summary>
        /// The title of the band.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)] public String wszTitle;

        /// <summary>
        /// A value that receives a set of flags that define the mode of operation for the band object. 
        /// </summary>
        /// <remarks>
        /// This must be one or a combination of the following values.
        ///     DBIMF_NORMAL
        ///     The band is normal in all respects. The other mode flags modify this flag.
        ///     DBIMF_VARIABLEHEIGHT
        ///     The height of the band object can be changed. The ptIntegral member defines the 
        ///     step value by which the band object can be resized. 
        ///     DBIMF_DEBOSSED
        ///     The band object is displayed with a sunken appearance.
        ///     DBIMF_BKCOLOR
        ///     The band will be displayed with the background color specified in crBkgnd.
        /// </remarks>
        public DBIMF dwModeFlags;

        /// <summary>
        /// The background color of the band.
        /// </summary>
        /// <remarks>
        /// This member is ignored if dwModeFlags does not contain the DBIMF_BKCOLOR flag. 
        /// </remarks>
        public COLORREF crBkgnd;

        /// <summary>
        /// The view mode of the band object. This is one of the following values.
        /// </summary>
        [Flags]
        public enum DBIF : uint
        {
            /// <summary>
            /// Band object is displayed in a horizontal band.
            /// </summary>
            DBIF_VIEWMODE_NORMAL = 0x0000,

            /// <summary>
            /// Band object is displayed in a vertical band.
            /// </summary>
            DBIF_VIEWMODE_VERTICAL = 0x0001,

            /// <summary>
            /// Band object is displayed in a floating band.
            /// </summary>
            DBIF_VIEWMODE_FLOATING = 0x0002,

            /// <summary>
            /// Band object is displayed in a transparent band.
            /// </summary>
            DBIF_VIEWMODE_TRANSPARENT = 0x0004
        }

        /// <summary>
        /// The set of flags that determine which members of this structure are being requested by the caller. One or more of the following values:
        /// </summary>
        [Flags]
        public enum DBIM : uint
        {
            /// <summary>
            /// ptMinSize is requested.
            /// </summary>
            DBIM_MINSIZE = 0x0001,

            /// <summary>
            /// ptMaxSize is requested.
            /// </summary>
            DBIM_MAXSIZE = 0x0002,

            /// <summary>
            /// ptIntegral is requested.
            /// </summary>
            DBIM_INTEGRAL = 0x0004,

            /// <summary>
            /// ptActual is requested.
            /// </summary>
            DBIM_ACTUAL = 0x0008,

            /// <summary>
            /// wszTitle is requested.
            /// </summary>
            DBIM_TITLE = 0x0010,

            /// <summary>
            /// dwModeFlags is requested.
            /// </summary>
            DBIM_MODEFLAGS = 0x0020,

            /// <summary>
            /// crBkgnd is requested.
            /// </summary>
            DBIM_BKCOLOR = 0x0040
        }

        /// <summary>
        /// A value that receives a set of flags that specify the mode of operation for the band object. One or more of the following values:
        /// </summary>
        [Flags]
        public enum DBIMF : uint
        {
            /// <summary>
            /// The band uses default properties. The other mode flags modify this flag.
            /// </summary>
            DBIMF_NORMAL = 0x0000,

            /// <summary>
            /// Windows XP and later: The band object is of a fixed sized and position. With this flag, a sizing grip is not displayed on the band object.
            /// </summary>
            DBIMF_FIXED = 0x0001,

            /// <summary>
            /// DBIMF_FIXEDBMP
            /// Windows XP and later: The band object uses a fixed bitmap (.bmp) file as its background. Note that backgrounds are not supported in all cases, so the bitmap may not be seen even when this flag is set.
            /// </summary>
            DBIMF_FIXEDBMP = 0x0004,

            /// <summary>
            /// The height of the band object can be changed. The ptIntegral member defines the step value by which the band object can be resized.
            /// </summary>
            DBIMF_VARIABLEHEIGHT = 0x0008,

            /// <summary>
            /// Windows XP and later: The band object cannot be removed from the band container.
            /// </summary>
            DBIMF_UNDELETEABLE = 0x0010,

            /// <summary>
            /// The band object is displayed with a sunken appearance.
            /// </summary>
            DBIMF_DEBOSSED = 0x0020,

            /// <summary>
            /// The band is displayed with the background color specified in crBkgnd.
            /// </summary>
            DBIMF_BKCOLOR = 0x0040,

            /// <summary>
            /// Windows XP and later: If the full band object cannot be displayed (that is, the band object is smaller than ptActual, a chevron is shown to indicate that there are more options available. These options are displayed when the chevron is clicked.
            /// </summary>
            DBIMF_USECHEVRON = 0x0080,

            /// <summary>
            /// Windows XP and later: The band object is displayed in a new row in the band container.
            /// </summary>
            DBIMF_BREAK = 0x0100,

            /// <summary>
            /// Windows XP and later: The band object is the first object in the band container.
            /// </summary>
            DBIMF_ADDTOFRONT = 0x0200,

            /// <summary>
            /// Windows XP and later: The band object is displayed in the top row of the band container.
            /// </summary>
            DBIMF_TOPALIGN = 0x0400,

            /// <summary>
            /// Windows Vista and later: No sizing grip is ever displayed to allow the user to move or resize the band object.
            /// </summary>
            DBIMF_NOGRIPPER = 0x0800,

            /// <summary>
            /// Windows Vista and later: A sizing grip that allows the user to move or resize the band object is always shown, even if that band object is the only one in the container.
            /// </summary>
            DBIMF_ALWAYSGRIPPER = 0x1000,

            /// <summary>
            /// Windows Vista and later: The band object should not display margins.
            /// </summary>
            DBIMF_NOMARGINS = 0x2000
        }
    }
}