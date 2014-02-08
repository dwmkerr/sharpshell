namespace SharpShell.Interop
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// The system currently defines these modifier flags.
    /// </summary>
    public enum SCHIDS : uint
    {
        /// <summary>
        /// Version 5.0. Compare all the information contained in the ITEMIDLIST structure, not just the display names. This flag is valid only for folder objects that support the IShellFolder2 interface. For instance, if the two items are files, the folder should compare their names, sizes, file times, attributes, and any other information in the structures. If this flag is set, the lower sixteen bits of lParam must be zero.
        /// </summary>
        SCHIDS_ALLFIELDS = 0x80000000,

        /// <summary>
        /// Version 5.0. Compare all the information contained in the ITEMIDLIST structure, not just the display names. This flag is valid only for folder objects that support the IShellFolder2 interface. For instance, if the two items are files, the folder should compare their names, sizes, file times, attributes, and any other information in the structures. If this flag is set, the lower sixteen bits of lParam must be zero.
        /// </summary>
        SHCIDS_CANONICALONLY = 0x10000000, 

        /// <summary>
        /// Bitmask for the flag field.
        /// </summary>
        SHCIDS_BITMASK = 0xFFFF0000,

        /// <summary>
        /// Bitmask for the column field.
        /// </summary>
        SHCIDS_COLUMNMASK = 0x0000FFFF
    }

    // ReSharper restore InconsistentNaming
}