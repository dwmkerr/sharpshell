using System;
using Microsoft.Win32;
using SharpShell.Registry;

namespace SharpShell.Diagnostics
{
    /// <summary>
    /// The ExplorerConfigurationManager can be used to manage explorer configuration relating to the shell.
    /// </summary>
    public class ExplorerConfigurationManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExplorerConfigurationManager"/> class.
        /// </summary>
        public ExplorerConfigurationManager()
        {
            //  Read the configuration.
            ReadConfiguration();
        }

        /// <summary>
        /// Checks the always unload DLL value.
        /// </summary>
        /// <returns>True if always unload dll is set.</returns>
        private static bool CheckAlwaysUnloadDll()
        {
            //  Get the registry service.
            var registry = ServiceRegistry.ServiceRegistry.GetService<IRegistry>();

            using (var localMachine = registry.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
            using (var alwaysUnloadDLLKey = localMachine.OpenSubKey(KeyName_AlwaysUnloadDll))
            {
                return alwaysUnloadDLLKey != null;
            }   
        }

        /// <summary>
        /// Checks the desktop process value.
        /// </summary>
        /// <returns>True if check desktop process is set.</returns>
        private static bool CheckDesktopProcess()
        {
            //  Get the registry service.
            var registry = ServiceRegistry.ServiceRegistry.GetService<IRegistry>();

            using (var currentUser = registry.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
            using (var explorerKey = OpenExporerSubkey(currentUser, RegistryKeyPermissionCheck.ReadSubTree))
            {
                //  Do we have the value?
                var value = explorerKey.GetValue(ValueName_DesktopProcess, null);

                //  If there's no value, or the value isn't a DWORD, it's not set.
                if (value == null || value is int == false)
                    return false;

                //  We can now check the value explicitly.
                return  ((int)value) == 1;
            }
        }

        /// <summary>
        /// Reads the configuration.
        /// </summary>
        private void ReadConfiguration()
        {
            alwaysUnloadDll = CheckAlwaysUnloadDll();
            desktopProcess = CheckDesktopProcess();

        }

        /// <summary>
        /// Sets the always unload DLL value.
        /// </summary>
        private void SetAlwaysUnloadDll()
        {
            //  Get the registry service.
            var registry = ServiceRegistry.ServiceRegistry.GetService<IRegistry>();

            //  Is the key there?
            bool valueSet = CheckAlwaysUnloadDll();

            //  If we're already set correctly, we're done.
            if (valueSet == alwaysUnloadDll)
                return;

            if (alwaysUnloadDll)
            {
                //  Open the explorer key and create the always unload key.
                using (var localMachine = registry.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
                using (var explorerKey = OpenExporerSubkey(localMachine, RegistryKeyPermissionCheck.ReadWriteSubTree))
                    explorerKey.CreateSubKey(SubKeyName_AlwaysUnloadDLL);
            }
            else
            {
                //  Open the explorer key and delete the always unload key.
                using (var localMachine = registry.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
                using (var explorerKey = OpenExporerSubkey(localMachine, RegistryKeyPermissionCheck.ReadWriteSubTree))
                    explorerKey.DeleteSubKeyTree(SubKeyName_AlwaysUnloadDLL);
            }
        }

        /// <summary>
        /// Sets the desktop process value.
        /// </summary>
        private void SetDesktopProcess()
        {
            //  Get the registry service.
            var registry = ServiceRegistry.ServiceRegistry.GetService<IRegistry>();

            //  Is the key there?
            bool valueSet = CheckDesktopProcess();

            //  If we're already set correctly, we're done.
            if (valueSet == desktopProcess)
                return;

            if (desktopProcess)
            {
                //  Open the explorer key and create the value.
                using (var currentUser = registry.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
                using (var explorerKey = OpenExporerSubkey(currentUser, RegistryKeyPermissionCheck.ReadWriteSubTree))
                    explorerKey.SetValue(ValueName_DesktopProcess, 1, RegistryValueKind.DWord);
            }
            else
            {
                //  Delete the key value.
                using (var currentUser = registry.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
                using (var explorerKey = OpenExporerSubkey(currentUser, RegistryKeyPermissionCheck.ReadWriteSubTree))
                    explorerKey.DeleteValue(ValueName_DesktopProcess);
            }
        }

        /// <summary>
        /// Opens the exporer subkey.
        /// </summary>
        /// <param name="hiveKey">The hive key.</param>
        /// <param name="permissionCheck">The permission check.</param>
        /// <returns>
        /// The explorer subkey.
        /// </returns>
        private static IRegistryKey OpenExporerSubkey(IRegistryKey hiveKey, RegistryKeyPermissionCheck permissionCheck)
        {
            //  Open the explorer key with the desired permissions.
            var explorerKey = hiveKey.OpenSubKey(KeyName_Explorer, permissionCheck);
            
            //  If we don't have it, we've got a critical error.
            if (explorerKey == null)
                throw new InvalidOperationException("Unable to open the Explorer key.");

            //  Return the key.
            return explorerKey;
        }

        /// <summary>
        /// The AlwaysUnloadDLL key name.
        /// </summary>
        private const string KeyName_AlwaysUnloadDll = @"Software\Microsoft\Windows\CurrentVersion\Explorer\AlwaysUnloadDLL";

        /// <summary>
        /// The AlwaysUnloadDLL sub key name.
        /// </summary>
        private const string SubKeyName_AlwaysUnloadDLL = @"AlwaysUnloadDLL";

        /// <summary>
        /// The windows explorer key namme.
        /// </summary>
        private const string KeyName_Explorer = @"Software\Microsoft\Windows\CurrentVersion\Explorer";

        /// <summary>
        /// The dekstop process value name.
        /// </summary>
        private const string ValueName_DesktopProcess = @"DesktopProcess";

        /// <summary>
        /// The always unload dll flag.
        /// </summary>
        private bool alwaysUnloadDll;

        /// <summary>
        /// The desktop process flag.
        /// </summary>
        private bool desktopProcess;

        /// <summary>
        /// Gets or sets a value indicating whether always unload DLL is set.
        /// </summary>
        /// <value>
        ///   <c>true</c> if always unload DLL is set; otherwise, <c>false</c>.
        /// </value>
        public bool AlwaysUnloadDll
        {

            get { return alwaysUnloadDll; }
            set
            {
                alwaysUnloadDll = value;
                SetAlwaysUnloadDll();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether desktop process is set.
        /// </summary>
        /// <value>
        ///   <c>true</c> if desktop process is set; otherwise, <c>false</c>.
        /// </value>
        public bool DesktopProcess
        {
            get { return desktopProcess; }
            set 
            { 
                desktopProcess = value;
                SetDesktopProcess();
            }
        }
    }
}
