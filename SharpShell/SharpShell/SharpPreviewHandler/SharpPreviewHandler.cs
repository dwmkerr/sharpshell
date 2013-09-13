using System;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using Microsoft.Win32;
using SharpShell.Extensions;
using SharpShell.Interop;
using SharpShell.Attributes;
using SharpShell.ServerRegistration;

namespace SharpShell.SharpPreviewHandler
{
    //  TODO: There should be a single base class that implements:
    //  IPersistFile, IInitializeWithFile and IInitializeWithStream that 
    //  provides a base for all single file servers.

    //  TODO: document main source of documentation http://msdn.microsoft.com/en-us/library/windows/desktop/cc144139
    //  also: http://msdn.microsoft.com/en-us/magazine/cc163487.aspx

    /// <summary>
    /// The SharpPreviewHandler is the base class for Shell Preview Handlers
    /// implemented with SharpShell.
    /// </summary>
    [ServerType(ServerType.ShellPreviewHander)]
    public abstract class SharpPreviewHandler : SharpShellServer,
                                                IInitializeWithFile,
                                                /*IInitializeWithStream,*/
                                                IObjectWithSite,
                                                IOleWindow,
                                                IPreviewHandler,
                                                IPreviewHandlerVisuals
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpPreviewHandler"/> class.
        /// </summary>
        protected SharpPreviewHandler()
        {
            //  Create the preview handler host, we must do this in the constructor because the constructor
            //  is called on the VI thread which is STA, all subsequent calls will be on MTA threads.
            previewHandlerHost = new PreviewHandlerHost();

            //  Now make sure it's actually created.
#pragma warning disable 168
            var handle = previewHandlerHost.Handle;
#pragma warning restore 168
        }

        /// <summary>
        /// Calls a function on the preview host thread.
        /// </summary>
        /// <param name="action">The action to invoke on the preview host thread.</param>
        private void OnPreviewHostThread(Action action)
        {
            //  Invoke the action on the preview host thread.
            if(previewHandlerHost.Handle != IntPtr.Zero)
                previewHandlerHost.Invoke(action);
        }

        /// <summary>
        /// Updates the host, setting the size, parent window and visibility if needed.
        /// </summary>
        private void UpdateHost()
        {
            //  On the preview host thread, set the parent and bounds.
            OnPreviewHostThread(
                () =>
                    {
                        //  Set the parent of the host.
                        if(previewHostHandle != IntPtr.Zero)
                            User32.SetParent(previewHandlerHost.Handle, previewHostHandle);

                        //  Set the bounds of the host.
                        previewHandlerHost.Bounds = new Rectangle(previewArea.left, previewArea.top, previewArea.Width(), previewArea.Height());

                        //  If we have a preview control, make sure its parent is set and it's visible.
                        if (previewHandlerControl != null)
                        {
                            User32.SetParent(previewHandlerControl.Handle, previewHandlerHost.Handle);
                            previewHandlerControl.Bounds = new Rectangle(previewArea.left, previewArea.top, previewArea.Width(), previewArea.Height());
                            previewHandlerHost.Visible = true;
                            previewHandlerControl.Visible = true;
                        }
                    });
        }
    
        #region Implementation of IInitializeWithFile

        /// <summary>
        /// Initializes a handler with a file path.
        /// </summary>
        /// <param name="pszFilePath">A pointer to a buffer that contains the file path as a null-terminated Unicode string.</param>
        /// <param name="grfMode">One of the following STGM values that indicates the access mode for pszFilePath. STGM_READ or STGM_READWRITE.</param>
        int IInitializeWithFile.Initialize(string pszFilePath, STGM grfMode)
        {
            //  Log key events.
            Log("IObjectWithSite.GetSite called.");

            //  Store the file path.
            SelectedFilePath = pszFilePath;

            //  Return success.
            return WinError.S_OK;
        }

        #endregion
        
