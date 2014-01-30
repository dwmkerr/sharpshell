using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;

namespace FileDialogs
{
    public class SelectFolderDialog : FileDialog
    {
        #region Member Fields

        private string m_selectedPath;
        private bool m_pathMustExist = true;

        #endregion

        #region Construction

        public SelectFolderDialog()
            : base("Select Directory")
        {
            lookInLabel.Text = "Look &in:";
            okButton.Enabled = true;
            okButton.Text = "&Open";
            fileNameLabel.Text = "Folder &name:";
            fileTypeLabel.Visible = false;
            fileTypeComboBox.Visible = false;
            searchTheWebToolStripButton.Enabled = false;
            viewsToolStripSplitButton.Enabled = false;
            m_excludeFiles = true;
            base.Text = "Select Directory";
        }

        #endregion

        #region Methods

        protected override void OnFileOK(object sender, EventArgs e)
        {
            if (m_selectedPidls != null && m_selectedPidls.Length > 0)
            {
                NativeMethods.SHGFAO attribs = NativeMethods.SHGFAO.SFGAO_STORAGE;
                m_currentFolder.GetAttributesOf(1, m_selectedPidls, ref attribs);

                if ((attribs & NativeMethods.SHGFAO.SFGAO_STORAGE) == 0)
                {
                    ((NativeMethods.IShellBrowser)this).BrowseObject(m_selectedPidls[0], NativeMethods.SBSP_RELATIVE);
                    return;
                }

                m_selectedPath = GetDisplayName(m_currentFolder, m_selectedPidls[0], NativeMethods.SHGNO.SHGDN_FORPARSING);
                DialogResult = DialogResult.OK;
            }
            else
            {
                IntPtr[] pidls = new IntPtr[] { m_pidlAbsCurrent };
                NativeMethods.SHGFAO attribs = NativeMethods.SHGFAO.SFGAO_STORAGE;
                m_desktopFolder.GetAttributesOf(1, pidls, ref attribs);

                if ((attribs & NativeMethods.SHGFAO.SFGAO_STORAGE) == 0)
                    return;

                string currentPath =
                    GetDisplayName(m_desktopFolder, m_pidlAbsCurrent, NativeMethods.SHGNO.SHGDN_FORPARSING);

                string path = fileNameComboBox.Text;
                string trimmedPath = path.Trim();
                if (path != trimmedPath)
                {
                    m_ignoreFileNameChange = true;

                    path = trimmedPath;
                    fileNameComboBox.Text = trimmedPath;
                }

                if (string.IsNullOrEmpty(path))
                {
                    m_selectedPath = currentPath;
                    DialogResult = DialogResult.OK;
                    return;
                }

                bool isPathRooted = Path.IsPathRooted(path);
                if (!isPathRooted)
                    path = Path.Combine(currentPath, path);

                if (path.Contains(".."))
                    path = Path.GetFullPath(path);

                if (!CheckPathExists || Directory.Exists(path))
                {
                    m_selectedPath = path;
                    DialogResult = DialogResult.OK;
                    return;
                }
                
                string message = string.Format(@"The folder '{0}' isn't accessible. The folder may be located in an unavailable location, protected with a password, or the filename contains a / or \.", path);
                MessageBoxWithFocusRestore(message, MessageBoxButtons.OK, MessageBoxIcon.Information);

                fileNameComboBox.SelectAll();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the path selected by the user.
        /// </summary>
        /// <returns>The path of the folder first selected in the dialog box or the last folder selected by the user. The default is an empty string ("").</returns>
        [Description("The path of the folder first selected in the dialog or the last one selected by the user.")]
        [Category("Folder Browsing")]
        [Editor("System.Windows.Forms.Design.SelectedPathEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Browsable(true)]
        [DefaultValue("")]
        [Localizable(true)]
        public string SelectedPath
        {
            get { return m_selectedPath; }
            set
            {
                m_selectedPath = value ?? string.Empty;
                InitialDirectory = m_selectedPath;
            }
        }

        //[Browsable(false)]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public new bool CheckFileExists
        //{
        //    get { return base.CheckFileExists; }
        //    set { base.CheckFileExists = value; }
        //}

        /// <summary>
        /// Gets or sets a value indicating whether the dialog box displays a warning if the user specifies a path that does not exist.
        /// </summary>
        /// <value>true if the dialog box displays a warning when the user specifies a path that does not exist; otherwise, false. The default value is true.</value>
        [Description("Checks that the specified path exists before returning from the dialog.")]
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool CheckPathExists
        {
            get { return m_pathMustExist; }
            set { m_pathMustExist = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new string FileName
        {
            get { return base.FileName; }
            set { base.FileName = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new string Filter
        {
            get { return base.Filter; }
            set { base.Filter = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new int FilterIndex
        {
            get { return base.FilterIndex; }
            set { base.FilterIndex = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new string InitialDirectory
        {
            get { return base.InitialDirectory; }
            set { base.InitialDirectory = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new bool RestoreLastViewMode
        {
            get { return base.RestoreLastViewMode; }
            set { base.RestoreLastViewMode = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new FileDialogViewMode ViewMode
        {
            get { return base.ViewMode; }
            set { base.ViewMode = value; }
        }

        #endregion
    }
}
