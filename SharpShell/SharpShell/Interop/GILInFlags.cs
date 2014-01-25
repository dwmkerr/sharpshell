using System;

namespace SharpShell.Interop
{
    /// <summary>
    /// Input GIL flags.
    /// </summary>
    [Flags]
    public enum GILInFlags : uint
    {
        /// <summary>
        /// Set this flag to determine whether the icon should be extracted asynchronously. If the icon can be extracted rapidly, this flag is usually ignored. If extraction will take more time, GetIconLocation should return E_PENDING
        /// </summary>
        GIL_ASYNC = 0x0020,
        /// <summary>
        /// Retrieve information about the fallback icon. Fallback icons are usually used while the desired icon is extracted and added to the cache.
        /// </summary>
        GIL_DEFAULTICON = 0x0040,
        /// <summary>
        /// The icon is displayed in a Shell folder.
        /// </summary>
        GIL_FORSHELL = 0x0002,
        /// <summary>
        /// The icon indicates a shortcut. However, the icon extractor should not apply the shortcut overlay; that will be done later. Shortcut icons are state-independent.
        /// </summary>
        GIL_FORSHORTCUT = 0x0080,
        /// <summary>
        /// The icon is in the open state if both open-state and closed-state images are available. If this flag is not specified, the icon is in the normal or closed state. This flag is typically used for folder objects.
        /// </summary>
        GIL_OPENICON = 0x0001,
        /// <summary>
        /// Explicitly return either GIL_SHIELD or GIL_FORCENOSHIELD in pwFlags. Do not block if GIL_ASYNC is set.
        /// </summary>
        GIL_CHECKSHIELD = 0x0200
    }
}