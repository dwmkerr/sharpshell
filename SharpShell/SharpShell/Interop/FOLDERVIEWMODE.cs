namespace SharpShell.Interop
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Specifies the folder view type.
    /// </summary>
    public enum FOLDERVIEWMODE
    {
        /// <summary>
        /// The view should determine the best option.
        /// </summary>
        FVM_AUTO = -1,

        /// <summary>
        /// The minimum constant value in FOLDERVIEWMODE, for validation purposes.
        /// </summary>
        FVM_FIRST = 1,
        
        /// <summary>
        /// The view should display medium-size icons.
        /// </summary>
        FVM_ICON = 1,

        /// <summary>
        /// The view should display small icons.
        /// </summary>
        FVM_SMALLICON = 2,

        /// <summary>
        /// Object names are displayed in a list view.
        /// </summary>
        FVM_LIST = 3,

        /// <summary>
        /// Object names and other selected information, such as the size or date last updated, are shown.
        /// </summary>
        FVM_DETAILS = 4,

        /// <summary>
        /// The view should display thumbnail icons.
        /// </summary>
        FVM_THUMBNAIL =5,

        /// <summary>
        /// The view should display large icons.
        /// </summary>
        FVM_TILE = 6,

        /// <summary>
        /// The view should display icons in a filmstrip format.
        /// </summary>
        FVM_THUMBSTRIP = 7,

        /// <summary>
        /// Windows 7 and later. The view should display content mode.
        /// </summary>
        FVM_CONTENT = 8,

        /// <summary>
        /// The maximum constant value in FOLDERVIEWMODE, for validation purposes.
        /// </summary>
        FVM_LAST = 8
    }

    // ReSharper restore InconsistentNaming
}