using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Gets information about a band object.
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("EB0FE172-1A3A-11D0-89B3-00A0C90A90AC")]
    public interface IDeskBand : IDockingWindow
    {
        #region IOleWindow Overrides

        [PreserveSig]
        new int GetWindow(out IntPtr phwnd);

        [PreserveSig]
        new int ContextSensitiveHelp(bool fEnterMode);

        #endregion

        #region Overrides of IDockingWindow

        [PreserveSig]
        new int ShowDW(bool bShow);

        [PreserveSig]
        new int CloseDW(UInt32 dwReserved);

        [PreserveSig]
        new int ResizeBorderDW(RECT rcBorder, IntPtr punkToolbarSite, bool fReserved);

        #endregion

        /// <summary>
        /// Gets state information for a band object.
        /// </summary>
        /// <param name="dwBandID">The identifier of the band, assigned by the container. The band object can retain this value if it is required.</param>
        /// <param name="dwViewMode">The view mode of the band object. One of the following values: DBIF_VIEWMODE_NORMAL, DBIF_VIEWMODE_VERTICAL, DBIF_VIEWMODE_FLOATING, DBIF_VIEWMODE_TRANSPARENT.</param>
        /// <param name="pdbi">The pdbi.</param>
        /// <returns></returns>
        [PreserveSig]
        int GetBandInfo(UInt32 dwBandID, DESKBANDINFO.DBIF dwViewMode, ref DESKBANDINFO pdbi);
    }
}