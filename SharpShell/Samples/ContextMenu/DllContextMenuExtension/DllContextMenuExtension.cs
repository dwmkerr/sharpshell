using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;

namespace DllContextMenuExtension
{
    [ComVisible(true)]
    [DisplayName("Dll Context Menu Extension")]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".dll")]
    public class DllContextMenuExtension : SharpContextMenu
    {
        /// <summary>
        /// Determines whether this instance can a shell context show menu, given the specified selected file list.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance should show a shell context menu for the specified file list; otherwise, <c>false</c>.
        /// </returns>
        protected override bool CanShowMenu()
        {
            //  We will support the dll extension only if we have one file, and it is a dll.
            return SelectedItemPaths.Count() == 1 && Path.GetExtension(SelectedItemPaths.First()).ToLower() == ".dll";
        }

        /// <summary>
        /// Creates the context menu. This can be a single menu item or a tree of them.
        /// </summary>
        /// <returns>
        /// The context menu for the shell context menu.
        /// </returns>
        protected override ContextMenuStrip CreateMenu()
        {
            //  Create a context menu strip.
            var menuStrip = new ContextMenuStrip();

            //  Create the registration item.
            var registrationItem = new ToolStripMenuItem("Registration");
            var regasm = new ToolStripMenuItem("regasm /i");
            regasm.Click += (sender, args) => MessageBox.Show("regasm /i");
            var regasmU = new ToolStripMenuItem("regasm /u");
            regasmU.Click += (sender, args) => MessageBox.Show("regasm /u");
            registrationItem.DropDownItems.Add(regasm);
            registrationItem.DropDownItems.Add(regasmU);
            menuStrip.Items.Add(registrationItem);

            //  Create the installation item.
            var installationItem = new ToolStripMenuItem("Registration");
            var gacInstall = new ToolStripMenuItem("Install to GAC");
            gacInstall.Click += (sender, args) => MessageBox.Show("Install to GAC");
            installationItem.DropDownItems.Add(gacInstall);
            menuStrip.Items.Add(installationItem);

            return menuStrip;
        }
    }
}
