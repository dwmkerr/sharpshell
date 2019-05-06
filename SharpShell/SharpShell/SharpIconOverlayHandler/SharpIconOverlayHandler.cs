using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using Microsoft.Win32;
using SharpShell.Attributes;
using SharpShell.Diagnostics;
using SharpShell.Extensions;
using SharpShell.Interop;
using SharpShell.ServerRegistration;

namespace SharpShell.SharpIconOverlayHandler
{
    /// <summary>
    /// The SharpIconHandler is the base class for SharpShell servers that provide
    /// custom Icon Handlers.
    /// </summary>
    [ServerType(ServerType.ShellIconOverlayHandler)]
    public abstract class SharpIconOverlayHandler : SharpShellServer, IShellIconOverlayIdentifier
    {
        #region Implementation of IShellIconOverlayIdentifier

        /// <summary>
        /// Specifies whether an icon overlay should be added to a Shell object's icon.
        /// </summary>
        /// <param name="pwszPath">A Unicode string that contains the fully qualified path of the Shell object.</param>
        /// <param name="dwAttrib">The object's attributes. For a complete list of file attributes and their associated flags, see IShellFolder::GetAttributesOf.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        int IShellIconOverlayIdentifier.IsMemberOf(string pwszPath, FILE_ATTRIBUTE dwAttrib)
        {
            //  Always wrap calls to the abstract functions in exception handlers.
            try
            {
                //  Can we show the overla?
                var canShowOverly = CanShowOverlay(pwszPath, dwAttrib);

                //  DebugLog this key event.
                Log(string.Format("IsMemberOf for {0} - returning {1}", pwszPath, canShowOverly));

                //  Return OK if we should show the overlay, otherwise false.
                return canShowOverly ? WinError.S_OK : WinError.S_FALSE;
            }
            catch (Exception exception)
            {
                //  DebugLog the exception.
                LogError("IsMemberOf: An exception occured when determining whether to show the overlay for '" + pwszPath + "'.", exception);

                //  Return false so we don't try and get the icon for a server that is erroring.
                return WinError.S_FALSE;
            }
        }

        /// <summary>
        /// Provides the location of the icon overlay's bitmap.
        /// </summary>
        /// <param name="pwszIconFile">A null-terminated Unicode string that contains the fully qualified path of the file containing the icon. The .dll, .exe, and .ico file types are all acceptable. You must set the ISIOI_ICONFILE flag in pdwFlags if you return a file name.</param>
        /// <param name="cchMax">The size of the pwszIconFile buffer, in Unicode characters.</param>
        /// <param name="pIndex">Pointer to an index value used to identify the icon in a file that contains multiple icons. You must set the ISIOI_ICONINDEX flag in pdwFlags if you return an index.</param>
        /// <param name="pdwFlags">Pointer to a bitmap that specifies the information that is being returned by the method. This parameter can be one or both of the following values: ISIOI_ICONFILE, ISIOI_ICONINDEX.</param>
        /// <returns>
        /// If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        int IShellIconOverlayIdentifier.GetOverlayInfo(IntPtr pwszIconFile, int cchMax, out int pIndex, out ISIOI pdwFlags)
        {
            //  Set the out variables to sensible defaults.
            pdwFlags = 0;
            pIndex = 0;

            try
            {
                //  Get the icon file path.
                var iconFilePath = GetIconFilePath();

                //  If we have no file, we must fail.
                if (string.IsNullOrEmpty(iconFilePath))
                {
                    LogError("Cannot create the overlay icon path.");
                    return WinError.E_FAIL;
                }

                //  Get the file path bytes.
                var bytes = Encoding.Unicode.GetBytes(iconFilePath);

                if (bytes.Length + 2 < cchMax)
                {
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        Marshal.WriteByte(pwszIconFile, i, bytes[i]);
                    }
                    Marshal.WriteByte(pwszIconFile, bytes.Length, 0);
                    Marshal.WriteByte(pwszIconFile, bytes.Length + 1, 0);
                }

                //  Set the flags, specifying we'll provide a path.
                pdwFlags = ISIOI.ISIOI_ICONFILE;

                return WinError.S_OK;
            }
            catch (Exception ex)
            {
                LogError("An exception occured getting overlay info.", ex);
                return WinError.E_FAIL;
            }
        }

