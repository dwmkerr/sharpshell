using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using SharpShell.Attributes;
using SharpShell.Interop;

namespace SharpShell.SharpIconHandler
{
    /// <summary>
    /// The SharpIconHandler is the base class for SharpShell servers that provide
    /// custom Icon Handlers.
    /// </summary>
    [ServerType(ServerType.ShellIconHandler)]
    public abstract class SharpIconHandler : PersistFileServer, IExtractIconA, IExtractIconW
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpIconHandler"/> class.
        /// </summary>
        protected SharpIconHandler()
        {
            //  By default, we WON'T cache icons.
            DontCacheIcons = true;
        }

       #region Implementation of IExtractIconA and IExtractIconW

        /// <summary>
        /// Gets the location and index of an icon.
        /// </summary>
        /// <param name="uFlags">One or more of the following values. This parameter can also be NULL.</param>
        /// <param name="szIconFile">A pointer to a buffer that receives the icon location. The icon location is a null-terminated string that identifies the file that contains the icon.</param>
        /// <param name="cchMax">The size of the buffer, in characters, pointed to by pszIconFile.</param>
        /// <param name="piIndex">A pointer to an int that receives the index of the icon in the file pointed to by pszIconFile.</param>
        /// <param name="pwFlags">A pointer to a UINT value that receives zero or a combination of the following value</param>
        /// <returns></returns>
        int IExtractIconA.GetIconLocation(GILInFlags uFlags, StringBuilder szIconFile, int cchMax, out int piIndex, out GILOutFlags pwFlags)
        {
            return GetIconLocation(uFlags, out piIndex, out pwFlags);
        }
        int IExtractIconW.GetIconLocation(GILInFlags uFlags, StringBuilder szIconFile, int cchMax, out int piIndex, out GILOutFlags pwFlags)
        {
            return GetIconLocation(uFlags, out piIndex, out pwFlags);
        }
        private int GetIconLocation(GILInFlags uFlags, out int piIndex, out GILOutFlags pwFlags)
        {
            //  DebugLog this key event.
            Log(string.Format("Getting icon location icon for {0}", SelectedItemPath));

            //  We're always going to return by handle.
            //  This will cause the 'Extract' function to be called.
            pwFlags = GILOutFlags.GIL_NOTFILENAME;
            if (DontCacheIcons)
                pwFlags |= GILOutFlags.GIL_DONTCACHE;

            //  No need for an index.
            piIndex = 0;

            //  Return success.
            return WinError.S_OK;
        }
        /// <summary>
        /// Extracts an icon image from the specified location.
        /// </summary>
        /// <param name="pszFile">A pointer to a null-terminated string that specifies the icon location.</param>
        /// <param name="nIconIndex">The index of the icon in the file pointed to by pszFile.</param>
        /// <param name="phiconLarge">A pointer to an HICON value that receives the handle to the large icon. This parameter may be NULL.</param>
        /// <param name="phiconSmall">A pointer to an HICON value that receives the handle to the small icon. This parameter may be NULL.</param>
        /// <param name="nIconSize">The desired size of the icon, in pixels. The low word contains the size of the large icon, and the high word contains the size of the small icon. The size specified can be the width or height. The width of an icon always equals its height.</param>
        /// <returns>
        /// Returns S_OK if the function extracted the icon, or S_FALSE if the calling application should extract the icon.
        /// </returns>
        int IExtractIconA.Extract(string pszFile, uint nIconIndex, out IntPtr phiconLarge, out IntPtr phiconSmall, uint nIconSize)
        {
            return Extract(out phiconLarge, out phiconSmall, nIconSize);
        }
        int IExtractIconW.Extract(string pszFile, uint nIconIndex, out IntPtr phiconLarge, out IntPtr phiconSmall, uint nIconSize)
        {
            return Extract(out phiconLarge, out phiconSmall, nIconSize);
        }
        private int Extract(out IntPtr phiconLarge, out IntPtr phiconSmall, uint nIconSize)
        {
            //  DebugLog this key event.
            Log(string.Format("Extracting icon for {0}", SelectedItemPath));

            //  Get the small and large icon sizes.
            var iconSizeSmall = (uint)User32.HighWord((int)nIconSize);
            var iconSizeLarge = (uint)User32.LowWord((int)nIconSize);

            //  Default the icons to null.
            phiconLarge = IntPtr.Zero;
            phiconSmall = IntPtr.Zero;

            try
            {
                //  Set the large and small icons.
                phiconLarge = GetIcon(false, iconSizeLarge).Handle;
                phiconSmall = GetIcon(true, iconSizeSmall).Handle;
            }
            catch (Exception exception)
            {
                //  DebugLog the exception.
                LogError("An exception occured extracting icons.", exception);
            }

            //  Return succes.
            return WinError.S_OK;
        }

        #endregion

        /// <summary>
        /// Gets an icon of a specific size from a set of icons.
        /// </summary>
        /// <param name="icon">The icon.</param>
        /// <param name="size">The size required.</param>
        /// <returns>The icon that is cloest to the size provided.</returns>
        protected static Icon GetIconSpecificSize(Icon icon, Size size)
        {
            //  Create a memory stream.
            using (var memoryStream = new MemoryStream())
            {
                //  Save the icon to the stream, then seek to the origin.
                icon.Save(memoryStream);
                memoryStream.Position = 0;

                //  Load the icon from the stream, but specify the size.
                return new Icon(memoryStream, size);
            }
        }
        
        /// <summary>
        /// Gets the icon.
        /// </summary>
        /// <param name="smallIcon">if set to <c>true</c> provide a small icon.</param>
        /// <param name="iconSize">Size of the icon.</param>
        /// <returns>The icon for the file.</returns>
        protected abstract Icon GetIcon(bool smallIcon, uint iconSize);

        /// <summary>
        /// Gets or sets a value indicating whether to force icons from this handler 
        /// to not be cached by the shell.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if icons mustn't be cached; otherwise, <c>false</c>.
        /// </value>
        public bool DontCacheIcons { get; protected set; }
    }
}