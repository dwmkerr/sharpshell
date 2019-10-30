using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace TxtThumbnailHandler.Renderer
{
    internal class TextThumbnailRenderer : IDisposable
    {
        public TextThumbnailRenderer()
        {
            _textFont = new Font("Courier New", 12f);
            _textBrush = new SolidBrush(Color.Black);
        }

        /// <summary>
        /// Creates the thumbnail for text, using the provided preview lines.
        /// </summary>
        /// <param name="previewLines">The preview lines.</param>
        /// <param name="width">The width.</param>
        /// <returns>
        /// A thumbnail for the text.
        /// </returns>
        public Bitmap CreateThumbnailForText(IEnumerable<string> previewLines, uint width)
        {
            //  Create the bitmap dimensions.
            var thumbnailSize = new Size((int)width, (int)width);

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
                var yOffset = width * 0.3f;
                var yLimit = width - yOffset;

                graphics.Clip = new Region(new RectangleF(xOffset, yOffset, thumbnailSize.Width - (xOffset * 2), thumbnailSize.Height - width * .1f));

                //  Render each line of text.
                foreach (var line in previewLines)
                {
                    graphics.DrawString(line, _textFont, _textBrush, xOffset, yOffset);
                    yOffset += 14f;
                    if (yOffset + 14f > yLimit)
                        break;
                }
            }

            //  Return the bitmap.
            return bitmap;
        }

        public void Dispose()
        {
            _textFont?.Dispose();
            _textBrush?.Dispose();
        }

        private readonly Font _textFont;
        private readonly Brush _textBrush;
    }
}