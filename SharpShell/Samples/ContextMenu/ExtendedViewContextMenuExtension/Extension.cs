using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using SharpShell.Attributes;
using SharpShell.SharpContextMenu;

namespace ExtendedViewContextMenu {

    /// <summary>
    /// Demonstrate a context menu shell extension with extended view support, implemented with SharpShell.
    /// Windows Explorer shows an extended view of the context menu by holding the Shift key while right-clicking an item.
    /// </summary>
    /// <remarks>
    /// The intended behavior can be tested in Microsoft Windows Explorer only.
    /// The Test Shell 
    /// </remarks>

    [ComVisible( true )]
    [Guid( "F7E256F0-DD40-4F5C-8496-67F472A24B28" )] // Defining COM CLSID explicitly makes it easier to find in Windows Registry
    [RegistrationName( "SharpShell.Samples.ExtendedViewContextMenu.Extension" )] // Defining ShellHandler registration name makes it easier to find in Windows Registry
    [COMServerAssociation( AssociationType.AllFilesAndFolders )] // Associate sample extension with all files and folders

    public class Extension
    : SharpContextMenu {

        /// <summary>
        /// Determines whether this instance can a shell context show menu, given the specified selected file list.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance should show a shell context menu for the specified file list; otherwise, <c>false</c>.
        /// </returns>
        protected override bool CanShowMenu() {
            return true;
        }

        /// <summary>
        /// Creates the context menu. This can be a single menu item or a tree of them.
        /// </summary>
        /// <returns>
        /// The context menu for the shell context menu.
        /// </returns>
        protected override ContextMenuStrip CreateMenu() {

            // Capture the state of the shift key and store it.
            // This ensures that the correct entries are displayed,
            // even if the user releases the key during menu creation.
            bool extendedView = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;

            ContextMenuStrip result = new ContextMenuStrip();

            // Add standard view items

            result.Items.Add( new ToolStripSeparator() );

            ToolStripMenuItem standardViewItem = new ToolStripMenuItem("Standard View Entry");
            standardViewItem.Click += HandleClickEvent;

            result.Items.Add( standardViewItem );


            // Add extended view items
            if ( extendedView ) {
                ToolStripMenuItem extendedViewItem = new ToolStripMenuItem( "Extended View Entry");
                extendedViewItem.Click += HandleClickEvent;
                result.Items.Add( extendedViewItem );
            }

            result.Items.Add( new ToolStripSeparator() );

            return result;

        }

        void HandleClickEvent( object sender, EventArgs e ) {
            MessageBox.Show( ((ToolStripMenuItem)sender).Text );
        }
    }
}
