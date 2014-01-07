namespace SharpShell.Interop
{
    /// <summary>
    /// TODO: Document from MSDN
    /// </summary>
    public enum KNOWN_FOLDER_FLAG : uint
    {
        KF_NO_FLAGS = 0,
        KF_FLAG_SIMPLE_IDLIST = 0x00000100,
        KF_FLAG_NOT_PARENT_RELATIVE = 0x00000200,
        KF_FLAG_DEFAULT_PATH = 0x00000400,
        KF_FLAG_INIT = 0x00000800,
        KF_FLAG_NO_ALIAS = 0x00001000,
        KF_FLAG_DONT_UNEXPAND = 0x00002000,
        KF_FLAG_DONT_VERIFY = 0x00004000,
        KF_FLAG_CREATE = 0x00008000,
        KF_FLAG_NO_APPCONTAINER_REDIRECTION = 0x00010000,
        KF_FLAG_ALIAS_ONLY = 0x80000000
    }
}