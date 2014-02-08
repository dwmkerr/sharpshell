using System;

namespace SharpShell.Interop
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Flags specifying the folder to be browsed. It can be zero or one or more of the following values.
    /// These flags specify whether another window is to be created.
    /// </summary>
    [Flags]
    public enum SBSP : uint
    {
        /// <summary>
        /// Use default behavior, which respects the view option (the user setting to create new windows or to browse in place). In most cases, calling applications should use this flag.
        /// </summary>
        SBSP_DEFBROWSER         = 0x0000,

        /// <summary>
        /// Browse to another folder with the same Windows Explorer window.
        /// </summary>
        SBSP_SAMEBROWSER        = 0x0001,

        /// <summary>
        /// Creates another window for the specified folder.
        /// </summary>
        SBSP_NEWBROWSER         = 0x0002,

        /// <summary>
        /// Use the current window.
        /// </summary>
        SBSP_DEFMODE            = 0x0000,

        /// <summary>
        /// Specifies no folder tree for the new browse window. If the current browser does not match the SBSP_OPENMODE of the browse object call, a new window is opened.
        /// </summary>
        SBSP_OPENMODE           = 0x0010,

        /// <summary>
        /// Specifies a folder tree for the new browse window. If the current browser does not match the SBSP_EXPLOREMODE of the browse object call, a new window is opened.
        /// </summary>
        SBSP_EXPLOREMODE        = 0x0020,

        /// <summary>
        /// Not supported. Do not use.
        /// </summary>
        SBSP_HELPMODE           = 0x0040,

        /// <summary>
        /// Do not transfer the browsing history to the new window.
        /// </summary>
        SBSP_NOTRANSFERHIST     = 0x0080,

        /// <summary>
        /// An absolute PIDL, relative to the desktop.
        /// </summary>
        SBSP_ABSOLUTE           = 0x0000,

        /// <summary>
        /// A relative PIDL, relative to the current folder.
        /// </summary>
        SBSP_RELATIVE           = 0x1000,

        /// <summary>
        /// Browse the parent folder, ignore the PIDL.
        /// </summary>
        SBSP_PARENT             = 0x2000,

        /// <summary>
        /// Navigate back, ignore the PIDL.
        /// </summary>
        SBSP_NAVIGATEBACK       = 0x4000,

        /// <summary>
        /// Navigate forward, ignore the PIDL.
        /// </summary>
        SBSP_NAVIGATEFORWARD    = 0x8000,

        /// <summary>
        /// Enable auto-navigation.
        /// </summary>
        SBSP_ALLOW_AUTONAVIGATE   = 0x00010000,

        /// <summary>
        /// Windows Vista and later. Not supported. Do not use.
        /// </summary>
        SBSP_KEEPSAMETEMPLATE     = 0x00020000,

        /// <summary>
        /// Windows Vista and later. Navigate without clearing the search entry field.
        /// </summary>
        SBSP_KEEPWORDWHEELTEXT    = 0x00040000,

        /// <summary>
        /// Windows Vista and later. Navigate without the default behavior of setting focus into the new view.
        /// </summary>
        SBSP_ACTIVATE_NOFOCUS     = 0x00080000,

        /// <summary>
        /// Windows 7 and later. Do not add a new entry to the travel log. When the user enters a search term in the search box and subsequently refines the query, the browser navigates forward but does not add an additional travel log entry.
        /// </summary>
        SBSP_CREATENOHISTORY      = 0x00100000,

        /// <summary>
        /// Windows 7 and later. Do not make the navigation complete sound for each keystroke in the search box.
        /// </summary>
        SBSP_PLAYNOSOUND          = 0x00200000,

        /// <summary>
        /// Microsoft Internet Explorer 6 Service Pack 2 (SP2) and later. The navigation was possibly initiated by a webpage with scripting code already present on the local system.
        /// </summary>
        SBSP_CALLERUNTRUSTED      = 0x00800000,

        /// <summary>
        /// Suppress selection in the history pane.
        /// </summary>
        SBSP_TRUSTFIRSTDOWNLOAD   = 0x01000000,

        /// <summary>
        /// Microsoft Internet Explorer 6 Service Pack 2 (SP2) and later. The window is navigating to an untrusted, non-HTML file. If the user attempts to download the file, do not allow the download.
        /// </summary>
        SBSP_UNTRUSTEDFORDOWNLOAD = 0x02000000,

        /// <summary>
        /// Suppress selection in the history pane.
        /// </summary>
        SBSP_NOAUTOSELECT         = 0x04000000,

        /// <summary>
        /// Write no history of this navigation in the history Shell folder.
        /// </summary>
        SBSP_WRITENOHISTORY       = 0x08000000,

        /// <summary>
        /// Microsoft Internet Explorer 6 Service Pack 2 (SP2) and later. The navigate should allow ActiveX prompts.
        /// </summary>
        SBSP_TRUSTEDFORACTIVEX    = 0x10000000,

        /// <summary>
        /// Windows Internet Explorer 7 and later. If allowed by current registry settings, give the browser a destination to navigate to.
        /// </summary>
        SBSP_FEEDNAVIGATION       = 0x20000000,

        /// <summary>
        /// Enables redirection to another URL.
        /// </summary>
        SBSP_REDIRECT                     = 0x40000000,
        
        /// <summary>
        /// 
        /// </summary>
        SBSP_INITIATEDBYHLINKFRAME        = 0x80000000
    }

    // ReSharper restore InconsistentNaming
}