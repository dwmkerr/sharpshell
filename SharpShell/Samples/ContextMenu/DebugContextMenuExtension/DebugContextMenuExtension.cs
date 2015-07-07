using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;

namespace DebugContextMenuExtension
{
    /// <summary>
    /// The most trivial context menu imaginable. Shows a message box with a list of the files selected.
    /// Useful for debugging issues.
    /// </summary>
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    public class DebugContextMenuExtension : SharpContextMenu
    {
        /// <summary>
        /// Determines whether this instance can a shell context show menu, given the specified selected file list.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance should show a shell context menu for the specified file list; otherwise, <c>false</c>.
        /// </returns>
        protected override bool CanShowMenu()
        {
            //  We always show the menu.
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
            //  Create the menu strip.
            var menu = new ContextMenuStrip();

            //  Create a 'debug' item.
            var itemDebug = new ToolStripMenuItem
                                     {
                                         Text = "Debug...",
                                         Image = Properties.Resources.DebugContextMenu
                                     };

            //  When we click, we'll invoke the action.
            itemDebug.Click += (sender, args) => DebugContextMenu();

            //  Add the item to the context menu.
            menu.Items.Add(itemDebug);

            //  Return the menu.
            return menu;
        }

        /// <summary>
        /// Shows diagnostic information.
        /// </summary>
        private void DebugContextMenu()
        {
            //  Builder for the output.
            var builder = new StringBuilder();
            
            //  Build a string with each path.
            SelectedItemPaths.ToList().ForEach(ip => builder.AppendLine(ip));

            //  Show the ouput.
            MessageBox.Show(builder.ToString());
        }
    }
}