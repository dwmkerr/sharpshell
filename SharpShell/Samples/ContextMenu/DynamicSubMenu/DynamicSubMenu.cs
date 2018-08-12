using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;
using System.Collections.Generic;

using System.Linq;

namespace DynamicSubMenus
{
    // <summary>
    // The SubMenuExtension is an example shell context menu extension,
    // implemented with SharpShell. It loads the menu dynamically
    // files.
    // </summary>
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    [COMServerAssociation(AssociationType.Directory)]
    public class DynamicSubMenuExtension : SharpContextMenu
    {
        //  lets create the menu strip.
        private ContextMenuStrip menu = new ContextMenuStrip();

        // <summary>
        // Determines whether the menu item can be shown for the selected item.
        // </summary>
        // <returns>
        //   <c>true</c> if item can be shown for the selected item for this instance.; otherwise, <c>false</c>.
        // </returns>
        protected override bool CanShowMenu()
        {
            
            //  We can show the item only for a single selection.
            if (SelectedItemPaths.Count() == 1)
            {
                this.UpdateMenu();
                return true;
            }
            else
            {
                return false;
            }
        }

        // <summary>
        // Creates the context menu. This can be a single menu item or a tree of them.
        // Here we create the menu based on the type of item
        // </summary>
        // <returns>
        // The context menu for the shell context menu.
        // </returns>
        protected override ContextMenuStrip CreateMenu()
        {
            menu.Items.Clear();
            FileAttributes attr = File.GetAttributes(SelectedItemPaths.First());

            // check if the selected item is a directory
            if (attr.HasFlag(FileAttributes.Directory)) 
            {
                this.MenuDirectory();
            }
            else
            {
                this.MenuFiles();
            }

            // return the menu item
            return menu;
        }

        // <summary>
        // Updates the context menu. 
        // </summary>
        private void UpdateMenu()
        {
            // release all resources associated to existing menu
            menu.Dispose();
            menu = CreateMenu();
        }

        // <summary>
        // Creates the context menu when the selected item is a folder.
        // </summary>
        protected void MenuDirectory()
        {
            ToolStripMenuItem MainMenu;
            MainMenu = new ToolStripMenuItem
            {
                Text = "MenuDirectory",
                Image = Properties.Resources.Folder_icon
            };

                    ToolStripMenuItem SubMenu1;
                    SubMenu1 = new ToolStripMenuItem
                    {
                        Text = "DirSubMenu1",
                        Image = Properties.Resources.Folder_icon
                    };

                    var SubMenu2 = new ToolStripMenuItem
                    {
                        Text = "DirSubMenu2",
                        Image = Properties.Resources.Folder_icon
                    };
                    SubMenu2.DropDownItems.Clear();
                    SubMenu2.Click += (sender, args) => ShowItemName();

                            var SubSubMenu1 = new ToolStripMenuItem
                            {
                                Text = "DirSubSubMenu1",
                                Image = Properties.Resources.Folder_icon
                            };
                            SubSubMenu1.Click += (sender, args) => ShowItemName();
            
            // Lets attach the submenus to the main menu
            SubMenu1.DropDownItems.Add(SubSubMenu1);
            MainMenu.DropDownItems.Add(SubMenu1);
            MainMenu.DropDownItems.Add(SubMenu2);

            menu.Items.Clear();
            menu.Items.Add(MainMenu);
        }

        // <summary>
        // Creates the context menu when the selected item is of file type.
        // </summary>
        protected void MenuFiles()
        {
            ToolStripMenuItem MainMenu;
            MainMenu = new ToolStripMenuItem
            {
                Text = "MenuFiles",
                Image = Properties.Resources.file_icon
            };

                    ToolStripMenuItem SubMenu3;
                    SubMenu3 = new ToolStripMenuItem
                    {
                        Text = "FileSubMenu1",
                        Image = Properties.Resources.file_icon
                    };

                    var SubMenu4 = new ToolStripMenuItem
                    {
                        Text = "FileSubMenu2",
                        Image = Properties.Resources.file_icon
                    };
                    SubMenu4.DropDownItems.Clear();
                    SubMenu4.Click += (sender, args) => ShowItemName();

                            var SubSubMenu3 = new ToolStripMenuItem
                            {
                                Text = "FileSubSubMenu1",
                                Image = Properties.Resources.file_icon
                            };
                            SubSubMenu3.Click += (sender, args) => ShowItemName();

            // Lets attach the submenus to the main menu
            SubMenu3.DropDownItems.Add(SubSubMenu3);
            MainMenu.DropDownItems.Add(SubMenu3);
            MainMenu.DropDownItems.Add(SubMenu4);

            menu.Items.Clear();
            menu.Items.Add(MainMenu);
        }

        // <summary>
        // Shows name of selected files.
        // </summary>
        private void ShowItemName()
        {
            //  Builder for the output.
            var builder = new StringBuilder();
            FileAttributes attr = File.GetAttributes(SelectedItemPaths.First());

            //  ckeck if selected item is a directory.
            if (attr.HasFlag(FileAttributes.Directory))
            {
                //  Show folder name.
                builder.AppendLine(string.Format("Selected folder name is {0}", Path.GetFileName(SelectedItemPaths.First())));
            }
            else
            {
                {
                    //  Show the file name.
                    builder.AppendLine(string.Format("Selected file is {0}", Path.GetFileName(SelectedItemPaths.First())));
                }
            }

            //  Show the ouput.
            MessageBox.Show(builder.ToString());
        }
    }
}