        /// <summary>
        /// Specifies the priority of an icon overlay.
        /// </summary>
        /// <param name="pPriority">The address of a value that indicates the priority of the overlay identifier. Possible values range from zero to 100, with zero the highest priority.</param>
        /// <returns>
        /// Returns S_OK if successful, or a COM error code otherwise.
        /// </returns>
        int IShellIconOverlayIdentifier.GetPriority(out int pPriority)
        {
            //  DebugLog this key event.
            Log("GetPriority called to get icon overlay priority.");

            //  By default, we'll set the lowest priority.
            pPriority = 100;

            //  Try can set the priority using the abstract function.
            try
            {
                //  Get the priority.
                var priority = GetPriority();

                //  Set the limit.
                if (priority < 0)
                    priority = 0;
                else if (priority > 100)
                    priority = 100;

                //  Set priority.
                pPriority = priority;
            }
            catch (Exception exception)
            {
                //  DebugLog the exception.
                LogError("An exception occured when trying to get the priority.", exception);
            }

            //  Return success.
            return WinError.S_OK;
        }

        #endregion

        /// <summary>
        /// Gets the icon file path, creating the icon file if needed.
        /// </summary>
        /// <returns>The icon file path.</returns>
        private string GetIconFilePath()
        {
            //  If we're not in debug mode and we've already created the temporary icon file,
            //  we can return it. If we're in debug mode, we'll always create it.
#if !DEBUG
            if(!string.IsNullOrEmpty(temporaryIconOverlayFilePath) && File.Exists(temporaryIconOverlayFilePath))
            {
                //  Set the icon file path.
                return temporaryIconOverlayFilePath;
            }
#endif

            //  Storage for the overlay icon.
            Icon overlayIcon;

            //  Always wrap calls to the abstract functions in exception handlers.
            try
            {
                //  Get the overlay icon.
                overlayIcon = GetOverlayIcon();
            }
            catch (Exception exception)
            {
                //  DebugLog the exception.
                LogError("An exception occured when trying to get the overlay icon.", exception);

                return null;
            }

            //  Create a temporary icon file path.
            try
            {
                CreateTemporaryIconFilePath();

                //  Save the file object to the icon file path.
                using (var fileStream = new FileStream(temporaryIconOverlayFilePath, FileMode.Create))
                {
                    //  Save the icon to the stream, then flush the stream.
                    overlayIcon.Save(fileStream);
                    fileStream.Flush(true);
                }

                //  Set the icon file path.
                return temporaryIconOverlayFilePath;
            }
            catch (Exception exception)
            {
                //  DebugLog the exception.
                LogError("An exception occured when trying to create the overlay icon.", exception);

                return null;
            }
        }

