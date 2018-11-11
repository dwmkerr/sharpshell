using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using Apex.WinForms.Interop;
using Apex.WinForms.Shell;
using Microsoft.Win32;
using SharpShell;
using SharpShell.Attributes;
using SharpShell.Interop;
using SharpShell.Registry;
using SharpShell.ServerRegistration;
using SharpShell.ServiceRegistry;
using SharpShell.SharpContextMenu;
using SharpShell.SharpDropHandler;
using SharpShell.SharpIconHandler;
using SharpShell.SharpIconOverlayHandler;
using SharpShell.SharpInfoTipHandler;
using SharpShell.SharpPreviewHandler;
using SharpShell.SharpThumbnailHandler;
using SFGAO = Apex.WinForms.Interop.SFGAO;

namespace ServerManager.TestShell
{
    /// <summary>
    /// The TestShellForm is a simple form that can be used to test SharpShell
    /// COM servers.
    /// </summary>
    public partial class TestShellForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestShellForm" /> class.
        /// </summary>
        public TestShellForm()
        {
            InitializeComponent();

            lazyBoldFont = new Lazy<Font>(() => new Font(Font, FontStyle.Bold));

            shellTreeView.OnShellItemAdded += new TreeViewEventHandler(shellTreeView_OnShellItemAdded);
            shellListView.OnShellItemAdded += new Apex.WinForms.Controls.ListViewItemEventHandler(shellListView_OnShellItemAdded);

            //  Create the ordered view menu items.
            orderedViewMenuItems.Add(largeIconsToolStripMenuItem);
            orderedViewMenuItems.Add(toolStripMenuItemSmallIcons);
            orderedViewMenuItems.Add(listToolStripMenuItem);
            orderedViewMenuItems.Add(detailsToolStripMenuItem);
            orderedViewMenuItems.Add(tileToolStripMenuItem);

            shellListView.Columns.Add(new ColumnHeader {Text = "Name"});
        }

        void shellTreeView_OnShellItemAdded(object sender, TreeViewEventArgs e)
        {
            /*var shellItem = shellTreeView.GetShellItem(e.Node);
            if (IsServerAssociatedWithShellItem(TestIconHandler, shellItem))
            {
                DoTestIconHandler(e.Node);
            }*/
        }

        void shellListView_OnShellItemAdded(object sender, Apex.WinForms.Controls.ListViewItemEventArgs args)
        {
            var shellItem = shellListView.GetShellItem(args.Item);

            //  If the icon handler is associated with the the item, test it.
            if (IsServerAssociatedWithShellItem(TestIconHandler, shellItem))
                DoTestIconHandler(args.Item);

            //  If the info tip handler is associated with the item, test it.
            if (IsServerAssociatedWithShellItem(TestInfoTipHandler, shellItem))
                DoTestInfoTipHandler(args.Item);

            //  If the drop handler is associated with the item, test it.
            if (IsServerAssociatedWithShellItem(TestDropHandler, shellItem))
                DoTestDropHandler(args.Item);

            //  If the preview handler is associated with the item, test it.
            if (IsServerAssociatedWithShellItem(TestPreviewHandler, shellItem))
                DoTestPreviewHandler(args.Item);

            //  If a thumbnail handleris associated with the item, highlight it.
            if(IsServerAssociatedWithShellItem(TestThumbnailHandler, shellItem))
                HighlightItem(args.Item);

            //  If an icon overlay handler is associated with the item, highlight it.
            if(IsServerAssociatedWithShellItem(TestIconOverlayHandler, shellItem))
                HighlightItem(args.Item);
        }

        private void shellTreeView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (shellTreeView.SelectedNode == null) return;
                
                //  Get the point in screen coords.
                var screenPoint = shellTreeView.PointToScreen(new Point(e.X, e.Y));

                //  Get the shell item.
                var shellItem = shellTreeView.GetShellItem(shellTreeView.SelectedNode);

