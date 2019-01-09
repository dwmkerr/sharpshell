using System;
using SharpShell.Attributes;

namespace SharpShell.ServerRegistration
{
    /// <summary>
    ///     The Shell Extension Type.
    /// </summary>
    [Flags]
    public enum ShellExtensionType
    {
        /// <summary>
        ///     None.
        /// </summary>
        None = 0,

        /// <summary>
        ///     The shortcut menu handler type.
        /// </summary>
        [HandlerSubKey(@"ContextMenuHandlers", true)]
        ShellContextMenu = 1 << 0,

        /// <summary>
        ///     The copy-hook handler type.
        /// </summary>
        [HandlerSubKey(@"CopyHookHandlers", true)]
        ShellCopyHookHandler = 1 << 1,

        /// <summary>
        ///     The drag and drop handler type.
        /// </summary>
        [HandlerSubKey(@"DragDropHandlers", true)]
        ShellDragDropHandlers = 1 << 2,

        /// <summary>
        ///     The property sheet handler type.
        /// </summary>
        [HandlerSubKey(@"PropertySheetHandlers", true)]
        ShellPropertySheet = 1 << 3,

        /// <summary>
        ///     The column provider handler type.
        /// </summary>
        [HandlerSubKey(@"ColumnHandlers", true)]
        ShellColumnProviderHandler = 1 << 4,

        /// <summary>
        ///     The data handler type.
        /// </summary>
        [HandlerSubKey(@"DataHandler", false)] ShellDataHandler = 1 << 5,

        /// <summary>
        ///     The drop handler type.
        /// </summary>
        [HandlerSubKey(@"DropHandler", false)] ShellDropHandler = 1 << 6,

        /// <summary>
        ///     The icon handler type.
        /// </summary>
        [HandlerSubKey(@"IconHandler", false)] ShellIconHandler = 1 << 7,

        /// <summary>
        ///     The image handler type.
        /// </summary>
        [HandlerSubKey(@"{BB2E617C-0920-11d1-9A0B-00C04FC2D6C1}", false)]
        ImageHandler = 1 << 8,

        /// <summary>
        ///     The thumbnail image handler type.
        /// </summary>
        [HandlerSubKey(@"{E357FCCD-A995-4576-B01F-234630154E96}", false)]
        ShellThumbnailHandler = 1 << 9,

        /// <summary>
        ///     The info-tip handler type.
        /// </summary>
        [HandlerSubKey(@"{00021500-0000-0000-C000-000000000046}", false)]
        ShellInfoTipHandler = 1 << 10,

        /// <summary>
        ///     The shell link ANSI type.
        /// </summary>
        [HandlerSubKey(@"{000214EE-0000-0000-C000-000000000046}", false)]
        ShellLinkAnsi = 1 << 11,

        /// <summary>
        ///     The shell link unicode type.
        /// </summary>
        [HandlerSubKey(@"{000214F9-0000-0000-C000-000000000046}", false)]
        ShellLinkUnicode = 1 << 12,

        /// <summary>
        ///     The structured storage type.
        /// </summary>
        [HandlerSubKey(@"{0000000B-0000-0000-C000-000000000046}", false)]
        ShellStructuredStorage = 1 << 13,

        /// <summary>
        ///     The metadata property store type.
        /// </summary>
        [HandlerSubKey(@"PropertyHandler", false)]
        ShellMetadataPropertyStore = 1 << 14,

        /// <summary>
        ///     The metadata property set storage type.
        /// </summary>
        [HandlerSubKey(@"PropertyHandler", false)]
        ShellMetadataPropertySetStorage = 1 << 15,

        /// <summary>
        ///     The pin to start menu type.
        /// </summary>
        [HandlerSubKey(@"{a2a9545d-a0c2-42b4-9708-a0b2badd77c8}", false)]
        ShellPinToStartMenu = 1 << 16,

        /// <summary>
        ///     The pin to task-bar type.
        /// </summary>
        [HandlerSubKey(@"{90AA3A4E-1CBA-4233-B8BB-535773D48449}", false)]
        ShellPinToTaskBar = 1 << 17,

        /// <summary>
        ///     The pin to start menu type.
        /// </summary>
        [HandlerSubKey(@"{8895b1c6-b41f-4c1c-a562-0d564250836f}", false)]
        ShellPreviewHandler = 1 << 18
    }
}