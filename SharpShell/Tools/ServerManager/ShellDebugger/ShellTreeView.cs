using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SharpShell.Interop;
using SharpShell.Pidl;

namespace ServerManager.ShellDebugger
{
    /// <summary>
    /// The ShellTreeView is a tree view that is designed to show contents of the system,
    /// just like in Windows Explorer.
    /// </summary>
    public class ShellTreeView : TreeView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellTreeView"/> class.
        /// </summary>
        public ShellTreeView()
        {
            //  TODO: Shell tree views should be double buffered.

            //  Set the image list to the shell image list.
            this.SetImageList(TreeViewExtensions.ImageListType.Normal, ShellImageList.GetImageList(ShellImageListSize.Small));

            this.AfterSelect += ShellTreeView_AfterSelect;
            
        }

        void ShellTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var shellItem = GetShellItem(e.Node);
            FireOnShellItemSelected(shellItem);
        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.Control.CreateControl"/> method.
        /// </summary>
        protected override void OnCreateControl()
        {
            //  Call the base.
            base.OnCreateControl();

            //  Add the desktop node, if we're not in design mode.
            if (!DesignMode)
                AddDesktopNode();
        }

        /// <summary>
        /// Adds the desktop node.
        /// </summary>
        private void AddDesktopNode()
        {
            //  Get the desktop folder.
            var desktopFolder = ShellItem.DesktopShellFolder;

            //  Create the desktop node.
            var desktopNode = new TreeNode
                                  {
                                      Text = desktopFolder.DisplayName,
                                      ImageIndex = desktopFolder.IconIndex,
                                      SelectedImageIndex = desktopFolder.IconIndex,
                                  };

            //  Map it and add it.
            nodesToFolders[desktopNode] = desktopFolder;
            Nodes.Add(desktopNode);

            //  Fire the event.
            FireOnShellItemAdded(desktopNode);
            
            //  Expand it.
            OnBeforeExpand(new TreeViewCancelEventArgs(desktopNode, false, TreeViewAction.Expand));
            desktopNode.Expand();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.TreeView.BeforeExpand"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.TreeViewCancelEventArgs"/> that contains the event data.</param>
        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            //  Get the node.
            var node = e.Node;

            //  Clear children - which may in fact be the placeholder.
            node.Nodes.Clear();

            //  Get the shell folder.
            var shellFolder = nodesToFolders[node];

            //  Create the enum flags.
            var childFlags = ChildTypes.Folders | ChildTypes.Files;
            if(ShowFiles)
                childFlags |= ChildTypes.Files;
            if (ShowHiddenFilesAndFolders)
                childFlags |= ChildTypes.Hidden;

            //  Disable update while adding children.
            BeginUpdate();

            //  Go through each child.
            foreach (var child in shellFolder.GetChildren(childFlags))
            {
                //  Create a child node.
                var childNode = new TreeNode
                                    {
                                        Text = child.DisplayName,
                                        ImageIndex = child.IconIndex,
                                        SelectedImageIndex = child.IconIndex,
                                    };

                //  Map the node to the shell folder.
                nodesToFolders[childNode] = child;

                //  If this item has children, add a child node as a placeholder.
                if (child.HasSubFolders)
                    childNode.Nodes.Add(string.Empty);

                //  Add the child node.
                node.Nodes.Add(childNode);

                //  Fire the shell item added event.
                FireOnShellItemAdded(childNode);
            }
            
            //  Enable update now that we've added the children.
            EndUpdate();

            //  Call the base.
            base.OnBeforeExpand(e);
        }

        /// <summary>
        /// Gets the shell item for a tree node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The shell item for the tree node.</returns>
        public ShellItem GetShellItem(TreeNode node)
        {
            ShellItem shellFolder;
            if(nodesToFolders.TryGetValue(node, out shellFolder))
                return shellFolder;
            return null;
        }

