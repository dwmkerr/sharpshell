using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using SharpShell.Attributes;
using SharpShell.Interop;
using SharpShell.SharpIconOverlayHandler;

namespace LockedFileIconOverlayHandler
{
    /// <summary>
    /// The LockedFileIconOverlayHandler shows a padlock over files 
    /// which APPEAR to be locked. Use with care.
    /// </summary>
    [ComVisible(true)]
    [DisplayName("Locked File Icon Overlay Handler")]
    public class LockedFileIconOverlayHandler : SharpIconOverlayHandler
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
            //  We're very low priority.
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
        /// <exception cref="System.NotImplementedException"></exception>
        protected override bool CanShowOverlay(string path, FILE_ATTRIBUTE attributes)
        {
#if DEBUG
            var result = TestIfFileIsLocked(path);
            var name = Path.GetFileName(path);
            Log(name + " is " + (result ? "locked" : "not locked"));
            return result;
#else
            return TestIfFileIsLocked(path);
#endif
        }

        /// <summary>
        /// Called to get the icon to show as the overlay icon.
        /// </summary>
        /// <returns>
        /// The overlay icon.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override Icon GetOverlayIcon()
        {
            return Properties.Resources.Locked;
        }

        private static bool TestIfFileIsLocked(string path)
        {
            //  We'll get an exception if the file is locked...

            try
            {
                using (File.Open(path, FileMode.Open, FileAccess.Write, FileShare.None))
                {
                    //  The file is not locked.
                    return false;
                }
            }
            catch (IOException exception)
            {
                //  Get the exception hresult, check for lock exceptions.
                var errorCode = Marshal.GetHRForException(exception) & ((1 << 16) - 1);
                return errorCode == 32 || errorCode == 33;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
