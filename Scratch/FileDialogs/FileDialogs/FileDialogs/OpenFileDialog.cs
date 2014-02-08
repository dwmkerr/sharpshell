using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;

namespace FileDialogs
{
    public class OpenFileDialog : FileDialog
    {
        #region Construction

        public OpenFileDialog()
            : base("Open File")
        {
            lookInLabel.Text = "Look &in:";
            okButton.Enabled = false;
            okButton.Text = "&Open";
            base.Text = "Open File";
        }

        #endregion

        #region Methods

        protected override void RunDialog()
        {
            base.RunDialog();

            fileNameComboBox.Text = Path.GetFileName(FileName);
            SelectCorrespondingFileType();
        }

        internal override bool PromptFileCreate()
        {
            string[] parts = fileNameComboBox.Text.Split(';');

            string prevExtension = null;
            bool sameExtension = true;

            m_currentFilePattern = "";
            for (int i = 0; i < parts.Length; i++)
            {
                string extension = "";
                int startIndex = parts[i].LastIndexOf('.') + 1;
                
                if (startIndex >= 0)
                    extension = parts[i].Substring(startIndex);

                if (prevExtension != null && prevExtension != extension)
                    sameExtension = false;
                prevExtension = extension;

                if (i > 0) m_currentFilePattern += "|";
                m_currentFilePattern += "(";
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
                if (sameExtension)
                {
                    foreach (string extension in fileType.Extensions)
                    {
                        if (extension == "*." + prevExtension)
                            found = true;
                        else if (found)
                            found = false;
                    }
                }
                else
                {
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
                }

                if (found)
                {
                    fileTypeComboBox.SelectedIndex = fileTypeIndex;
                    break;
                }

                fileTypeIndex++;
            }

            fileNameComboBox.SelectAll();

            if (m_shellView != null)
                m_shellView.Refresh(); // Causes the ICommDlgBrowser::IncludeObject to be called

            return false;
        }

        protected override void OnSelectionChanged()
        {
            base.OnSelectionChanged();

            okButton.Enabled = ((m_selectedPidls != null && m_selectedPidls.Length > 0) || !string.IsNullOrEmpty(fileNameComboBox.Text));
            AcceptButton = okButton.Enabled ? okButton : cancelButton;
        }

        #endregion

        #region Properties

        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Controls whether multiple files can be selected in the dialog.")]
        public bool Multiselect
        {
            get { return !GetFlag(NativeMethods.FOLDERFLAGS.FWF_SINGLESEL); }
            set { SetFlag(NativeMethods.FOLDERFLAGS.FWF_SINGLESEL, !value); }
        }

        #endregion
    }
}
