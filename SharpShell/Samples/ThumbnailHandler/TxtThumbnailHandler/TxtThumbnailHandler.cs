using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SharpShell.Attributes;
using SharpShell.SharpThumbnailHandler;

namespace TxtThumbnailHandler
{
    /// <summary>
    /// The TxtThumbnailHandler is a ThumbnailHandler for text files.
    /// </summary>
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".txt")]
    public class TxtThumbnailHandler : SharpThumbnailHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TxtThumbnailHandler"/> class.
        /// </summary>
        public TxtThumbnailHandler()
        {
            //  Create our lazy objects.
            lazyThumbnailFont = new Lazy<Font>(() => new Font("Courier New", 12f));
            lazyThumbnailTextBrush = new Lazy<Brush>(() => new SolidBrush(Color.Black));
        }

        /// <summary>
        /// Gets the thumbnail image.
        /// </summary>
        /// <param name="width">The width of the image that should be returned.</param>
        /// <returns>
        /// The image for the thumbnail.
        /// </returns>
        protected override Bitmap GetThumbnailImage(uint width)
        {
            //  Attempt to open the stream with a reader.
            try
            {
                using(var reader = new StreamReader(SelectedItemStream))
                {
                    //  Read up to ten lines of text.
                    var previewLines = new List<string>();
                    for (int i = 0; i < 10; i++)
                    {
                        var line = reader.ReadLine();
                        if (line == null)
                            break;
                        previewLines.Add(line);
                    }

                    //  Now return a preview of the lines.
                    return CreateThumbnailForText(previewLines, width);
                }
            }
            catch (Exception exception)
            {
                //  DebugLog the exception and return null for failure.
                LogError("An exception occured opening the text file.", exception);
                return null;
            }
        }

        /// <summary>
        /// Creates the thumbnail for text, using the provided preview lines.
        /// </summary>
        /// <param name="previewLines">The preview lines.</param>
        /// <param name="width">The width.</param>
        /// <returns>
        /// A thumbnail for the text.
        /// </returns>
        private Bitmap CreateThumbnailForText(IEnumerable<string> previewLines, uint width)
        {
            //  Create the bitmap dimensions.
            var thumbnailSize = new Size((int) width, (int) width);

            //  Create the bitmap.
            var bitmap = new Bitmap(thumbnailSize.Width, thumbnailSize.Height, PixelFormat.Format32bppArgb);

            //  Create a graphics object to render to the bitmap.
            using (var graphics = Graphics.FromImage(bitmap))
            {
                //  Set the rendering up for anti-aliasing.
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                //  Draw the page background.
                graphics.DrawImage(Properties.Resources.Page, 0, 0, thumbnailSize.Width, thumbnailSize.Height);
                
                //  Create offsets for the text.
                var xOffset = width * 0.2f;
                var yOffset = width * 0.3f ;
                var yLimit = width - yOffset;

                graphics.Clip = new Region(new RectangleF(xOffset, yOffset, thumbnailSize.Width - (xOffset * 2), thumbnailSize.Height - width*.1f));

                //  Render each line of text.
                foreach (var line in previewLines)
                {
                    graphics.DrawString(line, lazyThumbnailFont.Value, lazyThumbnailTextBrush.Value, xOffset, yOffset);
                    yOffset += 14f;
                    if (yOffset + 14f > yLimit)
                        break;
                }
            }

            //  Return the bitmap.
            return bitmap;
        }

        /// <summary>
        /// The lazy thumbnail font.
        /// </summary>
        private readonly Lazy<Font> lazyThumbnailFont;
        /// <summary>
        /// The lazy thumbnail text brush.
        /// </summary>
        private readonly Lazy<Brush> lazyThumbnailTextBrush;
    }
}
