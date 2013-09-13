using System;

namespace SharpShell.Interop
{
    /// <summary>
    /// Context Menu Command Invoke flags.
    /// </summary>
    [Flags]
    public enum CMIC : uint
    {
        /// <summary>
        /// The hIcon member is valid. As of Windows Vista this flag is not used.
        /// </summary>
        CMIC_MASK_ICON = 0x00000010,

        /// <summary>
        /// The dwHotKey member is valid.
        /// </summary>
        CMIC_MASK_HOTKEY = 0x00000020,

        /// <summary>
        /// Windows Vista and later. The implementation of IContextMenu::InvokeCommand should be synchronous, not returning before it is complete. Since this is recommended, calling applications that specify this flag cannot guarantee that this request will be honored if they are not familiar with the implementation of the verb that they are invoking.
        /// </summary>
        CMIC_MASK_NOASYNC = 0x00000100,

        /// <summary>
        /// The system is prevented from displaying user interface elements (for example, error messages) while carrying out a command.
        /// </summary>
        CMIC_MASK_FLAG_NO_UI = 0x00000400,

        /// <summary>
        /// The shortcut menu handler should use lpVerbW, lpParametersW, lpDirectoryW, and lpTitleW members instead of their ANSI equivalents. Because some shortcut menu handlers may not support Unicode, you should also pass valid ANSI strings in the lpVerb, lpParameters, lpDirectory, and lpTitle members.
        /// </summary>
        CMIC_MASK_UNICODE = 0x00004000,

        /// <summary>
        /// If a shortcut menu handler needs to create a new process, it will normally create a new console. Setting the CMIC_MASK_NO_CONSOLE flag suppresses the creation of a new console.
        /// </summary>
        CMIC_MASK_NO_CONSOLE = 0x00008000,

        /// <summary>
        /// Wait for the DDE conversation to terminate before returning.
        /// </summary>
        CMIC_MASK_ASYNCOK = 0x00100000,

        /// <summary>
        /// Do not perform a zone check. This flag allows ShellExecuteEx to bypass zone checking put into place by IAttachmentExecute.
        /// </summary>
        CMIC_MASK_NOZONECHECKS = 0x00800000,

        /// <summary>
        /// Indicates that the implementation of IContextMenu::InvokeCommand might want to keep track of the item being invoked for features like the "Recent documents" menu.
        /// </summary>
        CMIC_MASK_FLAG_LOG_USAGE = 0x04000000,

        /// <summary>
        /// The SHIFT key is pressed. Use this instead of polling the current state of the keyboard that may have changed since the verb was invoked.
        /// </summary>
        CMIC_MASK_SHIFT_DOWN = 0x10000000,

        /// <summary>
        /// The ptInvoke member is valid.
        /// </summary>
        CMIC_MASK_PTINVOKE = 0x20000000,

        /// <summary>
        /// The CTRL key is pressed. Use this instead of polling the current state of the keyboard that may have changed since the verb was invoked.
        /// </summary>
        CMIC_MASK_CONTROL_DOWN = 0x40000000,
        /*/// <summary>
        /// This flag is valid only when referring to a 16-bit Windows-based application. If set, the application that the shortcut points to runs in a private Virtual DOS Machine (VDM). See Remarks.
        /// </summary>
        CMIC_MASK_SEP_VDM = 0*/
    }
}