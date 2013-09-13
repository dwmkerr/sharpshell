using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SharpShell.Attributes;

namespace SharpShell.ServerRegistration
{
    [Flags]
    public enum ShellExtensionType
    {
        None = 0,

        [HandlerSubkey(true, @"ContextMenuHandlers")]
        ShortcutMenuHandler = 1 << 0,

        [HandlerSubkey(true, @"CopyHookHandlers")]
        CopyhookHandler = 1 << 1,

        [HandlerSubkey(true, @"DragDropHandlers")]
        DragAndDropHandler = 1 << 2,

        [HandlerSubkey(true, @"PropertySheetHandlers")]
        PropertySheetHandler = 1 << 3,

        [HandlerSubkey(true, @"ColumnHandlers")]
        ColumnProviderHandler = 1 << 4,

        [HandlerSubkey(false, @"DataHandler")]
        DataHandler = 1 << 5,

        [HandlerSubkey(false, @"DropHandler")]
        DropHandler = 1 << 6,

        [HandlerSubkey(false, @"IconHandler")]
        IconHandler = 1 << 7,

        [HandlerSubkey(false, @"{BB2E617C-0920-11d1-9A0B-00C04FC2D6C1}")]
        ImageHandler = 1 << 8,

        [HandlerSubkey(false, @"{E357FCCD-A995-4576-B01F-234630154E96}")]
        ThumbnailImageHandler = 1 << 9,

        [HandlerSubkey(false, @"{00021500-0000-0000-C000-000000000046}")]
        InfotipHandler = 1 << 10,

        [HandlerSubkey(false, @"{000214EE-0000-0000-C000-000000000046}")]
        ShellLinkANSI = 1 << 11,

        [HandlerSubkey(false, @"{000214F9-0000-0000-C000-000000000046}")]
        ShellLinkUNICODE = 1 << 12,

        [HandlerSubkey(false, @"{0000000B-0000-0000-C000-000000000046}")]
        StructuredStorage = 1 << 13,

        [HandlerSubkey(false, @"PropertyHandler")]
        MetadataPropertyStore = 1 << 14,

        [HandlerSubkey(false, @"PropertyHandler")]
        MetadataPropertySetStorage = 1 << 15,

        [HandlerSubkey(false, @"{a2a9545d-a0c2-42b4-9708-a0b2badd77c8}")]
        PinToStartMenu = 1 << 16,

        [HandlerSubkey(false, @"{90AA3A4E-1CBA-4233-B8BB-535773D48449}")]
        PinToTaskbar = 1 << 17
    }
}
