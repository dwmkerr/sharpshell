using System;
using System.Runtime.InteropServices;
using SharpShell.Diagnostics;
using SharpShell.Helpers;
using SharpShell.Interop;

namespace SharpShell.SharpPropertySheet
{
    /// <summary>
    /// The PropertyPageProxy is the object used to pass data between the 
    /// shell and the SharpPropertyPage.
    /// </summary>
    internal class PropertyPageProxy
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="PropertyPageProxy"/> class from being created.
        /// </summary>
        private PropertyPageProxy()
        {
            
        }

        #region Logging Helper Functions

        /// <summary>
        /// Logs the specified message. Will include the Shell Extension name and page name if available.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void Log(string message)
        {
            var level1 = Parent != null ? Parent.DisplayName : "Unknown";
            var level2 = Target != null ? Target.PageTitle : "Unknown";
            Logging.Log($"{level1} (Proxy for '{level2}' Page): {message}");
        }

        /// <summary>
        /// Logs the specified message as an error.  Will include the Shell Extension name and page name if available.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">Optional exception details.</param>
        protected void LogError(string message, Exception exception = null)
        {
            var level1 = Parent != null ? Parent.DisplayName : "Unknown";
            var level2 = Target != null ? Target.PageTitle : "Unknown";
            Logging.Error($"{level1} (Proxy for {level2}): {message}", exception);
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyPageProxy"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="propertyPage">The target property page.</param>
        internal PropertyPageProxy(SharpPropertySheet parent, SharpPropertyPage propertyPage)
        {
            //  Set the target.
            Parent = parent;
            Target = propertyPage;

            //  set the proxy reference in the property page.
            propertyPage.PropertyPageProxy = this;

            //  Create the dialog proc delegate (as a class member so it won't be garbage collected).
            dialogProc = new DialogProc(WindowProc);
            callbackProc = new PropSheetCallback(CallbackProc);
        }

        /// <summary>
        /// The WindowProc. Called by the shell and must delegate messages via the proxy to the user control.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="uMessage">The u message.</param>
        /// <param name="wParam">The w param.</param>
        /// <param name="lParam">The l param.</param>
        /// <returns></returns>
        private IntPtr WindowProc(IntPtr hWnd, uint uMessage, IntPtr wParam, IntPtr lParam)
        {
            switch (uMessage)
            {
                //  WM_SIZE will normally be sent once, early in the lifecycle. This is great as it allows us
                //  to explicitly set the size of the page. This means that the page can be of any size in the
                //  designer. As long as controls are anchored properly it will then be laid out correctly.
                case WindowsMessages.WM_SIZE:

                    //  Grab the new client size.
                    var width = Win32Helper.LoWord(lParam);
                    var height = Win32Helper.HiWord(lParam);

                    //  Pass the size onto the target window if we can.
                    Target?.SetBounds(0, 0, width, height);

                    break;

                case WindowsMessages.WM_INITDIALOG:
                    
                    try
                    {
                        //  Store the property sheet page handle.
                        propertySheetPageHandle = hWnd;

                        //  Set the parent of the property page to the host.
                        User32.SetParent(Target.Handle, hWnd);
                        
                        //  Get the handle to the property sheet.
                        propertySheetHandle = User32.GetParent(hWnd);

                        //  Update the page, invoking the page initialised function.
                        Target.OnPropertyPageInitialised(Parent);
                    }
                    catch (Exception exception)
                    {
                        LogError("Failed to set the parent to the host.", exception);
                    }

                    break;

                case WindowsMessages.WM_NOTIFY:

                    //  Get the NMHDR.
                    var nmhdr = (NMHDR)Marshal.PtrToStructure(lParam, typeof (NMHDR));

                    //  Is it PSN_APPLY?
                    if (nmhdr.code == (uint)PSN.PSN_APPLY)
                    {
                        //  Get the PSH notify struct.
                        var nmpsheet = (PSHNOTIFY) Marshal.PtrToStructure(lParam, typeof (PSHNOTIFY));

                        //  If lParam is 0, it's apply, otherwise it's OK.
                        if(nmpsheet.lParam == IntPtr.Zero)
                            Target.OnPropertySheetApply();
                        else
                            Target.OnPropertySheetOK();
                    }
                    else if(nmhdr.code == (uint)PSN.PSN_SETACTIVE)
                    {
                        //  Fire the page activated.
                        Target.OnPropertyPageSetActive();
                    }
                    else if (nmhdr.code == (uint)PSN.PSN_KILLACTIVE)
                    {
                        //  Fire the page deactivated.
                        Target.OnPropertyPageKillActive();
                    }
                    else if (nmhdr.code == (uint)PSN.PSN_RESET)
                    {
                        //  Get the PSH notify struct.
                        var nmpsheet = (PSHNOTIFY)Marshal.PtrToStructure(lParam, typeof(PSHNOTIFY));

                        //  If lParam is 0, it's cancel, otherwise it's close (with the cross button).
                        if (nmpsheet.lParam == IntPtr.Zero)
                            Target.OnPropertySheetCancel();
                        else
                            Target.OnPropertySheetClose();
                    }

                    break;

            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// The CallbackProc. Called by the shell to inform of key property page events.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="uMsg">The u MSG.</param>
        /// <param name="ppsp">The PPSP.</param>
        /// <returns></returns>
        private uint CallbackProc(IntPtr hWnd, PSPCB uMsg, ref PROPSHEETPAGE ppsp)
        {
            switch (uMsg)
            {
                case PSPCB.PSPCB_ADDREF:

                    //  Increment the reference count.
                    Log($"Add Ref {referenceCount} -> {referenceCount+1}");
                    referenceCount++;

                    break;

                case PSPCB.PSPCB_RELEASE:

                    Log($"Release Ref {referenceCount} -> {referenceCount - 1}");

                    //  Decrement the reference count.
                    referenceCount--;

                    //  If we're down to zero references, cleanup.
                    if (referenceCount == 0)
                        Cleanup();

                    break;

                case PSPCB.PSPCB_CREATE:

                    Log($"Create Callback");

                    //  Allow the sheet to be created.
                    return 1;
            }
            return 0;
        }

        private void Cleanup()
        {
            Log($"Cleanup");

            //  Destroy the target.
            Target.Dispose();

            //  Destroy the host.
            User32.DestroyWindow(HostWindowHandle);

            //  Clear the lot.
            Target = null;
            HostWindowHandle = IntPtr.Zero;
        }

        /// <summary>
        /// Creates the property page handle.
        /// </summary>
        public void CreatePropertyPageHandle(NativeBridge.NativeBridge nativeBridge)
        {
            Log("Creating property page handle via bridge.");

            //  Create a prop sheet page structure.
            var psp = new PROPSHEETPAGE();

            //  Set the key properties.
            psp.dwSize = (uint)Marshal.SizeOf(psp);

            psp.hInstance = nativeBridge.GetInstanceHandle();
            psp.dwFlags = PSP.PSP_DEFAULT | PSP.PSP_USETITLE | PSP.PSP_USECALLBACK;

            psp.pTemplate = nativeBridge.GetProxyHostTemplate();
            psp.pfnDlgProc = dialogProc;
            psp.pcRefParent = 0;
            psp.pfnCallback = callbackProc;
            psp.lParam = IntPtr.Zero;

            //  If we have a title, set it.
            if (!string.IsNullOrEmpty(Target.PageTitle))
            {
                psp.dwFlags |= PSP.PSP_USETITLE;
                psp.pszTitle = Target.PageTitle;
            }

            //  If we have an icon, set it.
            if (Target.PageIcon != null && Target.PageIcon.Handle != IntPtr.Zero)
            {
                psp.dwFlags |= PSP.PSP_USEHICON;
                psp.hIcon = Target.PageIcon.Handle;
            }

            //  Create a the property sheet page.
            HostWindowHandle = Comctl32.CreatePropertySheetPage(ref psp);
        }

        /// <summary>
        /// Sets the data changed state of the parent property sheet, enabling (or disabling) the apply button.
        /// </summary>
        /// <param name="changed">if set to <c>true</c> data has changed.</param>
        internal void SetDataChanged(bool changed)
        {
            //  Send the appropriate message to the property sheet.
            User32.SendMessage(propertySheetHandle,
                changed ? WindowsMessages.PSM_CHANGED : WindowsMessages.PSM_UNCHANGED, propertySheetPageHandle, IntPtr.Zero);
        }

        /// <summary>
        /// The dialog proc.
        /// </summary>
        private readonly DialogProc dialogProc;

        /// <summary>
        /// The callback proc.
        /// </summary>
        private readonly PropSheetCallback callbackProc;

        /// <summary>
        /// The property sheet handle.
        /// </summary>
        private IntPtr propertySheetHandle;

        /// <summary>
        /// The reference count.
        /// </summary>
        private int referenceCount;

        /// <summary>
        /// The property sheet page handle.
        /// </summary>
        private IntPtr propertySheetPageHandle;

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public SharpPropertySheet Parent { get; set; }

        /// <summary>
        /// Gets the property page.
        /// </summary>
        /// <value>
        /// The property page.
        /// </value>
        public SharpPropertyPage Target { get; private set; }

        /// <summary>
        /// Gets the host window handle.
        /// </summary>
        /// <value>
        /// The host window handle.
        /// </value>
        public IntPtr HostWindowHandle { get; private set; }
    }
}
