using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FileDialogs.Design;
using ComTypes = System.Runtime.InteropServices.ComTypes;

namespace FileDialogs
{
    /// <summary>
    /// Specifies the folder view type.
    /// </summary>
    public enum FileDialogViewMode : uint
    {
        /// <summary>
        /// The view should display medium-size icons.
        /// </summary>
        Icon = NativeMethods.FOLDERVIEWMODE.FVM_ICON,
        /// <summary>
        /// The view should display small icons.
        /// </summary>
        SmallIcon = NativeMethods.FOLDERVIEWMODE.FVM_SMALLICON,
        /// <summary>
        /// Object names are displayed in a list view.
        /// </summary>
        List = NativeMethods.FOLDERVIEWMODE.FVM_LIST,
        /// <summary>
        /// Object names and other selected information, such as the size or date last updated, are shown.
        /// </summary>
        Details = NativeMethods.FOLDERVIEWMODE.FVM_DETAILS,
        /// <summary>
        /// The view should display thumbnail icons.
        /// </summary>
        Thumbnail = NativeMethods.FOLDERVIEWMODE.FVM_THUMBNAIL,
        /// <summary>
        /// The view should display large icons.
        /// </summary>
        Tile = NativeMethods.FOLDERVIEWMODE.FVM_TILE,
    }

    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [DesignTimeVisible(true)]
    [ToolboxItem(true)]
    [Designer("System.ComponentModel.Design.ComponentDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public abstract partial class FileDialog : Form, NativeMethods.IShellBrowser, NativeMethods.ICommDlgBrowser,
                                               NativeMethods.IServiceProvider
    {
        #region Constants & Member Fields

        private string m_type;

        private const int MaxBackItems = 9;
        private readonly List<IntPtr> m_backItemsOverflow = new List<IntPtr>();
        private bool m_suspendAddBackItem;

        internal NativeMethods.IShellView m_shellView;
        private bool m_shellViewHasFocus;
        private IntPtr m_hWndListView;
        internal NativeMethods.IShellFolder m_desktopFolder;
        internal NativeMethods.IShellFolder m_currentFolder;

        protected IntPtr m_pidlAbsCurrent;
        protected IntPtr[] m_selectedPidls;

        private IntPtr m_desktopPidl;
        private IntPtr m_myCompPidl;
        private IntPtr m_myDocsPidl;

        private string m_desktopPath;
        private string m_myDocsPath;

        private NewFolderDialog m_newFolderDialog;

        private string[] m_fileNames;
        private string m_filter;
        private int m_filterIndex = 1;
        protected string m_currentFilePattern = string.Empty;
        private string m_initialDir;
        //private bool m_fileMustExist;
        //private bool m_pathMustExist = true;
        private bool m_useCreateNewFolderDialog = true;
        protected bool m_excludeFiles;

        private FileDialogViewMode m_viewMode = FileDialogViewMode.List;
        private bool m_restoreLastViewMode = true;

        protected bool m_ignoreFileNameChange;
        protected bool m_ignoreFileTypeChange;

        private StringCollection m_options;

        private const int LineLimit = 15;
        private readonly FileDialogPlacesCollection m_places;

        internal NativeMethods.FOLDERFLAGS m_flags = (NativeMethods.FOLDERFLAGS.FWF_SHOWSELALWAYS |
                                                      NativeMethods.FOLDERFLAGS.FWF_SINGLESEL |
                                                      NativeMethods.FOLDERFLAGS.FWF_NOWEBVIEW);

        private static readonly Padding FileViewPadding = new Padding(96, 28, 5, 53);

        #endregion

        #region Construction

        internal FileDialog(string type)
        {
            m_type = type;
            m_places = new FileDialogPlacesCollection();

            InitializeComponent();

            toolStrip.Renderer = new ToolStripFileDialogRenderer();
            placesBar.Renderer = new PlacesBarRenderer();

            // Associate each menu item with it's corresponding FOLDERVIEWMODE.
            thumbnailsToolStripMenuItem.Tag = (uint)NativeMethods.FOLDERVIEWMODE.FVM_THUMBNAIL;
            sideBySideToolStripMenuItem.Tag = (uint)NativeMethods.FOLDERVIEWMODE.FVM_TILE;
            iconsToolStripMenuItem.Tag = (uint)NativeMethods.FOLDERVIEWMODE.FVM_ICON;
            listToolStripMenuItem.Tag = (uint)NativeMethods.FOLDERVIEWMODE.FVM_LIST;
            detailsToolStripMenuItem.Tag = (uint)NativeMethods.FOLDERVIEWMODE.FVM_DETAILS;

            Init();
        }

        #endregion

        #region Overriden Methods

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            AdjustSystemMenu();
            InitPlacesBar();

            if (!string.IsNullOrEmpty(m_initialDir))
            {
                IntPtr pidl;
                m_desktopFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, m_initialDir, IntPtr.Zero, out pidl, 0);
                lookInComboBox.CurrentItem = new LookInComboBoxItem(GetDisplayName(m_desktopFolder, pidl), pidl, 0);
                ((NativeMethods.IShellBrowser)this).BrowseObject(pidl, NativeMethods.SBSP_ABSOLUTE);
            }
            else
            {
                // If no initial directory is set browse to the desktop
                lookInComboBox.CurrentItem =
                    new LookInComboBoxItem(GetDisplayName(m_desktopFolder, m_desktopPidl), m_desktopPidl, 0);
                ((NativeMethods.IShellBrowser)this).BrowseObject(m_desktopPidl, NativeMethods.SBSP_ABSOLUTE);
            }

            fileNameComboBox.SelectAll();

            // Reset the focus to the initial control, fileNameComboBox.Focus() doesn't work
            NativeMethods.User32.SetFocus(fileNameComboBox.Handle);

            // Add options to the OK button
            if (m_options == null || m_options.Count == 0)
                okButton.ShowSplit = false;
            else
            {
                okButton.ShowSplit = true;

                if (okButton.ContextMenuStrip != null)
                    okButton.ContextMenuStrip.Items.Clear();
                else
                {
                    okButton.ContextMenuStrip = new ContextMenuStrip();
                    okButton.ContextMenuStrip.Renderer = new ToolStripFileDialogRenderer();
                }

                foreach (string option in m_options)
                {
                    okButton.ContextMenuStrip.Items.Add(option);
                }
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            m_pidlAbsCurrent = IntPtr.Zero;
            m_selectedPidls = null;

            // Release the IShellView
            if (m_shellView != null)
            {
                if (m_restoreLastViewMode)
                {
                    // Saves the view mode if the RestoreLastViewMode property is set to true
                    NativeMethods.FOLDERSETTINGS fs = new NativeMethods.FOLDERSETTINGS();
                    m_shellView.GetCurrentInfo(ref fs);
                    m_viewMode = (FileDialogViewMode)fs.ViewMode;
                }

                m_shellView.UIActivate((uint)NativeMethods.SVUIA_STATUS.SVUIA_DEACTIVATE);
                m_shellView.DestroyViewWindow();
                Marshal.ReleaseComObject(m_shellView);
                m_shellView = null;
            }

            m_backItemsOverflow.Clear();
            backToolStripSplitButton.DropDownItems.Clear();
            backToolStripSplitButton.Enabled = false;

            base.OnHandleDestroyed(e);
        }

        private void AdjustSystemMenu()
        {
            // Remove the restore, maximize and minimize commands from the system menu
            IntPtr hMenu = NativeMethods.User32.GetSystemMenu(new HandleRef(this, Handle), false);

            NativeMethods.User32.RemoveMenu(new HandleRef(this, hMenu), NativeMethods.SC_RESTORE,
                                            NativeMethods.MF_BYCOMMAND);
            NativeMethods.User32.RemoveMenu(new HandleRef(this, hMenu), NativeMethods.SC_MAXIMIZE,
                                            NativeMethods.MF_BYCOMMAND);
            NativeMethods.User32.RemoveMenu(new HandleRef(this, hMenu), NativeMethods.SC_MINIMIZE,
                                            NativeMethods.MF_BYCOMMAND);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Resize the list view accordingly
            int x = FileViewPadding.Left;
            int y = FileViewPadding.Top;
            int width = ClientSize.Width - FileViewPadding.Horizontal;
            int height = ClientSize.Height - FileViewPadding.Vertical;
            NativeMethods.User32.SetWindowPos(new HandleRef(null, m_hWndListView), NativeMethods.NullHandleRef, x, y,
                                              width, height, NativeMethods.SWP_DRAWFRAME);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Becaues it's not possible to set the tab order of the IShellView,
            // this must be handled manually.
            if (keyData == Keys.Tab)
            {
                if (placesBar.Focused)
                {
                    m_shellView.UIActivate((uint)NativeMethods.SVUIA_STATUS.SVUIA_ACTIVATE_FOCUS);
                    return true;
                }
                if (m_shellViewHasFocus)
                {
                    m_shellView.UIActivate((uint)NativeMethods.SVUIA_STATUS.SVUIA_ACTIVATE_NOFOCUS);
                    fileNameComboBox.Focus();
                    return true;
                }
            }

            switch (keyData)
            {
                case Keys.Alt | Keys.D1:
                    if (backToolStripSplitButton.Enabled)
                    {
                        backToolStripSplitButton.PerformButtonClick();
                        return true;
                    }
                    break;

                case Keys.Alt | Keys.D2:
                    if (upOneLevelToolStripButton.Enabled)
                    {
                        upOneLevelToolStripButton.PerformClick();
                        return true;
                    }
                    break;

                case Keys.Alt | Keys.D3:
                    if (searchTheWebToolStripButton.Enabled)
                    {
                        searchTheWebToolStripButton.PerformClick();
                        return true;
                    }
                    break;

                case Keys.Alt | Keys.D4:
                    if (deleteToolStripButton.Enabled)
                    {
                        deleteToolStripButton.PerformClick();
                        return true;
                    }
                    break;

                case Keys.Alt | Keys.D5:
                    if (createNewFolderToolStripButton.Enabled)
                    {
                        createNewFolderToolStripButton.PerformClick();
                        return true;
                    }
                    break;

                case Keys.Alt | Keys.D6:
                    viewsToolStripSplitButton.PerformButtonClick();
                    return true;

                case Keys.Alt | Keys.D7:
                    toolsToolStripDropDownButton.ShowDropDown();
                    return true;
            }

            if (m_shellView != null)
            {
                if (!m_shellViewHasFocus)
                {
                    StringBuilder className = new StringBuilder(256);
                    NativeMethods.User32.GetClassName(msg.HWnd, className, 256);

                    // An item is being renamed
                    if (className.ToString() == "Edit" && msg.HWnd != GetComboBoxHWndEdit(fileNameComboBox))
                    {
                        if (keyData == Keys.Enter ||
                            keyData == Keys.Escape ||
                            (keyData & Keys.Left) == Keys.Left ||
                            (keyData & Keys.Up) == Keys.Up ||
                            (keyData & Keys.Right) == Keys.Right ||
                            (keyData & Keys.Down) == Keys.Down)
                        {

                            NativeMethods.User32.SendMessage(msg.HWnd, msg.Msg, msg.WParam, msg.LParam);
                            return true;
                        }
                        
                        return false;
                    }
                }
                else
                {
                    switch (keyData)
                    {
                        // The IShellView loses focus when one of the arrow keys are pressed,
                        // this occurs because the IShellView.TranslateAccelerator method doesn't return S_OK.
                        case Keys.Left:
                        case Keys.Up:
                        case Keys.Right:
                        case Keys.Down:
                            m_shellView.TranslateAccelerator(ref msg);
                            return true;

                        // The IShellView both opens the Properties window and OnDefaultCommand is called
                        case Keys.Alt | Keys.Enter:
                            m_shellView.TranslateAccelerator(ref msg);
                            return true;

                        case Keys.Enter:
                            break;

                        case Keys.Alt | Keys.Left: // Shortcut for Go Back
                            if (backToolStripSplitButton.Enabled)
                            {
                                backToolStripSplitButton.PerformButtonClick();
                                return true;
                            }
                            break;

                        case Keys.Back: // Shortcut for Up On Level
                            if (upOneLevelToolStripButton.Enabled)
                            {
                                upOneLevelToolStripButton.PerformClick();
                                return true;
                            }
                            break;

                        default:
                            // Let the IShellView translate all the other accelerator key strokes 
                            if (m_shellView.TranslateAccelerator(ref msg) == NativeMethods.S_OK)
                                return true;
                            break;
                    }
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override bool ProcessMnemonic(char charCode)
        {
            // The mnemonics of the labels is being processed without the ALT key.
            if ((ModifierKeys & Keys.Alt) == 0)
                return false;

            return base.ProcessMnemonic(charCode);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                // The IShellBrowser interface is to send the undocumented WM_GETISHELLBROWSER message to the dialog's parent,
                // but I have never recieved this messege.
                case NativeMethods.WM_GETISHELLBROWSER:
                    m.Result = Marshal.GetIUnknownForObject(this);
                    break;

                // Maximize the window when the title bar is double-clicked
                case NativeMethods.WM_NCLBUTTONDBLCLK:
                    WindowState = (WindowState == FormWindowState.Normal)
                                      ? FormWindowState.Maximized
                                      : FormWindowState.Normal;
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion

        #region FileDialog Methods

        internal bool GetFlag(NativeMethods.FOLDERFLAGS flag)
        {
            return ((m_flags & flag) != 0);
        }

        internal void SetFlag(NativeMethods.FOLDERFLAGS flag, bool value)
        {
            if (value)
                m_flags |= flag;
            else
                m_flags &= ~flag;
        }

        internal DialogResult MessageBoxWithFocusRestore(string message, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            DialogResult dr;
            IntPtr hwnd = NativeMethods.User32.GetFocus();
            try
            {
                dr = MessageBox.Show(null, message, Text, buttons, icon, MessageBoxDefaultButton.Button1, 0);
            }
            finally
            {
                NativeMethods.User32.SetFocus(hwnd);
            }
            return dr;
        }

        internal static bool FileExists(string fileName)
        {
            try
            {
                return File.Exists(fileName);
            }
            catch (PathTooLongException)
            {
                return false;
            }
        }

        internal virtual bool PromptInvalidPath(string path)
        {
            string text =
                string.Format(
                    @"The file name, location, or format '{0}' is not valid. Type the file name and location in the correct format, such as c:\location\file name.",
                    path);
            MessageBox.Show(this, text, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

            return false;
        }

        internal virtual bool PromptFileOverwrite(string fileName)
        {
            return true;
        }

        internal virtual bool PromptFileCreate()
        {
            return true;
        }

        internal virtual string AppendExtension(string fileName)
        {
            return fileName;
        }

        protected virtual void RunDialog()
        {
            fileTypeComboBox.Items.Clear();

            if (m_filter != null)
            {
                // Add the filter to the fileTypeComboBox
                string[] filterParts = m_filter.Split(new char[] {'|'});
                for (int i = 1; i < filterParts.Length; i += 2)
                {
                    fileTypeComboBox.Items.Add(new FileType(filterParts[i - 1], filterParts[i].Split(';')));
                }

                fileTypeComboBox.SelectedIndex = m_filterIndex - 1;
            }

#if REGISTRY_SUPPORT

            RegistryKey regKey = Registry.CurrentUser.CreateSubKey(@"Software\[ProductName]\Open Find\[ProductName]\Settings\" + m_type);
            if (regKey.GetValue("PositionInfo-Monitor1") != null)
            {
                byte[] bytes = (byte[])regKey.GetValue("PositionInfo-Monitor1");
                int x = BitConverter.ToInt32(bytes, 0);
                int y = BitConverter.ToInt32(bytes, 4);
                int width = BitConverter.ToInt32(bytes, 8);
                int height = BitConverter.ToInt32(bytes, 12);
            }

            regKey.Close();

            regKey = Registry.CurrentUser.CreateSubKey(@"Software\[ProductName]\Open Find\[ProductName]\Settings\" + m_type + @"\File Name MRU");
            if (regKey.GetValue("Value") != null)
            {
                string[] fileNameMRU = (string[])regKey.GetValue("Value");
                fileNameComboBox.Items.AddRange(fileNameMRU);
            }

            regKey.Close();

#endif

            // Reset the focus to the fileNameComboBox
            fileNameComboBox.Focus();
        }

        public new DialogResult ShowDialog()
        {
            RunDialog();
            return base.ShowDialog();
        }

        public new DialogResult ShowDialog(IWin32Window owner)
        {
            RunDialog();
            return base.ShowDialog(owner);
        }

        #endregion

        #region Places Bar Methods

        internal static string InsertLineBreaks(string text, out bool lineBreakInserted)
        {
            if (string.IsNullOrEmpty(text))
            {
                lineBreakInserted = false;
                return string.Empty;
            }

            string[] splitText = text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            text = "";

            int charactersOnCurrentLine = 0;
            lineBreakInserted = false;
            for (int wordIndex = 0; wordIndex < splitText.Length; wordIndex++)
            {
                string word = splitText[wordIndex];
                if (word.Length + charactersOnCurrentLine + 1 <= LineLimit)
                {
                    if (charactersOnCurrentLine != 0) text += " ";
                    text += word;
                    charactersOnCurrentLine += word.Length + 1;
                }
                else
                {
                    text += "\r\n" + word;
                    charactersOnCurrentLine = word.Length;
                    lineBreakInserted = true;
                }
            }
            return text;
        }

        private void AddItem(FileDialogPlaceBase place)
        {
            bool multipleLines;
            ToolStripButton placeButton =
                new ToolStripButton(InsertLineBreaks(place.Text, out multipleLines),
                                    ShellImageList.GetImage(place.PIDL));
            placeButton.Tag = place.PIDL;
            placeButton.Margin = new Padding(1, 0, 0, 0);
            placeButton.Padding = new Padding(0, multipleLines ? 3 : 8, 0, multipleLines ? 0 : 8);
            placeButton.ImageAlign = ContentAlignment.BottomCenter;
            placeButton.TextImageRelation = TextImageRelation.ImageAboveText;

            placesBar.Items.Add(placeButton);
        }

        private static NativeMethods.SHGFAO GetAttributesOf(IntPtr abspidl)
        {
            NativeMethods.SHFILEINFO sfi = new NativeMethods.SHFILEINFO();
            NativeMethods.Shell32.SHGetFileInfo(abspidl, NativeMethods.FILE_ATTRIBUTE_NORMAL, ref sfi,
                                                NativeMethods.cbFileInfo,
                                                NativeMethods.SHGFI_PIDL | NativeMethods.SHGFI_ATTRIBUTES);

            return (NativeMethods.SHGFAO)sfi.dwAttributes;
        }

        internal static string GetDisplayName(NativeMethods.IShellFolder shellFolder, IntPtr pidl,
                                              NativeMethods.SHGNO flags)
        {
            string text = string.Empty;

            IntPtr strr = Marshal.AllocCoTaskMem(NativeMethods.MAX_PATH * 2 + 4);
            Marshal.WriteInt32(strr, 0, 0);
            StringBuilder buf = new StringBuilder(NativeMethods.MAX_PATH);

            if (shellFolder.GetDisplayNameOf(
                    pidl,
                    flags,
                    strr) == NativeMethods.S_OK)
            {
                NativeMethods.Shlwapi.StrRetToBuf(strr, pidl, buf, NativeMethods.MAX_PATH);
                text = buf.ToString();
            }

            Marshal.FreeCoTaskMem(strr);
            return text;
        }

        internal string GetDisplayName(NativeMethods.IShellFolder shellFolder, IntPtr pidl)
        {
            string text = string.Empty;

            IntPtr strr = Marshal.AllocCoTaskMem(NativeMethods.MAX_PATH * 2 + 4);
            Marshal.WriteInt32(strr, 0, 0);
            StringBuilder buf = new StringBuilder(NativeMethods.MAX_PATH);

            int level;
            bool isMyCompChild = IsParentFolder(m_myCompPidl, pidl, out level);
            NativeMethods.SHGNO flags = NativeMethods.SHGNO.SHGDN_INFOLDER;

            if ((isMyCompChild && level >= 3) || (!isMyCompChild && level >= 2))
                flags |= NativeMethods.SHGNO.SHGDN_FORADDRESSBAR |
                         NativeMethods.SHGNO.SHGDN_FORPARSING;

            if (shellFolder.GetDisplayNameOf(
                    pidl,
                    flags,
                    strr) == NativeMethods.S_OK)
            {
                NativeMethods.Shlwapi.StrRetToBuf(strr, pidl, buf, NativeMethods.MAX_PATH);
                text = buf.ToString();

                if (text.Contains("\\"))
                {
                    string[] path = text.Split('\\');
                    text = path[path.Length - 1];
                }
            }

            Marshal.FreeCoTaskMem(strr);
            return text;
        }

        // Initialize the places bar
        private void InitPlacesBar()
        {
            if (Places.Count == 0)
            {
                Places.Add(SpecialFolder.Desktop);
                Places.Add(SpecialFolder.MyDocuments);
                Places.Add(SpecialFolder.MyComputer);
            }

            placesBar.Items.Clear();
            foreach (FileDialogPlaceBase place in Places)
            {
                AddItem(place);
            }
        }

        private void placesStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ((NativeMethods.IShellBrowser)this).BrowseObject((IntPtr)e.ClickedItem.Tag, NativeMethods.SBSP_ABSOLUTE);
        }

        #endregion

        #region Toolbar Methods

        // Adds an item to the Back menu
        private void AddBackItem(IntPtr selectedPidl)
        {
            if (selectedPidl == IntPtr.Zero)
                return;

            ToolStripMenuItem backItem = new ToolStripMenuItem(GetDisplayName(m_desktopFolder, selectedPidl));
            backItem.Name = backItem.Text;
            backItem.Tag = selectedPidl;
            backItem.ImageScaling = ToolStripItemImageScaling.None;

            // If the number of items exceeds the MaxBackItems
            // remove the last item from the menu and add it to the overflow list
            if (backToolStripSplitButton.DropDownItems.Count == MaxBackItems)
            {
                m_backItemsOverflow.Insert(0, (IntPtr)backToolStripSplitButton.DropDownItems[MaxBackItems - 1].Tag);
                backToolStripSplitButton.DropDownItems.RemoveAt(MaxBackItems - 1);
            }

            backToolStripSplitButton.DropDownItems.Insert(0, backItem);
            backToolStripSplitButton.ToolTipText = backItem.Text;
            backToolStripSplitButton.Enabled = true;
        }

        // Removes an item from the Back menu
        private void RemoveBackItem(ToolStripItem item)
        {
            int count = backToolStripSplitButton.DropDownItems.IndexOf(item) + 1;
            IntPtr selectedPidl = (IntPtr)item.Tag;

            // Removes the corresponding number of items from the menu
            for (int i = 0; i < count; i++)
            {
                backToolStripSplitButton.DropDownItems.RemoveAt(0);
            }

            backToolStripSplitButton.ToolTipText =
                GetDisplayName(m_desktopFolder, selectedPidl, NativeMethods.SHGNO.SHGDN_INFOLDER);

            // Add items from the overflow list to the menu if any exists
            if (m_backItemsOverflow.Count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    if (m_backItemsOverflow.Count == 0)
                        break;

                    IntPtr overflowPidl = m_backItemsOverflow[0];
                    m_backItemsOverflow.RemoveAt(0);

                    ToolStripMenuItem backItem =
                        new ToolStripMenuItem(
                            GetDisplayName(m_desktopFolder, overflowPidl, NativeMethods.SHGNO.SHGDN_INFOLDER));
                    backItem.Name = backItem.Text;
                    backItem.Tag = overflowPidl;
                    backItem.ImageScaling = ToolStripItemImageScaling.None;
                    backToolStripSplitButton.DropDownItems.Add(backItem);
                }
            }

            if (backToolStripSplitButton.DropDownItems.Count == 0)
                backToolStripSplitButton.Enabled = false;

            // Make sure the item isn't added again
            m_suspendAddBackItem = true;
            ((NativeMethods.IShellBrowser)this).BrowseObject(selectedPidl, NativeMethods.SBSP_ABSOLUTE);
        }

        private void backToolStripSplitButton_ButtonClick(object sender, EventArgs e)
        {
            // Browse back to the first item in the menu
            RemoveBackItem(backToolStripSplitButton.DropDownItems[0]);
        }

        private void backToolStripSplitButton_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Browse back to the clicked item in the menu
            RemoveBackItem(e.ClickedItem);
        }

        private void upOneLevelToolStripButton_Click(object sender, EventArgs e)
        {
            // Browse to the parent folder
            ((NativeMethods.IShellBrowser)this).BrowseObject(IntPtr.Zero, NativeMethods.SBSP_PARENT);
        }

        private void deleteToolStripButton_Click(object sender, EventArgs e)
        {
            if (m_selectedPidls != null && m_selectedPidls.Length > 0)
            {
                IntPtr iContextMenuPtr;
                m_shellView.GetItemObject((uint)NativeMethods.SVGIO.SVGIO_SELECTION, ref NativeMethods.IID_IContextMenu,
                                          out iContextMenuPtr);
                NativeMethods.IContextMenu iContextMenu = (NativeMethods.IContextMenu)
                                            Marshal.GetTypedObjectForIUnknown(iContextMenuPtr, typeof (NativeMethods.IContextMenu));

                NativeMethods.CMINVOKECOMMANDINFOEX cmi = new NativeMethods.CMINVOKECOMMANDINFOEX();
                cmi.cbSize = Marshal.SizeOf(typeof (NativeMethods.CMINVOKECOMMANDINFOEX));
                cmi.lpVerb = Marshal.StringToHGlobalAnsi("Delete");
                cmi.hwnd = Handle;

                // Invoke the delete command of the context menu
                iContextMenu.InvokeCommand(ref cmi);
            }
        }

        private void createNewFolderToolStripButton_Click(object sender, EventArgs e)
        {
            if (m_useCreateNewFolderDialog)
            {
                // A new folder is created through a dialog and then the folder is instantly browsed to
                string path = GetDisplayName(m_desktopFolder, m_pidlAbsCurrent, NativeMethods.SHGNO.SHGDN_FORPARSING);

                m_newFolderDialog = new NewFolderDialog(path);
                if (m_newFolderDialog.ShowDialog(this) == DialogResult.Cancel)
                {
                    m_newFolderDialog.FolderCreated = false;
                    m_newFolderDialog.FolderPath = string.Empty;
                }
            }
            else
            {
                // A new folder is created the traditional way
                IntPtr iContextMenuPtr;
                NativeMethods.IContextMenu iContextMenu;
                if (m_shellView.GetItemObject((uint)NativeMethods.SVGIO.SVGIO_BACKGROUND,
                                              ref NativeMethods.IID_IContextMenu,
                                              out iContextMenuPtr) == NativeMethods.S_OK)
                {
                    iContextMenu =
                        (NativeMethods.IContextMenu)
                        Marshal.GetTypedObjectForIUnknown(iContextMenuPtr, typeof (NativeMethods.IContextMenu));

                    IntPtr hMenu = NativeMethods.User32.CreatePopupMenu();
                    if (hMenu != IntPtr.Zero)
                    {
                        if (iContextMenu.QueryContextMenu(
                                hMenu, 0,
                                NativeMethods.MIN_SHELL_ID,
                                NativeMethods.MAX_SHELL_ID,
                                NativeMethods.CMF_NORMAL) > -1)
                        {
                            IntPtr iShellViewPtr = Marshal.GetIUnknownForObject(m_shellView);
                            IntPtr iFolderViewPtr;
                            Marshal.QueryInterface(iShellViewPtr, ref NativeMethods.IID_IFolderView, out iFolderViewPtr);
                            NativeMethods.IFolderView iFolderView =
                                (NativeMethods.IFolderView)Marshal.GetObjectForIUnknown(iFolderViewPtr);

                            int nCount;
                            iFolderView.ItemCount((int)NativeMethods.SVGIO.SVGIO_ALLVIEW, out nCount);
                            for (int i = 0; i < nCount; i++)
                            {
                                iFolderView.SelectItem(i, NativeMethods.SVSI_DESELECT);
                            }

                            NativeMethods.CMINVOKECOMMANDINFOEX cmi = new NativeMethods.CMINVOKECOMMANDINFOEX();
                            cmi.cbSize = Marshal.SizeOf(typeof (NativeMethods.CMINVOKECOMMANDINFOEX));
                            cmi.lpVerb = Marshal.StringToHGlobalAnsi("NewFolder");
                            cmi.lpVerbW = Marshal.StringToHGlobalUni("NewFolder");
                            cmi.nShow = NativeMethods.SW_SHOWNORMAL;
                            cmi.hwnd = Handle;
                            cmi.fMask = NativeMethods.CMIC_MASK_UNICODE | NativeMethods.CMIC_MASK_FLAG_NO_UI;

                            // Invoke the new folder command of the context menu
                            if (iContextMenu.InvokeCommand(ref cmi) == NativeMethods.S_OK)
                            {
                                iFolderView.ItemCount((int)NativeMethods.SVGIO.SVGIO_ALLVIEW, out nCount);
                                iFolderView.SelectItem(nCount - 1, NativeMethods.SVSI_EDIT);
                            }

                            Marshal.ReleaseComObject(iFolderView);
                            Marshal.Release(iFolderViewPtr);
                            Marshal.Release(iShellViewPtr);
                        }
                        NativeMethods.User32.DestroyMenu(hMenu);
                    }
                    Marshal.ReleaseComObject(iContextMenu);
                }
            }
        }

        // Is called from ICommDlgBrowser.IncludeObject
        private bool CreateNewFolder(string path, IntPtr pidl)
        {
            // Checks whether a new folder is created,
            // and if the specified path is equal to the folders path
            if (m_newFolderDialog != null &&
                m_newFolderDialog.FolderCreated &&
                path == m_newFolderDialog.FolderPath)
            {
                m_newFolderDialog.FolderCreated = false;
                m_newFolderDialog.FolderPath = string.Empty;

                // Browse to the newly created folder
                ((NativeMethods.IShellBrowser)this).BrowseObject(pidl, NativeMethods.SBSP_RELATIVE);
                return true;
            }
            return false;
        }

        private void viewsToolStripSplitButton_ButtonClick(object sender, EventArgs e)
        {
            NativeMethods.FOLDERSETTINGS fs = new NativeMethods.FOLDERSETTINGS();
            m_shellView.GetCurrentInfo(ref fs);

            foreach (ToolStripMenuItem item in viewsToolStripSplitButton.DropDownItems)
            {
                if (fs.ViewMode == (uint)item.Tag)
                {
                    // Set the view mode to the next menu item
                    int index = viewsToolStripSplitButton.DropDownItems.IndexOf(item) + 1;
                    if (index == viewsToolStripSplitButton.DropDownItems.Count)
                        index = 0;

                    SetCurrentViewMode((uint)viewsToolStripSplitButton.DropDownItems[index].Tag);
                    break;
                }
            }
        }

        private void viewsToolStripSplitButton_DropDownOpening(object sender, EventArgs e)
        {
            NativeMethods.FOLDERSETTINGS fs = new NativeMethods.FOLDERSETTINGS();
            m_shellView.GetCurrentInfo(ref fs);

            // Update the checked menu item to the current view mode
            foreach (ToolStripMenuItem item in viewsToolStripSplitButton.DropDownItems)
            {
                item.Checked = (fs.ViewMode == (uint)item.Tag);
            }
        }

        private void viewsToolStripSplitButton_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            NativeMethods.FOLDERSETTINGS fs = new NativeMethods.FOLDERSETTINGS();
            m_shellView.GetCurrentInfo(ref fs);

            // Each menu item is associated with it's corresponding view mode
            foreach (ToolStripMenuItem item in viewsToolStripSplitButton.DropDownItems)
            {
                if (fs.ViewMode == (uint)item.Tag)
                {
                    // Don't change view
                    if (item == e.ClickedItem)
                        return;
                    
                    item.Checked = false;
                }
                else if (item == e.ClickedItem)
                {
                    item.Checked = true;
                }
            }

            SetCurrentViewMode((uint)e.ClickedItem.Tag);
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Instead of using context menu, select the item in edit mode
            if (m_selectedPidls != null && m_selectedPidls.Length > 0)
                m_shellView.SelectItem(m_selectedPidls[0], NativeMethods.SVSI_EDIT);
        }

        private void propertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool childPidls = (m_selectedPidls != null && m_selectedPidls.Length > 0);

            IntPtr[] pidls;
            if (childPidls)
                pidls = m_selectedPidls;
            else
            {
                pidls = new IntPtr[1];
                pidls[0] = m_pidlAbsCurrent;
            }

            // If there are selected items or if the current folder is the desktop, invoke the command for the current folder
            NativeMethods.IShellFolder parent = (childPidls) ? m_currentFolder : m_desktopFolder;

            IntPtr iContextMenuPtr;
            NativeMethods.IContextMenu iContextMenu;

            if (GetIContextMenu(parent, pidls, out iContextMenuPtr, out iContextMenu))
            {
                try
                {
                    InvokeCommand(
                        iContextMenu,
                        "Properties",
                        GetDisplayName(
                            m_desktopFolder,
                            (childPidls) ? m_pidlAbsCurrent : GetParentPidl(m_pidlAbsCurrent),
                            NativeMethods.SHGNO.SHGDN_FORPARSING
                            )
                        );
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (iContextMenu != null)
                        Marshal.ReleaseComObject(iContextMenu);

                    if (iContextMenuPtr != IntPtr.Zero)
                        Marshal.Release(iContextMenuPtr);
                }
            }
        }

        private bool GetIContextMenu(NativeMethods.IShellFolder parent, IntPtr[] pidls, out IntPtr iContextMenuPtr,
                                     out NativeMethods.IContextMenu iContextMenu)
        {
            if (parent.GetUIObjectOf(
                    Handle,
                    (uint)pidls.Length,
                    pidls,
                    ref NativeMethods.IID_IContextMenu,
                    IntPtr.Zero,
                    out iContextMenuPtr) == NativeMethods.S_OK)
            {
                iContextMenu =
                    (NativeMethods.IContextMenu)Marshal.GetTypedObjectForIUnknown(
                                                    iContextMenuPtr, typeof (NativeMethods.IContextMenu));

                return true;
            }
            
            iContextMenuPtr = IntPtr.Zero;
            iContextMenu = null;

            return false;
        }

        private void InvokeCommand(NativeMethods.IContextMenu iContextMenu, string cmd, string parentDir)
        {
            NativeMethods.CMINVOKECOMMANDINFOEX invoke = new NativeMethods.CMINVOKECOMMANDINFOEX();
            invoke.hwnd = Handle;
            invoke.cbSize = Marshal.SizeOf(invoke);
            invoke.lpVerb = Marshal.StringToHGlobalAnsi(cmd);
            invoke.lpDirectory = Marshal.StringToHGlobalAnsi(parentDir);
            invoke.lpVerbW = Marshal.StringToHGlobalUni(cmd);
            invoke.lpDirectoryW = Marshal.StringToHGlobalUni(parentDir);
            invoke.fMask = NativeMethods.CMIC_MASK_UNICODE |
                           ((ModifierKeys & Keys.Control) != 0 ? NativeMethods.CMIC_MASK_CONTROL_DOWN : 0) |
                           ((ModifierKeys & Keys.Shift) != 0 ? NativeMethods.CMIC_MASK_SHIFT_DOWN : 0);
            invoke.nShow = NativeMethods.SW_SHOWNORMAL;

            iContextMenu.InvokeCommand(ref invoke);
        }

        private void connectNetworkDriveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Display the Connect Network Drive dialog
            NativeMethods.Mpr.WNetConnectionDialog(Handle, NativeMethods.RESOURCETYPE_DISK);
        }

        #endregion

        #region LookInComboBox Methods

        private void Init()
        {
            // My Computer
            NativeMethods.SHFILEINFO info = new NativeMethods.SHFILEINFO();
            NativeMethods.Shell32.SHGetSpecialFolderLocation(IntPtr.Zero, (int)SpecialFolder.MyComputer,
                                                             out m_myCompPidl);
            NativeMethods.Shell32.SHGetFileInfo(m_myCompPidl, 0, ref info, NativeMethods.cbFileInfo,
                                                NativeMethods.SHGFI_PIDL | NativeMethods.SHGFI_DISPLAYNAME);

            // Desktop
            NativeMethods.Shell32.SHGetSpecialFolderLocation(IntPtr.Zero, (int)SpecialFolder.Desktop, out m_desktopPidl);
            IntPtr desktopFolderPtr;
            NativeMethods.Shell32.SHGetDesktopFolder(out desktopFolderPtr);
            m_desktopFolder = (NativeMethods.IShellFolder)Marshal.GetObjectForIUnknown(desktopFolderPtr);
            m_desktopPath = GetDisplayName(m_desktopFolder, m_desktopPidl, NativeMethods.SHGNO.SHGDN_FORPARSING);
            m_currentFolder = m_desktopFolder;

            // My Documents
            m_desktopFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, NativeMethods.CLSID_MyDocuments, IntPtr.Zero,
                                             out m_myDocsPidl, 0);
            info = new NativeMethods.SHFILEINFO();
            NativeMethods.Shell32.SHGetFileInfo(m_myDocsPidl, 0, ref info, NativeMethods.cbFileInfo,
                                                NativeMethods.SHGFI_PIDL | NativeMethods.SHGFI_DISPLAYNAME);
            m_myDocsPath = GetDisplayName(m_desktopFolder, m_myDocsPidl, NativeMethods.SHGNO.SHGDN_FORPARSING);
        }

        // Populate the combo box
        private void EnumDesktop()
        {
            IntPtr[] path;
            int level = GetLevel(m_pidlAbsCurrent);
            bool isMyCompChild = IsParentFolder(m_myCompPidl, m_pidlAbsCurrent, out path);

            ArrayList pathList = new ArrayList();
            IntPtr current = m_pidlAbsCurrent;
            while (!NativeMethods.Shell32.ILIsEqual(current, m_desktopPidl))
            {
                pathList.Add(current);
                current = GetParentPidl(current);
            }

            pathList.Add(m_desktopPidl);
            pathList.Reverse();
            path = (IntPtr[])pathList.ToArray(typeof (IntPtr));

            NativeMethods.IEnumIDList folderEnum;

            NativeMethods.SHCONTF folderFlag =
                NativeMethods.SHCONTF.SHCONTF_FOLDERS |
                NativeMethods.SHCONTF.SHCONTF_INCLUDEHIDDEN;

            if (m_desktopFolder.EnumObjects(
                    IntPtr.Zero,
                    folderFlag,
                    out folderEnum) == NativeMethods.S_OK)
            {
                IntPtr pidlSubItem;
                int celtFetched;

                // Enumerate the desktop
                while (folderEnum.Next(1, out pidlSubItem, out celtFetched) == NativeMethods.S_OK && celtFetched == 1)
                {
                    IntPtr shellFolderPtr;
                    if (m_desktopFolder.BindToObject(
                            pidlSubItem,
                            IntPtr.Zero,
                            ref NativeMethods.IID_IShellFolder,
                            out shellFolderPtr) == NativeMethods.S_OK)
                    {
                        string text = GetDisplayName(m_desktopFolder, pidlSubItem, NativeMethods.SHGNO.SHGDN_INFOLDER);

                        IntPtr[] pidls = new IntPtr[] {pidlSubItem};
                        NativeMethods.SHGFAO attribs = NativeMethods.SHGFAO.SFGAO_FILESYSANCESTOR;
                        m_desktopFolder.GetAttributesOf(1, pidls, ref attribs);

                        if ((attribs & NativeMethods.SHGFAO.SFGAO_FILESYSANCESTOR) != 0)
                            lookInComboBox.Items.Add(new LookInComboBoxItem(text, pidlSubItem, 1));

                        // Check if the item is My Computer
                        if (NativeMethods.Shell32.ILIsEqual(pidlSubItem, m_myCompPidl))
                            EnumMyComputer();

                        if (path != null && !isMyCompChild && level >= 2 &&
                            text.Equals(GetDisplayName(m_desktopFolder, path[1], NativeMethods.SHGNO.SHGDN_INFOLDER)))
                        {
                            for (int i = 2; i < path.Length; i++)
                            {
                                string childText = GetDisplayName(m_desktopFolder, path[i]);
                                lookInComboBox.Items.Add(new LookInComboBoxItem(childText, path[i], i));
                            }
                        }
                    }
                }
            }
        }

        private void EnumMyComputer()
        {
            IntPtr[] path;
            int level = GetLevel(m_pidlAbsCurrent);
            bool isMyCompChild = IsParentFolder(m_myCompPidl, m_pidlAbsCurrent, out path);

            IntPtr myComputerPtr;
            if (m_desktopFolder.BindToObject(
                    m_myCompPidl,
                    IntPtr.Zero,
                    ref NativeMethods.IID_IShellFolder,
                    out myComputerPtr) == NativeMethods.S_OK)
            {
                NativeMethods.IShellFolder myComputer =
                    (NativeMethods.IShellFolder)Marshal.GetObjectForIUnknown(myComputerPtr);

                NativeMethods.IEnumIDList folderEnum;

                NativeMethods.SHCONTF folderFlag =
                    NativeMethods.SHCONTF.SHCONTF_FOLDERS |
                    NativeMethods.SHCONTF.SHCONTF_INCLUDEHIDDEN;

                if (myComputer.EnumObjects(
                        IntPtr.Zero,
                        folderFlag,
                        out folderEnum) == NativeMethods.S_OK)
                {
                    // Enumerate My Computer
                    IntPtr pidlSubItem;
                    int celtFetched;
                    while (folderEnum.Next(1, out pidlSubItem, out celtFetched) == NativeMethods.S_OK &&
                           celtFetched == 1)
                    {
                        IntPtr abspidl = NativeMethods.Shell32.ILCombine(m_myCompPidl, pidlSubItem);

                        string text = GetDisplayName(m_desktopFolder, abspidl, NativeMethods.SHGNO.SHGDN_INFOLDER);

                        IntPtr[] pidls = new IntPtr[] {pidlSubItem};
                        NativeMethods.SHGFAO attribs = NativeMethods.SHGFAO.SFGAO_FILESYSANCESTOR;
                        myComputer.GetAttributesOf(1, pidls, ref attribs);

                        if ((attribs & NativeMethods.SHGFAO.SFGAO_FILESYSANCESTOR) != 0)
                            lookInComboBox.Items.Add(new LookInComboBoxItem(text, abspidl, 2));

                        if (path != null && isMyCompChild && level >= 3 &&
                            text.Equals(GetDisplayName(m_desktopFolder, path[2], NativeMethods.SHGNO.SHGDN_INFOLDER)))
                        {
                            for (int i = 3; i < path.Length; i++)
                            {
                                string childText = GetDisplayName(m_desktopFolder, path[i]);
                                lookInComboBox.Items.Add(new LookInComboBoxItem(childText, path[i], i));
                            }
                        }
                    }
                }
            }
        }

        // Gets the number of levels away from the desktop.
        private int GetLevel(IntPtr pidl)
        {
            int level = 0;
            IntPtr current = pidl;
            while (!NativeMethods.Shell32.ILIsEqual(current, m_desktopPidl))
            {
                level++;
                current = GetParentPidl(current);
            }

            return level;
        }

        // Checks if a pidl is parent to another pidl and returns the level
        private bool IsParentFolder(IntPtr parent, IntPtr child, out int level)
        {
            level = 0;

            if (parent != IntPtr.Zero && child != IntPtr.Zero)
            {
                bool found = false;
                IntPtr current = child;
                while (!NativeMethods.Shell32.ILIsEqual(current, m_desktopPidl))
                {
                    if (NativeMethods.Shell32.ILIsEqual(current, parent))
                        found = true;

                    level++;
                    current = GetParentPidl(current);
                }

                return found;
            }

            return false;
        }

        // Checks if a pidl is parent to another pidl and returns path of pidls
        private bool IsParentFolder(IntPtr parent, IntPtr child, out IntPtr[] path)
        {
            if (parent != IntPtr.Zero && child != IntPtr.Zero)
            {
                ArrayList pathList = new ArrayList();

                IntPtr current = child;
                while (!NativeMethods.Shell32.ILIsEqual(current, m_desktopPidl))
                {
                    pathList.Add(current);
                    if (NativeMethods.Shell32.ILIsEqual(current, parent))
                    {
                        pathList.Add(parent);
                        pathList.Reverse();
                        path = (IntPtr[])pathList.ToArray(typeof (IntPtr));
                        return true;
                    }

                    current = GetParentPidl(current);
                }
            }

            path = null;
            return false;
        }

        private void lookInComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (lookInComboBox.SelectedIndex >= 0)
            {
                IntPtr pidl = ((LookInComboBoxItem)lookInComboBox.Items[lookInComboBox.SelectedIndex]).PIDL;
                ((NativeMethods.IShellBrowser)this).BrowseObject(pidl, NativeMethods.SBSP_ABSOLUTE);
            }
        }

        private void lookInComboBox_DropDown(object sender, EventArgs e)
        {
            if (lookInComboBox.Items.Count > 0)
                lookInComboBox.Items.Clear();

            // The combo box is populated every time it is dropped down, so that every new folder is added
            // TODO: Find a better solution where the combo box isn't populated every time it is dropped down

            lookInComboBox.Items.Add(
                new LookInComboBoxItem(
                    GetDisplayName(m_desktopFolder, m_desktopPidl, NativeMethods.SHGNO.SHGDN_INFOLDER), m_desktopPidl, 0));
            EnumDesktop();

            int index = 0;
            foreach (LookInComboBoxItem item in lookInComboBox.Items)
            {
                if (item.Text.Equals(lookInComboBox.CurrentItem.Text))
                {
                    lookInComboBox.SelectedIndex = index;
                    break;
                }

                index++;
            }
        }

        #endregion

        #region File Name & File Type Methods

        /// <summary>
        /// Represents a file type
        /// </summary>
        internal class FileType
        {
            #region Member Fields

            private readonly string m_name;
            private readonly string[] m_extensions;
            private readonly string m_filterPattern;
            private readonly bool m_includeAllFiles;

            #endregion

            #region Construction

            public FileType(string name, string[] extensions)
            {
                m_name = name;
                m_extensions = extensions;

                if (extensions.Length == 1 && extensions[0] == "*.*")
                    m_includeAllFiles = true;

                m_filterPattern = "^";
                for (int i = 0; i < extensions.Length; i++)
                {
                    if (i > 0) m_filterPattern += "|";
                    m_filterPattern += "(";
                    for (int j = 0; j < extensions[i].Length; j++)
                    {
                        char c = extensions[i][j];
                        switch (c)
                        {
                            case '*':
                                m_filterPattern += ".*";
                                break;
                            case '?':
                                m_filterPattern += ".";
                                break;
                            case '\\':
                                if (j < extensions[i].Length - 1)
                                    m_filterPattern += Regex.Escape(extensions[i][++j].ToString());
                                break;
                            default:
                                m_filterPattern += Regex.Escape(extensions[i][j].ToString());
                                break;
                        }
                    }
                    m_filterPattern += ")";
                }
                m_filterPattern += "$";
            }

            #endregion

            #region Methods

            public override string ToString()
            {
                return m_name;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets the friendly name of the filter.
            /// </summary>
            public string Name
            {
                get { return m_name; }
            }

            /// <summary>
            /// Gets the extensions of the file type.
            /// </summary>
            public string[] Extensions
            {
                get { return m_extensions; }
            }

            /// <summary>
            /// Gets the filter pattern.
            /// </summary>
            public string FilterPattern
            {
                get { return m_filterPattern; }
            }

            /// <summary>
            /// Gets a boolean value indicating weather to include all files.
            /// </summary>
            public bool IncludeAllFiles
            {
                get { return m_includeAllFiles; }
            }

            #endregion
        }

        private static IntPtr GetComboBoxHWndEdit(ComboBox comboBox)
        {
            NativeMethods.COMBOBOXINFO cbInfo = new NativeMethods.COMBOBOXINFO();
            cbInfo.cbSize = NativeMethods.cbComboBoxInfo;
            if (NativeMethods.User32.GetComboBoxInfo(comboBox.Handle, ref cbInfo))
                return cbInfo.hwndEdit;

            return IntPtr.Zero;
        }

        private void UpdateAutoComplete()
        {
            IntPtr hwndEdit = GetComboBoxHWndEdit(fileNameComboBox);
            if (hwndEdit != IntPtr.Zero)
            {
                NativeMethods.IAutoComplete2 iac2 =
                    (NativeMethods.IAutoComplete2)Activator.CreateInstance(
                                                      Type.GetTypeFromCLSID(NativeMethods.CLSID_AutoComplete));

                ACListISF acl = new ACListISF();

                // Set the current working directory so relatives path auto completes
                acl.CurrentWorkingDirectory =
                    GetDisplayName(m_desktopFolder, m_pidlAbsCurrent, NativeMethods.SHGNO.SHGDN_FORPARSING);
                acl.ExcludeFiles = m_excludeFiles;

                iac2.Init(hwndEdit, acl, string.Empty, string.Empty);
                iac2.SetOptions((uint)NativeMethods.AUTOCOMPLETEOPTIONS.ACO_AUTOAPPEND);
                iac2.Enable(1);

                Marshal.ReleaseComObject(iac2);
            }
        }

        private void fileNameComboBox_TextChanged(object sender, EventArgs e)
        {
            if (m_ignoreFileNameChange)
            {
                m_ignoreFileNameChange = false;
                return;
            }

            if (!m_excludeFiles)
            {
                okButton.Enabled = ((m_selectedPidls != null && m_selectedPidls.Length > 0) ||
                                    !string.IsNullOrEmpty(fileNameComboBox.Text));
            }

            // Deselect all selected items
            if (m_shellView != null)
                m_shellView.SelectItem(IntPtr.Zero, NativeMethods.SVSI_DESELECTOTHERS);
        }

        private void okButton_EnabledChanged(object sender, EventArgs e)
        {
            AcceptButton = okButton.Enabled ? okButton : cancelButton;
        }

        private void fileTypeComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (m_currentFilePattern != string.Empty)
            {
                m_currentFilePattern = string.Empty;
                fileNameComboBox.Text = string.Empty;
            }

            if (m_shellView != null)
                m_shellView.Refresh(); // Causes the ICommDlgBrowser::IncludeObject to be called
        }

        protected void SelectCorrespondingFileType()
        {
            // Select the file type that corresponds to the pattern in the fileNameComboBox
            for (int i = 0; i < fileTypeComboBox.Items.Count; i++)
            {
                FileType fileType = (FileType)fileTypeComboBox.Items[i];

                if (fileType.IncludeAllFiles)
                    continue;

                if (Regex.IsMatch(FileName, fileType.FilterPattern))
                {
                    fileTypeComboBox.SelectedIndex = i;
                    return;
                }
            }
        }

        protected virtual void OnFileOK(object sender, EventArgs e)
        {
            if (m_selectedPidls != null && m_selectedPidls.Length > 0)
            {
                // Check if the first item is a folder.
                // It would be better to check against the SFGAO_BROWSABLE attribute, but it is never set.
                // Instead check against the SFGAO_STREAM attribute, to assure that the item isn't a folder.
                IntPtr[] pidls = new IntPtr[] { m_selectedPidls[0] };
                NativeMethods.SHGFAO attribs = NativeMethods.SHGFAO.SFGAO_STREAM | NativeMethods.SHGFAO.SFGAO_LINK;
                m_currentFolder.GetAttributesOf(1, pidls, ref attribs);
                if ((attribs & NativeMethods.SHGFAO.SFGAO_STREAM) == 0)
                {
                    ((NativeMethods.IShellBrowser)this).BrowseObject(m_selectedPidls[0], NativeMethods.SBSP_RELATIVE);
                }
                // Check if the item is a shortcut
                else if ((attribs & NativeMethods.SHGFAO.SFGAO_LINK) == NativeMethods.SHGFAO.SFGAO_LINK)
                {
                    BrowseToLink(m_selectedPidls[0]);
                }
                else
                {
                    int index = 0;
                    string[] fileNames = new string[m_selectedPidls.Length];
                    fileNames[0] =
                        GetDisplayName(m_currentFolder, m_selectedPidls[0], NativeMethods.SHGNO.SHGDN_FORPARSING);

                    for (int i = 1; i < m_selectedPidls.Length; i++)
                    {
                        pidls = new IntPtr[] { m_selectedPidls[i] };
                        attribs = NativeMethods.SHGFAO.SFGAO_STREAM;
                        m_currentFolder.GetAttributesOf(1, pidls, ref attribs);

                        if ((attribs & NativeMethods.SHGFAO.SFGAO_STREAM) == NativeMethods.SHGFAO.SFGAO_STREAM)
                        {
                            fileNames[++index] =
                                GetDisplayName(m_currentFolder, m_selectedPidls[i], NativeMethods.SHGNO.SHGDN_FORPARSING);
                        }
                    }

                    if (fileNames.Length > index + 1)
                        Array.Resize(ref fileNames, index + 1);

                    if (!File.Exists(fileNames[0]) || PromptFileOverwrite(Path.GetFileName(fileNames[0])))
                    {
                        m_fileNames = fileNames;
                        DialogResult = DialogResult.OK;
                    }
                }
            }
            else
            {
                string path = fileNameComboBox.Text, driveName;
                string trimmedPath = path.Trim();
                if (path != trimmedPath)
                {
                    m_ignoreFileNameChange = true;

                    path = trimmedPath;
                    fileNameComboBox.Text = trimmedPath;
                }

                if (string.IsNullOrEmpty(path))
                {
                    if (!string.IsNullOrEmpty(m_currentFilePattern) && m_shellView != null)
                    {
                        m_currentFilePattern = string.Empty;
                        m_shellView.Refresh(); // Causes the ICommDlgBrowser::IncludeObject to be called
                    }

                    return;
                }

                bool lastCharIsSeparator = (path[path.Length - 1] == Path.DirectorySeparatorChar);

                UpdateFileNameMRU(path);

                bool isPathRooted = Path.IsPathRooted(path);
                string orgPath = path;
                if (!isPathRooted)
                    path = Path.Combine(GetDisplayName(m_desktopFolder, m_pidlAbsCurrent, NativeMethods.SHGNO.SHGDN_FORPARSING), path);

                //string rootedPath = path;
                if (path.Contains(".."))
                    path = Path.GetFullPath(path);

                if (Directory.Exists(path))
                {
                    //if (!isPathRooted)
                    //{
                    //    IntPtr pidl;
                    //    m_currentFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, orgPath, IntPtr.Zero, out pidl, 0);
                    //    ((NativeMethods.IShellBrowser)this).BrowseObject(pidl, NativeMethods.SBSP_RELATIVE);
                    //}
                    //else
                    //{
                    IntPtr pidl;
                    m_desktopFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, path, IntPtr.Zero, out pidl, 0);
                    ((NativeMethods.IShellBrowser)this).BrowseObject(pidl, NativeMethods.SBSP_ABSOLUTE);
                    //}

                    fileNameComboBox.Text = string.Empty;
                    return;
                }
                
                // If the path root is a drive make sure it's available
                if (path.Length <= 3 && PathUtils.PathRootIsDrive(path, out driveName))
                {
                    DriveInfo di = new DriveInfo(driveName);
                    if (di.DriveType == DriveType.CDRom)
                    {
                        // The drive is an optical disk drive
                        string message = "The selected disk drive is not in use. Check to make sure a disk is inserted.";
                        while (!di.IsReady)
                        {
                            // Loop until a disk is inserted or cancel is clicked
                            if (MessageBoxWithFocusRestore(message, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) ==
                                DialogResult.Cancel)
                            {
                                fileNameComboBox.SelectAll();
                                return;
                            }
                        }
                    }
                    else if (di.DriveType == DriveType.NoRootDirectory)
                    {
                        // The drive does not have a root directory, e.g. an unconnected network drive
                        string message =
                            string.Format("The drive '{0}' is not valid. Enter a valid drive letter.",
                                          driveName.Substring(0, 2));
                        MessageBoxWithFocusRestore(message, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        fileNameComboBox.SelectAll();
                        return;
                    }

                    IntPtr pidl;
                    m_desktopFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, driveName, IntPtr.Zero, out pidl, 0);
                    ((NativeMethods.IShellBrowser)this).BrowseObject(pidl, NativeMethods.SBSP_ABSOLUTE);

                    fileNameComboBox.Text = string.Empty;
                    return;
                }
                
                if (lastCharIsSeparator) // TODO: Find out what to do when m_pathMustExist != true
                {
                    string message =
                        string.Format(
                            @"The folder '{0}' isn't accessible. The folder may be located in an unavailable location, protected with a password, or the filename contains a / or \.",
                            path);
                    MessageBoxWithFocusRestore(message, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    fileNameComboBox.SelectAll();
                    return;
                }

                path = AppendExtension(path);
                orgPath = AppendExtension(orgPath);
                string parentPath = Path.GetDirectoryName(path);

                if (File.Exists(path))
                {
                    fileNameComboBox.Text = AppendExtension(fileNameComboBox.Text);
                    if (PromptFileOverwrite(Path.GetFileName(path)))
                    {
                        FileName = path;
                        DialogResult = DialogResult.OK;
                    }
                }
                else if (Directory.Exists(parentPath))
                {
                    string currentPath =
                        GetDisplayName(m_desktopFolder, m_pidlAbsCurrent, NativeMethods.SHGNO.SHGDN_FORPARSING);

                    if (parentPath != currentPath)
                    {
                        IntPtr pidl;
                        if (!isPathRooted)
                        {
                            m_currentFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, Path.GetDirectoryName(orgPath), IntPtr.Zero, out pidl, 0);
                            ((NativeMethods.IShellBrowser)this).BrowseObject(pidl, NativeMethods.SBSP_RELATIVE);
                            fileNameComboBox.Text = Path.GetFileName(orgPath);
                        }
                        else
                        {
                            m_desktopFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, parentPath, IntPtr.Zero, out pidl, 0);
                            ((NativeMethods.IShellBrowser)this).BrowseObject(pidl, NativeMethods.SBSP_ABSOLUTE);
                            fileNameComboBox.Text = Path.GetFileName(path);
                        }
                    }
                    if (PromptFileCreate())
                    {
                        FileName = path;
                        DialogResult = DialogResult.OK;
                    }
                }
                else // TODO: Find out what to do when m_fileMustExist != false
                {
                    fileNameComboBox.Text = AppendExtension(fileNameComboBox.Text);
                    if (PromptFileCreate())
                    {
                        FileName = path;
                        DialogResult = DialogResult.OK;
                    }
                }
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            OnFileOK(sender, e);
        }

