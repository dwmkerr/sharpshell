using System.Windows.Forms;
using System.Drawing;

namespace SharpShell.SharpPreviewHandler
{
    /// <summary>
    /// Base class for preview handler controls.
    /// </summary>
    public class PreviewHandlerControl : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewHandlerControl"/> class.
        /// </summary>
        protected PreviewHandlerControl()
        {
            FormBorderStyle = FormBorderStyle.None;
        }

        /// <summary>
        /// Sets the color of the background, if possible, to coordinate with the windows
        /// color scheme.
        /// </summary>
        /// <param name="color">The color.</param>
        protected internal virtual void SetVisualsBackgroundColor(Color color){}

        /// <summary>
        /// Sets the color of the text, if possible, to coordinate with the windows
        /// color scheme.
        /// </summary>
        /// <param name="color">The color.</param>
        protected internal virtual void SetVisualsTextColor(Color color){}

        /// <summary>
        /// Sets the font, if possible, to coordinate with the windows
        /// color scheme.
        /// </summary>
        /// <param name="font">The font.</param>
        protected internal virtual void SetVisualsFont(Font font){}
    }
}
