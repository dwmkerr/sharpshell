namespace SharpShell.SharpNamespaceExtension
{
    /// <summary>
    /// The context that a Display Name will be used in, see <see cref="IShellNamespaceItem.GetDisplayName"/>
    /// for details.
    /// </summary>
    public enum DisplayNameContext
    {
        /// <summary>
        /// The normal display name. Shown in the context of a parent folder and in the address bar.
        /// </summary>
        Normal,

        /// <summary>
        /// The out of folder context, meaning that extra information can be shown if needed. For example,
        /// a printer might not just show the printer name, but something like "Printer1 on Computer3".
        /// </summary>
        OutOfFolder,

        /// <summary>
        /// The display name as it will be edited by the user. Typically the display name, but this can
        /// be different if this makes sense.
        /// </summary>
        Editing
    }
}