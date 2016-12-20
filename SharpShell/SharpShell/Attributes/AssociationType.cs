namespace SharpShell.Attributes
{
    /// <summary>
    /// The AssociationType determines what kind of associate a COM
    /// server is made to a class, such as a file class or a drive.
    /// </summary>
    public enum AssociationType
    {
        /// <summary>
        /// No server association.
        /// </summary>
        None,

        /// <summary>
        /// Create an association to a specific file extension.
        /// </summary>
        FileExtension,

        /// <summary>
        /// Create an association to the class of a specific file extension.
        /// </summary>
        ClassOfExtension,

        /// <summary>
        /// Create an association to a class.
        /// </summary>
        Class,

        /// <summary>
        /// Create an association to the all files class.
        /// </summary>
        AllFiles,

        /// <summary>
        /// Create an association to the directory class.
        /// </summary>
        Directory,

        /// <summary>
        /// Create an association to the background of folders and the desktop
        /// </summary>
        DirectoryBackground,

        /// <summary>
        /// Create an association to the background of the desktop (Windows 7 and higher)
        /// </summary>
        DesktopBackground,
        
        /// <summary>
        /// Create an association to the drive class.
        /// </summary>
        Drive,

        /// <summary>
        /// Create an association to the unknown files class.
        /// </summary>
        UnknownFiles
    }
}
