using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using SharpShell.Attributes;
using SharpShell.Interop;

namespace SharpShell.SharpThumbnailHandler
{
    /// <summary>
    /// The SharpIconHandler is the base class for SharpShell servers that provide
    /// custom Thumbnail Image Handlers.
    /// </summary>
    [ServerType(ServerType.ShellThumbnailHandler)]
    public abstract class SharpThumbnailHandler : InitializeWithStreamServer, IThumbnailProvider
    {
        /// <summary>
        /// Gets a thumbnail image and alpha type.
        /// </summary>
        /// <param name="cx">The maximum thumbnail size, in pixels. The Shell draws the returned bitmap at this size or smaller. The returned bitmap should fit into a square of width and height cx, though it does not need to be a square image. The Shell scales the bitmap to render at lower sizes. For example, if the image has a 6:4 aspect ratio, then the returned bitmap should also have a 6:4 aspect ratio.</param>
        /// <param name="phbmp">When this method returns, contains a pointer to the thumbnail image handle. The image must be a DIB section and 32 bits per pixel. The Shell scales down the bitmap if its width or height is larger than the size specified by cx. The Shell always respects the aspect ratio and never scales a bitmap larger than its original size.</param>
        /// <param name="pdwAlpha">When this method returns, contains a pointer to one of the following values from the WTS_ALPHATYPE enumeration.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        int IThumbnailProvider.GetThumbnail(uint cx, out IntPtr phbmp, out WTS_ALPHATYPE pdwAlpha)
        {
            //  DebugLog this key event.
            Log($"GetThumbnail for item stream, width {cx}.");

            //  Set the out variables to default values.
            phbmp = IntPtr.Zero;
            pdwAlpha = WTS_ALPHATYPE.WTSAT_UNKNOWN;
            Bitmap thumbnailImage;

            try
            {
                //  Get the thumbnail image.
                thumbnailImage = GetThumbnailImage(cx);
            }
            catch(Exception exception)
            {
                //  DebugLog the exception and return failure.
                LogError("An exception occured when getting the thumbnail image.", exception);
                return WinError.E_FAIL;
            }

            //  If we couldn't get an image, return failure.
            if(thumbnailImage == null)
            {
                //  DebugLog a warning return failure.
                Log("The internal GetThumbnail function failed to return a valid thumbnail.");
                return WinError.E_FAIL;
            }

            //  Now we can set the image.
            phbmp = thumbnailImage.GetHbitmap();
            pdwAlpha = WTS_ALPHATYPE.WTSAT_ARGB;
          
            //  Return success.
            return WinError.S_OK;
        }

        /// <summary>
        /// Gets the thumbnail image for the currently selected item (SelectedItemStream).
        /// </summary>
        /// <param name="width">The width of the image that should be returned.</param>
        /// <returns>The image for the thumbnail.</returns>
        protected abstract Bitmap GetThumbnailImage(uint width);
    }
}