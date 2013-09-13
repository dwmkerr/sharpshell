namespace SharpShell.Interop
{
    /// <summary>
    /// Property Sheet Notifications.
    /// </summary>
    public enum PSN : uint
    {
        /// <summary>
        /// Notifies a page that it is about to be activated.
        /// </summary>
        PSN_SETACTIVE = 0xffffff38,

        /// <summary>
        /// Notifies a page that it is about to lose activation either because another page is being activated or the user has clicked the OK button.
        /// </summary>
        PSN_KILLACTIVE = 0xffffff37,

        /// <summary>
        /// Sent to every page in the property sheet to indicate that the user has clicked the OK, Close, or Apply button and wants all changes to take effect. This notification code is sent in the form of a WM_NOTIFY message.
        /// </summary>
        PSN_APPLY = 0xffffff36,

        /// <summary>
        /// Notifies a page that the property sheet is about to be destroyed. 
        /// </summary>
        PSN_RESET = 0xffffff35
    }
}