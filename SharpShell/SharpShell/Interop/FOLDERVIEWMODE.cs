namespace SharpShell.Interop
{
    /// <summary>
    /// Specifies the folder view type.
    /// TODO document and tidy this up.
    /// </summary>
    public enum FOLDERVIEWMODE
    {
        FVM_AUTO = -1,    //The view should determine the best option.
        FVM_FIRST = 1,    //The minimum constant value in FOLDERVIEWMODE, for validation purposes.
        FVM_ICON = 1,     //The view should display medium-size icons.
        FVM_SMALLICON = 2,    //The view should display small icons.
        FVM_LIST = 3,     //Object names are displayed in a list view.
        FVM_DETAILS = 4,      //Object names and other selected information, such as the size or date last updated, are shown.
        FVM_THUMBNAIL =5,    //The view should display thumbnail icons.
        FVM_TILE = 6,       //The view should display large icons.
        FVM_THUMBSTRIP = 7, //The view should display icons in a filmstrip format.
        FVM_CONTENT = 8,    //Windows 7 and later. The view should display content mode.
        FVM_LAST = 8       //The maximum constant value in FOLDERVIEWMODE, for validation purposes.
    }
}