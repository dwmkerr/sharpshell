namespace SharpShell.SharpNamespaceExtension
{
    /// <summary>
    /// Defines the availability for a namespace extension, either everyone or the current user only.
    /// </summary>
    public enum NamespaceExtensionAvailability
    {
        /// <summary>
        /// All users can use the namespace extension.
        /// </summary>
        Everyone,
        /// <summary>
        /// Only the current user can use the namespace extension.
        /// </summary>
        CurrentUser
    }
}