        /* 
         * TODO: MSDN says initialize with stream is strongly preferred,
         * however i'm not sure that most implementers will want the stream...
         * 
        #region Implementation of IInitializeWithStream

        /// <summary>
        /// Initializes a handler with a stream.
        /// </summary>
        /// <param name="pstream">A pointer to an IStream interface that represents the stream source.</param>
        /// <param name="grfMode">One of the following STGM values that indicates the access mode for pstream. STGM_READ or STGM_READWRITE.</param>
        void IInitializeWithStream.Initialize(IStream pstream, uint grfMode)
        {
            //  Log key events.
            Log("IObjectWithSite.GetSite called.");

            fileStream = new ShellStream(pstream);
        }

        #endregion
        */
        #region Implementation of IObjectWithSite

        /// <summary>
        /// Retrieves the latest site passed using SetSite.
        /// </summary>
        /// <param name="riid">The IID of the interface pointer that should be returned in ppvSite.</param>
        /// <param name="ppvSite">Address of pointer variable that receives the interface pointer requested in riid. Upon successful return, *ppvSite contains the requested interface pointer to the site last seen in SetSite. The specific interface returned depends on the riid argument—in essence, the two arguments act identically to those in QueryInterface. If the appropriate interface pointer is available, the object must call AddRef on that pointer before returning successfully. If no site is available, or the requested interface is not supported, this method must *ppvSite to NULL and return a failure code.</param>
        /// <returns>
        /// This method returns S_OK on success.
        /// </returns>
        int IObjectWithSite.GetSite(ref Guid riid, out object ppvSite)
        {
            //  Log key events.
            Log("IObjectWithSite.GetSite called.");

            //  Return the site.
            ppvSite = site;

            //  Return success.
            return WinError.S_OK;
        }

        /// <summary>
        /// Enables a container to pass an object a pointer to the interface for its site.
        /// </summary>
        /// <param name="pUnkSite">A pointer to the IUnknown interface pointer of the site managing this object. If NULL, the object should call Release on any existing site at which point the object no longer knows its site.</param>
        /// <returns>
        /// This method returns S_OK on success.
        /// </returns>
        int IObjectWithSite.SetSite(object pUnkSite)
        {
            //  Log key events.
            Log("IObjectWithSite.SetSite called.");

            //  Store the site. If it's a frame, store that too.
            site = pUnkSite;
            previewHandlerFrame = pUnkSite as IPreviewHandlerFrame;

            //  Return success.
            return WinError.S_OK;
        }

        #endregion

        #region Implementation of IOleWindow

        /// <summary>
        /// Retrieves a handle to one of the windows participating in in-place activation (frame, document, parent, or in-place object window).
        /// </summary>
        /// <param name="phwnd">A pointer to a variable that receives the window handle.</param>
        /// <returns>
        /// This method returns S_OK on success.
        /// </returns>
        int IOleWindow.GetWindow(out IntPtr phwnd)
        {
            //  Log key events.
            Log("IOleWindow.GetWindow called.");

            //  Set the host window handle.
            phwnd = previewHandlerHost.Handle;
            
            //  Return success.
            return WinError.E_FAIL;
        }

        /// <summary>
        /// Determines whether context-sensitive help mode should be entered during an in-place activation session.
        /// </summary>
        /// <param name="fEnterMode">TRUE if help mode should be entered; FALSE if it should be exited.</param>
        /// <returns>
        /// This method returns S_OK if the help mode was entered or exited successfully, depending on the value passed in fEnterMode.
        /// </returns>
        int IOleWindow.ContextSensitiveHelp(bool fEnterMode)
        {
            //  Log key events.
            Log("IOleWindow.ContextSensitiveHelp called.");

            //  As defined by MSDN, we must return E_NOTIMPL for this function.
            return WinError.E_NOTIMPL;
        }

        #endregion

        #region Implementation of IPreviewHandler

        /// <summary>
        /// Sets the parent window of the previewer window, as well as the area within the parent to be used for the previewer window.
        /// </summary>
        /// <param name="hwnd">A handle to the parent window.</param>
        /// <param name="prc">A pointer to a RECT defining the area for the previewer.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        int IPreviewHandler.SetWindow(IntPtr hwnd, ref RECT prc)
        {
            //  Log key events.
            Log("IPreviewHandler.SetWindow called.");

            //  Set the preview host and preview rect.
            previewArea = prc;
            previewHostHandle = hwnd;

            //  Return success.
            return WinError.S_OK;
        }

