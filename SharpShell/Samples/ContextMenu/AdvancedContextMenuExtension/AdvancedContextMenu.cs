using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;

namespace AdvancedContextMenuExtension
{
    /// <summary>
    /// The CountLinesExtensions is an example shell context menu extension,
    /// implemented with SharpShell. It adds the command 'Count Lines' to text
    /// files.
    /// </summary>
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    public class AdvancedContextMenu : SharpContextMenu
    {
        /// <summary>
        /// Determines whether this instance can a shell context show menu, given the specified selected file list.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance should show a shell context menu for the specified file list; otherwise, <c>false</c>.
        /// </returns>
        protected override bool CanShowMenu()
        {
            //  We can show the item only for a single selection.
            return SelectedItemPaths.Count() == 1;
        }

        /// <summary>
        /// Creates the context menu. This can be a single menu item or a tree of them.
        /// </summary>
        /// <returns>
        /// The context menu for the shell context menu.
        /// </returns>
        protected override ContextMenuStrip CreateMenu()
        {
            //  Create the menu strip.
            var menu = new ContextMenuStrip();

            //  Create a menu item to hold all of the subitems.
            var advancedItem = new ToolStripMenuItem
                {
                    Text = "Advanced",
                Image = Properties.Resources.Copy
                };

            //  Now add the child items.
            var readOnlyItem = new ToolStripMenuItem
                {
                    Text = "Read Only",
                    Checked = File.GetAttributes(SelectedItemPaths.First()).HasFlag(FileAttributes.ReadOnly)
                };
            readOnlyItem.Click += (sender, args) => DoToggleReadOnly();

            //  Add the touch item.
            var touchItem = new ToolStripMenuItem
                {
                    Text = "Touch",
                    Enabled = !File.GetAttributes(SelectedItemPaths.First()).HasFlag(FileAttributes.ReadOnly)
                };
            touchItem.Click += (sender, args) => DoTouch();
            
            var copyPathItem = new ToolStripMenuItem
            {
                Text = "Copy Path",
                Image = Properties.Resources.Copy
            };
            copyPathItem.Click += (sender, args) => DoCopyPath();

            //  Add the items.
            advancedItem.DropDownItems.Add(readOnlyItem);
            advancedItem.DropDownItems.Add(touchItem);
            advancedItem.DropDownItems.Add(new ToolStripSeparator());
            advancedItem.DropDownItems.Add(copyPathItem);
            
            //  Add the item to the context menu.
            menu.Items.Add(advancedItem);

            //  Return the menu.
            return menu;
        }

        protected void DoToggleReadOnly()
        {
            //  Get the attributes.
            var path = SelectedItemPaths.First();
            var attributes = File.GetAttributes(path);

            //  Toggle the readonly flag.
            if ((attributes & FileAttributes.ReadOnly) != 0)
                attributes &= ~FileAttributes.ReadOnly;
            else
                attributes |= FileAttributes.ReadOnly;
            
            //  Set the attributes.
            File.SetAttributes(path, attributes);
        }

        protected void DoCopyPath()
        {
            Clipboard.SetText(SelectedItemPaths.First());
        }

        protected void DoTouch()
        {
            File.SetLastAccessTime(SelectedItemPaths.First(), DateTime.Now);
        }
    }
}
