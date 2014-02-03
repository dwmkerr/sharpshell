using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using SharpShell.Interop;

namespace SharpShell.Components
{
    /// <summary>
    /// The ExtractIconImpl class is an implementation of <see cref="IExtractIconA "/>
    /// and <see cref="IExtractIconW"/> which can return a .NET <see cref="System.Drawing.Icon" />
    /// that contains mulitple images sizes to the shell.
    /// </summary>
    public class ExtractIconImpl : IExtractIconA, IExtractIconW
    {
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
            //  Get the small and large icon sizes.
            var iconSizeSmall = (uint)User32.HighWord((int)nIconSize);
            var iconSizeLarge = (uint)User32.LowWord((int)nIconSize);

            //  Default the icons to null.
            phiconLarge = IntPtr.Zero;
            phiconSmall = IntPtr.Zero;

            try
            {
                //  Set the large and small icons.
                phiconLarge = GetIcon( iconSizeLarge).Handle;
                phiconSmall = GetIcon( iconSizeSmall).Handle;
            }
            catch (Exception)
            {
                //  DebugLog the exception.
                //LogError("An exception occured extracting icons.", exception);
            }

            //  Return succes.
            return WinError.S_OK;
        }

        private Icon GetIcon(uint iconSize)
        {
            return GetIconSpecificSize(new Size((int)iconSize, (int)iconSize));
        }

        protected Icon GetIconSpecificSize(Size size)
        {
            //  Create a memory stream.
            using (var memoryStream = new MemoryStream())
            {
                //  Save the icon to the stream, then seek to the origin.
                Icon.Save(memoryStream);
                memoryStream.Position = 0;

                //  Load the icon from the stream, but specify the size.
                return new Icon(memoryStream, size);
            }
        }

        #endregion


        public bool DontCacheIcons { get; set; }

        public Icon Icon { get; set; }
    }
}