        /// <summary>
        /// Fires the on shell item added event.
        /// </summary>
        /// <param name="nodeAdded">The node added.</param>
        private void FireOnShellItemAdded(TreeNode nodeAdded)
        {
            //  Fire the event if we have it.
            var theEvent = OnShellItemAdded;
            if(theEvent != null)
                theEvent(this, new TreeViewEventArgs(nodeAdded));
        }

        private void FireOnShellItemSelected(ShellItem shellItem)
        {
            var theEvent = OnShellItemSelected;
            if(theEvent != null)
                theEvent(this, new ShellTreeEventArgs(shellItem));
        }

        /// <summary>
        /// A map of tree nodes to the Shell Folders.
        /// </summary>
        private readonly Dictionary<TreeNode, ShellItem> nodesToFolders = new Dictionary<TreeNode, ShellItem>();

        /// <summary>
        /// Gets or sets a value indicating whether to show hidden files and folders.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if show hidden files and folders; otherwise, <c>false</c>.
        /// </value>
        [Category("Shell Tree View")]
        [Description("If set to true, hidden files and folders will be shown.")]
        public bool ShowHiddenFilesAndFolders { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show files.
        /// </summary>
        /// <value>
        ///   <c>true</c> if show files; otherwise, <c>false</c>.
        /// </value>
        [Category("Shell Tree View")]
        [Description("If set to true, files will be shown as well as folders.")]
        public bool ShowFiles { get; set; }

        /// <summary>
        /// Gets a value that indicates whether the <see cref="T:System.ComponentModel.Component"/> is currently in design mode.
        /// </summary>
        /// <returns>true if the <see cref="T:System.ComponentModel.Component"/> is in design mode; otherwise, false.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool DesignMode { get { return (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv"); } }

        /// <summary>
        /// Occurs when a shell item is added.
        /// </summary>
        [Category("Shell Tree View")]
        [Description("Called when a shell item is added.")]
        public event TreeViewEventHandler OnShellItemAdded;

        public event ShellItemTreeEventHandler OnShellItemSelected;

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ShellTreeView
            // 
            this.ResumeLayout(false);

        }

        protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
        {
            base.OnNodeMouseClick(e);

            if (e.Button == MouseButtons.Right)
            {
                //  Get the item hit.
                var itemHit = GetShellItem(e.Node);

                //  Create a default context menu.
                OpenItemContextMenu(itemHit, e.X, e.Y);

            }
        }

