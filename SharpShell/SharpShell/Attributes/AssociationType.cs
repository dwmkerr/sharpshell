using System;

namespace SharpShell.Attributes
{
    //  TODO; There's a design issue here. We're mixing two concepts, the predefined shell
    //  objects, and the concept of a 'type' of association to make. These should be separated
    //  at some stage.

    /// <summary>
    /// The AssociationType determines what kind of associate a COM
    /// server is made to a class, such as a file class or a drive.
    /// </summary>
    public enum AssociationType
    {
        /// <summary>
        /// No server association.
        /// </summary>
        None = 0,

        /// <summary>
        /// Create an association to a specific file extension.
        /// This attribute is deprecated. Shell extensions should not be registered directly on file extensions,
        /// but on the class of the extension.
        /// </summary>
        [Obsolete("FileExtension is deprecated. Use 'ClassOfExtension' instead.")]
        FileExtension = 1,

        /// <summary>
        /// Create an association to the class of a specific file extension.
        /// </summary>
        ClassOfExtension = 2,

        /// <summary>
        /// Create an association to a class.
        /// </summary>
        Class = 3,

        /// <summary>
        /// Create an association to the 'all files' class.
        /// </summary>
        [PredefinedShellObject(@"*")]
        AllFiles = 4,

        /// <summary>
        /// Create an association to the 'all files and folders' class.
        /// </summary>
        [PredefinedShellObject(@"AllFileSystemObjects")]
        AllFilesAndFolders = 5,

        /// <summary>
        /// Create an association to the 'directory' class, i.e. file-system folders.
        /// </summary>
        [PredefinedShellObject(@"Directory")]
        Directory = 6,

        /// <summary>
        /// Create an association to the background of folders and the desktop
        /// </summary>
        [PredefinedShellObject(@"Directory\Background")]
        DirectoryBackground = 7,

        /// <summary>
        /// Create an association to the background of the desktop (Windows 7 and higher)
        /// </summary>
        [PredefinedShellObject(@"DesktopBackground")]
        DesktopBackground = 8,

        /// <summary>
        /// Create an association to the drive class.
        /// </summary>
        [PredefinedShellObject(@"Drive")]
        Drive = 9,

        /// <summary>
        /// Create an association to the 'folder' class, i.e. all containers.
        /// </summary>
        [PredefinedShellObject(@"Folder")]
        Folder = 10,

        /// <summary>
        /// Create an association to the unknown files class.
        /// </summary>
        [PredefinedShellObject(@"Unknown")]
        UnknownFiles = 11
    }
}