        /// <summary>
        /// Directs the preview handler to change the area within the parent hwnd that it draws into.
        /// </summary>
        /// <param name="prc">A pointer to a RECT to be used for the preview.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        int IPreviewHandler.SetRect(RECT prc)
        {
            //  Log key events.
            Log("IPreviewHandler.SetRect called.");

            //  Set the preview area.
            previewArea = prc;

            //  Update the host.
            UpdateHost();

            //  Return success.
            return WinError.S_OK;
        }

        /// <summary>
        /// Directs the preview handler to load data from the source specified in an earlier Initialize method call, and to begin rendering to the previewer window.
        /// </summary>
        /// <returns>
        /// This method can return one of these values.
        /// </returns>
        int IPreviewHandler.DoPreview()
        {
            //  Log key events.
            Log("IPreviewHandler.DoPreview called.");

            //  Call the main function to create the preview handler.
            try
            {
                //  Call the base.
                OnPreviewHostThread(() => { previewHandlerControl = DoPreview(); });

                //  Update bounds.
                UpdateHost();
            }
            catch (Exception exception)
            {
                //  Log the error and return failure.
                LogError("An exception occured when trying to create the preview handler.", exception);
                return WinError.E_FAIL;
            }

            //  We've successfully create the preview.
            return WinError.S_OK;
        }

        /// <summary>
        /// Directs the preview handler to cease rendering a preview and to release all resources that have been allocated based on the item passed in during the initialization.
        /// </summary>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        int IPreviewHandler.Unload()
        {
            //  Log key events.
            Log("IPreviewHandler.DoPreview called.");

            //  If our preview handler already exists, dispose of it.
            if (previewHandlerControl != null)
            {
                try
                {
                    OnPreviewHostThread(
                        () =>
                            {
                                if (previewHandlerControl != null) previewHandlerControl.Dispose();
                            });
                    
                }
                catch (Exception exception)
                {
                    //  Log the error.
                    LogError("An exception occured when trying to dispose of an existing preview handler.", exception);
                }
            }

            //  TODO: If we have a stream, we must release that too.
            
            //  Return success.
            return WinError.S_OK;
        }

        /// <summary>
        /// Directs the preview handler to set focus to itself.
        /// </summary>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        int IPreviewHandler.SetFocus()
        {
            //  Log key events.
            Log("IPreviewHandler.SetFocus called.");

            //  TODO: according to MSDN we should set the focus to the last
            //  item in the control if shift is held down, we should automatically
            //  set focus to the first item here anyway.

            //  If we have a preview handler, focus it.
            if (previewHandlerControl != null)
                OnPreviewHostThread(() => previewHandlerControl.Focus());
                
            //  Return success.
            return WinError.S_OK;
        }

        /// <summary>
        /// Directs the preview handler to return the HWND from calling the GetFocus Function.
        /// </summary>
        /// <param name="phwnd">When this method returns, contains a pointer to the HWND returned from calling the GetFocus Function from the preview handler's foreground thread.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        int IPreviewHandler.QueryFocus(out IntPtr phwnd)
        {
            //  Log key events.
            Log("IPreviewHandler.QueryFocus called.");

            //  Default focus will be nothing.
            IntPtr focusHandle = IntPtr.Zero;

            //  If we have a preview handler, focus it.
            if (previewHandlerControl != null)
            {
                OnPreviewHostThread(() =>
                                    {
                                        var focusedControl = previewHandlerControl.FindFocusedControl();
                                        focusHandle = focusedControl != null ? focusedControl.Handle : IntPtr.Zero;
                                    });
            }

            phwnd = focusHandle;

            //  Return success.
            return WinError.S_OK;
        }

