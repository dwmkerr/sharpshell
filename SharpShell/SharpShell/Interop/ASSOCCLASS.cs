namespace SharpShell.Interop
{
// ReSharper disable InconsistentNaming

    /// <summary>
    /// Where to obtain association data and the form the data is stored in. One of the following values from the ASSOCCLASS enumeration.   
    /// </summary>
    public enum ASSOCCLASS
    {
        /// <summary>
        /// The hkClass member names a key found as HKEY_CLASSES_ROOT\SystemFileAssociations\hkClass.
        /// </summary>
        ASSOCCLASS_SHELL_KEY = 0,

        /// <summary>
        /// The hkClass member provides the full registry path of a ProgID.
        /// </summary>
        ASSOCCLASS_PROGID_KEY,

        /// <summary>
        /// The pszClass member names a ProgID found as HKEY_CLASSES_ROOT\pszClass.
        /// </summary>
        ASSOCCLASS_PROGID_STR,

        /// <summary>
        /// The hkClass member provides the full registry path of a CLSID.  
        /// </summary>
        ASSOCCLASS_CLSID_KEY,

        /// <summary>
        /// The hkClass member names a CLSID found as HKEY_CLASSES_ROOT\CLSID\pszClass.
        /// </summary>
        ASSOCCLASS_CLSID_STR,

        /// <summary>
        /// The hkClass member provides the full registry path of an application identifier (APPID).
        /// </summary>
        ASSOCCLASS_APP_KEY,

        /// <summary>
        /// The APPID storing the application information is found at HKEY_CLASSES_ROOT\Applications\FileName where FileName is obtained by sending pszClass to PathFindFileName.
        /// </summary>
        ASSOCCLASS_APP_STR,

        /// <summary>
        /// The pszClass member names a key found as HKEY_CLASSES_ROOT\SystemFileAssociations\pszClass.
        /// </summary>
        ASSOCCLASS_SYSTEM_STR,

        /// <summary>
        /// Use the association information for folders stored under HKEY_CLASSES_ROOT\Folder. When this flag is set, hkClass and pszClass are ignored.
        /// </summary>
        ASSOCCLASS_FOLDER,

        /// <summary>
        /// Use the association information stored under the HKEY_CLASSES_ROOT\* subkey. When this flag is set, hkClass and pszClass are ignored.
        /// </summary>
        ASSOCCLASS_STAR,

        /// <summary>
        /// Introduced in Windows 8. Do not use the user defaults to apply the mapping of the class specified by the pszClass member.
        /// </summary>
        ASSOCCLASS_FIXED_PROGID_STR,

        /// <summary>
        /// Introduced in Windows 8. Use the user defaults to apply the mapping of the class specified by the pszClass member; the class is a protocol.
        /// </summary>
        ASSOCCLASS_PROTOCOL_STR
    }

    // ReSharper restore InconsistentNaming
}