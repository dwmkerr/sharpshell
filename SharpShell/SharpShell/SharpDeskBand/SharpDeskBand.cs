using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpShell.Attributes;
using SharpShell.Interop;
using System.Windows.Forms;

namespace SharpShell.SharpDeskBand
{
    [ServerType(ServerType.ShellDeskBand)]
    public abstract class SharpDeskBand : SharpShellServer, IDeskBand, IPersistStream, IObjectWithSite
    {
        //  TODO: Implement IContextMenu
        //  TODO: Implement IInputObject

        /// <summary>
        /// The COM site (see IObjectWithSite implementation).
        /// </summary>
        private IInputObjectSite inputObjectSite;

        /// <summary>
        /// The handle to the parent window site.
        /// </summary>
        private IntPtr parentWindowHandle;

        /// <summary>
        /// The actual deskband user control.
        /// </summary>
        private UserControl deskBand;

        /// <summary>
        /// The band ID provided by explorer to identify the band.
        /// </summary>
        private uint explorerBandId = 0;

        #region Implmentation of the IObjectWithSite interface

        int IObjectWithSite.GetSite(ref Guid riid, out object ppvSite)
        {
            //  Provide the site.
            ppvSite = inputObjectSite;

            //  Got the site successfully.
            return WinError.S_OK;
        }

        int IObjectWithSite.SetSite(object pUnkSite)
        {
            //  If we have a site, free it. This won't actually release the COM
            //  interface until garbage collection happens.
            inputObjectSite = null;

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

                //  Create the desk band user interface.
                deskBand = CreateDeskBand();

                //  Set the parent.
                User32.SetParent(deskBand.Handle, parentWindowHandle);
                
                //  Free the OLE window, we're done with it.
                oleWindow = null;
            }
            catch(Exception exception)
            {
                LogError("Failed to cast the provided site to an IOleWindow.", exception);
                return WinError.E_FAIL;
            }

            //  Store the input site.
            inputObjectSite = (IInputObjectSite)pUnkSite;

            return WinError.S_OK;
        }

        /// <summary>
        /// Called when the band is being removed from explorer.
        /// </summary>
        protected virtual void OnBandRemoved()
        { 
        }

        #endregion

        #region Implementation of IPersistStream

        int IPersistStream.IsDirty()
        {
            //  TODO: return S_OK to indicate the object has changed
            //  since the last time is was saved to a stream.

            //  Until we need explorer bar persistence, we're not dirty.
            return WinError.S_FALSE;
        }

        int IPersistStream.Load(System.Runtime.InteropServices.ComTypes.IStream pStm)
        {
            //  Not implemented: Explorer provided Persistence.
            return WinError.S_OK;
        }

        int IPersistStream.Save(System.Runtime.InteropServices.ComTypes.IStream pStm, bool fClearDirty)
        {
            //  Not implemented: Explorer provided Persistence.
            return WinError.S_OK;
        }

        int IPersistStream.GetSizeMax(out ulong pcbSize)
        {
            //  Not implemented: Explorer provided Persistence.
            pcbSize = 0;
            return WinError.S_OK;
        }
        
        int IPersist.GetClassID(out Guid pClassID)
        {
            //  The class ID is just a unique identifier for the class, meaning
            //  that we can use the class GUID as it will be provided for
            //  all SharpShell servers.
            pClassID = ServerClsid;

            //  Return success.
            return WinError.S_OK;
        }

        #endregion

        #region Implmentation of IDeskBand

        int IDeskBand.GetBandInfo(uint dwBandID, DBIF dwViewMode, ref DESKBANDINFO pdbi)
        {
            //  Store the band id.
            explorerBandId = dwBandID;

            //  Depending on what we've been asked for, we'll return various band properties.

            //  Return the min size if needed.
            if (pdbi.dwMask.HasFlag(DBIM.DBIM_MINSIZE))
            {
                //  TODO: provide minsize.
                pdbi.ptMinSize.X = 200;
                pdbi.ptMinSize.Y = 30;
            }

            if (pdbi.dwMask.HasFlag(DBIM.DBIM_MAXSIZE))
            {
                //  TODO: check documentation for this.
                pdbi.ptMaxSize.Y = -1;
            }

            if (pdbi.dwMask.HasFlag(DBIM.DBIM_INTEGRAL))
            {
                //  TODO: check documentation for this.
                pdbi.ptIntegral.Y = 1;
            }

            if (pdbi.dwMask.HasFlag(DBIM.DBIM_ACTUAL))
            {
                //  TODO: Return actual size.
                pdbi.ptActual.X = 200;
                pdbi.ptActual.Y = 30;
            }

            if (pdbi.dwMask.HasFlag(DBIM.DBIM_TITLE))
            {
                //  TODO: Handle titles.
                // Don't show title by removing this flag.
                pdbi.dwMask &= ~DBIM.DBIM_TITLE;
            }

            if (pdbi.dwMask.HasFlag(DBIM.DBIM_MODEFLAGS))
            {
                //  TODO: Handle flags.
                pdbi.dwModeFlags = DBIMF.DBIMF_NORMAL | DBIMF.DBIMF_VARIABLEHEIGHT;
            }
            
            if (pdbi.dwMask.HasFlag(DBIM.DBIM_BKCOLOR))
            {
                //  TODO: Handle background colour.
                // Use the default background color by removing this flag.
                pdbi.dwMask &= ~DBIM.DBIM_BKCOLOR;
            }
                        
            //  Return success.
            return WinError.S_OK;
        }

        int IDockingWindow.ShowDW(bool bShow)
        {
            //  If we've got a content window, show it or hide it.
            if (deskBand != null)
            {
                //  TODO: Desk Bands: Toggle the visibility of the content.
            }

            //  TODO: Desk Bands: Call a OnShowOrHide function or something similar.

            //  Return success.
            return WinError.S_OK;
        }

        int IDockingWindow.CloseDW(uint dwReserved)
        {
            //  If we've got a content window, hide it and then destroy it.
            if (deskBand != null)
            {
                //  TODO: Desk Bands: Destroy the content.
            }

            //  TODO: Desk Bands: Call a OnClose function or something similar.

            //  Return success.
            return WinError.S_OK;
        }

        int IDockingWindow.ResizeBorderDW(RECT rcBorder, IntPtr punkToolbarSite, bool fReserved)
        {
            //  This function is not used for Window's Desk Bands and in an IDeskBand implementation
            //  should always return E_NOTIMPL.
            return WinError.E_NOTIMPL;
        }

        int IOleWindow.GetWindow(out IntPtr phwnd)
        {
            //   Easy enough, just return the handle of the deskband content.
            phwnd = deskBand != null ? deskBand.Handle : IntPtr.Zero;

            //  Return success.
            return WinError.S_OK;
        }

        int IOleWindow.ContextSensitiveHelp(bool fEnterMode)
        {
            //  TODO: Context sensitive help currently is not supported.
            return WinError.E_NOTIMPL;
        }

        #endregion

        /// <summary>
        /// This function should return a new instance of the desk band's user interface,
        /// which will simply be a usercontrol.
        /// </summary>
        /// <returns></returns>
        protected abstract UserControl CreateDeskBand();
    }
}
