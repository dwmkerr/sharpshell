using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SharpShell.Attributes;
using SharpShell.Interop;
using SharpShell.SharpIconOverlayHandler;

namespace ReadOnlyFileIconOverlayHandler
{
    /// <summary>
    /// The ReadOnlyFileIconOverlayHandler is an IconOverlayHandler that shows
    /// a padlock icon over files that are read only.
    /// </summary>
    [ComVisible(true)]
    [RegistrationName("  ReadOnlyFileIconOverlayHandler")] // push our way up the list by putting spaces in the name...
    public class ReadOnlyFileIconOverlayHandler : SharpIconOverlayHandler
    {
        /// <summary>
        /// Called by the system to get the priority, which is used to determine
        /// which icon overlay to use if there are multiple handlers. The priority
        /// must be between 0 and 100, where 0 is the highest priority.
        /// </summary>
        /// <returns>
        /// A value between 0 and 100, where 0 is the highest priority.
        /// </returns>
        protected override int GetPriority()
        {
            //  The read only icon overlay is very low priority.
            return 90;
        }

        /// <summary>
        /// Determines whether an overlay should be shown for the shell item with the path 'path' and
        /// the shell attributes 'attributes'.
        /// </summary>
        /// <param name="path">The path for the shell item. This is not necessarily the path
        /// to a physical file or folder.</param>
        /// <param name="attributes">The attributes of the shell item.</param>
        /// <returns>
        ///   <c>true</c> if this an overlay should be shown for the specified item; otherwise, <c>false</c>.
        /// </returns>
        protected override bool CanShowOverlay(string path, FILE_ATTRIBUTE attributes)
        {
            try
            {
                //  Get the file attributes.
                var fileAttributes = new FileInfo(path);

                //  Return true if the file is read only, meaning we'll show the overlay.
                return fileAttributes.IsReadOnly;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Called to get the icon to show as the overlay icon.
        /// </summary>
        /// <returns>
        /// The overlay icon.
        /// </returns>
        protected override System.Drawing.Icon GetOverlayIcon()
        {
            //  Return the read only icon.
            return Properties.Resources.ReadOnly;
        }
    }
}
