using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace XsdDropHandler
{
    /// <summary>
    /// The Validaton Output Form.
    /// </summary>
    public partial class ValidationOutputForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationOutputForm"/> class.
        /// </summary>
        public ValidationOutputForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the ValidationOutputForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ValidationOutputForm_Load(object sender, EventArgs e)
        {
            //  Create the XSD validator.
            var validator = new XsdValidator();
            validator.Validate(XsdFilePath, XmlFilePaths);

            //  Lock the list view for update.
            listViewXSDValidation.BeginUpdate();

            //  Go through each file path.
            foreach (var filePath in validator.ValidationMessages.Keys)
            {
                //  Get the name.
                string fileName;
                try
                {
                    fileName = Path.GetFileName(filePath);
                }
                catch
                {
                    fileName = filePath;
                }

                //  Go through the messages, adding each to the list view.
                foreach (var message in validator.ValidationMessages[filePath])
                    AddValidationMessageToListView(fileName, message);
            }

            //  Release the list view for update.
            listViewXSDValidation.EndUpdate();
        }

        /// <summary>
        /// Adds the validation message to the list view.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="validationMessage">The validation message.</param>
        private void AddValidationMessageToListView(string fileName, ValidationMessage validationMessage)
        {
            //  Create the image list index.
            var imageIndex = 0;
            switch (validationMessage.ValidationType)
            {
                case ValidationType.Success:
                    imageIndex = ImageList_Index_Success;
                    break;
                case ValidationType.Warning:
                    imageIndex = ImageList_Index_Warning;
                    break;
                case ValidationType.Error:
                    imageIndex = ImageList_Index_Error;
                    break;
            }

            //  Add the list view item with the appropriate icon index.
            listViewXSDValidation.Items.Add(new ListViewItem(new[] { fileName, validationMessage.Message },
                imageIndex));
        }

        /// <summary>
        /// Success image index.
        /// </summary>
        private const int ImageList_Index_Success = 0;

        /// <summary>
        /// Warning image index.
        /// </summary>
        private const int ImageList_Index_Warning = 1;

        /// <summary>
        /// Error image index.
        /// </summary>
        private const int ImageList_Index_Error = 2;

        /// <summary>
        /// Gets or sets the XSD file path.
        /// </summary>
        /// <value>
        /// The XSD file path.
        /// </value>
        public string XsdFilePath { get; set; }

        /// <summary>
        /// Gets or sets the XML file paths.
        /// </summary>
        /// <value>
        /// The XML file paths.
        /// </value>
        public IEnumerable<string> XmlFilePaths { get; set; }
    }
}
