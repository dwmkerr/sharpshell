using System;

namespace SharpShell.Interop
{
    /// <summary>
    /// Flags that specify how the shortcut menu can be changed.
    /// </summary>
    [Flags]
    public enum CMF : uint
    {
        /// <summary>
        /// Indicates normal operation. A shortcut menu extension, namespace extension, or drag-and-drop handler can add all menu items.
        /// </summary>
        CMF_NORMAL = 0x00000000,

        /// <summary>
        /// The user is activating the default action, typically by double-clicking. This flag provides a hint for the shortcut menu extension to add nothing if it does not modify the default item in the menu. A shortcut menu extension or drag-and-drop handler should not add any menu items if this value is specified. A namespace extension should at most add only the default item.
        /// </summary>
        CMF_DEFAULTONLY = 0x00000001,

        /// <summary>
        /// The shortcut menu is that of a shortcut file (normally, a .lnk file). Shortcut menu handlers should ignore this value.
        /// </summary>
        CMF_VERBSONLY = 0x00000002,

        /// <summary>
        /// The Windows Explorer tree window is present.
        /// </summary>
        CMF_EXPLORE = 0x00000004,

        /// <summary>
        /// This flag is set for items displayed in the Send To menu. Shortcut menu handlers should ignore this value.
        /// </summary>
        CMF_NOVERBS = 0x00000008,

        /// <summary>
        /// The calling application supports renaming of items. A shortcut menu or drag-and-drop handler should ignore this flag. A namespace extension should add a Rename item to the menu if applicable.
        /// </summary>
        CMF_CANRENAME = 0x00000010,

        /// <summary>
        /// No item in the menu has been set as the default. A drag-and-drop handler should ignore this flag. A namespace extension should not set any of the menu items as the default.
        /// </summary>
        CMF_NODEFAULT = 0x00000020,

        /// <summary>
        /// A static menu is being constructed. Only the browser should use this flag; all other shortcut menu extensions should ignore it.
        /// </summary>
        CMF_INCLUDESTATIC = 0x00000040,

        /// <summary>
        /// The calling application is invoking a shortcut menu on an item in the view (as opposed to the background of the view).
        /// Windows Server 2003 and Windows XP:  This value is not available.
        /// </summary>
        CMF_ITEMMENU = 0x00000080,

        /// <summary>
        /// The calling application wants extended verbs. Normal verbs are displayed when the user right-clicks an object. To display extended verbs, the user must right-click while pressing the Shift key.
        /// </summary>
        CMF_EXTENDEDVERBS = 0x00000100,

        /// <summary>
        /// The calling application intends to invoke verbs that are disabled, such as legacy menus.
        /// Windows Server 2003 and Windows XP:  This value is not available.
        /// </summary>
        CMF_DISABLEDVERBS = 0x00000200,

        /// <summary>
        /// The verb state can be evaluated asynchronously.
        /// Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:  This value is not available.
        /// </summary>
        CMF_ASYNCVERBSTATE = 0x00000400,

        /// <summary>
        /// Informs context menu handlers that do not support the invocation of a verb through a canonical verb name to bypass IContextMenu::QueryContextMenu in their implementation.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:  This value is not available.
        /// </summary>
        CMF_OPTIMIZEFORINVOKE = 0x00000800,

        /// <summary>
        /// Populate submenus synchronously.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:  This value is not available.
        /// </summary>
        CMF_SYNCCASCADEMENU = 0x00001000,

        /// <summary>
        /// When no verb is explicitly specified, do not use a default verb in its place.
        ///     Windows Server 2008, Windows Vista, Windows Server 2003, and Windows XP:  This value is not available.
        /// </summary>
        CMF_DONOTPICKDEFAULT = 0x00002000,

        /// <summary>
        /// This flag is a bitmask that specifies all bits that should not be used. This is to be used only as a mask. Do not pass this as a parameter value.
        /// </summary>
        CMF_RESERVED = 0xFFFF0000
    }
}