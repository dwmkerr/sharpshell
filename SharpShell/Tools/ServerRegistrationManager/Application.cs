using System;
using System.IO;
using System.Linq;
using ServerRegistrationManager.Actions;
using ServerRegistrationManager.OutputService;
using SharpShell;
using SharpShell.Diagnostics;
using SharpShell.Helpers;
using SharpShell.ServerRegistration;

namespace ServerRegistrationManager
{
    /// <summary>
    /// The main Server Registration Manager application.
    /// </summary>
    public class Application
    {
        /// <summary>
        /// The output service.
        /// </summary>
        private readonly IOutputService outputService;

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        /// <param name="outputService">The output service.</param>
        public Application(IOutputService outputService)
        {
            this.outputService = outputService;
        }

        /// <summary>
        /// Runs the specified application using the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void Run(string[] args)
        {
            Logging.Log("Started the Server Registration Manager.");

            //  Show the welcome.
            ShowWelcome();

            //  If we have no verb or target or our verb is help, show the help.
            if (args.Length == 0 || args.First() == VerbHelp)
            {
                ShowHelpAction.Execute(outputService);
                return;
            }

            //  Get the architecture.
            var registrationType = Environment.Is64BitOperatingSystem ? RegistrationType.OS64Bit : RegistrationType.OS32Bit;

            //  Get the verb, target and parameters.
            var verb = args[0];
            var target = args.Length > 1 ? args[1] : null; // TODO tidy this up.
            var parameters = args.Skip(1).ToArray();

            //Allow user to override registrationType with -os32 or -os64
            if (parameters.Any(p => p.Equals(ParameterOS32, StringComparison.InvariantCultureIgnoreCase)))
            {
                registrationType = RegistrationType.OS32Bit;
            }
            else if (parameters.Any(p => p.Equals(ParameterOS64, StringComparison.InvariantCultureIgnoreCase)))
            {
                registrationType = RegistrationType.OS64Bit;
            }

            var isExperimental = parameters.Any(p => p.Equals(ParameterExperimental, StringComparison.InvariantCultureIgnoreCase));
            var codebase = parameters.Any(p => p.Equals(ParameterCodebase, StringComparison.InvariantCultureIgnoreCase));

            //  Based on the verb, perform the action.
            if (verb == VerbInstall)
                if (isExperimental)
                    InstallServer(target, registrationType, codebase);
                else
                    InstallServerViaRegAsm(target, registrationType, codebase);
            else if (verb == VerbUninstall)
                if (isExperimental)
                    UninstallServer(target, registrationType);
                else
                    UninstallServerViaRegAsm(target, registrationType);
            else if (verb == VerbConfig)
                ConfigAction.Execute(outputService, parameters);
            else if (verb == VerbEnableEventLog)
                EnableEventLogAction.Execute(outputService);
            else
                ShowHelpAction.Execute(outputService);
        }

        /// <summary>
        /// Installs a SharpShell server at the specified path.
        /// </summary>
        /// <param name="path">The path to the SharpShell server.</param>
        /// <param name="registrationType">Type of the registration.</param>
        /// <param name="codeBase">if set to <c>true</c> install from codebase rather than GAC.</param>
        private void InstallServer(string path, RegistrationType registrationType, bool codeBase)
        {
            //  Validate the path.
            if (string.IsNullOrWhiteSpace(path) || File.Exists(path) == false)
            {
                outputService.WriteError("File '" + path + "' does not exist.", true);
                return;
            }

            bool success = true;
            try
            {
                //  Load any servers from the assembly.
                var servers = SharpShell.ServerRegistration.ServerRegistrationManager.EnumerateFromFile(path);

                foreach (var server in servers)
                {
                    var name = server.GetType().Name;
                    try
                    {
                        name = server.DisplayName;
                        SharpShell.ServerRegistration.ServerRegistrationManager.InstallServer(server, registrationType, codeBase);
                        SharpShell.ServerRegistration.ServerRegistrationManager.RegisterServer(server, registrationType);
                    }
                    catch (Exception e)
                    {
                        success = false;
                        outputService.WriteError(e.ToString());
                        outputService.WriteError($"Failed to install and register a server. [{name}]", true);
                    }
                }
            }
            catch (Exception e)
            {
                success = false;
                outputService.WriteError(e.ToString());
            }
            
            if (success)
            {
                outputService.WriteSuccess($"    {path} installed and registered.", true);
            }
            else
            {
                outputService.WriteError($"    {path} failed to register.", true);
            }
        }

