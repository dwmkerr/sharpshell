using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;

namespace CopyDirectoryLocationHandler
{
    /// <summary>
    /// This extension adds the 'Copy location' command to the background of folders.
    /// </summary>
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.Class, @"Directory\Background")]
    public class CopyDirectoryLocationHandler : SharpContextMenu
    {
        /// <summary>
        /// Determines whether this instance can a shell context show menu, given the specified selected file list.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance should show a shell context menu for the specified file list; otherwise, <c>false</c>.
        /// </returns>
        protected override bool CanShowMenu()
        {
            return true;
        }

        /// <summary>
        /// Creates the context menu. This can be a single menu item or a tree of them.
        /// </summary>
        /// <returns>
        /// The context menu for the shell context menu.
        /// </returns>
        protected override ContextMenuStrip CreateMenu()
        {
            //  Create the context menu.
            var contextMenu = new ContextMenuStrip();

            //  Add the 'copy path' item. This just copies the folder path for the extension.
            var menuItem = new ToolStripMenuItem("Copy Location");
            menuItem.Click += (sender, args) => Clipboard.SetText(FolderPath);
            contextMenu.Items.Add(menuItem);

            //  Return the menu.
            return contextMenu;
        }
    }
}
