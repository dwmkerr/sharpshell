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
            Logging.Log($"{level1} (Proxy {HostWindowHandle.ToString("x8")} for '{level2}' Page): {message}");
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
            Logging.Error($"{level1} (Proxy {HostWindowHandle.ToString("x8")} for {level2}): {message}", exception);
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
                //  Unless we explicitly handle the WM_ACTIVATE message, then when we lose and then regain focus
                //  in a property page which contains a tab control, then the server will crash.
                //  Detailed investigation has not helped us yes understand *why* this is the case, so at the moment
                //  this is a required defensive measure. For more details, see:
                //      https://github.com/dwmkerr/sharpshell/issues/233
                case WindowsMessages.WM_ACTIVATE:
                    return new IntPtr(-1);

                //  The shell creates our property pages from a template, then adds them as a child of the sheet.
                //  This means when it closes the sheet it will destroy the pages for us. This event handler
                //  primarily exists for diagnostics, to allow us to see where in the lifecycle the actual destroy
                //  event happens. Note that from this point onwards it would not be safe to use any part of the
                //  form - it is destroyed.
                case WindowsMessages.WM_DESTROY:

                    Log("Proxy is being destroyed...");

                    break;

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

                //  The proxy window is really just a container for the user control which holds the user defined
                //  property sheet content. So make it transparent (otherwise we'll get a grey dialog background).
                case WindowsMessages.WM_ERASEBKGND:
                    
                    //  Return true - i.e. we handled erasing the background (by doing nothing).
                    return new IntPtr(1);

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
                        LogError("Failed to initialise the property page.", exception);
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
            //  Important: The docs at: https://docs.microsoft.com/en-us/windows/desktop/shell/how-to-register-and-implement-a-property-sheet-handler-for-a-file-type
            //  Imply we *must* set PSP_USEPARENTREF and give access to the parent reference count, to avoid the server being
            //  unloaded while the property sheet is still visible. It is damn near impossible to do this as
            //  the IUnknown of the IShellPropSheetExt is managed by the runtime. Instead, when the internal
            //  reference count increases, we will manually increment the COM server ref count, then release
            //  it when our property sheet tells us we are done. This *appears* to have resolved the lifetime
            //  issues. The theory is that we are using the lifecycle hooks below to manage our IShellPropSheetExt
            //  server ref count and ensure that the shell will not unload it while the property sheet is in use.

            switch (uMsg)
            {
                case PSPCB.PSPCB_ADDREF:
                {
                    //  Increment the internal reference count.
                    Log($"Add Internal Ref {referenceCount} -> {referenceCount + 1}");
                    referenceCount++;

                    //  At this point, increment the IPropSheetShellExt interface reference count, so that the
                    //  shell doesn't try and release the server before we are done.
                    var pUnk = Marshal.GetIUnknownForObject(Parent); // i.e. IShellPropSheetExt
                    var newCount = Marshal.AddRef(pUnk);
                    Log($"IShellPropSheetExt: Add Ref {newCount - 1} -> {newCount}");

                    break;
                }

                case PSPCB.PSPCB_RELEASE:
                {
                    Log($"Release Internal Ref {referenceCount} -> {referenceCount - 1}");

                    //  Decrement the internal reference count.
                    referenceCount--;
                    
                    //  If we're down to zero references, cleanup.
                    if (referenceCount == 0)
                    {
                        //  The Target is a child of the host window handle, and that is child of the sheet.
                        //  So these windows will be destroyed as part of the normal lifecycle. It's important
                        //  we *don't* destroy them here or they could be destroyed twice. This is the place
                        //  however to free up other resources which might be used by the page.
                        try
                        {
                            Target?.OnRelease();
                        }
                        catch (Exception exception)
                        {
                            LogError("An exception occured releasing the property page", exception);
                        }
                    }

                    //  Balance out the AddRef all from PSPCB_ADDREF by releasing now.
                    var pUnk = Marshal.GetIUnknownForObject(Parent); // i.e. IShellPropSheetExt
                    var newCount = Marshal.Release(pUnk);
                    Log($"IShellPropSheetExt: Release {newCount + 1} -> {newCount}");

                    break;
                }

                case PSPCB.PSPCB_CREATE:

                    Log($"Create Callback");

                    //  Allow the sheet to be created.
                    return 1;
            }
            return 0;
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
