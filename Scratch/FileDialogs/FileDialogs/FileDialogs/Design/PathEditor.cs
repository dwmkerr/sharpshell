using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace FileDialogs.Design
{
    /// <summary>
    /// The PathEditor is used as a replacement for the FolderNameEditor.
    /// In this implementation no special folders can be selected.
    /// </summary>
    internal class PathEditor : UITypeEditor
    {
        #region Member Fields

        private FolderBrowserDialog folderBrowserDialog;

        #endregion

        #region Methods

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (folderBrowserDialog == null)
            {
                folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.ShowNewFolderButton = false;
            }

            folderBrowserDialog.SelectedPath = value.ToString();

            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                return value;

            return folderBrowserDialog.SelectedPath;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        #endregion
    }
}
