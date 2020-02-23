using System.ComponentModel;

namespace SharpShell
{
    /// <summary>
    /// The Server Type.
    /// </summary>
    public enum ServerType
    {
        /// <summary>
        /// No Server Type.
        /// </summary>
        [Description("Not a SharpShell Server")]
        None = 0,

        /// <summary>
        /// A Shell Context Menu.
        /// </summary>
        [Description(@"Shell Context Menu")]
        ShellContextMenu = 1,

        /// <summary>
        /// A Shell Property Sheet.
        /// </summary>
        [Description(@"Shell Property Sheet")]
        ShellPropertySheet = 2,

        /// <summary>
        /// A Shell Icon Handler.
        /// </summary>
        [Description(@"Shell Icon Handler")]
        ShellIconHandler = 3,

        /// <summary>
        /// A Shell Info Tip Handler.
        /// </summary>
        [Description(@"Shell Info Tip Handler")]
        ShellInfoTipHandler = 4,

        /// <summary>
        /// A Shell Drop Handler
        /// </summary>
        [Description(@"Shell Drop Handler")]
        ShellDropHandler = 5,

        /// <summary>
        /// A Shell Icon Overlay Handler.
        /// </summary>
        [Description(@"Shell Icon Overlay Handler")]
        ShellIconOverlayHandler = 6,

        /// <summary>
        /// A Shell Preview Handler
        /// </summary>
        [Description(@"Shell Preview Handler")]
        ShellPreviewHandler = 7,

        /// <summary>
        /// A Shell Data Handler
        /// </summary>
        [Description(@"Shell Data Handler")]
        ShellDataHandler = 8,

        /// <summary>
        /// A Shell Thumbnail Handler
        /// </summary>
        [Description(@"Shell Thumbnail Handler")]
        ShellThumbnailHandler = 9,

        /// <summary>
        /// A Shell Namespace Extension
        /// </summary>
        [Description(@"Shell Namespace Extension")]
        ShellNamespaceExtension = 10,

        /// <summary>
        /// A Shell Desk Band Extension
        /// </summary>
        [Description(@"Shell Desk Band Extension")]
        ShellDeskBand = 11
    }
}