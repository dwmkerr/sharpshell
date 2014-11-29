using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SharpShell.Diagnostics;
using SharpShell.SharpPreviewHandler;

namespace AbcPreviewHandler
{
    /// <summary>
    /// A user control that shows the icons in an icon file.
    /// </summary>
    public partial class AbcPreviewHandlerControl : PreviewHandlerControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbcPreviewHandlerControl"/> class.
        /// </summary>
        public AbcPreviewHandlerControl()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Does the preview.
        /// </summary>
        /// <param name="selectedFilePath">The selected file path.</param>
        public void DoPreview(string selectedFilePath)
        {

        }

        /// <summary>
        /// Sets the color of the background, if possible, to coordinate with the windows
        /// color scheme.
        /// </summary>
        /// <param name="color">The color.</param>
        protected override void SetVisualsBackgroundColor(Color color)
        {
            //  Set the background color.
            //BackColor = color;
        }
    }
}
