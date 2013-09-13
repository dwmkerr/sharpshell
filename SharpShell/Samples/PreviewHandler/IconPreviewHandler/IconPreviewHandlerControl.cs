using System.Collections.Generic;
using System.Drawing;
using System.Drawing.IconLib;
using System.Linq;
using System.Windows.Forms;
using SharpShell.SharpPreviewHandler;

namespace IconPreviewHandler
{
    /// <summary>
    /// A user control that shows the icons in an icon file.
    /// </summary>
    public partial class IconPreviewHandlerControl : PreviewHandlerControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IconPreviewHandlerControl"/> class.
        /// </summary>
        public IconPreviewHandlerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Adds the icons to the control.
        /// </summary>
        private void AddIconsToControl()
        {
            BackColor = Color.White;

            DoubleBuffered = true;

            //  Go through each icon, keep track of y pos.
            int yPos = 12;
            foreach (var iconImage in iconImages)
            {
                //  Create the description.
                var descriptionLabel = new Label
                                           {
                                               Location = new Point(12, yPos),
                                               Text = string.Format("{0}x{1} - {2}",
                                                                    iconImage.Size.Width, iconImage.Size.Height, iconImage.PixelFormat),
                                               AutoSize = true,
                                               BackColor = Color.White
                                           };
                yPos += 20;

                //  Create the picture box.
                var pictureBox = new PictureBox
                                     {
                                         Location = new Point(12, yPos),
                                         Image = iconImage.Icon.ToBitmap(),
                                         Width = iconImage.Size.Width,
                                         Height = iconImage.Size.Height
                                     };
                yPos += iconImage.Size.Height + 20;
                panelImages.Controls.Add(descriptionLabel);
                panelImages.Controls.Add(pictureBox);

                //  Keep track of generated labels.
                generatedLabels.Add(descriptionLabel);
            }
        }

        /// <summary>
        /// Does the preview.
        /// </summary>
        /// <param name="selectedFilePath">The selected file path.</param>
public void DoPreview(string selectedFilePath)
{
    //  Load the icons.
    try
    {
        var multiIcon = new MultiIcon();
        multiIcon.Load(selectedFilePath);

        //  Add the icon images.
        foreach (var iconImage in multiIcon.SelectMany(singleIcon => singleIcon))
            iconImages.Add(iconImage);

        //  Add the icons to the control.
        AddIconsToControl();
    }
    catch
    {
        //  Maybe we could show something to the user in the preview
        //  window, but for now we'll just ignore any exceptions.
    }
}

        /// <summary>
        /// Sets the color of the background, if possible, to coordinate with the windows
        /// color scheme.
        /// </summary>
        /// <param name="color">The color.</param>
        protected override void SetVisualsBackgroundColor(Color color)
        {
            //  Set the background color.
            BackColor = color;
            generatedLabels.ForEach(gl => gl.BackColor = color);
        }

        /// <summary>
        /// Sets the color of the text, if possible, to coordinate with the windows
        /// color scheme.
        /// </summary>
        /// <param name="color">The color.</param>
        protected override void SetVisualsTextColor(Color color)
        {
            //  Set the foreground color.
            generatedLabels.ForEach(gl => gl.ForeColor = color);
        }

        /// <summary>
        /// Sets the font, if possible, to coordinate with the windows
        /// color scheme.
        /// </summary>
        /// <param name="font">The font.</param>
        protected override void SetVisualsFont(Font font)
        {
            //  Set the font.
            generatedLabels.ForEach(gl => gl.Font = font);
        }

        /// <summary>
        /// The set of generated labels.
        /// </summary>
        private readonly List<Label> generatedLabels = new List<Label>();

        /// <summary>
        /// The set of icon images.
        /// </summary>
        private readonly List<IconImage> iconImages = new List<IconImage>();
    }
}
