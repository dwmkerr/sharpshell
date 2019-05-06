using System;
using System.Collections.Generic;
using System.Linq;
using SharpShell.Attributes;
using SharpShell.Diagnostics;
using SharpShell.Interop;

namespace SharpShell.SharpPropertySheet
{
    /// <summary>
    /// SharpPropertySheet is the base class for Shell Property Sheet Extensions supported
    /// by SharpShell.
    /// </summary>
    [ServerType(ServerType.ShellPropertySheet)]
    public abstract class SharpPropertySheet : ShellExtInitServer, IShellPropSheetExt
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpPropertySheet"/> class.
        /// </summary>
        protected SharpPropertySheet()
        {
            //  DebugLog the event.
            Log("Constructing property sheet.");

            //  The lazy property sheet pages will be created by the abstract 
            //  create pages function.
            propertySheetPages = new Lazy<List<SharpPropertyPage>>( () => CreatePages().ToList());
        }

       #region Implementation of IShellPropSheetExt

        /// <summary>
        /// Adds the pages.
        /// </summary>
        /// <param name="pfnAddPage">The PFN add page.</param>
        /// <param name="lParam">The l param.</param>
        /// <returns>
        /// If successful, returns a one-based index to specify the page that should be initially displayed. See Remarks for more information.
        /// </returns>
        int IShellPropSheetExt.AddPages(IntPtr pfnAddPage, IntPtr lParam)
        {
            //  DebugLog the event.
            Log("Adding Pages...");
            
            //  If we are not showing the sheet, we can end now.
            if (CanShowSheet() == false)
                return 0;

            //  Create the bridge.
            var bridge = new NativeBridge.NativeBridge();
            
            //  Initialise it.
            if(bridge.Initialise() == false)
            {
                Logging.Error("Failed to initialise the NativeBridge.", null);
                return 0;
            }

            //  Go through each page - that has a property page.
            foreach (var page in propertySheetPages.Value.Where(p => p.Handle != IntPtr.Zero))
            {
                //  Create a property page proxy for this page.
                var proxy = new PropertyPageProxy(this, page);

                //  Using the proxy, create the property page handle.
                proxy.CreatePropertyPageHandle(bridge);
                var propertyPageHandle = proxy.HostWindowHandle;

                //  Name the page, to aid with debugging.
                User32.SetWindowText(propertyPageHandle, "SharpShell Host for " + page.GetType().Name);
                
                //  DebugLog the event.
                Log("Created Page Proxy, handle is " + propertyPageHandle.ToString("x8"));

                //  Now that we have the page handle, add the page via the callback.
                //  Note that if the call fails, we *don't* have to destroy the property
                //  page handle, the bridge actually takes care of that for us.
                bridge.CallAddPropSheetPage(pfnAddPage, propertyPageHandle, lParam);
            }

            //  Release the bridge.
            //bridge.Deinitialise();

            //  DebugLog the event.
            Log("Adding Pages (Done)");

            //  We've succeeded.
            return WinError.S_OK;
        }

        /// <summary>
        /// Replaces the page.
        /// </summary>
        /// <param name="uPageID">The u page ID.</param>
        /// <param name="lpfnReplacePage">The LPFN replace page.</param>
        /// <param name="lParam">The l param.</param>
        /// <returns></returns>
        int IShellPropSheetExt.ReplacePage(uint uPageID, AddPropertySheetPageDelegate lpfnReplacePage, IntPtr lParam)
        {
            return 0;
        }

        #endregion

        /// <summary>
        /// Determines whether this instance can show a shell property sheet, given the specified selected file list.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance should show a shell property sheet for the specified file list; otherwise, <c>false</c>.
        /// </returns>
        protected abstract bool CanShowSheet();

        /// <summary>
        /// Creates the pages.
        /// </summary>
        /// <returns>The property sheet pages.</returns>
        protected abstract IEnumerable<SharpPropertyPage> CreatePages();

        /// <summary>
        /// The lazy property sheet pages, only created when we actually need them.
        /// </summary>
        private readonly Lazy<List<SharpPropertyPage>> propertySheetPages;
        
        /// <summary>
        /// Gets the pages.
        /// </summary>
        public IEnumerable<SharpPropertyPage> Pages
        {
            get { return propertySheetPages.Value; }
        }
    }
}
