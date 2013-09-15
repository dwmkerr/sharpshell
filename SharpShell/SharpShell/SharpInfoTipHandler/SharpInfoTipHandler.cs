using System;
using SharpShell.Attributes;
using SharpShell.Interop;

namespace SharpShell.SharpInfoTipHandler
{
    /// <summary>
    /// The SharpIconHandler is the base class for SharpShell servers that provide
    /// custom Icon Handlers.
    /// </summary>
    [ServerType(ServerType.ShellInfoTipHandler)]
    public abstract class SharpInfoTipHandler : PersistFileServer, IQueryInfo
    {
        /// <summary>
        /// Gets the info tip text for an item.
        /// </summary>
        /// <param name="dwFlags">Flags that direct the handling of the item from which you're retrieving the info tip text. This value is commonly zero.</param>
        /// <param name="ppwszTip">The address of a Unicode string pointer that, when this method returns successfully, receives the tip string pointer. Applications that implement this method must allocate memory for ppwszTip by calling CoTaskMemAlloc.
        /// Calling applications must call CoTaskMemFree to free the memory when it is no longer needed.</param>
        /// <returns>
        /// Returns S_OK if the function succeeds. If no info tip text is available, ppwszTip is set to NULL. Otherwise, returns a COM-defined error value.
        /// </returns>
        int IQueryInfo.GetInfoTip(QITIPF dwFlags, out string ppwszTip)
        {
            //  Do we need to get the tip on a single line?
            var singleLine = dwFlags.HasFlag(QITIPF.QITIPF_SINGLELINE);

            //  Now work out what type of info to get.
            RequestedInfoType infoType;
            if(dwFlags.HasFlag(QITIPF.QITIPF_USENAME))
                infoType = RequestedInfoType.Name;
            else if(dwFlags.HasFlag(QITIPF.QITIPF_LINKUSETARGET))
                infoType = RequestedInfoType.InfoOfShortcutTarget;
            else if(dwFlags.HasFlag(QITIPF.QITIPF_LINKNOTARGET))
                infoType = RequestedInfoType.InfoOfShortcut;
            else
                infoType = RequestedInfoType.InfoTip;
            
            try
            {
                //  Get the requested info.
                var infoTip = GetInfo(infoType, singleLine);

                //  Set the tip. The runtime marshals out strings with CoTaskMemAlloc for us.
                ppwszTip = infoTip;
            }
            catch (Exception exception)
            {
                //  DebugLog the exception.
                LogError("An exception occured getting the info tip.", exception);

                //  If an exception is thrown, we cannot return any info.
                ppwszTip = string.Empty;
            }

            //  Return success.
            return WinError.S_OK;
        }

        /// <summary>
        /// Gets the information flags for an item. This method is not currently used.
        /// </summary>
        /// <param name="pdwFlags">A pointer to a value that receives the flags for the item. If no flags are to be returned, this value should be set to zero.</param>
        /// <returns>
        /// Returns S_OK if pdwFlags returns any flag values, or a COM-defined error value otherwise.
        /// </returns>
        int IQueryInfo.GetInfoFlags(out int pdwFlags)
        {
            //  Currently, GetInfoFlags shouldn't be called by the system. Return success if it is.
            pdwFlags = 0;
            return WinError.S_OK;
        }

        /// <summary>
        /// Gets info for the selected item (SelectedItemPath).
        /// </summary>
        /// <param name="infoType">Type of info to return.</param>
        /// <param name="singleLine">if set to <c>true</c>, put the info in a single line.</param>
        /// <returns>
        /// Specified info for the selected file.
        /// </returns>
        protected abstract string GetInfo(RequestedInfoType infoType, bool singleLine);
    }

    /// <summary>
    /// Specifies the type of information requested.
    /// </summary>
    public enum RequestedInfoType
    {
        /// <summary>
        /// The InfoTip - the text that will be shown in the tooltip for the item.
        /// </summary>
        InfoTip,

        /// <summary>
        /// Return the name of the item.
        /// </summary>
        Name,

        /// <summary>
        /// If the item is a shortcut file, get the info of the shortcut itself.
        /// </summary>
        InfoOfShortcut,

        /// <summary>
        /// If the item is a shortcut file, get the info of the target of the shortcut.
        /// </summary>
        InfoOfShortcutTarget
    }
}