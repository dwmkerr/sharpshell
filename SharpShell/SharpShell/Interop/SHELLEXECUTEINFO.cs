using System;

namespace SharpShell.Interop
{
    /// <summary>
    /// Contains information used by ShellExecuteEx.
    /// </summary>
    public struct SHELLEXECUTEINFO
    {
        /// <summary>
        /// Required. The size of this structure, in bytes.
        /// </summary>
        public int cbSize;

        /// <summary>
        /// Flags that indicate the content and validity of the other structure members.
        /// </summary>
        public SEE fMask;

        /// <summary>
        /// Optional. A handle to the parent window, used to display any message boxes that the system might produce while executing this function. This value can be NULL.
        /// </summary>
        public IntPtr hwnd;

        /// <summary>
        /// A string, referred to as a verb, that specifies the action to be performed. The set of available verbs depends on the particular file or folder. Generally, the actions available from an object's shortcut menu are available verbs. This parameter can be NULL, in which case the default verb is used if available. If not, the "open" verb is used. If neither verb is available, the system uses the first verb listed in the registry.
        /// </summary>
        public string lpVerb;

        /// <summary>
        /// The address of a null-terminated string that specifies the name of the file or object on which ShellExecuteEx will perform the action specified by the lpVerb parameter. The system registry verbs that are supported by the ShellExecuteEx function include "open" for executable files and document files and "print" for document files for which a print handler has been registered. Other applications might have added Shell verbs through the system registry, such as "play" for .avi and .wav files. To specify a Shell namespace object, pass the fully qualified parse name and set the SEE_MASK_INVOKEIDLIST flag in the fMask parameter.
        /// </summary>
        public string lpFile;

        /// <summary>
        /// Optional. The address of a null-terminated string that contains the application parameters. The parameters must be separated by spaces. If the lpFile member specifies a document file, lpParameters should be NULL.
        /// </summary>
        public string lpParameters;

        /// <summary>
        /// Optional. The address of a null-terminated string that specifies the name of the working directory. If this member is NULL, the current directory is used as the working directory.
        /// </summary>
        public string lpDirectory;

        /// <summary>
        /// Required. Flags that specify how an application is to be shown when it is opened; one of the SW_ values listed for the ShellExecute function. If lpFile specifies a document file, the flag is simply passed to the associated application. It is up to the application to decide how to handle it.
        /// </summary>
        public int nShow;

        /// <summary>
        /// [out] If SEE_MASK_NOCLOSEPROCESS is set and the ShellExecuteEx call succeeds, it sets this member to a value greater than 32. If the function fails, it is set to an SE_ERR_XXX error value that indicates the cause of the failure. Although hInstApp is declared as an HINSTANCE for compatibility with 16-bit Windows applications, it is not a true HINSTANCE. It can be cast only to an int and compared to either 32 or the following SE_ERR_XXX error codes.
        /// </summary>
        public IntPtr hInstApp;

        /// <summary>
        /// The address of an absolute ITEMIDLIST structure (PCIDLIST_ABSOLUTE) to contain an item identifier list that uniquely identifies the file to execute. This member is ignored if the fMask member does not include SEE_MASK_IDLIST or SEE_MASK_INVOKEIDLIST.
        /// </summary>
        public IntPtr lpIDList;

        /// <summary>
        /// The address of a null-terminated string that specifies one of the following:
        ///     A ProgId. For example, "Paint.Picture".
        ///     A URI protocol scheme. For example, "http".
        ///     A file extension. For example, ".txt".
        ///     A registry path under HKEY_CLASSES_ROOT that names a subkey that contains one or more Shell verbs. This key will have a subkey that conforms to the Shell verb registry schema, such as
        ///     shell\verb name
        /// </summary>
        public string lpClass;

        /// <summary>
        /// A handle to the registry key for the file type. The access rights for this registry key should be set to KEY_READ. This member is ignored if fMask does not include SEE_MASK_CLASSKEY.
        /// </summary>
        public IntPtr hkeyClass;

        /// <summary>
        /// A keyboard shortcut to associate with the application. The low-order word is the virtual key code, and the high-order word is a modifier flag (HOTKEYF_). For a list of modifier flags, see the description of the WM_SETHOTKEY message. This member is ignored if fMask does not include SEE_MASK_HOTKEY.
        /// </summary>
        public int dwHotKey;

        /// <summary>
        /// A handle to the icon for the file type. This member is ignored if fMask does not include SEE_MASK_ICON. This value is used only in Windows XP and earlier. It is ignored as of Windows Vista.
        /// </summary>
        public IntPtr hIcon;

        /// <summary>
        /// A handle to the monitor upon which the document is to be displayed. This member is ignored if fMask does not include SEE_MASK_HMONITOR.
        /// </summary>
        public IntPtr hProcess;
    }
}