        private void UpdateFileNameMRU(string fileName)
        {
#if REGISTRY_SUPPORT

            RegistryKey regKey = Registry.CurrentUser.CreateSubKey(@"Software\[ProductName]\Open Find\[ProductName]\Settings\" + m_type + @"\File Name MRU");

            int maxEntries;
            if (regKey.GetValue("Maximum Entries") != null &&
                regKey.GetValueKind("Maximum Entries") == RegistryValueKind.DWord)
                maxEntries = (int)regKey.GetValue("Maximum Entries");
            else
            {
                regKey.SetValue("Maximum Entries", 10, RegistryValueKind.DWord);
                maxEntries = 10;
            }

            fileNameComboBox.Items.Insert(0, fileName);

            string[] fileNameMRU = new string[Math.Min(fileNameComboBox.Items.Count, maxEntries)];
            for (int i = 0; i < fileNameComboBox.Items.Count; i++)
            {
                if (i >= maxEntries)
                    fileNameComboBox.Items.RemoveAt(maxEntries);
                else
                {
                    fileNameMRU[i] = fileNameComboBox.Items[i].ToString();
                }
            }

            regKey.SetValue("Value", fileNameMRU, RegistryValueKind.MultiString);
            regKey.Close();

#endif
        }

        #endregion

        #region BrowseObject Methods

        private void BrowseToLink(IntPtr relpidl)
        {
            string path = GetDisplayName(m_currentFolder, relpidl, NativeMethods.SHGNO.SHGDN_FORPARSING);

            NativeMethods.IShellLink psl;
            psl = (NativeMethods.IShellLink)new NativeMethods.ShellLink();

            NativeMethods.IPersistFile ppf;
            ppf = (NativeMethods.IPersistFile)psl;
            ppf.Load(path, 0);

            psl.Resolve(Handle, NativeMethods.SLR_FLAGS.SLR_NOTRACK);

            IntPtr pidl;
            psl.GetIDList(out pidl);

            NativeMethods.SHGFAO attribs = GetAttributesOf(pidl);
            string linkPath = GetDisplayName(m_desktopFolder, pidl, NativeMethods.SHGNO.SHGDN_FORPARSING);

            if ((attribs & NativeMethods.SHGFAO.SFGAO_FOLDER) == NativeMethods.SHGFAO.SFGAO_FOLDER)
                ((NativeMethods.IShellBrowser)this).BrowseObject(pidl, NativeMethods.SBSP_ABSOLUTE);
            else if ((attribs & NativeMethods.SHGFAO.SFGAO_STREAM) == NativeMethods.SHGFAO.SFGAO_STREAM)
            {
                if (!File.Exists(linkPath) || PromptFileOverwrite(Path.GetFileName(linkPath)))
                {
                    m_fileNames = new string[] { linkPath };
                    DialogResult = DialogResult.OK;
                }
            }

            NativeMethods.Shell32.ILFree(pidl);

            Marshal.ReleaseComObject(ppf);
            Marshal.ReleaseComObject(psl);
        }

        private void UpdateUI(IntPtr previousPidl)
        {
            // Disable the Up On Level if the current folder is the desktop
            bool isDesktop = NativeMethods.Shell32.ILIsEqual(m_pidlAbsCurrent, m_desktopPidl);
            upOneLevelToolStripButton.Enabled = !isDesktop;

            IntPtr childPidl = NativeMethods.Shell32.ILFindLastID(m_pidlAbsCurrent);
            IntPtr parentPidl = GetParentPidl(m_pidlAbsCurrent);
            NativeMethods.IShellFolder parentFolder = m_desktopFolder;
            if (!NativeMethods.Shell32.ILIsEqual(parentPidl, m_desktopPidl))
            {
                IntPtr parentFolderPtr;
                m_desktopFolder.BindToObject(parentPidl, IntPtr.Zero, ref NativeMethods.IID_IShellFolder,
                                             out parentFolderPtr);
                if (parentFolderPtr != IntPtr.Zero)
                    parentFolder = (NativeMethods.IShellFolder)Marshal.GetObjectForIUnknown(parentFolderPtr);
                else
                    parentFolder = null;
            }

            IntPtr[] pidls = {childPidl};
            NativeMethods.SHGFAO attribs = NativeMethods.SHGFAO.SFGAO_STORAGE;

            if (parentFolder != null)
                parentFolder.GetAttributesOf(1, pidls, ref attribs);

            // Disable the Create New Folder if the current folder isn't a storage, e.g. My Computer
            createNewFolderToolStripButton.Enabled = ((attribs & NativeMethods.SHGFAO.SFGAO_STORAGE) ==
                                                      NativeMethods.SHGFAO.SFGAO_STORAGE);

            if (m_suspendAddBackItem)
                m_suspendAddBackItem = false;
            else
                AddBackItem(previousPidl);

            foreach (ToolStripButton placeButton in placesBar.Items)
            {
                placeButton.Checked = NativeMethods.Shell32.ILIsEqual((IntPtr)placeButton.Tag, m_pidlAbsCurrent);
            }

            // Change the current item in the lookInComboBox
            if (parentFolder != null)
            {
                lookInComboBox.Items.Clear();
                lookInComboBox.CurrentItem =
                    new LookInComboBoxItem(GetDisplayName(m_desktopFolder, m_pidlAbsCurrent), m_pidlAbsCurrent, 0);
                lookInComboBox.Refresh();
            }

            UpdateAutoComplete();
        }

        private void SetCurrentViewMode(uint viewMode)
        {
            // Instead of creating a new IShellView use the IFolderView interface

            IntPtr iShellViewPtr = Marshal.GetIUnknownForObject(m_shellView);
            IntPtr iFolderViewPtr;
            Marshal.QueryInterface(iShellViewPtr, ref NativeMethods.IID_IFolderView, out iFolderViewPtr);
            NativeMethods.IFolderView iFolderView =
                (NativeMethods.IFolderView)Marshal.GetObjectForIUnknown(iFolderViewPtr);
            iFolderView.SetCurrentViewMode(viewMode);
            Marshal.ReleaseComObject(iFolderView);
            Marshal.Release(iFolderViewPtr);
            Marshal.Release(iShellViewPtr);
        }

        #endregion

        #region IShellBrowser Members

        // Return the window handle.
        int NativeMethods.IShellBrowser.GetWindow(out IntPtr hwnd)
        {
            hwnd = Handle;
            return NativeMethods.S_OK;
        }

        int NativeMethods.IShellBrowser.ContextSensitiveHelp(int fEnterMode)
        {
            return NativeMethods.E_NOTIMPL;
        }

        // Allows the container to insert its menu groups into the composite menu
        // that is displayed when an extended namespace is being viewed or used.
        int NativeMethods.IShellBrowser.InsertMenusSB(IntPtr hmenuShared, IntPtr lpMenuWidths)
        {
            return NativeMethods.E_NOTIMPL;
        }

        // Installs the composite menu in the view window.
        int NativeMethods.IShellBrowser.SetMenuSB(IntPtr hmenuShared, IntPtr holemenuRes, IntPtr hwndActiveObject)
        {
            return NativeMethods.E_NOTIMPL;
        }

        // Permits the container to remove any of its menu elements from the in-place
        // composite menu and to free all associated resources.
        int NativeMethods.IShellBrowser.RemoveMenusSB(IntPtr hmenuShared)
        {
            return NativeMethods.E_NOTIMPL;
        }

        // Sets and displays status text about the in-place object in the
        // container's frame-window status bar.
        int NativeMethods.IShellBrowser.SetStatusTextSB(IntPtr pszStatusText)
        {
            return NativeMethods.E_NOTIMPL;
        }

        // Tells Microsoft Windows Explorer to enable or disable its modeless dialog boxes.
        int NativeMethods.IShellBrowser.EnableModelessSB(bool fEnable)
        {
            return NativeMethods.E_NOTIMPL;
        }

        // Translates accelerator keystrokes intended for the browser's frame while the view is active.
        int NativeMethods.IShellBrowser.TranslateAcceleratorSB(IntPtr pmsg, short wID)
        {
            return NativeMethods.S_OK;
        }

        private IntPtr GetParentPidl(IntPtr pidl)
        {
            // Get the parent pidl by removing the last pidl
            IntPtr pidlParent = NativeMethods.Shell32.ILClone(pidl);
            NativeMethods.Shell32.ILRemoveLastID(pidlParent);

            return pidlParent;
        }

        // Informs Microsoft Windows Explorer to browse to another folder.
        int NativeMethods.IShellBrowser.BrowseObject(IntPtr pidl, uint wFlags)
        {
            int hr;
            IntPtr folderTmpPtr;
            NativeMethods.IShellFolder folderTmp;
            IntPtr pidlTmp;

            if (NativeMethods.Shell32.ILIsEqual(pidl, m_desktopPidl))
            {
                // pidl is desktop folder
                pidlTmp = NativeMethods.Shell32.ILClone(m_desktopPidl);
                folderTmp = m_desktopFolder;
            }
            else if ((wFlags & NativeMethods.SBSP_RELATIVE) != 0)
            {
                // SBSP_RELATIVE - pidl is relative from the current folder
                if ((hr = m_currentFolder.BindToObject(pidl, IntPtr.Zero, ref NativeMethods.IID_IShellFolder,
                                                       out folderTmpPtr)) != NativeMethods.S_OK)
                    return hr;
                pidlTmp = NativeMethods.Shell32.ILCombine(m_pidlAbsCurrent, pidl);
                folderTmp = (NativeMethods.IShellFolder)Marshal.GetObjectForIUnknown(folderTmpPtr);
            }
            else if ((wFlags & NativeMethods.SBSP_PARENT) != 0)
            {
                // SBSP_PARENT - Browse the parent folder (ignores the pidl)
                pidlTmp = GetParentPidl(m_pidlAbsCurrent);
                string pathTmp = GetDisplayName(m_desktopFolder, pidlTmp, NativeMethods.SHGNO.SHGDN_FORPARSING);
                if (pathTmp.Equals(m_desktopPath))
                {
                    pidlTmp = NativeMethods.Shell32.ILClone(m_desktopPidl);
                    folderTmp = m_desktopFolder;
                }
                else
                {
                    if ((hr = m_desktopFolder.BindToObject(pidlTmp, IntPtr.Zero, ref NativeMethods.IID_IShellFolder,
                                                           out folderTmpPtr)) != NativeMethods.S_OK)
                        return hr;
                    folderTmp = (NativeMethods.IShellFolder)Marshal.GetObjectForIUnknown(folderTmpPtr);
                }
            }
            else
            {
                // SBSP_ABSOLUTE - pidl is an absolute pidl (relative from desktop)
                pidlTmp = NativeMethods.Shell32.ILClone(pidl);
                if ((hr = m_desktopFolder.BindToObject(pidlTmp, IntPtr.Zero, ref NativeMethods.IID_IShellFolder,
                                                       out folderTmpPtr)) != NativeMethods.S_OK)
                    return hr;
                folderTmp = (NativeMethods.IShellFolder)Marshal.GetObjectForIUnknown(folderTmpPtr);
            }

            if (folderTmp == null)
            {
                NativeMethods.Shell32.ILFree(pidlTmp);
                return NativeMethods.E_FAIL;
            }

            string path = GetDisplayName(m_desktopFolder, pidlTmp, NativeMethods.SHGNO.SHGDN_FORPARSING);

            // FIX: Force the special folder My documents
            if (path == m_myDocsPath && !NativeMethods.Shell32.ILIsEqual(pidlTmp, m_myDocsPidl))
            {
                NativeMethods.Shell32.ILFree(pidlTmp);
                pidlTmp = NativeMethods.Shell32.ILClone(m_myDocsPidl);
            }

            // If the path root is a drive make sure it's available
            string driveName;
            if (PathUtils.PathRootIsDrive(path, out driveName))
            {
                DriveInfo di = new DriveInfo(driveName);
                if (di.DriveType == DriveType.CDRom)
                {
                    // The drive is an optical disk drive
                    string message = "The selected disk drive is not in use. Check to make sure a disk is inserted.";
                    while (!di.IsReady)
                    {
                        // Loop until a disk is inserted or cancel is clicked
                        if (MessageBoxWithFocusRestore(message, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) ==
                            DialogResult.Cancel)
                            return NativeMethods.S_OK;
                    }
                }
                else if (di.DriveType == DriveType.NoRootDirectory)
                {
                    // The drive does not have a root directory, e.g. an unconnected network drive
                    string message =
                        string.Format("The drive '{0}' is not valid. Enter a valid drive letter.",
                                      driveName.Substring(0, 2));
                    MessageBoxWithFocusRestore(message, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return NativeMethods.S_OK;
                }
            }

            m_currentFolder = folderTmp;

            NativeMethods.FOLDERSETTINGS fs = new NativeMethods.FOLDERSETTINGS();
            NativeMethods.IShellView lastIShellView = m_shellView;

            if (lastIShellView != null)
                lastIShellView.GetCurrentInfo(ref fs); // Copy the old folder settings
            else
            {
                fs = new NativeMethods.FOLDERSETTINGS();
                fs.fFlags = (uint)m_flags;
                fs.ViewMode = (uint)m_viewMode;
            }

            // Create the IShellView
            IntPtr iShellViewPtr;
            hr = folderTmp.CreateViewObject(Handle, ref NativeMethods.IID_IShellView, out iShellViewPtr);
            if (hr == NativeMethods.S_OK)
            {
                m_shellView = (NativeMethods.IShellView)Marshal.GetObjectForIUnknown(iShellViewPtr);

                m_hWndListView = IntPtr.Zero;
                NativeMethods.RECT rc =
                    new NativeMethods.RECT(FileViewPadding.Left, FileViewPadding.Top,
                                           ClientSize.Width - FileViewPadding.Right,
                                           ClientSize.Height - FileViewPadding.Bottom);

                int res;

                try
                {
                    // Create the list view
                    res = m_shellView.CreateViewWindow(lastIShellView, ref fs, this, ref rc, ref m_hWndListView);
                }
                catch (COMException)
                {
                    return NativeMethods.E_FAIL;
                }

                if (res < 0)
                    return NativeMethods.E_FAIL;

                bool shellViewHasFocus = m_shellViewHasFocus;

                // Release the old IShellView
                if (lastIShellView != null)
                {
                    lastIShellView.GetCurrentInfo(ref fs);
                    lastIShellView.UIActivate((uint)NativeMethods.SVUIA_STATUS.SVUIA_DEACTIVATE);
                    lastIShellView.DestroyViewWindow();
                    Marshal.ReleaseComObject(lastIShellView);
                }

                // Give the new IShellView focus if the old one had focus
                m_shellView.UIActivate(shellViewHasFocus
                                           ? (uint)NativeMethods.SVUIA_STATUS.SVUIA_ACTIVATE_FOCUS
                                           : (uint)NativeMethods.SVUIA_STATUS.SVUIA_ACTIVATE_NOFOCUS);
                IntPtr previousPidl = m_pidlAbsCurrent;
                m_pidlAbsCurrent = pidlTmp;
                UpdateUI(previousPidl);

                // Clear the selection
                m_selectedPidls = null;
                OnSelectionChanged();
            }

            return NativeMethods.S_OK;
        }

        // This method is used to save and restore the persistent state for a view
        // (the icon positions, the column widths, and the current scroll position, for example). 
        int NativeMethods.IShellBrowser.GetViewStateStream(uint grfMode, IntPtr ppStrm)
        {
            return NativeMethods.E_NOTIMPL;
        }

        // GetControlWindow is used so views can directly manipulate the browser's controls.
        int NativeMethods.IShellBrowser.GetControlWindow(uint id, out IntPtr phwnd)
        {
            phwnd = IntPtr.Zero;
            return NativeMethods.S_FALSE;
        }

        // Sends control messages to either the toolbar or the status bar in a Microsoft Windows Explorer window. 
        int NativeMethods.IShellBrowser.SendControlMsg(uint id, uint uMsg, uint wParam, uint lParam, IntPtr pret)
        {
            return NativeMethods.E_NOTIMPL;
        }

        // Retrieves the currently active (displayed) Shell view object.
        int NativeMethods.IShellBrowser.QueryActiveShellView(ref NativeMethods.IShellView ppshv)
        {
            Marshal.AddRef(Marshal.GetIUnknownForObject(m_shellView));
            ppshv = m_shellView;
            return NativeMethods.S_OK;
        }

        // This method informs the browser that the view is getting the focus
        // (when the mouse is clicked on the view, for example).
        int NativeMethods.IShellBrowser.OnViewWindowActive(NativeMethods.IShellView pshv)
        {
            return NativeMethods.E_NOTIMPL;
        }

        // Adds toolbar items to Microsoft Windows Explorer's toolbar.
        int NativeMethods.IShellBrowser.SetToolbarItems(IntPtr lpButtons, uint nButtons, uint uFlags)
        {
            return NativeMethods.E_NOTIMPL;
        }

        #endregion

        #region ICommDlgBrowser Members

        // Called when a user double-clicks in the view or presses the ENTER key.
        int NativeMethods.ICommDlgBrowser.OnDefaultCommand(IntPtr ppshv)
        {
            Console.WriteLine("In OnDefaultCommand");
            // Retrieve the data object that represents the selected items
            IntPtr pDataObjectPtr;
            if (m_shellView.GetItemObject(
                    (uint)NativeMethods.SVGIO.SVGIO_SELECTION,
                    ref NativeMethods.IID_IDataObject,
                    out pDataObjectPtr) == NativeMethods.S_OK)
            {
                bool processed = false;

                ComTypes.IDataObject pDataObject =
                    (ComTypes.IDataObject)Marshal.GetObjectForIUnknown(pDataObjectPtr);

                short cfFormat = (short)NativeMethods.User32.RegisterClipboardFormat(NativeMethods.CFSTR_SHELLIDLIST);

                ComTypes.FORMATETC fmt = new ComTypes.FORMATETC();

                ComTypes.STGMEDIUM stg;

                fmt.cfFormat = cfFormat;
                fmt.ptd = IntPtr.Zero;
                fmt.dwAspect = ComTypes.DVASPECT.DVASPECT_CONTENT;
                fmt.lindex = -1;
                fmt.tymed = ComTypes.TYMED.TYMED_HGLOBAL;

                if (pDataObject.QueryGetData(ref fmt) == NativeMethods.S_OK)
                {
                    pDataObject.GetData(ref fmt, out stg);

                    IntPtr pIDList = stg.unionmember;
                    NativeMethods.Kernel32.GlobalLock(stg.unionmember);

                    // Number of relative IDList
                    uint cidl = (uint)Marshal.ReadInt32(pIDList);

                    int offset = sizeof (uint);
                    IntPtr parentpidl = (IntPtr)((int)pIDList + (uint)Marshal.ReadInt32(pIDList, offset));

                    // Only perform check for the first item
                    offset += sizeof (uint);
                    IntPtr relpidl = (IntPtr)((int)pIDList + (uint)Marshal.ReadInt32(pIDList, offset));
                    IntPtr abspidl = NativeMethods.Shell32.ILCombine(parentpidl, relpidl);

                    IntPtr[] pidls = new IntPtr[] {relpidl};
                    NativeMethods.SHGFAO attribs = NativeMethods.SHGFAO.SFGAO_LINK | NativeMethods.SHGFAO.SFGAO_STREAM;
                    m_currentFolder.GetAttributesOf(1, pidls, ref attribs);

                    // The IShellBrowser::BrowseObject method isn't called for 'My documents'
                    if (NativeMethods.Shell32.ILIsEqual(abspidl, m_myDocsPidl))
                    {
                        ((NativeMethods.IShellBrowser)this).BrowseObject(m_myDocsPidl, NativeMethods.SBSP_RELATIVE);
                        processed = true;
                    }
                    // Check if the item is a shortcut
                    else if ((attribs & NativeMethods.SHGFAO.SFGAO_LINK) == NativeMethods.SHGFAO.SFGAO_LINK)
                    {
                        BrowseToLink(relpidl);
                        processed = true;
                    }
                    // Check if the item is a file
                    else if ((attribs & NativeMethods.SHGFAO.SFGAO_STREAM) == NativeMethods.SHGFAO.SFGAO_STREAM)
                    {
                        int index = 0;
                        string[] fileNames = new string[cidl];
                        fileNames[0] = GetDisplayName(m_currentFolder, relpidl, NativeMethods.SHGNO.SHGDN_FORPARSING);

                        for (int i = 1; i < cidl; i++)
                        {
                            NativeMethods.Shell32.ILFree(relpidl);

                            offset += sizeof (uint);
                            relpidl = (IntPtr)((int)pIDList + (uint)Marshal.ReadInt32(pIDList, offset));

                            attribs = NativeMethods.SHGFAO.SFGAO_STREAM;
                            m_currentFolder.GetAttributesOf(1, new IntPtr[] {relpidl}, ref attribs);

                            if ((attribs & NativeMethods.SHGFAO.SFGAO_STREAM) == NativeMethods.SHGFAO.SFGAO_STREAM)
                            {
                                fileNames[++index] =
                                    GetDisplayName(m_currentFolder, relpidl, NativeMethods.SHGNO.SHGDN_FORPARSING);
                            }
                        }

                        if (fileNames.Length > index + 1)
                            Array.Resize(ref fileNames, index + 1);

                        if (!File.Exists(fileNames[0]) || PromptFileOverwrite(Path.GetFileName(fileNames[0])))
                        {
                            m_fileNames = fileNames;
                            DialogResult = DialogResult.OK;
                        }

                        processed = true;
                    }

                    NativeMethods.Shell32.ILFree(abspidl);
                    NativeMethods.Shell32.ILFree(relpidl);
                    NativeMethods.Shell32.ILFree(parentpidl);

                    NativeMethods.Kernel32.GlobalUnlock(stg.unionmember);
                    NativeMethods.Ole32.ReleaseStgMedium(ref stg);

                    if (processed)
                        return NativeMethods.S_OK;
                }
            }

            return NativeMethods.S_FALSE;
        }

        protected virtual void OnSelectionChanged()
        {
        }

        // Called after a state, identified by the uChange parameter, has changed in the IShellView interface.
        int NativeMethods.ICommDlgBrowser.OnStateChange(IntPtr ppshv, IntPtr uChange)
        {
            switch (uChange.ToInt32())
            {
                case NativeMethods.CDBOSC_SETFOCUS: // The focus has been set to the view.
                    m_shellViewHasFocus = true;

                    // If the cancel button loses focuse it won't update it's style
                    if (okButton.Enabled)
                    {
                        okButton.NotifyDefault(true);
                        cancelButton.NotifyDefault(false);
                    }
                    break;

                case NativeMethods.CDBOSC_KILLFOCUS: // The view has lost the focus.
                    m_shellViewHasFocus = false;
                    break;

                case NativeMethods.CDBOSC_SELCHANGE: // The selection has changed.
                    bool itemsSelected = false;
                    NativeMethods.SHGFAO attribs = NativeMethods.SHGFAO.SFGAO_CANDELETE |
                                                   NativeMethods.SHGFAO.SFGAO_CANRENAME;
                    IntPtr pDataObjectPtr;
                    if (m_shellView.GetItemObject(
                            (uint)NativeMethods.SVGIO.SVGIO_SELECTION,
                            ref NativeMethods.IID_IDataObject,
                            out pDataObjectPtr) != NativeMethods.S_OK)
                    {
                        m_selectedPidls = null;
                    }
                    else
                    {
                        ComTypes.IDataObject pDataObject =
                            (ComTypes.IDataObject)Marshal.GetObjectForIUnknown(pDataObjectPtr);

                        short cfFormat =
                            (short)NativeMethods.User32.RegisterClipboardFormat(NativeMethods.CFSTR_SHELLIDLIST);

                        ComTypes.FORMATETC fmt = new ComTypes.FORMATETC();
                        ComTypes.STGMEDIUM stg;

                        fmt.cfFormat = cfFormat;
                        fmt.ptd = IntPtr.Zero;
                        fmt.dwAspect = ComTypes.DVASPECT.DVASPECT_CONTENT;
                        fmt.lindex = -1;
                        fmt.tymed = ComTypes.TYMED.TYMED_HGLOBAL;

                        if (pDataObject.QueryGetData(ref fmt) == NativeMethods.S_OK)
                        {
                            pDataObject.GetData(ref fmt, out stg);

                            IntPtr pIDList = stg.unionmember;
                            NativeMethods.Kernel32.GlobalLock(stg.unionmember);

                            // Number of relative IDList
                            uint cidl = (uint)Marshal.ReadInt32(stg.unionmember);
                            if (cidl == 0)
                                m_selectedPidls = null; // No items selected
                            else
                            {
                                itemsSelected = true;

                                int offset = sizeof (uint);
                                m_selectedPidls = new IntPtr[cidl];
                                for (int i = 0; i < cidl; i++)
                                {
                                    offset += sizeof (uint);
                                    m_selectedPidls[i] =
                                        (IntPtr)((int)pIDList + (uint)Marshal.ReadInt32(pIDList, offset));
                                }

                                m_currentFolder.GetAttributesOf(cidl, m_selectedPidls, ref attribs);
                            }
                        }

                        Marshal.ReleaseComObject(pDataObject);
                        Marshal.Release(pDataObjectPtr);
                    }

                    OnSelectionChanged();

                    // Enable/disable delete and rename commands
                    deleteToolStripButton.Enabled =
                        deleteToolStripMenuItem.Enabled =
                        itemsSelected &&
                        ((attribs & NativeMethods.SHGFAO.SFGAO_CANDELETE) == NativeMethods.SHGFAO.SFGAO_CANDELETE);
                    renameToolStripMenuItem.Enabled = itemsSelected &&
                                                      ((attribs & NativeMethods.SHGFAO.SFGAO_CANRENAME) ==
                                                       NativeMethods.SHGFAO.SFGAO_CANRENAME);
                    break;
            }

            return NativeMethods.S_OK;
        }

        // Allows the common dialog box to filter objects that the view displays. 
        int NativeMethods.ICommDlgBrowser.IncludeObject(IntPtr ppshv, IntPtr pidl)
        {
            IntPtr strr = Marshal.AllocCoTaskMem(NativeMethods.MAX_PATH * 2 + 4);
            Marshal.WriteInt32(strr, 0, 0);
            StringBuilder buf = new StringBuilder(NativeMethods.MAX_PATH);

            IntPtr[] pidls = new IntPtr[] { pidl };
            NativeMethods.SHGFAO attribs = NativeMethods.SHGFAO.SFGAO_FILESYSANCESTOR |
                                           NativeMethods.SHGFAO.SFGAO_STREAM |
                                           NativeMethods.SHGFAO.SFGAO_LINK;
            m_currentFolder.GetAttributesOf(1, pidls, ref attribs);
            bool isFile = ((attribs & NativeMethods.SHGFAO.SFGAO_STREAM) == NativeMethods.SHGFAO.SFGAO_STREAM);
            bool isLink = ((attribs & NativeMethods.SHGFAO.SFGAO_LINK) == NativeMethods.SHGFAO.SFGAO_LINK);

            if (m_excludeFiles && !isLink && (isFile || (attribs & NativeMethods.SHGFAO.SFGAO_FILESYSANCESTOR) == 0))
                return NativeMethods.S_FALSE;
            if (!isFile && (attribs & NativeMethods.SHGFAO.SFGAO_FILESYSANCESTOR) == 0)
                return NativeMethods.S_FALSE;

            if (m_currentFolder.GetDisplayNameOf(
                    pidl,
                    NativeMethods.SHGNO.SHGDN_FORPARSING,
                    strr) == NativeMethods.S_OK)
            {
                NativeMethods.Shlwapi.StrRetToBuf(strr, pidl, buf, NativeMethods.MAX_PATH);
                string path = buf.ToString();
                string name = path.Substring(path.LastIndexOf('\\') + 1);

                // If the object is a newly created folder browser to that folder
                if (CreateNewFolder(path, pidl))
                    return NativeMethods.S_OK;

                if (isLink && File.Exists(path))
                {
                    NativeMethods.IShellLink psl;
                    psl = (NativeMethods.IShellLink)new NativeMethods.ShellLink();

                    NativeMethods.IPersistFile ppf;
                    ppf = (NativeMethods.IPersistFile)psl;
                    ppf.Load(path, 0);

                    IntPtr linkPidl;
                    psl.GetIDList(out linkPidl);

                    // IShellFolder.GetAttributesOf doesn't work
                    NativeMethods.SHFILEINFO sfi = new NativeMethods.SHFILEINFO();
                    NativeMethods.Shell32.SHGetFileInfo(linkPidl, NativeMethods.FILE_ATTRIBUTE_NORMAL, ref sfi,
                                                        NativeMethods.cbFileInfo,
                                                        NativeMethods.SHGFI_PIDL | NativeMethods.SHGFI_ATTRIBUTES);

                    string linkPath = GetDisplayName(m_desktopFolder, linkPidl, NativeMethods.SHGNO.SHGDN_FORPARSING);

                    NativeMethods.Shell32.ILFree(linkPidl);
                    Marshal.ReleaseComObject(ppf);
                    Marshal.ReleaseComObject(psl);

                    if ((sfi.dwAttributes & (int)NativeMethods.SHGFAO.SFGAO_FOLDER) ==
                        (int)NativeMethods.SHGFAO.SFGAO_FOLDER)
                        return NativeMethods.S_OK;

                    if ((sfi.dwAttributes & (int)NativeMethods.SHGFAO.SFGAO_STREAM) ==
                             (int)NativeMethods.SHGFAO.SFGAO_STREAM)
                    {
                        if (m_excludeFiles)
                            return NativeMethods.S_FALSE;

                        string linkName = linkPath.Substring(linkPath.LastIndexOf('\\') + 1);
                        FileType linkFileType = (FileType)fileTypeComboBox.SelectedItem;
                        if (m_currentFilePattern != string.Empty)
                        {
                            // If the object is a file and the file doesn't match the pattern, don't include the object
                            if (isFile && !Regex.IsMatch(linkName, m_currentFilePattern, RegexOptions.Compiled))
                                return NativeMethods.S_FALSE;
                        }
                        else if (linkFileType != null)
                        {
                            if (linkFileType.IncludeAllFiles)
                                return NativeMethods.S_OK;

                            // If the object is a file and the file doesn't math the pattern, don't include the object
                            if (isFile && !Regex.IsMatch(name, linkFileType.FilterPattern, RegexOptions.Compiled))
                                return NativeMethods.S_FALSE;
                        }

                        return NativeMethods.S_OK;
                    }
                }

                if (m_excludeFiles)
                    return NativeMethods.S_OK;

                FileType fileType = (FileType)fileTypeComboBox.SelectedItem;
                if (m_currentFilePattern != string.Empty)
                {
                    // If the object is a file and the file doesn't match the pattern, don't include the object
                    if (isFile && !Regex.IsMatch(name, m_currentFilePattern, RegexOptions.Compiled))
                        return NativeMethods.S_FALSE;
                }
                else if (fileType != null)
                {
                    if (fileType.IncludeAllFiles)
                        return NativeMethods.S_OK;

                    // If the object is a file and the file doesn't math the pattern, don't include the object
                    if (isFile && !Regex.IsMatch(name, fileType.FilterPattern, RegexOptions.Compiled))
                        return NativeMethods.S_FALSE;
                }
            }

            return NativeMethods.S_OK;
        }

        #endregion

        #region IServiceProvider Members

        // If you don't implement IServiceProvider (or don't return an IShellBrowser from QueryService),
        // IShellView just launches a whole new Windows Explorer to display the new folder.
        int NativeMethods.IServiceProvider.QueryService(ref Guid guidService, ref Guid riid,
                                                        out NativeMethods.IShellBrowser ppvObject)
        {
            if (riid == NativeMethods.IID_IShellBrowser)
            {
                ppvObject = this;
                return NativeMethods.S_OK;
            }

            ppvObject = null;
            return NativeMethods.E_NOINTERFACE;
        }

        #endregion

        #region Properties

        // Removed CheckFileExists and CheckPathExists because neihter of them affects the behavior of the FileDialog.

        ///// <summary>
        ///// Gets or sets a value indicating whether the dialog box displays a warning if the user specifies a file name that does not exist.
        ///// </summary>
        ///// <value>true if the dialog box displays a warning if the user specifies a file name that does not exist; otherwise, false. The default value is false.</value>
        //[Description("Checks that the specified file exists before returning from the dialog.")]
        //[Category("Behavior")]
        //[DefaultValue(false)]
        //public virtual bool CheckFileExists
        //{
        //    get { return m_fileMustExist; }
        //    set { m_fileMustExist = value; }
        //}

        ///// <summary>
        ///// Gets or sets a value indicating whether the dialog box displays a warning if the user specifies a path that does not exist.
        ///// </summary>
        ///// <value>true if the dialog box displays a warning when the user specifies a path that does not exist; otherwise, false. The default value is true.</value>
        //[Description("Checks that the specified path exists before returning from the dialog.")]
        //[Category("Behavior")]
        //[DefaultValue(true)]
        //public bool CheckPathExists
        //{
        //    get { return m_pathMustExist; }
        //    set { m_pathMustExist = value; }
        //}

        /// <summary>
        /// Gets or sets a string containing the file name selected in the file dialog box.
        /// </summary>
        /// <value>The file name selected in the file dialog box. The default value is an empty string ("").</value>
        [Category("Data")]
        [DefaultValue("")]
        [Description("The file first shown in the dialog box, or the last one selected by the user.")]
        public string FileName
        {
            get
            {
                if (m_fileNames == null)
                    return "";

                if (m_fileNames[0].Length <= 0)
                    return "";

                return m_fileNames[0];
            }
            set
            {
                if (value == null)
                    m_fileNames = null;
                else
                    m_fileNames = new string[] {value};
            }
        }

        /// <summary>
        /// Gets the file names of all selected files in the dialog box.
        /// </summary>
        /// <value>An array of type <see cref="System.String"/>, containing the file names of all selected files in the dialog box.</value>
        [Browsable(false)]
        [Description("Retrieves the file names of all selected files in the dialog box.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] FileNames
        {
            get
            {
                if (m_fileNames == null)
                    return new string[0];

                return (string[])m_fileNames.Clone();
            }
        }

        /// <summary>
        /// Gets or sets the current file name filter string, which determines the choices that appear in the "Save as file type" or "Files of type" box in the dialog box.
        /// </summary>
        /// <value>The file filtering options available in the dialog box.</value>
        /// <exception cref="System.ArgumentException">Filter format is invalid.</exception>
        [Description("The file filters to display in the dialog box, for example, \"C# files|*.cs|All files|*.*\".")]
        [Category("Behavior")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Filter
        {
            get
            {
                if (m_filter != null)
                    return m_filter;

                return "";
            }
            set
            {
                if (value != m_filter)
                {
                    if (string.IsNullOrEmpty(value))
                        value = null;
                    else
                    {
                        string[] filterParts = value.Split(new char[] { '|' });
                        if (filterParts == null || (filterParts.Length % 2) != 0)
                            throw new ArgumentException(
                                "Filter string you provided is not valid. The filter string must contain a description of the filter, followed by the vertical bar (|) and the filter pattern. The strings for different filtering options must also be separated by the vertical bar. Example: \"Text files (*.txt)|*.txt|All files (*.*)|*.*\"");
                    }

                    m_filter = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the index of the filter currently selected in the file dialog box.
        /// </summary>
        /// <value>A value containing the index of the filter currently selected in the file dialog box. The default value is 1.</value>
        [Category("Behavior")]
        [Description("The index of the file filter selected in the dialog box. The first item has an index of 1.")]
        [DefaultValue(1)]
        public int FilterIndex
        {
            get { return m_filterIndex; }
            set { m_filterIndex = value; }
        }

        /// <summary>
        /// Gets or sets the initial directory displayed by the file dialog box.
        /// </summary>
        /// <value>The initial directory displayed by the file dialog box. The default is an empty string ("").</value>
        [Category("Data")]
        [DefaultValue("")]
        [Description("The initial directory for the dialog box.")]
        public string InitialDirectory
        {
            get
            {
                if (m_initialDir != null)
                    return m_initialDir;

                return "";
            }
            set { m_initialDir = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to display a dialog when the create new folder button is clicked.
        /// </summary>
        /// <value>true if a dialog should be displayed when the create new folder button is clicked; otherwise, false. The default value is true.</value>
        [Description("Display a dialog when the create new folder button is clicked.")]
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool UseCreateNewFolderDialog
        {
            get { return m_useCreateNewFolderDialog; }
            set { m_useCreateNewFolderDialog = value; }
        }

        /// <summary>
        /// Gets or sets the view mode to display in the shell view.
        /// </summary>
        [Description("View mode to display in the shell view.")]
        [Category("Behavior")]
        [DefaultValue(FileDialogViewMode.List)]
        public FileDialogViewMode ViewMode
        {
            get { return m_viewMode; }
            set { m_viewMode = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to restore the last view mode when the dialog is shown.
        /// </summary>
        /// <value>true if the last view mode should be restored when the dialog is shown; otherwise, false. The default value is true.</value>
        [Description("Restore the last view mode when the dialog is shown.")]
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool RestoreLastViewMode
        {
            get { return m_restoreLastViewMode; }
            set { m_restoreLastViewMode = value; }
        }

        /// <summary>
        /// Gets the items of the drop-down portion of the OK button.
        /// </summary>
        /// <remarks>If no items is specified the OK button appears as a normal button.</remarks>
        [Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Category("Data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Localizable(true)] 
        [MergableProperty(false)]
        [Description("The items of the drop-down portion of the OK button.")]
        public StringCollection Options
        {
            get
            {
                if (m_options == null)
                    m_options = new StringCollection();
                
                return m_options;
            }
        }

        /// <summary>
        /// Gets the zero-based index of the selected option that was clicked from in the drop-down portion of the OK button.
        /// </summary>
        /// <returns>A zero-based index of the selected option that was clicked. A value of negative one (-1) is returned if the button was clicked.</returns>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public int SelectedOptionIndex
        {
            get { return okButton.ClickedItemIndex; }
        }

        /// <summary>
        /// Gets the places collection for this FileDialog instance.
        /// </summary>
        [Editor(typeof(FileDialogPlacesEditor), typeof(UITypeEditor))]
        [Category("Data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Localizable(true)]
        [MergableProperty(false)]
        [Description("The places of the places bar.")]
        public FileDialogPlacesCollection Places
        {
            get { return m_places; }
        }

        /// <summary>
        /// Gets or sets the file dialog box title.
        /// </summary>
        [Localizable(true)]
        [Description("The string to display in the title bar of the dialog box.")]
        [DefaultValue("")]
        [Category("Appearance")]
        public string Title
        {
            get { return Text; }
            set { Text = value; }
        }

        #endregion

        #region Hide unnecessary properties inherited from System.Windows.Forms.Form

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new IButtonControl AcceptButton
        {
            get { return base.AcceptButton; }
            set { base.AcceptButton = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new string AccessibleDescription
        {
            get { return base.AccessibleDescription; }
            set { base.AccessibleDescription = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new string AccessibleName
        {
            get { return base.AccessibleName; }
            set { base.AccessibleName = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new AccessibleRole AccessibleRole
        {
            get { return base.AccessibleRole; }
            set { base.AccessibleRole = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override bool AllowDrop
        {
            get { return base.AllowDrop; }
            set { base.AllowDrop = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override AnchorStyles Anchor
        {
            get { return base.Anchor; }
            set { base.Anchor = value; }
        }

        //[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        //public new bool AutoScale
        //{
        //    get { return base.AutoScale; }
        //    set { base.AutoScale = value; }
        //}

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete(
            "This property has been deprecated. Use the AutoScaleDimensions property instead.  http://go.microsoft.com/fwlink/?linkid=14202"
            )]
        public override Size AutoScaleBaseSize
        {
            get { return base.AutoScaleBaseSize; }
            set { }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override bool AutoScroll
        {
            get { return base.AutoScroll; }
            set { base.AutoScroll = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Size AutoScrollMargin
        {
            get { return base.AutoScrollMargin; }
            set { base.AutoScrollMargin = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Size AutoScrollMinSize
        {
            get { return base.AutoScrollMinSize; }
            set { base.AutoScrollMinSize = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new AutoSizeMode AutoSizeMode
        {
            get { return base.AutoSizeMode; }
            set { base.AutoSizeMode = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override AutoValidate AutoValidate
        {
            get { return base.AutoValidate; }
            set { base.AutoValidate = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set { base.BackgroundImage = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override ImageLayout BackgroundImageLayout
        {
            get { return base.BackgroundImageLayout; }
            set { base.BackgroundImageLayout = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new IButtonControl CancelButton
        {
            get { return base.CancelButton; }
            set { base.CancelButton = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool CausesValidation
        {
            get { return base.CausesValidation; }
            set { base.CausesValidation = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override ContextMenu ContextMenu
        {
            get { return base.ContextMenu; }
            set { base.ContextMenu = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override ContextMenuStrip ContextMenuStrip
        {
            get { return base.ContextMenuStrip; }
            set { base.ContextMenuStrip = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool ControlBox
        {
            get { return base.ControlBox; }
            set { base.ControlBox = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Cursor Cursor
        {
            get { return base.Cursor; }
            set { base.Cursor = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new ControlBindingsCollection DataBindings
        {
            get { return base.DataBindings; }
        }

        protected override Size DefaultMinimumSize
        {
            get { return new Size(0x177, 250); }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override DockStyle Dock
        {
            get { return base.Dock; }
            set { base.Dock = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool Enabled
        {
            get { return base.Enabled; }
            set { base.Enabled = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new FormBorderStyle FormBorderStyle
        {
            get { return base.FormBorderStyle; }
            set { base.FormBorderStyle = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool HelpButton
        {
            get { return base.HelpButton; }
            set { base.HelpButton = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public new Icon Icon
        {
            get { return base.Icon; }
            set { base.Icon = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public new ImeMode ImeMode
        {
            get { return base.ImeMode; }
            set { base.ImeMode = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public new bool IsMdiContainer
        {
            get { return base.IsMdiContainer; }
            set { base.IsMdiContainer = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool KeyPreview
        {
            get { return base.KeyPreview; }
            set { base.KeyPreview = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Point Location
        {
            get { return base.Location; }
            set { base.Location = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new MenuStrip MainMenuStrip
        {
            get { return base.MainMenuStrip; }
            set { base.MainMenuStrip = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Padding Margin
        {
            get { return base.Margin; }
            set { base.Margin = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool MaximizeBox
        {
            get { return base.MaximizeBox; }
            set { base.MaximizeBox = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Size MaximumSize
        {
            get { return base.MaximumSize; }
            set { base.MaximumSize = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new MainMenu Menu
        {
            get { return base.Menu; }
            set { base.Menu = value; }
        }


        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(false)]
        public new bool MinimizeBox
        {
            get { return base.MinimizeBox; }
            set { base.MinimizeBox = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Size MinimumSize
        {
            get { return base.MinimumSize; }
            set { base.MinimumSize = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new double Opacity
        {
            get { return base.Opacity; }
            set { base.Opacity = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Padding Padding
        {
            get { return base.Padding; }
            set { base.Padding = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override RightToLeft RightToLeft
        {
            get { return base.RightToLeft; }
            set { base.RightToLeft = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override bool RightToLeftLayout
        {
            get { return base.RightToLeftLayout; }
            set { base.RightToLeftLayout = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(false)]
        public new bool ShowIcon
        {
            get { return base.ShowIcon; }
            set { base.ShowIcon = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(false)]
        public new bool ShowInTaskbar
        {
            get { return base.ShowInTaskbar; }
            set { base.ShowInTaskbar = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Size Size
        {
            get { return base.Size; }
            set { base.Size = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DefaultValue(2)]
        public new SizeGripStyle SizeGripStyle
        {
            get { return base.SizeGripStyle; }
            set { base.SizeGripStyle = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new FormStartPosition StartPosition
        {
            get { return base.StartPosition; }
            set { base.StartPosition = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool TabStop
        {
            get { return base.TabStop; }
            set { base.TabStop = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new object Tag
        {
            get { return base.Tag; }
            set { base.Tag = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool TopMost
        {
            get { return base.TopMost; }
            set { base.TopMost = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Color TransparencyKey
        {
            get { return base.TransparencyKey; }
            set { base.TransparencyKey = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool UseWaitCursor
        {
            get { return base.UseWaitCursor; }
            set { base.UseWaitCursor = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new FormWindowState WindowState
        {
            get { return base.WindowState; }
            set { base.WindowState = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool Visible
        {
            get { return base.Visible; }
            set { base.Visible = value; }
        }

        #endregion
    }
}