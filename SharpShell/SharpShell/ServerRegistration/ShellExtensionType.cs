using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SharpShell.Attributes;

namespace SharpShell.ServerRegistration
{
    /// <summary>
    /// The Shell Extension Type.
    /// </summary>
    [Flags]
    public enum ShellExtensionType
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// The shortcut menu handler type.
        /// </summary>
        [HandlerSubkey(true, @"ContextMenuHandlers")]
        ShortcutMenuHandler = 1 << 0,

        /// <summary>
        /// The copyhook handler type.
        /// </summary>
        [HandlerSubkey(true, @"CopyHookHandlers")]
        CopyhookHandler = 1 << 1,

        /// <summary>
        /// The drag and drop handler type.
        /// </summary>
        [HandlerSubkey(true, @"DragDropHandlers")]
        DragAndDropHandler = 1 << 2,

        /// <summary>
        /// The property sheet handler type.
        /// </summary>
        [HandlerSubkey(true, @"PropertySheetHandlers")]
        PropertySheetHandler = 1 << 3,

        /// <summary>
        /// The column provider handler type.
        /// </summary>
        [HandlerSubkey(true, @"ColumnHandlers")]
        ColumnProviderHandler = 1 << 4,

        /// <summary>
        /// The data handler type.
        /// </summary>
        [HandlerSubkey(false, @"DataHandler")]
        DataHandler = 1 << 5,

        /// <summary>
        /// The drop handler type.
        /// </summary>
        [HandlerSubkey(false, @"DropHandler")]
        DropHandler = 1 << 6,

        /// <summary>
        /// The icon handler type.
        /// </summary>
        [HandlerSubkey(false, @"IconHandler")]
        IconHandler = 1 << 7,

        /// <summary>
        /// The image handler type.
        /// </summary>
        [HandlerSubkey(false, @"{BB2E617C-0920-11d1-9A0B-00C04FC2D6C1}")]
        ImageHandler = 1 << 8,

        /// <summary>
        /// The thumbnail image handler type.
        /// </summary>
        [HandlerSubkey(false, @"{E357FCCD-A995-4576-B01F-234630154E96}")]
        ThumbnailImageHandler = 1 << 9,

        /// <summary>
        /// The infotip handler type.
        /// </summary>
        [HandlerSubkey(false, @"{00021500-0000-0000-C000-000000000046}")]
        InfotipHandler = 1 << 10,

        /// <summary>
        /// The shell link ANSI type.
        /// </summary>
        [HandlerSubkey(false, @"{000214EE-0000-0000-C000-000000000046}")]
        ShellLinkANSI = 1 << 11,

        /// <summary>
        /// The shell link unicode type.
        /// </summary>
        [HandlerSubkey(false, @"{000214F9-0000-0000-C000-000000000046}")]
        ShellLinkUNICODE = 1 << 12,

        /// <summary>
        /// The structured storage type.
        /// </summary>
        [HandlerSubkey(false, @"{0000000B-0000-0000-C000-000000000046}")]
        StructuredStorage = 1 << 13,

        /// <summary>
        /// The metadata property store type.
        /// </summary>
        [HandlerSubkey(false, @"PropertyHandler")]
        MetadataPropertyStore = 1 << 14,

        /// <summary>
        /// The metadata property set storage type.
        /// </summary>
        [HandlerSubkey(false, @"PropertyHandler")]
        MetadataPropertySetStorage = 1 << 15,

        /// <summary>
        /// The pin to start menu type.
        /// </summary>
        [HandlerSubkey(false, @"{a2a9545d-a0c2-42b4-9708-a0b2badd77c8}")]
        PinToStartMenu = 1 << 16,

        /// <summary>
        /// The pin to taskbar type.
        /// </summary>
        [HandlerSubkey(false, @"{90AA3A4E-1CBA-4233-B8BB-535773D48449}")]
        PinToTaskbar = 1 << 17
    }
}
