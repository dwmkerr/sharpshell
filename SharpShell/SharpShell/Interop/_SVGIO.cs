namespace SharpShell.Interop
{
    /// <summary>
    /// TODO document this
    /// </summary>
    public enum _SVGIO : uint { 
        SVGIO_BACKGROUND      = 0x00000000,
        SVGIO_SELECTION       = 0x00000001,
        SVGIO_ALLVIEW         = 0x00000002,
        SVGIO_CHECKED         = 0x00000003,
        SVGIO_TYPE_MASK       = 0x0000000F,
        SVGIO_FLAG_VIEWORDER  = 0x80000000
    } ;
}