using System;
using System.IO;
using System.Linq;
using ServerRegistrationManager.Actions;
using ServerRegistrationManager.OutputService;
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

            //  Based on the verb, perform the action.
            if (verb == VerbInstall)
                InstallServer(target, registrationType, parameters.Any(p => p == ParameterCodebase));
            else if (verb == VerbUninstall)
                UninstallServer(target, registrationType);
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
            if (File.Exists(path) == false)
            {
                outputService.WriteError("File '" + path + "' does not exist.", true);
                return;
            }

            var regasm = new RegAsm();
            var success = registrationType == RegistrationType.OS32Bit ? regasm.Register32(path, codeBase) : regasm.Register64(path, codeBase);

            if (success)
            {
                outputService.WriteSuccess($"    {path} installed and registered.", true);
                outputService.WriteMessage(regasm.StandardOutput);
            }
            else
            {
                outputService.WriteError($"    {path} failed to register.", true);
                outputService.WriteError(regasm.StandardError);
            }
        }

        /// <summary>
        /// Uninstalls a SharpShell server located at 'path'.
        /// </summary>
        /// <param name="path">The path to the SharpShell server.</param>
        /// <param name="registrationType">Type of the registration.</param>
        private void UninstallServer(string path, RegistrationType registrationType)
        {
            var regasm = new RegAsm();
            var success = registrationType == RegistrationType.OS32Bit ? regasm.Unregister32(path) : regasm.Unregister64(path);

            if (success)
            {
                outputService.WriteSuccess($"    {path} uninstalled.", true);
                outputService.WriteMessage(regasm.StandardOutput);
            }
            else
            {
                outputService.WriteError($"    {path} failed to uninstall.", true);
                outputService.WriteError(regasm.StandardError);
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
    }
}
