using System;

namespace SharpShell.Interop
{
    /// <summary>
    /// Output GIL flags.
    /// </summary>
    [Flags]
    public enum GILOutFlags : uint
    {
        /// <summary>
        /// The physical image bits for this icon are not cached by the calling application.
        /// </summary>
        GIL_DONTCACHE = 0x0010,
        /// <summary>
        /// The location is not a file name/index pair. The values in pszIconFile and piIndex cannot be passed to ExtractIcon or ExtractIconEx.
        /// </summary>
        GIL_NOTFILENAME = 0x0008,
        /// <summary>
        /// All objects of this class have the same icon. This flag is used internally by the Shell. Typical implementations of IExtractIcon do not require this flag because the flag implies that an icon handler is not required to resolve the icon on a per-object basis. The recommended method for implementing per-class icons is to register a DefaultIcon for the class.
        /// </summary>
        GIL_PERCLASS = 0x0004,
        /// <summary>
        /// Each object of this class has its own icon. This flag is used internally by the Shell to handle cases like Setup.exe, where objects with identical names can have different icons. Typical implementations of IExtractIcon do not require this flag.
        /// </summary>
        GIL_PERINSTANCE = 0x0002,
        /// <summary>
        /// The calling application should create a document icon using the specified icon.
        /// </summary>
        GIL_SIMULATEDOC = 0x0001,
        /// <summary>
        /// Windows Vista only. The calling application must stamp the icon with the UAC shield.
        /// </summary>
        GIL_SHIELD = 0x0200,//Windows Vista only
        /// <summary>
        /// Windows Vista only. The calling application must not stamp the icon with the UAC shield.
        /// </summary>
        GIL_FORCENOSHIELD = 0x0400//Windows Vista only
    }
}