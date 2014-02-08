using System;

namespace SharpShell.Interop
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// A set of flags that specify folder view options. The flags are independent of each other and can be used in any combination.
    /// </summary>
    [Flags]
    public enum FOLDERFLAGS : uint
    {
        /// <summary>
        /// Windows 7 and later. No special view options.
        /// </summary>
        FWF_NONE = 0x00000000,

        /// <summary>
        /// Automatically arrange the elements in the view. This implies LVS_AUTOARRANGE if the list-view control is used to implement the view.
        /// </summary>
        FWF_AUTOARRANGE = 0x00000001,

        /// <summary>
        /// Not supported.
        /// </summary>
        FWF_ABBREVIATEDNAMES = 0x00000002,

        /// <summary>
        /// Not supported.
        /// </summary>
        FWF_SNAPTOGRID = 0x00000004,

        /// <summary>
        /// Not supported.
        /// </summary>
        FWF_OWNERDATA = 0x00000008,

        /// <summary>
        /// Not supported.
        /// </summary>
        FWF_BESTFITWINDOW = 0x00000010,

        /// <summary>
        /// Make the folder behave like the desktop. This value applies only to the desktop and is not used for typical Shell folders. This flag implies FWF_NOCLIENTEDGE and FWF_NOSCROLL.
        /// </summary>
        FWF_DESKTOP = 0x00000020,

        /// <summary>
        /// Do not allow more than a single item to be selected. This is used in the common dialog boxes.
        /// </summary>
        FWF_SINGLESEL = 0x00000040,

        /// <summary>
        /// Do not show subfolders.
        /// </summary>
        FWF_NOSUBFOLDERS = 0x00000080,

        /// <summary>
        /// Draw transparently. This is used only for the desktop.
        /// </summary>
        FWF_TRANSPARENT = 0x00000100,

        /// <summary>
        /// Not supported.
        /// </summary>
        FWF_NOCLIENTEDGE = 0x00000200,

        /// <summary>
        /// Do not add scroll bars. This is used only for the desktop.
        /// </summary>
        FWF_NOSCROLL = 0x00000400,

        /// <summary>
        /// The view should be left-aligned. This implies LVS_ALIGNLEFT if the list-view control is used to implement the view.
        /// </summary>
        FWF_ALIGNLEFT = 0x00000800,

        /// <summary>
        /// The view should not display icons.
        /// </summary>
        FWF_NOICONS = 0x00001000,

        /// <summary>
        /// This flag is deprecated as of Windows XP and has no effect. Always show the selection.
        /// </summary>
        FWF_SHOWSELALWAYS = 0x00002000,

        /// <summary>
        /// Not supported.
        /// </summary>
        FWF_NOVISIBLE = 0x00004000,

        /// <summary>
        /// Not supported.
        /// </summary>
        FWF_SINGLECLICKACTIVATE = 0x00008000,

        /// <summary>
        /// The view should not be shown as a web view.
        /// </summary>
        FWF_NOWEBVIEW = 0x00010000,

        /// <summary>
        /// The view should not display file names.
        /// </summary>
        FWF_HIDEFILENAMES = 0x00020000,

        /// <summary>
        /// Turns on the check mode for the view.
        /// </summary>
        FWF_CHECKSELECT = 0x00040000,

        /// <summary>
        /// Windows Vista and later. Do not re-enumerate the view (or drop the current contents of the view) when the view is refreshed.
        /// </summary>
        FWF_NOENUMREFRESH = 0x00080000,

        /// <summary>
        /// Windows Vista and later. Do not allow grouping in the view
        /// </summary>
        FWF_NOGROUPING = 0x00100000,

        /// <summary>
        /// Windows Vista and later. When an item is selected, the item and all its sub-items are highlighted.
        /// </summary>
        FWF_FULLROWSELECT = 0x00200000,

        /// <summary>
        /// Windows Vista and later. Do not display filters in the view.
        /// </summary>
        FWF_NOFILTERS = 0x00400000,

        /// <summary>
        /// Windows Vista and later. Do not display a column header in the view in any view mode.
        /// </summary>
        FWF_NOCOLUMNHEADER = 0x00800000,

        /// <summary>
        /// Windows Vista and later. Only show the column header in details view mode.
        /// </summary>
        FWF_NOHEADERINALLVIEWS = 0x01000000,

        /// <summary>
        /// Windows Vista and later. When the view is in "tile view mode" the layout of a single item should be extended to the width of the view.
        /// </summary>
        FWF_EXTENDEDTILES = 0x02000000,

        /// <summary>
        /// Windows Vista and later. Not supported.
        /// </summary>
        FWF_TRICHECKSELECT = 0x04000000,

        /// <summary>
        /// Windows Vista and later. Items can be selected using checkboxes.
        /// </summary>
        FWF_AUTOCHECKSELECT = 0x08000000,

        /// <summary>
        /// Windows Vista and later. The view should not save view state in the browser.
        /// </summary>
        FWF_NOBROWSERVIEWSTATE = 0x10000000,

        /// <summary>
        /// Windows Vista and later. The view should list the number of items displayed in each group. To be used with IFolderView2::SetGroupSubsetCount.
        /// </summary>
        FWF_SUBSETGROUPS = 0x20000000,

        /// <summary>
        /// Windows Vista and later. Use the search folder for stacking and searching.
        /// </summary>
        FWF_USESEARCHFOLDER = 0x40000000,
        
        /// <summary>
        /// Windows Vista and later. Ensure right-to-left reading layout in a right-to-left system. Without this flag, the view displays strings from left-to-right both on systems set to left-to-right and right-to-left reading layout, which ensures that file names display correctly.
        /// </summary>
        FWF_ALLOWRTLREADING = 0x80000000
    }

    // ReSharper restore InconsistentNaming
}