using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SharpShell.Attributes;
using SharpShell.SharpInfoTipHandler;

namespace FolderInfoTipHandler
{
    /// <summary>
    /// The FolderInfoTip handler is an example SharpInfoTipHandler that provides an info tip
    /// for folders that shows the number of items in the folder.
    /// </summary>
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.Directory)]
    public class FolderInfoTipHandler : SharpInfoTipHandler
    {
        /// <summary>
        /// Gets info for the selected item (SelectedItemPath).
        /// </summary>
        /// <param name="infoType">Type of info to return.</param>
        /// <param name="singleLine">if set to <c>true</c>, put the info in a single line.</param>
        /// <returns>
        /// Specified info for the selected file.
        /// </returns>
        protected override string GetInfo(RequestedInfoType infoType, bool singleLine)
        {
            //  Switch on the tip of info we need to provide.
            switch (infoType)
            {
                case RequestedInfoType.InfoTip:

                    //  Format the formatted info tip.
                    return string.Format(singleLine
                                           ? "{0} - {1} Items"
                                           : "{0}" + Environment.NewLine + "Contains {1} Items",
                                           Path.GetFileName(SelectedItemPath), Directory.GetFiles(SelectedItemPath).Length);

                case RequestedInfoType.Name:
                    
                    //  Return the name of the folder.
                    return string.Format("Folder '{0}'", Path.GetFileName(SelectedItemPath));
                    
                default:

                    //  We won't be asked for anything else, like shortcut paths, for folders, so we 
                    //  can return an empty string in the default case.
                    return string.Empty;
            }
        }
    }
}
