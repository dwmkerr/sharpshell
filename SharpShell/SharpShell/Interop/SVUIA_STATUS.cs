namespace SharpShell.Interop
{
    /// <summary>
    /// Used with the IBrowserService2::_UIActivateView method to set the state of a browser view.
    /// </summary>
    public enum SVUIA_STATUS : uint
    {
        /// <summary>
        /// Windows Explorer is about to destroy the Shell view window. The Shell view should remove all extended user interfaces. These are typically merged menus and merged modeless pop-up windows.
        /// </summary>
        SVUIA_DEACTIVATE = 0,

        /// <summary>
        /// The Shell view is losing the input focus, or it has just been created without the input focus. The Shell view should be able to set menu items appropriate for the nonfocused state. This means no selection-specific items should be added.
        /// </summary>
        SVUIA_ACTIVATE_NOFOCUS = 1,

        /// <summary>
        /// Windows Explorer has just created the view window with the input focus. This means the Shell view should be able to set menu items appropriate for the focused state.
        /// </summary>
        SVUIA_ACTIVATE_FOCUS = 2,

        /// <summary>
        /// The Shell view is active without focus. This flag is only used when UIActivate is exposed through the IShellView2 interface.
        /// </summary>
        SVUIA_INPLACEACTIVATE = 3
    }
}