        /// <summary>
        /// The custom registration function.
        /// </summary>
        /// <param name="serverType">Type of the server.</param>
        /// <param name="registrationType">Type of the registration.</param>
        [CustomRegisterFunction]
        internal static void CustomRegisterFunction(Type serverType, RegistrationType registrationType)
        {
            var keyName = RegistrationNameAttribute.GetRegistrationNameOrTypeName(serverType);
            Logging.Log($"IconOverlayHandler: Preparing to register {registrationType} Icon Overlay Handler for type '{serverType.Name}' with key name '{keyName}'");

            //  Open the local machine.
            using (var localMachineBaseKey = registrationType == RegistrationType.OS64Bit
                ? RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64) :
                  RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            {
                //  Open the ShellIconOverlayIdentifiers.
                using(var overlayIdentifiers = localMachineBaseKey
                    .OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\ShellIconOverlayIdentifiers",
                    RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.EnumerateSubKeys | RegistryRights.QueryValues | RegistryRights.CreateSubKey | RegistryRights.CreateSubKey))
                {
                    //  If we don't have the key, we've got a problem.
                    if(overlayIdentifiers == null)
                        throw new InvalidOperationException("Cannot open the ShellIconOverlayIdentifiers key.");

                    //  How many shell icon overlay identifiers do we have?
                    var overlayHandlersCount = overlayIdentifiers.GetSubKeyNames().Count();
                    if(overlayHandlersCount >= MaximumOverlayIdentifiers)
                        Logging.Error("There are already the maximum number of overlay " +
                            "handlers registered for the system. Although " + serverType.Name + " is " +
                                                  "being registered, it will not be used by Windows Explorer.");

                    //  Create the overlay key.
                    using (var overlayKey = overlayIdentifiers.CreateSubKey(keyName))
                    {
                        //  If we don't have the overlay key, we've got a problem.
                        if(overlayKey == null)
                            throw new InvalidOperationException("Cannot create the key for the overlay server.");

                        //  Set the server CLSID.
                        overlayKey.SetValue(null, serverType.GUID.ToRegistryString());
                    }
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
            var keyName = RegistrationNameAttribute.GetRegistrationNameOrTypeName(serverType);
            Logging.Log($"IconOverlayHandler: Preparing to unregister {registrationType} Icon Overlay Handler for type '{serverType.Name}' with key name '{keyName}'");

            //  Open the local machine.
            using (var localMachineBaseKey = registrationType == RegistrationType.OS64Bit
                ? RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64) :
                  RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            {
                //  Open the ShellIconOverlayIdentifiers.
                using (var overlayIdentifiers = localMachineBaseKey
                    .OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\ShellIconOverlayIdentifiers",
                    RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.Delete | RegistryRights.EnumerateSubKeys | RegistryRights.ReadKey))
                {
                    //  If we don't have the key, we've got a problem.
                    if (overlayIdentifiers == null)
                        throw new InvalidOperationException("Cannot open the ShellIconOverlayIdentifiers key.");

                    //  Delete the overlay key.
                    if (overlayIdentifiers.GetSubKeyNames().Any(skn => skn == keyName))
                        overlayIdentifiers.DeleteSubKey(keyName);
                }
            }
        }

        /// <summary>
        /// Creates the temporary icon file path.
        /// </summary>
        private void CreateTemporaryIconFilePath()
        {
            //  Create the temporary icon file path.
            temporaryIconOverlayFilePath = Path.Combine(Path.GetTempPath(), GetType().Name + ".ico");

            //  If the path exists (which is possible when debugging or if the server has run before)
            //  then delete it.
            //  TODO: Is this safe in a multi-threaded shell?
            if(File.Exists(temporaryIconOverlayFilePath))
                File.Delete(temporaryIconOverlayFilePath);
        }

        /// <summary>
        /// Called by the system to get the priority, which is used to determine
        /// which icon overlay to use if there are multiple handlers. The priority
        /// must be between 0 and 100, where 0 is the highest priority.
        /// </summary>
        /// <returns>A value between 0 and 100, where 0 is the highest priority.</returns>
        protected abstract int GetPriority();

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
        protected abstract bool CanShowOverlay(string path, FILE_ATTRIBUTE attributes);

        /// <summary>
        /// Called to get the icon to show as the overlay icon.
        /// </summary>
        /// <returns>The overlay icon.</returns>
        protected abstract Icon GetOverlayIcon();

        /// <summary>
        /// The temporary icon overlay file path.
        /// </summary>
        private string temporaryIconOverlayFilePath;

        /// <summary>
        /// The maximum number of overlay identifiers in Windows.
        /// </summary>
        private const int MaximumOverlayIdentifiers = 15;
    }
}
