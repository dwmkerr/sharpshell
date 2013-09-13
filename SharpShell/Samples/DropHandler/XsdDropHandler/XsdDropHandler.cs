using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpShell.Attributes;
using SharpShell.SharpDropHandler;

namespace XsdDropHandler
{
    /// <summary>
    /// The XSD Drop Handler is a Shell Drop Handler Extension that allows xml files
    /// to be validated by dropping them on an XSD.
    /// </summary>
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".xsd")]
    public class XsdDropHandler : SharpDropHandler
    {
        /// <summary>
        /// Checks what operations are available for dragging onto the target with the drag files.
        /// </summary>
        /// <param name="dragEventArgs">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        protected override void DragEnter(DragEventArgs dragEventArgs)
        {
            //  Check the drag files - if they're all XML, we can validate them against the XSD.
            dragEventArgs.Effect =
                DragItems.All(di => string.Compare(Path.GetExtension(di), ".xml", StringComparison.InvariantCultureIgnoreCase) == 0)
                    ? DragDropEffects.Link : DragDropEffects.None;
        }

        /// <summary>
        /// Performs the drop.
        /// </summary>
        /// <param name="dragEventArgs">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        protected override void Drop(DragEventArgs dragEventArgs)
        {
            //  Create the validator output form.
            var validatorOutputForm = new ValidationOutputForm {XsdFilePath = SelectedItemPath, XmlFilePaths = DragItems};
            validatorOutputForm.ShowDialog();
        }
    }
}