                //  Test it.
                DoTestMenu(new [] {shellItem}, screenPoint.X, screenPoint.Y);
            }
        }

        private void shellListView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //  Get the highlighted items.
                var items = shellListView.SelectedItems.OfType<ListViewItem>().Select(lvi => shellListView.GetShellItem(lvi)).ToArray();

                //  Get the point in screen coords.
                var screenPoint = shellListView.PointToScreen(new Point(e.X, e.Y));

                //  Test it.
                DoTestMenu(items, screenPoint.X, screenPoint.Y);
            }
        }

        /// <summary>
        /// Tests the context menu.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        private void DoTestMenu(ShellItem[] items, int x, int y)
        {
            //  If we don't have a context menu, we can bail now.
            if (TestContextMenu == null)
                return;

            //  Get the interfaces we need to test with.
            var shellExtInitInterface = (IShellExtInit) TestContextMenu;
            var contextMenuInterface = (IContextMenu) TestContextMenu;
            
            //  Try init first.
            try
            {
                //  Create the file paths.
                var filePaths = new StringCollection();
                filePaths.AddRange(items.Select(i => i.Path).ToArray());

                //  Create the data object from the file paths.
                var dataObject = new DataObject();
                dataObject.SetFileDropList(filePaths);

                //  Get the IUnknown COM interface address. Jesus .NET makes this easy.
                var dataObjectInterfacePointer = Marshal.GetIUnknownForObject(dataObject);

                //  Pass the data to the shell extension, attempt to initialise it.
                //  We must provide the data object as well as the parent folder PIDL.
                if (items.Any())
                {
                    var folderPIDL = items.First().ParentItem.PIDL;
                    shellExtInitInterface.Initialize(folderPIDL, dataObjectInterfacePointer, IntPtr.Zero);
                }
            }
            catch (Exception)
            {
                //  Not supported for the file
                return;
            }

            //  Create a native menu.
            var menuHandle = CreatePopupMenu();

            //  Build the menu.
            contextMenuInterface.QueryContextMenu(menuHandle, 0, 0, 1, 0);

            //  Track the menu.
            TrackPopupMenu(menuHandle,
                           0, x, y, 0, Handle, IntPtr.Zero);
        }

        /// <summary>
        /// The windows message pump.
        /// </summary>
        /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            //  Do we have a comamnd and a shell context menu we're testing?
            if (m.Msg == WM_COMMAND && TestContextMenu != null)
            {
                var loword = LowWord(m.WParam.ToInt32());
                var hiword = HighWord(m.WParam.ToInt32());

                //  If the hiword is 0 it's a menu command.
                if (hiword == 0)
                {
                    //  Create command info.
                    var commandInfo = new CMINVOKECOMMANDINFO();
                    commandInfo.cbSize = (uint) Marshal.SizeOf(commandInfo);
                    commandInfo.verb = new IntPtr(loword);

                    //  Get a pointer to the structure.
                    var commandInfoPointer = Marshal.AllocHGlobal(Marshal.SizeOf(commandInfo));
                    Marshal.StructureToPtr(commandInfo, commandInfoPointer, false);

                    ((IContextMenu) TestContextMenu).InvokeCommand(commandInfoPointer);
                }
            }
        }

        public static int HighWord(int number)
        {
            return ((number & 0x80000000) == 0x80000000) ?
                                                             (number >> 16) : ((number >> 16) & 0xffff);
        }

        public static int LowWord(int number)
        {
            return number & 0xffff;
        }

        private void shellListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var shellItem = shellListView.SelectedItems.Count > 0 ? shellListView.GetShellItem(shellListView.SelectedItems[0]) : null;

            if (shellItem != null)
                toolStripStatusLabelAttributes.Text = "Attributes: " + shellItem.Attributes.ToString();

            propertyGridSelectedObject.SelectedObject = shellItem;
            
            if (IsServerAssociatedWithShellItem(TestPreviewHandler, shellItem))
            {
                shellPreviewHost1.SetPreviewHandler(TestPreviewHandler.ServerClsid);
                shellPreviewHost1.SetPreviewItem(shellItem.Path);
            }

            if (IsServerAssociatedWithShellItem(TestThumbnailHandler, shellItem))
            {
                shellThumbnailHost1.SetThumbnailHandler(TestThumbnailHandler);
                shellThumbnailHost1.SetPreviewItem(shellItem);
            }

            if(IsServerAssociatedWithShellItem(TestIconOverlayHandler, shellItem))
            {
                /*
                 * Icon overlays currently not tested due to exception.
                 * 
                var initialString = new string(' ', 260);
                var stringPointer  = Marshal.StringToHGlobalUni(initialString);
                int iconIndex;
                ISIOI iconFlags;
                ((IShellIconOverlayIdentifier) TestIconOverlayHandler).GetOverlayInfo(stringPointer, 260, out iconIndex,
                                                                                      out iconFlags);
                var path = Marshal.PtrToStringUni(stringPointer);

                //  Load the icon.
                var icon = new Icon(path);
                var iconImage = icon.ToBitmap();
                pictureBoxIconOverlay.Image = iconImage;
                 * 
                 * */
            }
        }

        private void DoTestIconHandler(ListViewItem item)
        {
            //  Get the shell item.
            try
            {
                var shellItem = shellListView.GetShellItem(item);

                IntPtr iconSmall, iconLarge;
                GetIconHandlerIcons(TestIconHandler, shellItem.Path, out iconSmall, out iconLarge);

                //  We're testing the item, so make it bold.
                item.Font = lazyBoldFont.Value;

                //  Add the icons.
                var largeIcon = Icon.FromHandle(iconLarge);
                var smallIcon = Icon.FromHandle(iconSmall);
                var newIndex = shellListView.LargeImageList.Images.Count;
                shellListView.LargeImageList.Images.Add(largeIcon);
                shellListView.SmallImageList.Images.Add(smallIcon);

                //  Set the icon.
                item.ImageIndex = newIndex;
            }
            catch (Exception)
            {
            }
        }
        
        private void GetIconHandlerIcons(SharpIconHandler iconHandler, string path, out IntPtr iconSmall, out IntPtr iconLarge)
        {
            //  Test the persist file.
            var persistFile = (IPersistFile)iconHandler;
            persistFile.Load(path, 0);

            //  Test the icon handler.
            var extractIcon = (IExtractIconW)iconHandler;
            var size = 32 + (16 << 16);
            extractIcon.Extract(path, 0, out iconLarge, out iconSmall, (uint)size);
        }

        private void DoTestInfoTipHandler(ListViewItem item)
        {
            if (TestInfoTipHandler == null)
                return;

            //  Get the shell item.
            try
            {
                var shellItem = shellListView.GetShellItem(item);

                //  Initialise the icon handler.
                var persistFileInterface = (IPersistFile)TestInfoTipHandler;
                persistFileInterface.Load(shellItem.Path, 0);

                //  Get the info tip.
                var queryInfoInterface = (IQueryInfo)TestInfoTipHandler;
                string infoTip;
                queryInfoInterface.GetInfoTip(QITIPF.QITIPF_DEFAULT, out infoTip);

                //  Set the tooltip.
                item.Font = lazyBoldFont.Value;
                item.ToolTipText = infoTip;
            }
            catch (Exception)
            {
            }
        }



        private void DoTestDropHandler(ListViewItem item)
        {
            if (TestDropHandler == null)
                return;

                //  Add the item to the set of test drop items.
                testDropItems.Add(item);

                //  Highlight the item.
                HighlightItem(item);
        }

        private void DoTestPreviewHandler(ListViewItem item)
        {
            if (TestPreviewHandler == null)
                return;
            
            //  Highlight the item.
            HighlightItem(item);
        }

        /// <summary>
        /// Highlights a list view item.
        /// </summary>
        /// <param name="listViewItem">The list view item.</param>
        private void HighlightItem(ListViewItem listViewItem)
        {
            //  Set the font to bold.
            listViewItem.Font = lazyBoldFont.Value;
        }

        private readonly List<ListViewItem> testDropItems = new List<ListViewItem>(); 

        /// <summary>
        /// Determines whether a server is associated with a shell item.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="shellItem">The shell item.</param>
        /// <returns>
        ///   <c>true</c> if a server is associated with the shell item; otherwise, <c>false</c>.
        /// </returns>
        private bool IsServerAssociatedWithShellItem(ISharpShellServer server, ShellItem shellItem)
        {
            //  If we don't have the server, bail.
            if (server == null || shellItem == null)
                return false;

            //  Get the associations.
            var associationType = COMServerAssociationAttribute.GetAssociationType(server.GetType());
            var associations = COMServerAssociationAttribute.GetAssociations(server.GetType());

            //  TODO: This is potentially a very useful check - maybe it should be moved into the
            //  COMServerAssociationAttribute class so that it can be reused.

            //  We have a special case for icon overlays.
            if (server is SharpIconOverlayHandler && TestIconOverlayHandler != null && shellItem.Attributes.HasFlag(SFGAO.SFGAO_FILESYSTEM))
                if (((IShellIconOverlayIdentifier)TestIconOverlayHandler).IsMemberOf(shellItem.Path, FILE_ATTRIBUTE.FILE_ATTRIBUTE_NORMAL) == 0)
                    return true;

            //  Based on the assocation type, we can check the shell item.
            switch (associationType)
            {
                //  This is checked for backwards compatibility.
#pragma warning disable 618
                case AssociationType.FileExtension:
#pragma warning restore 618

                    //  File extensions are easy to check.
                    if (shellItem.Attributes.HasFlag(SFGAO.SFGAO_FILESYSTEM))
                    {
                        return
                            associations.Any(
                                a =>
                                string.Compare(Path.GetExtension(shellItem.DisplayName), a,
                                               StringComparison.OrdinalIgnoreCase) == 0);
                    }

                    break;

                case AssociationType.ClassOfExtension:

                    //  TODO must be tested.
                    if (shellItem.Attributes.HasFlag(SFGAO.SFGAO_FILESYSTEM))
                    {
                        //  Get our class.
                        var registry = ServiceRegistry.GetService<IRegistry>();
                        using (var classesRoot = registry.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Default))
                        {
                            var fileClass = FileExtensionClass.Get(classesRoot, Path.GetExtension(shellItem.DisplayName), false);

                            //  Do we match it?
                            return associations.Any(a => string.Compare(fileClass, FileExtensionClass.Get(classesRoot, a, false), StringComparison.InvariantCultureIgnoreCase) == 0);
                        }

                        
                    }

                    break;

                case AssociationType.Class:
                    //  TODO must be tested.
                    break;

                case AssociationType.AllFiles:

                    //  TODO must be tested.
                    return shellItem.Attributes.HasFlag(SFGAO.SFGAO_FILESYSTEM) && shellItem.IsFolder == false;

                case AssociationType.Directory:

                    //  Directories are filesystem, not streams, and folder.
                    return shellItem.Attributes.HasFlag(SFGAO.SFGAO_FILESYSTEM) && !shellItem.Attributes.HasFlag(SFGAO.SFGAO_STREAM) && shellItem.IsFolder;

                case AssociationType.Drive:

                    //  TODO must be tested.
                    return shellItem.Attributes.HasFlag(SFGAO.SFGAO_STORAGEANCESTOR);

                case AssociationType.UnknownFiles:
                    //  TODO must be tested.
                    break;
            }

            return false;
        }

        private void toolStripMenuItemSmallIcons_Click(object sender, EventArgs e)
        {
            shellListView.View = View.SmallIcon;
            SetViewIndex(1);
        }

        private void largeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            shellListView.View = View.LargeIcon;
            SetViewIndex(0);
        }

        private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            shellListView.View = View.Details;
            SetViewIndex(3);
        }

        private void tileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            shellListView.View = View.Tile;
            SetViewIndex(4);
        }

        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            shellListView.View = View.List;
            SetViewIndex(2);
        }

        /// <summary>
        /// The view menu items, in order.
        /// </summary>
        private List<ToolStripMenuItem> orderedViewMenuItems = new List<ToolStripMenuItem>();

        private readonly Lazy<Font> lazyBoldFont;

        private const uint WM_CONTEXTMENU = 0x007B;
        private const uint WM_COMMAND = 0x0111;


        [DllImport("User32.dll")]
        internal static extern IntPtr CreateMenu();

        [DllImport("User32.dll")]
        internal static extern IntPtr CreatePopupMenu();

        [DllImport("User32.dll")]
        internal static extern bool TrackPopupMenu(IntPtr hMenu,
                                                   uint uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr pRect);

        [DllImport("User32.dll")]
        internal static extern int GetMenuItemCount(IntPtr hMenu);

        /// <summary>
        /// Gets or sets the test server.
        /// </summary>
        /// <value>
        /// The test server.
        /// </value>
        public ISharpShellServer TestServer { get; set; }

        /// <summary>
        /// Gets or sets the test context menu.
        /// </summary>
        /// <value>
        /// The test context menu.
        /// </value>
        public SharpContextMenu TestContextMenu { get { return TestServer as SharpContextMenu; } }

        /// <summary>
        /// Gets or sets the test icon handler.
        /// </summary>
        /// <value>
        /// The test icon handler.
        /// </value>
        public SharpIconHandler TestIconHandler { get { return TestServer as SharpIconHandler; } }

        /// <summary>
        /// Gets the test thumbnail handler.
        /// </summary>
        /// <value>
        /// The test thumbnail handler.
        /// </value>
        public SharpThumbnailHandler TestThumbnailHandler { get { return TestServer as SharpThumbnailHandler; } }

        /// <summary>
        /// Gets or sets the test info tip handler.
        /// </summary>
        /// <value>
        /// The test info tip handler.
        /// </value>
        public SharpInfoTipHandler TestInfoTipHandler { get { return TestServer as SharpInfoTipHandler; } }

        /// <summary>
        /// Gets the test drop handler.
        /// </summary>
        public SharpDropHandler TestDropHandler { get { return TestServer as SharpDropHandler; } }

        /// <summary>
        /// Gets the test preview handler.
        /// </summary>
        /// <value>
        /// The test preview handler.
        /// </value>
        public SharpPreviewHandler TestPreviewHandler { get { return TestServer as SharpPreviewHandler; } }

        /// <summary>
        /// Gets the test icon overlay handler.
        /// </summary>
        public SharpIconOverlayHandler TestIconOverlayHandler { get { return TestServer as SharpIconOverlayHandler; } }

        private void toolStripSplitButtonChangeYourView_ButtonClick(object sender, EventArgs e)
        {
           //  Update it.
            currentViewIndex++;
            if (currentViewIndex >= orderedViewMenuItems.Count)
                currentViewIndex = 0;

            SetViewIndex(currentViewIndex);
            orderedViewMenuItems[currentViewIndex].PerformClick();
        }

        private void SetViewIndex(int index)
        {
            currentViewIndex = index;

            //  Get the menu item to change to.
            var newItem = orderedViewMenuItems[currentViewIndex];

            //  Set the icon.
            toolStripSplitButtonChangeYourView.Image = newItem.Image;
        }

        private int currentViewIndex = 0;

        private void shellListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            //dragItems.Clear();
            //dragItems.Add( shellListView.GetShellItem((ListViewItem)e.Item));
            //  Allow drags.
            //shellListView.DoDragDrop(e.Item, DragDropEffects.All);
        }

        private List<ShellItem> dragItems = new List<ShellItem>(); 

        private void shellListView_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            //e.Action = DragAction.Continue;
            /*var info = shellListView.HitTest(shellListView.PointToClient(System.Windows.Forms.Cursor.Position));
            if (info.Item != null && testDropItems.Contains(info.Item) && TestDropHandler != null)
            {
                //  Get the paths.
                var paths = new StringCollection();
                paths.AddRange(dragItems.Select(di => di.Path).ToArray());

                uint effect = 0;
                var dataObject = new DataObject();
                var position = new POINT() {X = Cursor.Position.X, Y = Cursor.Position.Y};
                dataObject.SetFileDropList(paths);
                ((SharpShell.Interop.IDropTarget) TestDropHandler).DragEnter(
                    dataObject, 0, position, ref effect);

                var effects = (DragDropEffects) effect;

                e.Action = effects != DragDropEffects.None ? DragAction.Drop : DragAction.Cancel;
            }*/
        }

        private void shellListView_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            
        }

        private void shellListView_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void toolStripButtonShowProperties_Click(object sender, EventArgs e)
        {
            var path = shellListView.GetShellItem(shellListView.SelectedItems[0]).Path;

            const int SW_SHOW = 5;

            var shell_ex = new SHELLEXECUTEINFO
            {
                cbSize = Marshal.SizeOf(new SHELLEXECUTEINFO()),
                lpFile = path,
                nShow = SW_SHOW,
                fMask = SEE.SEE_MASK_INVOKEIDLIST,
                lpVerb = "Properties"
            };

            Shell32.ShellExecuteEx(ref shell_ex);        
        }

        private void toolStripButtonShellOpenDialog_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog(this);
        }

        private void shellTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //  Set the selected of the property grid.
            propertyGridSelectedObject.SelectedObject = e.Node != null ? shellTreeView.GetShellItem(e.Node) : null;
        }
    }
}