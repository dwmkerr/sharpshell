using System;
using System.IO;
using System.Windows.Forms;

namespace FileDialogs
{
    internal partial class NewFolderDialog : Form
    {
        #region Member Fields

        private static readonly char[] InvalidFileNameChars = Path.GetInvalidFileNameChars();

        private bool m_created = false;
        private string m_path = string.Empty;

        #endregion

        #region Construction

        public NewFolderDialog(string path)
        {
            InitializeComponent();

            m_path = path;
        }

        #endregion

        #region Methods

        private void okButton_Click(object sender, EventArgs e)
        {
            string folderName = nameTextBox.Text.Trim();
            m_path = Path.Combine(m_path, folderName);

            // Check that the folder name isn't empty
            if (string.IsNullOrEmpty(folderName))
            {
                string message = "Type a name for the new folder.";
                if (MessageBox.Show(this, message, Text, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    DialogResult = DialogResult.Cancel;

                return;
            }

            // Check that the folder name doesn't contain any invalida characters
            if (folderName.IndexOfAny(InvalidFileNameChars) >= 0)
            {
                string message = string.Format("The name '{0}' is not a valid folder name because it contains characters that cannot be used in a folder name. Type another name that does not include the characters <>|*?/.", folderName);
                if (MessageBox.Show(this, message, Text, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    DialogResult = DialogResult.Cancel;

                return;
            }

            // Check that the folder doesn't already exist
            if (Directory.Exists(m_path))
            {
                string message = string.Format("A folder named '{0}' already exists. Type another name for the folder.", folderName);
                if (MessageBox.Show(this, message, Text, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    DialogResult = DialogResult.Cancel;

                return;
            }

            try
            {
                m_created = true;
                Directory.CreateDirectory(m_path);
            }
            catch
            {
                m_created = false;
                string message = string.Format("The folder '{0}' could not be created. You may not have access privileges to create a new folder in this location.", folderName);
                if (MessageBox.Show(this, message, Text, MessageBoxButtons.OK) == DialogResult.OK)
                    DialogResult = DialogResult.Cancel;

                return;
            }

            DialogResult = DialogResult.OK;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating weather a folder has been created.
        /// </summary>
        public bool FolderCreated
        {
            get { return m_created; }
            set { m_created = value; }
        }

        /// <summary>
        /// Gets or sets the path of a newly created folder.
        /// </summary>
        public string FolderPath
        {
            get { return m_path; }
            set { m_path = value; }
        }

        #endregion
    }
}