        /// <summary>
        /// Directs the preview handler to handle a keystroke passed up from the message pump of the process in which the preview handler is running.
        /// </summary>
        /// <param name="pmsg">A pointer to a window message.</param>
        /// <returns>
        /// If the keystroke message can be processed by the preview handler, the handler will process it and return S_OK. If the preview handler cannot process the keystroke message, it will offer it to the host using TranslateAccelerator. If the host processes the message, this method will return S_OK. If the host does not process the message, this method will return S_FALSE.
        /// </returns>
        int IPreviewHandler.TranslateAccelerator(MSG pmsg)
        {
            //  TODO: We must offer the ability to handle the translation ourselves - currently we'll just pass onto the frame.
            if (previewHandlerFrame != null)
                return previewHandlerFrame.TranslateAccelerator(pmsg);



            //  Return success.
            return WinError.S_OK;
        }

        #endregion
        
        #region Implementation of IPreviewHandlerVisuals

        /// <summary>
        /// Sets the background color of the preview handler.
        /// </summary>
        /// <param name="color">A value of type COLORREF to use for the preview handler background.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        int IPreviewHandlerVisuals.SetBackgroundColor(COLORREF color)
        {
            //  Log key events.
            Log("IPreviewHandlerVisuals.SetBackgroundColor called.");

            //  Call the virtual function.
            try
            {
                OnPreviewHostThread(
                    () => 
                    {
                        //  Set the background color of the host.
                        previewHandlerHost.BackColor = color.Color;

                        //  Call the abstract function.
                        if(previewHandlerControl != null)
                            previewHandlerControl.SetVisualsBackgroundColor(color.Color);
                    });
            }
            catch (Exception exception)
            {
                //  Log the error.
                LogError("An exception occured when setting the background color.", exception);
                throw;
            }

            //  Return success.
            return WinError.S_OK;
        }

        /// <summary>
        /// Sets the font attributes to be used for text within the preview handler.
        /// </summary>
        /// <param name="plf">A pointer to a LOGFONTW Structure containing the necessary attributes for the font to use.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        int IPreviewHandlerVisuals.SetFont(ref LOGFONT plf)
        {
            //  Log key events.
            Log("IPreviewHandlerVisuals.SetFont called.");

            //  Call the virtual function.
            try
            {
                LOGFONT logfont = plf;
                OnPreviewHostThread(
                    () =>
                        {
                            //  Call the abstract function.
                            if (previewHandlerControl != null)
                                previewHandlerControl.SetVisualsFont(Font.FromLogFont(logfont));
                        });
            }
            catch (Exception exception)
            {
                //  Log the error.
                LogError("An exception occured when setting the font.", exception);
                throw;
            }

            //  Return success.
            return WinError.S_OK;
        }

        /// <summary>
        /// Sets the color of the text within the preview handler.
        /// </summary>
        /// <param name="color">A value of type COLORREF to use for the preview handler text color.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error cod
        /// </returns>
        int IPreviewHandlerVisuals.SetTextColor(COLORREF color)
        {
            //  Log key events.
            Log("IPreviewHandlerVisuals.SetTextColor called.");

            //  Call the virtual function.
            try
            {
                OnPreviewHostThread(
                    () =>
                        {
                            //  Call the abstract function.
                            if (previewHandlerControl != null)
                                previewHandlerControl.SetVisualsTextColor(color.Color);
                        });
            }
            catch (Exception exception)
            {
                //  Log the error.
                LogError("An exception occured when setting the text color.", exception);
                throw;
            }

            //  Return success.
            return WinError.S_OK;
        }

        #endregion
        
        /// <summary>
        /// The custom registration function.
        /// </summary>
        /// <param name="serverType">Type of the server.</param>
        /// <param name="registrationType">Type of the registration.</param>
        [CustomRegisterFunction]
        internal static void CustomRegisterFunction(Type serverType, RegistrationType registrationType)
        {
            //  We will use the display name a few times.
            var displayName = DisplayNameAttribute.GetDisplayNameOrTypeName(serverType);

            //  Open the local machine.
            using (var localMachineBaseKey = registrationType == RegistrationType.OS64Bit
                ? RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64) :
                  RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            {
                //  Open the Preview Handlers.
                using (var previewHandlersKey = localMachineBaseKey
                    .OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\PreviewHandlers",
                    RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.WriteKey))
                {
                    //  If we don't have the key, we've got a problem.
                    if (previewHandlersKey == null)
                        throw new InvalidOperationException("Cannot open the PreviewHandlers key.");

                    //  Write the server guid as a name, and the display name as the value.
                    //  The display name isn't needed, it's just helpful for debugging and checking the registry.
                    previewHandlersKey.SetValue(serverType.GUID.ToRegistryString(), displayName);
                }
            }

