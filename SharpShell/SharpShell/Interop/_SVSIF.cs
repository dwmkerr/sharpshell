namespace SharpShell.Interop
{
    /// <summary>
    /// TODO document this
    /// </summary>
    public enum _SVSIF : uint { 
        SVSI_DESELECT        = 0x00000000,
        SVSI_SELECT          = 0x00000001,
        SVSI_EDIT            = 0x00000003,
        SVSI_DESELECTOTHERS  = 0x00000004,
        SVSI_ENSUREVISIBLE   = 0x00000008,
        SVSI_FOCUSED         = 0x00000010,
        SVSI_TRANSLATEPT     = 0x00000020,
        SVSI_SELECTIONMARK   = 0x00000040,
        SVSI_POSITIONITEM    = 0x00000080,
        SVSI_CHECK           = 0x00000100,
        SVSI_CHECK2          = 0x00000200,
        SVSI_KEYBOARDSELECT  = 0x00000401,
        SVSI_NOTAKEFOCUS     = 0x40000000
    };
}