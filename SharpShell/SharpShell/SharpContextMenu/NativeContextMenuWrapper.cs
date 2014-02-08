using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpShell.Interop;

namespace SharpShell.SharpContextMenu
{
    /// <summary>
    /// The Native Context Menu Wrapper builds a native context menu from a WinForms context
    /// menu. It also allows command indexes and verbs back to the original menu items.
    /// </summary>
    internal class NativeContextMenuWrapper
    {
        /// <summary>
        /// Resets the native context menu.
        /// </summary>
        public void ResetNativeContextMenu()
        {
            //  Reset any existing data.
            indexedCommands.Clear();
            verbsToCommands.Clear();
        }

        /// <summary>
        /// Builds a native context menu, on to the provided HMENU.
        /// </summary>
        /// <param name="hMenu">The handle to the menu.</param>
        /// <param name="firstItemId">The first item id.</param>
        /// <param name="toolStripItems">The tool strip menu items.</param>
        /// <returns>The index of the last item created.</returns>
        public uint BuildNativeContextMenu(IntPtr hMenu, uint firstItemId, ToolStripItemCollection toolStripItems)
        {
            //  Create an ID counter and position counter.
            var idCounter = firstItemId;
            uint positionCounter = 0;

            //  Go through every tool strip item.
            foreach (ToolStripItem item in toolStripItems)
            {
                //  If there's not a name, set one. 
                //  This will be used as the verb.
                if (string.IsNullOrEmpty(item.Name))
                    item.Name = Guid.NewGuid().ToString();

                //  Map the command by verb and id.
                verbsToCommands[item.Name] = item;
                idsToItems[idCounter] = item;

                //  Create the native menu item info.
                var menuItemInfo = CreateNativeMenuItem(item, idCounter);

                //  Insert the native menu item.
                if (User32.InsertMenuItem(hMenu, positionCounter, true, ref menuItemInfo) == false)
                {
                    //  We failed to build the item, so don't try and do anything more with it.
                    continue;
                }

                //  We successfully created the menu item, so increment the counters.
                indexedCommands.Add(item);
                idCounter++;
                positionCounter++;

                //  Have we just built a menu item? If so, does it have child items?
                var toolStripMenuItem = item as ToolStripMenuItem;
                if (toolStripMenuItem != null && toolStripMenuItem.HasDropDownItems)
                {
                    //  Create each drop down item.
                    idCounter = BuildNativeContextMenu(menuItemInfo.hSubMenu, idCounter, toolStripMenuItem.DropDownItems);
                }
            }

            //  Return the counter.
            return idCounter;
        }

        /// <summary>
        /// Creates the native menu item.
        /// </summary>
        /// <param name="toolStripItem">The tool strip item.</param>
        /// <param name="id">The id.</param>
        /// <returns>The native menu ite,.</returns>
        private static MENUITEMINFO CreateNativeMenuItem(ToolStripItem toolStripItem, uint id)
        {
            //  Create a menu item info, set its size.
            var menuItemInfo = new MENUITEMINFO();
            menuItemInfo.cbSize = (uint)Marshal.SizeOf(menuItemInfo);
            menuItemInfo.wID = id;
            
            //  Depending on the type of the item, we'll call the appropriate building function.
            if(toolStripItem is ToolStripMenuItem)
                BuildMenuItemInfo(ref menuItemInfo, (ToolStripMenuItem)toolStripItem);
            else if(toolStripItem is ToolStripSeparator)
                BuildSeparatorMenuItemInfo(ref menuItemInfo);

            //  Return the menu item info.
            return menuItemInfo;

        }

        /// <summary>
        /// Builds the menu item info.
        /// </summary>
        /// <param name="menuItemInfo">The menu item info.</param>
        /// <param name="menuItem">The menu item.</param>
        private static void BuildMenuItemInfo(ref MENUITEMINFO menuItemInfo, ToolStripMenuItem menuItem)
        {
            //  Set the mask - we're interested in essentially everything.
            menuItemInfo.fMask = (uint)(MIIM.MIIM_BITMAP | MIIM.MIIM_STRING | MIIM.MIIM_FTYPE |
                                         MIIM.MIIM_ID | MIIM.MIIM_STATE);

            //  If the menu item has children, we'll also create the submenu.
            if (menuItem.HasDropDownItems)
            {
                menuItemInfo.fMask += (uint) MIIM.MIIM_SUBMENU;
                menuItemInfo.hSubMenu = User32.CreatePopupMenu();
            }
            
            //  The type is the string.
            menuItemInfo.fType = (uint)MFT.MFT_STRING;

            //  The type data is the text of the menu item.
            menuItemInfo.dwTypeData = menuItem.Text;

            //  The state is enabled.
            menuItemInfo.fState = menuItem.Enabled ? (uint)MFS.MFS_ENABLED : (uint)(MFS.MFS_DISABLED | MFS.MFS_GRAYED);

            //  If the menu item is checked, add the check state.
            if (menuItem.Checked)
            {
                menuItemInfo.fState += (uint) MFS.MFS_CHECKED;
            }

            //  Is there an icon?
            var bitmap = menuItem.Image as Bitmap;
            if (bitmap != null)
                menuItemInfo.hbmpItem = PARGB32.CreatePARGB32HBitmap(bitmap.GetHicon(), bitmap.Size);
        }


        /// <summary>
        /// Builds the menu item info.
        /// </summary>
        /// <param name="menuItemInfo">The menu item info.</param>
        private static void BuildSeparatorMenuItemInfo(ref MENUITEMINFO menuItemInfo)
        {
            //  Set the mask to the type only, and the type to the separator type.
            menuItemInfo.fMask = (uint)MIIM.MIIM_TYPE;
            menuItemInfo.fType = (uint)MFT.MFT_SEPARATOR;
        }

        /// <summary>
        /// Tries to invoke the command.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>True if the command is invoked.</returns>
        public bool TryInvokeCommand(int index)
        {
            //  Get the item. If we don't have it, return false.
            if (index >= indexedCommands.Count)
                return false;

            //  Fire the click event.
            indexedCommands[index].PerformClick();

            //  The command was invoked, return true,
            return true;
        }

        /// <summary>
        /// Tries to invoke the command.
        /// </summary>
        /// <param name="verb">The verb.</param>
        /// <returns>True if the command is invoked.</returns>
        public bool TryInvokeCommand(string verb)
        {
            //  Get the item. If we don't have it, return false.
            ToolStripItem item;
            if (verbsToCommands.TryGetValue(verb, out item) == false)
                return false;

            //  Fire the click event.
            item.PerformClick();

            //  The command was invoked, return true,
            return true;
        }

        private readonly Dictionary<uint, ToolStripItem> idsToItems = new Dictionary<uint, ToolStripItem>();
        
        /// <summary>
        /// Map of indexes to commands.
        /// </summary>
        private readonly List<ToolStripItem> indexedCommands = new List<ToolStripItem>();

        /// <summary>
        /// Map of verbs to commands.
        /// </summary>
        private readonly Dictionary<string, ToolStripItem> verbsToCommands = new Dictionary<string, ToolStripItem>();
    }
}
