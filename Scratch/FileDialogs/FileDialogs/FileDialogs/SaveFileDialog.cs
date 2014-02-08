using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FileDialogs
{
    /// <summary>
    /// Prompts the user to select a location for saving a file. This class cannot be inherited.
    /// </summary>
    public sealed class SaveFileDialog : FileDialog
    {
        #region Construction

        public SaveFileDialog()
            : base("Save File As")
        {
            lookInLabel.Text = "Save &in:";
            okButton.Enabled = false;
            okButton.Text = "&Save";
            Text = "Save File As";
        }

        #endregion

        #region Methods

        internal override bool PromptFileOverwrite(string fileName)
        {
            string text = string.Format(@"The file {0} already exists. Do you want to replace the existing file?", fileName);
            return MessageBoxWithFocusRestore(text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes;
        }

        internal override string AppendExtension(string fileName)
        {
            FileType fileType = (FileType)fileTypeComboBox.SelectedItem;
            if (fileType.IncludeAllFiles)
                return fileName;

            foreach (string extension in fileType.Extensions)
            {
                if (fileName.EndsWith(extension.Substring(1)))
                    return fileName;
            }

            // Append the first extension to the end of the file name
            return fileName + fileType.Extensions[0].Substring(1);
        }

        internal override bool PromptFileCreate()
        {
            if (fileNameComboBox.Text.IndexOfAny(new char[] {'*', '?'}) < 0)
                return true;

            string[] parts = fileNameComboBox.Text.Split(';');

            m_currentFilePattern = "";
            for (int i = 0; i < parts.Length; i++)
            {
                if (i > 0) m_currentFilePattern += "|";
                m_currentFilePattern += "(";

                parts[i] = parts[i].Trim();
                for (int j = 0; j < parts[i].Length; j++)
                {
                    char c = parts[i][j];
                    switch (c)
                    {
                        case '*':
                            m_currentFilePattern += ".*";
                            break;
                        case '?':
                            m_currentFilePattern += ".";
                            break;
                        case '\\':
                            if (j < parts[i].Length - 1)
                                m_currentFilePattern += Regex.Escape(parts[i][++j].ToString());
                            break;
                        default:
                            m_currentFilePattern += Regex.Escape(parts[i][j].ToString());
                            break;
                    }
                }
                m_currentFilePattern += ")";
            }

            int fileTypeIndex = 0;
            foreach (FileType fileType in fileTypeComboBox.Items)
            {
                bool found = false;
                for (int i = 0; i < parts.Length; i++)
                {
                    foreach (string extension in fileType.Extensions)
                    {
                        if (extension == parts[i])
                            found = true;
                        else if (found)
                            found = false;
                    }

                    if (!found)
                        break;
                }

                if (found)
                    fileTypeComboBox.SelectedIndex = fileTypeIndex;

                fileTypeIndex++;
            }

            fileNameComboBox.SelectAll();

            if (m_shellView != null)
                m_shellView.Refresh(); // Causes the ICommDlgBrowser::IncludeObject to be called

            return false;
        }

        protected override void RunDialog()
        {
            base.RunDialog();

            fileNameComboBox.Text = Path.GetFileName(FileName);
            SelectCorrespondingFileType();
        }

        protected override void OnSelectionChanged()
        {
            base.OnSelectionChanged();

            if (m_selectedPidls == null || m_selectedPidls.Length == 0)
            {
                if (okButton.Text != "&Save")
                    okButton.Text = "&Save";

                if (!fileNameComboBox.Focused && fileNameComboBox.Text.Trim() == string.Empty)
                {
                    m_ignoreFileNameChange = true;
                    fileNameComboBox.Text = FileName;
                }
            }
            else
            {
                IntPtr[] pidls = new IntPtr[] { m_selectedPidls[0] };
                NativeMethods.SHGFAO attribs = NativeMethods.SHGFAO.SFGAO_STREAM;
                m_currentFolder.GetAttributesOf(1, pidls, ref attribs);

                // If a folder is selected set ok button text to Open, otherwise Save
                if ((attribs & NativeMethods.SHGFAO.SFGAO_STREAM) == 0)
                {
                    if (okButton.Text != "&Open")
                        okButton.Text = "&Open";
                }
                else
                {
                    if (okButton.Text != "&Save")
                        okButton.Text = "&Save";

                    m_ignoreFileNameChange = true;
                    fileNameComboBox.Text = GetDisplayName(m_currentFolder, m_selectedPidls[0]);
                }
            }

            okButton.Enabled = ((m_selectedPidls != null && m_selectedPidls.Length > 0) || !string.IsNullOrEmpty(fileNameComboBox.Text));
            AcceptButton = okButton.Enabled ? okButton : cancelButton;
        }

        #endregion
    }
}
