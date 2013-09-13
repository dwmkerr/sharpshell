using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using SharpShell.Attributes;
using SharpShell.SharpPreviewHandler;

namespace IconPreviewHandler
{
    /// <summary>
    /// The IconPreviewHandler is a preview handler that shows the icons contained
    /// in an ico file.
    /// </summary>
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".ico")]
    [DisplayName("Icon Preview Handler")]
    [Guid("A643C50D-4206-4121-A895-9EA5C919557A")]
    public class IconPreviewHandler : SharpPreviewHandler 
    {
        /// <summary>
        /// DoPreview must create the preview handler user interface and initialize it with data
        /// provided by the shell.
        /// </summary>
        /// <returns>
        /// The preview handler user interface.
        /// </returns>
        protected override PreviewHandlerControl DoPreview()
        {
            //  Create the handler control.
            var handler = new IconPreviewHandlerControl();

            //  Do we have a file path? If so, we can do a preview.
            if(!string.IsNullOrEmpty(SelectedFilePath))
                handler.DoPreview(SelectedFilePath);

            //  Return the handler control.
            return handler;
        }
    }
}
