using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using SharpShell.Attributes;
using SharpShell.Diagnostics;
using SharpShell.SharpIconHandler;

namespace DllIconHandler
{
    /// <summary>
    /// The DllIconHandler is a Shell Icon Handler exception that
    /// shows different icons for native and managed dlls.
    /// </summary>
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".dll")]
    public class DllIconHandler : SharpIconHandler
    {
        /// <summary>
        /// Gets the icon.
        /// </summary>
        /// <param name="smallIcon">if set to <c>true</c> provide a small icon.</param>
        /// <param name="iconSize">Size of the icon.</param>
        /// <returns>
        /// The icon for the file.
        /// </returns>
        protected override Icon GetIcon(bool smallIcon, uint iconSize)
        {
            //  The icon we'll return.
            Icon icon = null;

            //  Check the assembly name. If it's a native dll, this'll throw an exception.
            try
            {
                //  SelectedItemPath is provided by 'SharpIconHandlder' and contains the path of the file.
                AssemblyName.GetAssemblyName(SelectedItemPath);
            }
            catch (BadImageFormatException)
            {
                //  The file is not an assembly.
                icon = Properties.Resources.NativeDll;
            }
            catch (Exception)
            {
                //  Some other eception occured, so assume we're native.
                icon = Properties.Resources.NativeDll;
            }

            //  If we haven't determined that the dll is native, use the managed icon.
            if (icon == null)
                icon = Properties.Resources.ManagedDll;
            
            //  Return the icon with the correct size. Use the SharpIconHandler 'GetIconSpecificSize'
            //  function to extract the icon of the required size.
            return GetIconSpecificSize(icon, new Size((int)iconSize, (int)iconSize));
        }
    }
}
