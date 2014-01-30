namespace FileDialogs
{
    /// <summary>
    /// Specifies enumerated constants used to retrieve directory paths to system special folders.
    /// </summary>
    public enum SpecialFolder
    {
        None = -1,
        /// <summary>
        /// The logical Desktop rather than the physical file system location.
        /// </summary>
        Desktop = 0x00,
        /// <summary>
        /// The folder for Internet Explorer. 
        /// </summary>
        InternetExplorer = 0x01,
        /// <summary>
        /// The directory that contains the user's program groups.
        /// </summary>
        Programs = 0x02,
        /// <summary>
        /// The folder that contains icons for the Control Panel applications.
        /// </summary>
        ControlPanel = 0x03,
        /// <summary>
        /// The folder that contains installed printers.
        /// </summary>
        Printers = 0x04,
        /// <summary>
        /// The "My Documents" folder.
        /// </summary>
        MyDocuments = 0x05,
        /// <summary>
        /// The directory that serves as a common repository for the user's favorite items.
        /// </summary>
        Favorites = 0x06,
        /// <summary>
        /// The directory that corresponds to the user's Startup program group.
        /// </summary>
        Startup = 0x07,
        /// <summary>
        /// The directory that contains the user's most recently used documents.
        /// </summary>
        Recent = 0x08,
        /// <summary>
        /// The directory that contains the Send To menu items.
        /// </summary>
        SendTo = 0x09,
        /// <summary>
        /// The "Recycle Bin" folder.
        /// </summary>
        RecycleBin = 0x0A,
        /// <summary>
        /// The directory that contains the Start menu items.
        /// </summary>
        StartMenu = 0x0B,
        /// <summary>
        /// The "My Music" folder.
        /// </summary>
        MyMusic = 0x0D,
        /// <summary>
        /// The "My Video" folder.
        /// </summary>
        MyVideo = 0x0E,
        /// <summary>
        /// The directory used to physically store file objects on the desktop.
        /// </summary>
        DesktopDirectory = 0x10,
        /// <summary>
        /// The "My Computer" folder.
        /// </summary>
        MyComputer = 0x11,
        /// <summary>
        /// The "My Network Places" folder.
        /// </summary>
        Network = 0x12,
        /// <summary>
        /// The "Fonts" folder.
        /// </summary>
        Fonts = 0x14,
        /// <summary>
        /// The directory that serves as a common repository for document templates.
        /// </summary>
        Templates = 0x15,
        /// <summary>
        /// The directory that serves as a common repository for application-specific data for the current roaming user.
        /// </summary>
        ApplicationData = 0x1A,
        /// <summary>
        /// The directory that serves as a common repository for application-specific data that is used by the current, non-roaming user.
        /// </summary>
        LocalApplicationData = 0x1C,
        /// <summary>
        /// The directory that serves as a common repository for temporary Internet files.
        /// </summary>
        InternetCache = 0x20,
        /// <summary>
        /// The directory that serves as a common repository for Internet cookies.
        /// </summary>
        Cookies = 0x21,
        /// <summary>
        /// The directory that serves as a common repository for Internet history items.
        /// </summary>
        History = 0x22,
        /// <summary>
        /// The directory that serves as a common repository for application-specific data that is used by all users.
        /// </summary>
        CommonApplicationData = 0x23,
        /// <summary>
        /// The Windows directory.
        /// </summary>
        Windows = 0x24,
        /// <summary>
        /// The System directory.
        /// </summary>
        System = 0x25,
        /// <summary>
        /// The program files directory.
        /// </summary>
        ProgramFiles = 0x26,
        /// <summary>
        /// The "My Pictures" folder.
        /// </summary>
        MyPictures = 0x27,
        /// <summary>
        /// The directory for components that are shared across applications.
        /// </summary>
        CommonProgramFiles = 0x2B
    }
}
