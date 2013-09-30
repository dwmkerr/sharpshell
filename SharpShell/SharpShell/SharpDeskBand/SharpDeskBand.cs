using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpShell.Attributes;
using SharpShell.Interop;

namespace SharpShell.SharpDeskBand
{
    [ServerType(ServerType.ShellDeskBand)]
    public class SharpDeskBand : SharpShellServer, IDeskBand, IPersistStream, IObjectWithSite
    {
        /// <summary>
        /// The COM site (see IObjectWithSite implementation).
        /// </summary>
        private object comSite;

        /// <summary>
        /// The handle to the parent window site.
        /// </summary>
        private IntPtr parentWindowHandle;

        #region Implmentation of the IObjectWithSite interface

        int IObjectWithSite.GetSite(ref Guid riid, out object ppvSite)
        {
            //  Provide the site.
            ppvSite = comSite;

            //  Got the site successfully.
            return WinError.S_OK;
        }

        int IObjectWithSite.SetSite(object pUnkSite)
        {
            //  If we have a site, free it. This won't actually release the COM
            //  interface until garbage collection happens.
            comSite = null;

            //  If we have not been provided a site, the band is being removed.
            if (pUnkSite == null)
            {                
                OnBandRemoved();
                return WinError.S_OK;
            }

            //  We've been given a site, that means we can get the site window.
            try
            {
                //  Get the OLE window.
                var oleWindow = (IOleWindow)pUnkSite;

                //  Get the parent window handle.
                if (oleWindow.GetWindow(out parentWindowHandle) != WinError.S_OK)
                {
                    LogError("Failed to get the handle to the site window.");
                    return WinError.E_FAIL;
                }

                //  Create the desk band.
                CreateDeskBand();
                
                //  Free the OLE window, we're done with it.
                oleWindow = null;
            }
            catch(Exception exception)
            {
                LogError("Failed to cast the provided site to an IOleWindow.", exception);
                return WinError.E_FAIL;
            }

            //  Store the input site.
            //  TODO comSite is of type IInputObjectSite, get it here (cast for QueryInterface) and release it
            //  as above. may need to write a wrapper for it.

            return WinError.S_OK;
        }

        protected void CreateDeskBand()
        {
            //  TODO create the desk band with the appropriate parent window.
        }

        /// <summary>
        /// Called when the band is being removed from explorer.
        /// </summary>
        protected virtual void OnBandRemoved();

        #endregion
    }
}
