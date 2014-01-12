namespace SharpShell.Interop
{
    /// <summary>
    /// TODO: document this.
    /// </summary>
    public enum SCHIDS : uint
    {
        SCHIDS_ALLFIELDS = 0x80000000,
        SHCIDS_CANONICALONLY = 0x10000000, 
        SHCIDS_BITMASK = 0xFFFF0000,
        SHCIDS_COLUMNMASK = 0x0000FFFF,

    }
}