            //  Open the classes root.
            using (var classesBaseKey = registrationType == RegistrationType.OS64Bit
                ? RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64) :
                  RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry32))
            {
                //  Our server guid.
                var serverGuid = serverType.GUID.ToRegistryString();

                //  Open the Class Key.
                using (var classKey = classesBaseKey
                    .OpenSubKey(string.Format(@"CLSID\{0}", serverGuid),
                    RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.WriteKey))
                {
                    //  If we don't have the key, we've got a problem.
                    if (classKey == null)
                        throw new InvalidOperationException("Cannot open the class key.");

                    //  Set the AppID as the preview host surrogate.
                    classKey.SetValue(null, serverType.Name);
                    classKey.SetValue("AppID", "{6d2b5079-2f0b-48dd-ab7f-97cec514d30b}");
                    
                    //  Set the display name and TODO icon.
                    classKey.SetValue("DisplayName", displayName, RegistryValueKind.String);
                    classKey.SetValue("Icon", "%SystemRoot%\\system32\\fontext.dll,10", RegistryValueKind.ExpandString);

                    //  Disable low integrity process isolation - TODO maybe this should be optional.
                    classKey.SetValue("DisableLowILProcessIsolation", 1, RegistryValueKind.DWord);
                }
            }
        }

        /// <summary>
        /// Customs the unregister function.
        /// </summary>
        /// <param name="serverType">Type of the server.</param>
        /// <param name="registrationType">Type of the registration.</param>
        [CustomUnregisterFunction]
        internal static void CustomUnregisterFunction(Type serverType, RegistrationType registrationType)
        {
            //  Open the local machine.
            using (var localMachineBaseKey = registrationType == RegistrationType.OS64Bit
                ? RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64) :
                  RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            {
                //  Open the Preview Handlers.
                using (var previewHandlersKey = localMachineBaseKey
                    .OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\PreviewHandlers",
                    RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.WriteKey | RegistryRights.ReadKey))
                {
                    //  If we don't have the key, we've got a problem.
                    if (previewHandlersKey == null)
                        throw new InvalidOperationException("Cannot open the PreviewHandlers key.");

                    //  If there's a value for the server, delete it.
                    var serverGuid = serverType.GUID.ToRegistryString();
                    if(previewHandlersKey.GetValueNames().Any(vm => vm == serverGuid))
                        previewHandlersKey.DeleteValue(serverGuid);
                }
            }
        }

        /// <summary>
        /// DoPreview must create the preview handler user interface and initialize it with data
        /// provided by the shell.
        /// </summary>
        /// <returns>The preview handler user interface.</returns>
        protected abstract PreviewHandlerControl DoPreview();

        /// <summary>
        /// The preview area.
        /// </summary>
        private RECT previewArea;

        /// <summary>
        /// The preview host handle.
        /// </summary>
        private IntPtr previewHostHandle;

        /// <summary>
        /// The preview handler host control.
        /// </summary>
        private readonly PreviewHandlerHost previewHandlerHost;

        /// <summary>
        /// The site provided by the system.
        /// </summary>
        private object site;

        /// <summary>
        /// The preview handler frame.
        /// </summary>
        private IPreviewHandlerFrame previewHandlerFrame;

        /// <summary>
        /// Gets the selected file path.
        /// </summary>
        public string SelectedFilePath { get; private set; }

        /// <summary>
        /// The preview handler control.
        /// </summary>
        private PreviewHandlerControl previewHandlerControl;
        
        /// <summary>
        /// Gets or sets a value indicating whether to automatically apply visuals.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if apply visuals automatically; otherwise, <c>false</c>.
        /// </value>
        public bool AutomaticallyApplyVisuals { get; set; }
    }
}
