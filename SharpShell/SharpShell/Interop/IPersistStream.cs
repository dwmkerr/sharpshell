using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace SharpShell.Interop
{
    /// <summary>
    /// Enables the saving and loading of objects that use a simple serial stream for their storage needs.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00000109-0000-0000-C000-000000000046")]
    public interface IPersistStream : IPersist
    {
        [PreserveSig]
        int IsDirty();

        /// <summary>
        /// Initializes an object from the stream where it was saved previously.
        /// </summary>
        /// <param name="pStm">An IStream pointer to the stream from which the object should be loaded.</param>
        /// <returns>This method can return the following values. S_OK, E_OUTOFMEMORY, E_FAIL.</returns>
        [PreserveSig]
        int Load(IStream pStm);

        /// <summary>
        /// Saves an object to the specified stream.
        /// </summary>
        /// <param name="pStm">An IStream pointer to the stream into which the object should be saved.</param>
        /// <param name="fClearDirty">Indicates whether to clear the dirty flag after the save is complete. If TRUE, the flag should be cleared. If FALSE, the flag should be left unchanged.</param>
        /// <returns>This method can return the following values. S_OK, STG_E_CANTSAVE, STG_E_MEDIUMFULL.</returns>
        [PreserveSig]
        int Save(IStream pStm, bool fClearDirty);

        /// <summary>
        /// Retrieves the size of the stream needed to save the object.
        /// </summary>
        /// <param name="pcbSize">The size in bytes of the stream needed to save this object, in bytes.</param>
        /// <returns>This method returns S_OK to indicate that the size was retrieved successfully.</returns>
        [PreserveSig]
        int GetSizeMax(out UInt64 pcbSize);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("EB0FE172-1A3A-11D0-89B3-00A0C90A90AC")]
    public interface IDeskBand : IDockingWindow
    {
        /// <summary>
        /// Gets state information for a band object.
        /// </summary>
        /// <param name="dwBandID">The identifier of the band, assigned by the container. The band object can retain this value if it is required.</param>
        /// <param name="dwViewMode">The view mode of the band object. One of the following values: DBIF_VIEWMODE_NORMAL, DBIF_VIEWMODE_VERTICAL, DBIF_VIEWMODE_FLOATING, DBIF_VIEWMODE_TRANSPARENT.</param>
        /// <param name="pdbi">The pdbi.</param>
        /// <returns></returns>
        [PreserveSig]
        int GetBandInfo(UInt32 dwBandID, DBIF dwViewMode, ref DESKBANDINFO pdbi);

    };

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
    }

    [Flags]
    public enum DBIM : uint
    {
        DBIM_MINSIZE = 0x0001,
        DBIM_MAXSIZE = 0x0002,
        DBIM_INTEGRAL = 0x0004,
        DBIM_ACTUAL = 0x0008,
        DBIM_TITLE = 0x0010,
        DBIM_MODEFLAGS = 0x0020,
        DBIM_BKCOLOR = 0x0040
    }

    [Flags]
    public enum DBIMF : uint
    {
        DBIMF_NORMAL = 0x0000,
        DBIMF_FIXED = 0x0001,
        DBIMF_FIXEDBMP = 0x0004, // a fixed background bitmap (if supported)
        DBIMF_VARIABLEHEIGHT = 0x0008,
        DBIMF_UNDELETEABLE = 0x0010,
        DBIMF_DEBOSSED = 0x0020,
        DBIMF_BKCOLOR = 0x0040,
        DBIMF_USECHEVRON = 0x0080,
        DBIMF_BREAK = 0x0100,
        DBIMF_ADDTOFRONT = 0x0200,
        DBIMF_TOPALIGN = 0x0400,
        DBIMF_NOGRIPPER = 0x0800,
        DBIMF_ALWAYSGRIPPER = 0x1000,
        DBIMF_NOMARGINS = 0x2000
    }

    [Flags]
    public enum DBIF : uint
    {
        DBIF_VIEWMODE_NORMAL = 0x0000,
        DBIF_VIEWMODE_VERTICAL = 0x0001,
        DBIF_VIEWMODE_FLOATING = 0x0002,
        DBIF_VIEWMODE_TRANSPARENT = 0x0004
    }
}