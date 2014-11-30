using System.Runtime.InteropServices;   
using SharpShell.Attributes;
using SharpShell.SharpPreviewHandler;

namespace AbcPreviewHandler
{
    /// <summary>
    /// The Abc Preview Handler is a preview handler that shows a simple
    /// coloured background for ABC files.
    /// </summary>
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".abc")]
    [DisplayName("Abc Preview Handler")]
    [Guid("B86199F1-13D0-4711-AFE6-08C1E4C58905")]
    [PreviewHandler(DisableLowILProcessIsolation = false)]
    public class AbcPreviewHandler : SharpPreviewHandler 
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
            var handler = new AbcPreviewHandlerControl();

            //  Do we have a file path? If so, we can do a preview.
            if(!string.IsNullOrEmpty(SelectedFilePath))
                handler.DoPreview(SelectedFilePath);

            //  Return the handler control.
            return handler;
        }
    }
}