        private void OpenItemContextMenu(ShellItem itemHit, int x, int y)
        {
                //  TODO: we need a min and max for the menu items.

            //  see http://www.codeproject.com/Articles/4025/Use-Shell-ContextMenu-in-your-applications for more

            //  The shell folder we use to get the UI object is either the folder itself if the
            //  item is a folder, or the parent folder otherwise.
            var shellFolder = itemHit.IsFolder ? itemHit.ShellFolderInterface : itemHit.ParentItem.ShellFolderInterface;

            //  The item pidl is either the folder if the item is a folder, or the combined pidl otherwise.
            var fullIdList = itemHit.IsFolder
                ? PidlManager.PidlToIdlist(itemHit.PIDL)
                : PidlManager.Combine(PidlManager.PidlToIdlist(itemHit.ParentItem.PIDL),
                    PidlManager.PidlToIdlist(itemHit.RelativePIDL));
                

            //  Get the UI object of the context menu.
            IntPtr apidl = PidlManager.PidlsToAPidl(new IntPtr[] {PidlManager.IdListToPidl(fullIdList)});


            IntPtr ppv = IntPtr.Zero;
            shellFolder.GetUIObjectOf(Handle, 1, apidl, Shell32.IID_IContextMenu, 0,
                out ppv);

            //  If we have an item, cast it.
            if (ppv != IntPtr.Zero)
            {
                IContextMenu contextMenu = (IContextMenu)Marshal.GetObjectForIUnknown(ppv);

                var popupMenu = new ContextMenu();
                contextMenu.QueryContextMenu(popupMenu.Handle, 0, 0, 65525, CMF.CMF_EXPLORE);
                popupMenu.Show(this, new Point(x, y));
            }
        }
    }

    public delegate void ShellItemTreeEventHandler(object sender, ShellTreeEventArgs e);

    public static class TreeViewExtensions
    {
        /// <summary>
        /// TreeView ImageList type.
        /// </summary>
        public enum ImageListType
        {
            /// <summary>
            /// Normal images.
            /// </summary>
            Normal = 0,

            /// <summary>
            /// State images.
            /// </summary>
            State = 2
        }

        /// <summary>
        /// Normal tree view image.
        /// </summary>
        private const uint TVSIL_NORMAL = 0;

        /// <summary>
        /// The state images.
        /// </summary>
        private const uint TVSIL_STATE = 2;

        /// <summary>
        /// First tree view message.
        /// </summary>
        private const uint TV_FIRST = 0x1100;

        /// <summary>
        /// Get image list message.
        /// </summary>
        private const uint TVM_GETIMAGELIST = TV_FIRST + 8;

        /// <summary>
        /// Set image list message.
        /// </summary>
        private const uint TVM_SETIMAGELIST = TV_FIRST + 9;

        /// <summary>
        /// Sets the image list.
        /// </summary>
        /// <param name="this">The tree view instance.</param>
        /// <param name="imageListType">Type of the image list.</param>
        /// <param name="imageListHandle">The image list handle.</param>
        public static void SetImageList(this TreeView @this, ImageListType imageListType, IntPtr imageListHandle)
        {
            //  Set the image list.
            User32.SendMessage(@this.Handle, TVM_SETIMAGELIST, (uint)imageListType, imageListHandle);
        }

        /// <summary>
        /// Gets the image list.
        /// </summary>
        /// <param name="this">The tree view instance.</param>
        /// <param name="imageListType">Type of the image list.</param>
        /// <returns>The image list handle.</returns>
        public static IntPtr GetImageList(this TreeView @this, ImageListType imageListType)
        {
            //  Set the image list.
            return new IntPtr(User32.SendMessage(@this.Handle, TVM_GETIMAGELIST, (uint)imageListType, IntPtr.Zero));
        }
    }

    /// <summary>
    /// The Shell Image List.
    /// </summary>
    public static class ShellImageList
    {
        /// <summary>
        /// Initializes the <see cref="ShellImageList"/> class.
        /// </summary>
        static ShellImageList()
        {
        }

        /// <summary>
        /// Gets the image list interface.
        /// </summary>
        /// <param name="imageListSize">Size of the image list.</param>
        /// <returns>The IImageList for the shell image list of the given size.</returns>
        public static IntPtr GetImageList(ShellImageListSize imageListSize)
        {
            //  Do we have the image list?
            IImageList imageList;
            if (imageLists.TryGetValue(imageListSize, out imageList))
                return GetImageListHandle(imageList);

            //  We don't have the image list, create it.
            Shell32.SHGetImageList((int)imageListSize, ref Shell32.IID_IImageList, ref imageList);

            //  Add it to the dictionary.
            imageLists.Add(imageListSize, imageList);

            //  Return it.
            return GetImageListHandle(imageList);
        }

        /// <summary>
        /// Gets the image list handle.
        /// </summary>
        /// <param name="imageList">The image list.</param>
        /// <returns>The image list handle for the image list.</returns>
        private static IntPtr GetImageListHandle(IImageList imageList)
        {
            return Marshal.GetIUnknownForObject(imageList);
        }

        /// <summary>
        /// The shell image lists.
        /// </summary>
        private readonly static Dictionary<ShellImageListSize, IImageList> imageLists = new Dictionary<ShellImageListSize, IImageList>();
    }

    /// <summary>
    /// Shell Image List sizes. These correspond exactly by value to the sizes such 
    /// as SHIL_LARGE, SHIL_JUMBO, etc.
    /// </summary>
    public enum ShellImageListSize
    {
        /// <summary>
        /// The image size is normally 32x32 pixels. However, if the Use large icons option is selected from the Effects section of the Appearance tab in Display Properties, the image is 48x48 pixels.
        /// </summary>
        Large = 0x0,

        /// <summary>
        /// These images are the Shell standard small icon size of 16x16, but the size can be customized by the user.
        /// </summary>
        Small = 0x1,

        /// <summary>
        /// These images are the Shell standard extra-large icon size. This is typically 48x48, but the size can be customized by the user.
        /// </summary>
        ExtraLarge = 0x2,

        /// <summary>
        /// These images are the size specified by GetSystemMetrics called with SM_CXSMICON and GetSystemMetrics called with SM_CYSMICON.
        /// </summary>
        SysSmall = 0x3,

        /// <summary>
        /// Windows Vista and later. The image is normally 256x256 pixels.
        /// </summary>
        Jumbo = 0x4
    }

    /// <summary>
    /// Represents a ShellItem object.
    /// </summary>
    public class ShellItem : IDisposable
    {
        /// <summary>
        /// Initializes the <see cref="ShellItem"/> class.
        /// </summary>
        static ShellItem()
        {
            //  Create the lazy desktop shell folder.
            desktopShellFolder = new Lazy<ShellItem>(CreateDesktopShellFolder);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellItem"/> class.
        /// </summary>
        public ShellItem()
        {
            //  Create the lazy path.
            path = new Lazy<string>(CreatePath);
            overlayIcon = new Lazy<Icon>(CreateOverlayIcon);
        }

        /// <summary>
        /// Creates the icon.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private Icon CreateOverlayIcon()
        {
            var fileInfo = new SHFILEINFO();
            Shell32.SHGetFileInfo(PIDL, 0, out fileInfo, (uint)Marshal.SizeOf(fileInfo),
                SHGFI.SHGFI_PIDL | SHGFI.SHGFI_ICON | SHGFI.SHGFI_ADDOVERLAYS);
            return fileInfo.hIcon != IntPtr.Zero ? Icon.FromHandle(fileInfo.hIcon) : null;
        }

        /// <summary>
        /// Creates the desktop shell folder.
        /// </summary>
        /// <returns>The desktop shell folder.</returns>
        private static ShellItem CreateDesktopShellFolder()
        {
            //  Get the desktop shell folder interface. 
            IShellFolder desktopShellFolderInterface = null;
            var result = Shell32.SHGetDesktopFolder(out desktopShellFolderInterface);

            //  Validate the result.
            if (result != 0)
            {
                //  Throw the failure as an exception.
                Marshal.ThrowExceptionForHR(result);
            }

            //  Get the dekstop PDIL.
            var desktopPIDL = IntPtr.Zero;
            result = Shell32.SHGetSpecialFolderLocation(IntPtr.Zero, CSIDL.CSIDL_DESKTOP, ref desktopPIDL);
            result = Shell32.SHGetFolderLocation(IntPtr.Zero, CSIDL.CSIDL_DESKTOP, IntPtr.Zero, 0, out desktopPIDL);

            //  Validate the result.
            if (result != 0)
            {
                //  Throw the failure as an exception.
                Marshal.ThrowExceptionForHR(result);
            }

            //  Get the file info.
            var fileInfo = new SHFILEINFO();
            Shell32.SHGetFileInfo(desktopPIDL, 0, out fileInfo, (uint)Marshal.SizeOf(fileInfo),
                SHGFI.SHGFI_DISPLAYNAME | SHGFI.SHGFI_PIDL | SHGFI.SHGFI_SMALLICON | SHGFI.SHGFI_SYSICONINDEX);

            //  Return the Shell Folder.
            return new ShellItem
            {
                DisplayName = fileInfo.szDisplayName,
                IconIndex = fileInfo.iIcon,
                HasSubFolders = true,
                IsFolder = true,
                ShellFolderInterface = desktopShellFolderInterface,
                PIDL = desktopPIDL,
                RelativePIDL = desktopPIDL
            };
        }

        /// <summary>
        /// Initialises the ShellItem, from its PIDL and parent.
        /// </summary>
        /// <param name="pidl">The pidl.</param>
        /// <param name="parentFolder">The parent folder.</param>
        private void Initialise(IntPtr pidl, ShellItem parentFolder)
        {
            //  Set the parent item and relative pidl.
            ParentItem = parentFolder;
            RelativePIDL = pidl;

            //  Create the fully qualified PIDL.
            PIDL = Shell32.ILCombine(parentFolder.PIDL, pidl);

            //  Use the desktop folder to get attributes.
            var flags = SFGAO.SFGAO_FOLDER | SFGAO.SFGAO_HASSUBFOLDER | SFGAO.SFGAO_BROWSABLE | SFGAO.SFGAO_FILESYSTEM;
            //todo was this parentFolder.ShellFolderInterface.GetAttributesOf(1, ref pidl, ref flags);

            var apidl = Marshal.AllocCoTaskMem(IntPtr.Size*1);
            Marshal.Copy(new IntPtr[] {pidl}, 0, apidl, 1);

            parentFolder.ShellFolderInterface.GetAttributesOf(1, apidl, ref flags);
           
            IsFolder = (flags & SFGAO.SFGAO_FOLDER) != 0;
            HasSubFolders = (flags & SFGAO.SFGAO_HASSUBFOLDER) != 0;

            //  Get the file info.
            var fileInfo = new SHFILEINFO();
            Shell32.SHGetFileInfo(PIDL, 0, out fileInfo, (uint)Marshal.SizeOf(fileInfo),
                SHGFI.SHGFI_SMALLICON | SHGFI.SHGFI_SYSICONINDEX | SHGFI.SHGFI_PIDL | SHGFI.SHGFI_DISPLAYNAME | SHGFI.SHGFI_ATTRIBUTES);

            //  Set extended attributes.
            DisplayName = fileInfo.szDisplayName;
            Attributes = (SFGAO)fileInfo.dwAttributes;
            TypeName = fileInfo.szTypeName;
            IconIndex = fileInfo.iIcon;

            //  Are we a folder?
            if (IsFolder)
            {
                //  Bind the shell folder interface.
                IShellFolder shellFolderInterface;
                IntPtr ppv = IntPtr.Zero;
                var result = parentFolder.ShellFolderInterface.BindToObject(pidl, IntPtr.Zero, ref Shell32.IID_IShellFolder,
                    out ppv);//out shellFolderInterface);
                shellFolderInterface = ((IShellFolder) Marshal.GetObjectForIUnknown(ppv));
                ShellFolderInterface = shellFolderInterface;

                //  Validate the result.
                if (result != 0)
                {
                    //  Throw the failure as an exception.
                    Marshal.ThrowExceptionForHR((int)result);
                }
            }
        }

        /// <summary>
        /// Gets the system path for this shell item.
        /// </summary>
        /// <returns>A path string.</returns>
        private string CreatePath()
        {
            var stringBuilder = new StringBuilder(256);
            Shell32.SHGetPathFromIDList(PIDL, stringBuilder);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <param name="childTypes">The child types.</param>
        /// <returns>
        /// The children.
        /// </returns>
        public IEnumerable<ShellItem> GetChildren(ChildTypes childTypes)
        {
            //  We'll return a list of children.
            var children = new List<ShellItem>();

            //  Create the enum flags from the childtypes.
            SHCONTF enumFlags = 0;
            if (childTypes.HasFlag(ChildTypes.Folders))
                enumFlags |= SHCONTF.SHCONTF_FOLDERS;
            if (childTypes.HasFlag(ChildTypes.Files))
                enumFlags |= SHCONTF.SHCONTF_NONFOLDERS;
            if (childTypes.HasFlag(ChildTypes.Hidden))
                enumFlags |= SHCONTF.SHCONTF_INCLUDEHIDDEN;

            try
            {
                //  Create an enumerator for the children.
                IEnumIDList pEnum;
                var result = ShellFolderInterface.EnumObjects(IntPtr.Zero, enumFlags, out pEnum);

                //  Validate the result.
                if (result != 0)
                {
                    //  Throw the failure as an exception.
                    Marshal.ThrowExceptionForHR((int)result);
                }

                // TODO: This logic should go in the pidl manager.

                //  Enumerate the children, ten at a time.
                const int batchSize = 10;
                var pidlArray = Marshal.AllocCoTaskMem(IntPtr.Size * 10);
                uint itemsFetched;
                result = WinError.S_OK;
                do
                {
                    result = pEnum.Next(batchSize, pidlArray, out itemsFetched);

                    //  Get each pidl.
                    var pidls = new IntPtr[itemsFetched];
                    Marshal.Copy(pidlArray, pidls, 0, (int) itemsFetched);
                    foreach (var childPidl in pidls)
                    {
                        //  Create a new shell folder.
                        var childShellFolder = new ShellItem();

                        //  Initialize it.
                        try
                        {
                            childShellFolder.Initialise(childPidl, this);
                        }
                        catch (Exception exception)
                        {
                            throw new InvalidOperationException("Failed to initialise child.", exception);
                        }

                        //  Add the child.
                        children.Add(childShellFolder);

                        //  Free the PIDL, reset the result.
                        Marshal.FreeCoTaskMem(childPidl);
                    }
                } while (result == WinError.S_OK);
            
                Marshal.FreeCoTaskMem(pidlArray);

                //  Release the enumerator.
                if(Marshal.IsComObject(pEnum))
                    Marshal.ReleaseComObject(pEnum);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Failed to enumerate children.", exception);
            }

            //  Sort the children.
            var sortedChildren = children.Where(c => c.IsFolder).ToList();
            sortedChildren.AddRange(children.Where(c => !c.IsFolder));

            //  Return the children.
            return sortedChildren;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //  Release the shell folder interface.
            if (ShellFolderInterface != null)
                Marshal.ReleaseComObject(ShellFolderInterface);

            //  Free the PIDL.
            if (PIDL != IntPtr.Zero)
                Marshal.FreeCoTaskMem(PIDL);

            //  Suppress finalization.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.IsNullOrEmpty(DisplayName) ? base.ToString() : DisplayName;
        }

        /// <summary>
        /// The lazy desktop shell folder.
        /// </summary>
        private static readonly Lazy<ShellItem> desktopShellFolder;

        /// <summary>
        /// The lazy path.
        /// </summary>
        private readonly Lazy<string> path;

        /// <summary>
        /// The overlay icon.
        /// </summary>
        private readonly Lazy<Icon> overlayIcon;

        /// <summary>
        /// Gets the parent item.
        /// </summary>
        public ShellItem ParentItem { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is folder.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is folder; otherwise, <c>false</c>.
        /// </value>
        public bool IsFolder { get; private set; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        /// <value>
        /// The name of the type.
        /// </value>
        public string TypeName { get; private set; }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        public SFGAO Attributes { get; private set; }

        /// <summary>
        /// Gets the index of the icon.
        /// </summary>
        /// <value>
        /// The index of the icon.
        /// </value>
        public int IconIndex { get; private set; }

        /// <summary>
        /// Gets the ShellFolder of the Desktop.
        /// </summary>
        public static ShellItem DesktopShellFolder { get { return desktopShellFolder.Value; } }

        /// <summary>
        /// Gets the shell folder interface.
        /// </summary>
        public IShellFolder ShellFolderInterface { get; private set; }

        /// <summary>
        /// Gets the Full PIDL.
        /// </summary>
        public IntPtr PIDL { get; private set; }

        /// <summary>
        /// Gets the relative PIDL.
        /// </summary>
        public IntPtr RelativePIDL { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has children.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has children; otherwise, <c>false</c>.
        /// </value>
        public bool HasSubFolders { get; private set; }

        /// <summary>
        /// Gets the path.
        /// </summary>
        public string Path { get { return path.Value; } }

        /// <summary>
        /// Gets the overlay icon.
        /// </summary>
        /// <value>
        /// The overlay icon.
        /// </value>
        public Icon OverlayIcon { get { return overlayIcon.Value; } }
    }
    /// <summary>
    /// The Child Type flags.
    /// </summary>
    [Flags]
    public enum ChildTypes
    {
        Folders = 1,
        Files = 2,
        Hidden = 4
    }
}
