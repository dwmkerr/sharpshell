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
using TxtThumbnailHandler.Renderer;

namespace TxtThumbnailHandler
{
    /// <summary>
    /// The HtmlFileThumbnailHandler is a SharpFileThumbnailHandler for text files.
    ///
    /// Note that the SharpFileThumbnailHandler is less performant than the <see cref="SharpThumbnailHandler"/>,
    /// which uses streams for its implementation, but may be required if you need the full file path.
    /// </summary>
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".txt")]
    public class HtmlFileThumbnailHandler : SharpFileThumbnailHandler, IDisposable
    {
        public HtmlFileThumbnailHandler()
        {
            _renderer = new TextThumbnailRenderer();
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
            Log($"Creating thumbnail for '{Path.GetFileName(SelectedItemPath)}'");

            //  Grab up to three lines of HTML.

            //  Attempt to open the stream with a reader.
            try
            {
                using(var reader = new StreamReader(SelectedItemPath))
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
                    return _renderer.CreateThumbnailForText(previewLines, width);
                }
            }
            catch (Exception exception)
            {
                //  DebugLog the exception and return null for failure.
                LogError("An exception occured opening the text file.", exception);
                return null;
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _renderer?.Dispose();
        }

        /// <summary>The renderer used to create the text.</summary>
        private readonly TextThumbnailRenderer _renderer;
    }
}