        /// <summary>
        /// Installs a SharpShell server at the specified path via RegAsm.
        /// </summary>
        /// <param name="path">The path to the SharpShell server.</param>
        /// <param name="registrationType">Type of the registration.</param>
        /// <param name="codeBase">if set to <c>true</c> install from codebase rather than GAC.</param>
        private void InstallServerViaRegAsm(string path, RegistrationType registrationType, bool codeBase)
        {
            //  Validate the path.
            if (string.IsNullOrWhiteSpace(path) || File.Exists(path) == false)
            {
                outputService.WriteError("File '" + path + "' does not exist.", true);
                return;
            }

            var regAsm = new RegAsm();

            var success =
                registrationType == RegistrationType.OS32Bit
                    ? regAsm.Register32(path, codeBase)
                    : regAsm.Register64(path, codeBase);
            
            if (success)
            {
                outputService.WriteSuccess($"    {path} installed and registered.", true);
                outputService.WriteMessage(regAsm.StandardOutput);
            }
            else
            {
                outputService.WriteError($"    {path} failed to register.", true);
                outputService.WriteError(regAsm.StandardError);
            }
        }

        /// <summary>
        /// Uninstalls a SharpShell server located at 'path'.
        /// </summary>
        /// <param name="path">The path to the SharpShell server.</param>
        /// <param name="registrationType">Type of the registration.</param>
        private void UninstallServer(string path, RegistrationType registrationType)
        {
            //  Validate the path.
            if (string.IsNullOrWhiteSpace(path) || File.Exists(path) == false)
            {
                outputService.WriteError("File '" + path + "' does not exist.", true);
                return;
            }

            bool success = true;
            try
            {
                //  Load any servers from the assembly.
                var servers = SharpShell.ServerRegistration.ServerRegistrationManager.EnumerateFromFile(path);

                foreach (var server in servers)
                {
                    var name = server.GetType().Name;
                    try
                    {
                        name = server.DisplayName;
                        SharpShell.ServerRegistration.ServerRegistrationManager.UninstallServer(server, registrationType);
                        SharpShell.ServerRegistration.ServerRegistrationManager.UnregisterServer(server, registrationType);
                    }
                    catch (Exception e)
                    {
                        success = false;
                        outputService.WriteError(e.ToString());
                        outputService.WriteError($"Failed to uninstall and unregister a server. [{name}]", true);
                    }
                }
            }
            catch (Exception e)
            {
                success = false;
                outputService.WriteError(e.ToString());
            }

            if (success)
            {
                outputService.WriteSuccess($"    {path} uninstalled.", true);
            }
            else
            {
                outputService.WriteError($"    {path} failed to uninstall.", true);
            }
        }

        /// <summary>
        /// Uninstalls a SharpShell server located at 'path' vis RegAsm.
        /// </summary>
        /// <param name="path">The path to the SharpShell server.</param>
        /// <param name="registrationType">Type of the registration.</param>
        private void UninstallServerViaRegAsm(string path, RegistrationType registrationType)
        {
            //  Validate the path.
            if (string.IsNullOrWhiteSpace(path) || File.Exists(path) == false)
            {
                outputService.WriteError("File '" + path + "' does not exist.", true);
                return;
            }

            var regAsm = new RegAsm();

            var success =
                registrationType == RegistrationType.OS32Bit
                    ? regAsm.Unregister32(path)
                    : regAsm.Unregister64(path);
            
            if (success)
            {
                outputService.WriteSuccess($"    {path} uninstalled.", true);
                outputService.WriteMessage(regAsm.StandardOutput);
            }
            else
            {
                outputService.WriteError($"    {path} failed to uninstall.", true);
                outputService.WriteError(regAsm.StandardError);
            }
        }

        /// <summary>
        /// Shows the welcome message.
        /// </summary>
        private void ShowWelcome()
        {
            outputService.WriteMessage("");
            outputService.WriteMessage("========================================");
            outputService.WriteMessage("SharpShell - Server Registration Manager");
            outputService.WriteMessage("========================================");
            outputService.WriteMessage("");
        }
        
        private const string VerbHelp = @"help";
        private const string VerbInstall = @"install";
        private const string VerbUninstall = @"uninstall";
        private const string VerbConfig = @"config";
        private const string VerbEnableEventLog = @"enableeventlog";

        private const string ParameterCodebase = @"-codebase";

        private const string ParameterOS32 = @"-os32";
        private const string ParameterOS64 = @"-os64";

        private const string ParameterExperimental = @"-experimental";
    }
}
