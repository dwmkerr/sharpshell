using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Exposes methods that manipulate and interact with image lists.
    /// </summary>
    [ComImport]
    [Guid("46EB5926-582E-4017-9FDF-E8998DAA0950")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IImageList
    {
        /// <summary>
        /// Adds an image or images to an image list.
        /// </summary>
        /// <param name="hbmImage">A handle to the bitmap that contains the image or images. The number of images is inferred from the width of the bitmap.</param>
        /// <param name="hbmMask">A handle to the bitmap that contains the mask. If no mask is used with the image list, this parameter is ignored.</param>
        /// <param name="pi">When this method returns, contains a pointer to the index of the first new image. If the method fails to successfully add the new image, this value is -1.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int Add(IntPtr hbmImage, IntPtr hbmMask, ref int pi);

        /// <summary>
        /// Replaces an image with an icon or cursor.
        /// </summary>
        /// <param name="i">A value of type int that contains the index of the image to replace. If i is -1, the function adds the image to the end of the list.</param>
        /// <param name="hicon">A handle to the icon or cursor that contains the bitmap and mask for the new image.</param>
        /// <param name="pi">A pointer to an int that will contain the index of the image on return if successful, or -1 otherwise.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int ReplaceIcon(int i, IntPtr hicon, ref int pi);

        /// <summary>
        /// Adds a specified image to the list of images used as overlay masks. An image list can have up to four overlay masks in Common Controls version 4.70 and earlier, and up to 15 in version 4.71 or later. The method assigns an overlay mask index to the specified image.
        /// </summary>
        /// <param name="iImage">A value of type int that contains the zero-based index of an image in the image list. This index identifies the image to use as an overlay mask.</param>
        /// <param name="iOverlay">A value of type int that contains the one-based index of the overlay mask.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int SetOverlayImage(int iImage, int iOverlay);

        /// <summary>
        /// Replaces an image in an image list with a new image.
        /// </summary>
        /// <param name="i">A value of type int that contains the index of the image to replace.</param>
        /// <param name="hbmImage">A handle to the bitmap that contains the image.</param>
        /// <param name="hbmMask">A handle to the bitmap that contains the mask. If no mask is used with the image list, this parameter is ignored.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int Replace(int i, IntPtr hbmImage, IntPtr hbmMask);

        /// <summary>
        /// Adds an image or images to an image list, generating a mask from the specified bitmap.
        /// </summary>
        /// <param name="hbmImage">A handle to the bitmap that contains one or more images. The number of images is inferred from the width of the bitmap.</param>
        /// <param name="crMask">The color used to generate the mask. Each pixel of this color in the specified bitmap is changed to black, and the corresponding bit in the mask is set to 1.</param>
        /// <param name="pi">A pointer to an int that contains the index of the first new image when it returns, if successful, or -1 otherwise.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int AddMasked(IntPtr hbmImage, int crMask, ref int pi);

        /// <summary>
        /// Draws an image list item in the specified device context.
        /// </summary>
        /// <param name="pimldp">A pointer to an IMAGELISTDRAWPARAMS structure that contains the drawing parameters.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int Draw(
            ref IMAGELISTDRAWPARAMS pimldp);

        /// <summary>
        /// Removes an image from an image list.
        /// </summary>
        /// <param name="i">A value of type int that contains the index of the image to remove. If this parameter is -1, the method removes all images.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int Remove(int i);

        /// <summary>
        /// Creates an icon from an image and a mask in an image list.
        /// </summary>
        /// <param name="i">A value of type int that contains the index of the image.</param>
        /// <param name="flags">A combination of flags that specify the drawing style. For a list of values, see IImageList::Draw.</param>
        /// <param name="picon">A pointer to an int that contains the handle to the icon if successful, or NULL if otherwise.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int GetIcon(int i, int flags, ref IntPtr picon);

        /// <summary>
        /// Gets information about an image.
        /// </summary>
        /// <param name="i">A value of type int that contains the index of the image.</param>
        /// <param name="pImageInfo">A pointer to an IMAGEINFO structure that receives information about the image. The information in this structure can directly manipulate the bitmaps of the image.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int GetImageInfo(int i, ref IMAGEINFO pImageInfo);

        /// <summary>
        /// Copies images from a given image list.
        /// </summary>
        /// <param name="iDst">A value of type int that contains the zero-based index of the destination image for the copy operation.</param>
        /// <param name="punkSrc">A pointer to the IUnknown interface for the source image list.</param>
        /// <param name="iSrc">A value of type int that contains the zero-based index of the source image for the copy operation.</param>
        /// <param name="uFlags">A value that specifies the type of copy operation to be made.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int Copy(int iDst, IImageList punkSrc, int iSrc, int uFlags);

        /// <summary>
        /// Creates a new image by combining two existing images. This method also creates a new image list in which to store the image.
        /// </summary>
        /// <param name="i1">A value of type int that contains the index of the first existing image.</param>
        /// <param name="punk2">A pointer to the IUnknown interface of the image list that contains the second image.</param>
        /// <param name="i2">A value of type int that contains the index of the second existing image.</param>
        /// <param name="dx">A value of type int that contains the x-component of the offset of the second image relative to the first image.</param>
        /// <param name="dy">A value of type int that contains the y-component of the offset of the second image relative to the first image.</param>
        /// <param name="riid">An IID of the interface for the new image list.</param>
        /// <param name="ppv">A raw pointer to the interface for the new image list.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int Merge(int i1, IImageList punk2, int i2, int dx, int dy, ref Guid riid, ref IntPtr ppv);

        /// <summary>
        /// Clones an existing image list.
        /// </summary>
        /// <param name="riid">An IID for the new image list.</param>
        /// <param name="ppv">The address of a pointer to the interface for the new image list.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int Clone(ref Guid riid, ref IntPtr ppv);

        /// <summary>
        /// Gets an image's bounding rectangle.
        /// </summary>
        /// <param name="i">A value of type int that contains the index of the image.</param>
        /// <param name="prc">A pointer to a RECT that contains the bounding rectangle when the method returns.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int GetImageRect(int i, ref RECT prc);

        /// <summary>
        /// Gets the dimensions of images in an image list. All images in an image list have the same dimensions.
        /// </summary>
        /// <param name="cx">A pointer to an int that receives the width, in pixels, of each image.</param>
        /// <param name="cy">A pointer to an int that receives the height, in pixels, of each image.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int GetIconSize(ref int cx, ref int cy);

        /// <summary>
        /// Sets the dimensions of images in an image list and removes all images from the list.
        /// </summary>
        /// <param name="cx">A value of type int that contains the width, in pixels, of the images in the image list. All images in an image list have the same dimensions.</param>
        /// <param name="cy">A value of type int that contains the height, in pixels, of the images in the image list. All images in an image list have the same dimensions.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int SetIconSize(int cx, int cy);

        /// <summary>
        /// Gets the number of images in an image list.
        /// </summary>
        /// <param name="pi">A pointer to an int that contains the number of images when the method returns.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int GetImageCount(ref int pi);

        /// <summary>
        /// Resizes an existing image list.
        /// </summary>
        /// <param name="uNewCount">A value that specifies the new size of the image list.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int SetImageCount(int uNewCount);

        /// <summary>
        /// Sets the background color for an image list. This method only functions if you add an icon to the image list or use the IImageList::AddMasked method to add a black and white bitmap. Without a mask, the entire image draws, and the background color is not visible.
        /// </summary>
        /// <param name="clrBk">The background color to set. If this parameter is set to CLR_NONE, then images draw transparently using the mask.</param>
        /// <param name="pclr">A pointer to a COLORREF that contains the previous background color on return if successful, or CLR_NONE otherwise.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int SetBkColor(int clrBk, ref int pclr);

        /// <summary>
        /// Gets the current background color for an image list.
        /// </summary>
        /// <param name="pclr">A pointer to a COLORREF that contains the background color when the method returns.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int GetBkColor(ref int pclr);

        /// <summary>
        /// Begins dragging an image.
        /// </summary>
        /// <param name="iTrack">A value of type int that contains the index of the image to drag.</param>
        /// <param name="dxHotspot">A value of type int that contains the x-component of the drag position relative to the upper-left corner of the image.</param>
        /// <param name="dyHotspot">A value of type int that contains the y-component of the drag position relative to the upper-left corner of the image.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int BeginDrag(int iTrack, int dxHotspot, int dyHotspot);

        /// <summary>
        /// Ends a drag operation.
        /// </summary>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int EndDrag();

        /// <summary>
        /// Locks updates to the specified window during a drag operation and displays the drag image at the specified position within the window.
        /// </summary>
        /// <param name="hwndLock">A handle to the window that owns the drag image.</param>
        /// <param name="x">The x-coordinate at which to display the drag image. The coordinate is relative to the upper-left corner of the window, not the client area.</param>
        /// <param name="y">The y-coordinate at which to display the drag image. The coordinate is relative to the upper-left corner of the window, not the client area.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int DragEnter(IntPtr hwndLock, int x, int y);

        /// <summary>
        /// Unlocks the specified window and hides the drag image, which enables the window to update.
        /// </summary>
        /// <param name="hwndLock">A handle to the window that contains the drag image.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int DragLeave(IntPtr hwndLock);

        /// <summary>
        /// Moves the image that is being dragged during a drag-and-drop operation. This function is typically called in response to a WM_MOUSEMOVE message.
        /// </summary>
        /// <param name="x">A value of type int that contains the x-coordinate where the drag image appears. The coordinate is relative to the upper-left corner of the window, not the client area.</param>
        /// <param name="y">A value of type int that contains the y-coordinate where the drag image appears. The coordinate is relative to the upper-left corner of the window, not the client area.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int DragMove(int x, int y);

        /// <summary>
        /// Creates a new drag image by combining the specified image, which is typically a mouse cursor image, with the current drag image.
        /// </summary>
        /// <param name="punk">A pointer to the IUnknown interface that accesses the image list interface, which contains the new image to combine with the drag image.</param>
        /// <param name="iDrag">A value of type int that contains the index of the new image to combine with the drag image.</param>
        /// <param name="dxHotspot">A value of type int that contains the x-component of the hot spot within the new image.</param>
        /// <param name="dyHotspot">A value of type int that contains the x-component of the hot spot within the new image.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int SetDragCursorImage(ref IImageList punk, int iDrag, int dxHotspot, int dyHotspot);

        /// <summary>
        /// Shows or hides the image being dragged.
        /// </summary>
        /// <param name="fShow">A value that specifies whether to show or hide the image being dragged. Specify TRUE to show the image or FALSE to hide the image.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int DragShowNolock(int fShow);

        /// <summary>
        /// Gets the temporary image list that is used for the drag image. The function also retrieves the current drag position and the offset of the drag image relative to the drag position.
        /// </summary>
        /// <param name="ppt">A pointer to a POINT structure that receives the current drag position. Can be NULL.</param>
        /// <param name="pptHotspot">A pointer to a POINT structure that receives the offset of the drag image relative to the drag position. Can be NULL.</param>
        /// <param name="riid">An IID for the image list.</param>
        /// <param name="ppv">The address of a pointer to the interface for the image list if successful, NULL otherwise.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int GetDragImage(ref POINT ppt, ref POINT pptHotspot, ref Guid riid, ref IntPtr ppv);

        /// <summary>
        /// Gets the flags of an image.
        /// </summary>
        /// <param name="i">A value of type int that contains the index of the images whose flags need to be retrieved.</param>
        /// <param name="dwFlags">A pointer to a DWORD that contains the flags when the method returns. One of the following values:</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int GetItemFlags(int i, ref int dwFlags);

        /// <summary>
        /// Retrieves a specified image from the list of images used as overlay masks.
        /// </summary>
        /// <param name="iOverlay">A value of type int that contains the one-based index of the overlay mask.</param>
        /// <param name="piIndex">A pointer to an int that receives the zero-based index of an image in the image list. This index identifies the image that is used as an overlay mask.</param>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        int GetOverlayImage(int iOverlay, ref int piIndex);
    };
}