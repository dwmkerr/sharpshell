using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerRegistrationManager.OutputService;
using SharpShell;
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
            //  Show the welcome.
            ShowWelcome();

            //  If we have no verb or target or our verb is help, show the help.
            if (args.Length < 2 || args.First() == VerbHelp)
            {
                ShowHelp();
                return;
            }

            //  Get the architecture.
            var registrationType = Environment.Is64BitOperatingSystem ? RegistrationType.OS64Bit : RegistrationType.OS32Bit;

            //  Get the verb, target and parameters.
            var verb = args[0];
            var target = args[1];
            var parameters = args.Skip(2);

            //  Based on the verb, perform the action.
            if (verb == VerbInstall)
                InstallServer(target, registrationType, parameters.Any(p => p == ParameterCodebase));
            else if (verb == VerbUninstall)
                UninstallServer(target, registrationType);
            else
                ShowHelp();
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
                outputService.WriteError("File '" + path + "' does not exist.");
                return;
            }
            
            //  Try and load the server types.
            IEnumerable<ISharpShellServer> serverTypes = null;
            try
            {
                serverTypes = LoadServerTypes(path);
            }
            catch (Exception e)
            {
                outputService.WriteError("An unhandled exception occured when loading the SharpShell");
                outputService.WriteError("Server Types from the specified assembly. Is it a SharpShell");
                outputService.WriteError("Server Assembly?");
                System.Diagnostics.Trace.Write(e);
            }

            //  Install each server type.
            foreach (var serverType in serverTypes)
            {
                //  Inform the user we're going to install the server.
                outputService.WriteMessage("Preparing to install (" + registrationType + "): " + serverType.DisplayName);

                //  Install the server.
                SharpShell.ServerRegistration.ServerRegistrationManager.InstallServer(serverType, registrationType, codeBase);
                outputService.WriteMessage("Installed OK... Preparing to register...");
                SharpShell.ServerRegistration.ServerRegistrationManager.RegisterServer(serverType, registrationType);
                outputService.WriteMessage("Registered OK.");
            }
        }

        /// <summary>
        /// Uninstalls a SharpShell server located at 'path'.
        /// </summary>
        /// <param name="path">The path to the SharpShell server.</param>
        /// <param name="registrationType">Type of the registration.</param>
        private void UninstallServer(string path, RegistrationType registrationType)
        {
            //  Try and load the server types.
            IEnumerable<ISharpShellServer> serverTypes = null;
            try
            {
                serverTypes = LoadServerTypes(path);
            }
            catch (Exception e)
            {
                outputService.WriteError("An unhandled exception occured when loading the SharpShell");
                outputService.WriteError("Server Types from the specified assembly. Is it a SharpShell");
                outputService.WriteError("Server Assembly?");
                System.Diagnostics.Trace.Write(e);
            }

            //  Install each server type.
            foreach (var serverType in serverTypes)
            {
                //  Inform the user we're going to install the server.
                Console.WriteLine("Preparing to uninstall (" + registrationType + "): " + serverType.DisplayName);

                //  Install the server.
                SharpShell.ServerRegistration.ServerRegistrationManager.UnregisterServer(serverType, registrationType);
                outputService.WriteMessage("Unregisterd OK... Preparing to uninstall...");
                SharpShell.ServerRegistration.ServerRegistrationManager.UninstallServer(serverType, registrationType);
                outputService.WriteMessage("Uninstalled OK.");
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

        /// <summary>
        /// Shows the help message.
        /// </summary>
        private void ShowHelp()
        {
            outputService.WriteMessage("To get help:");
            outputService.WriteMessage("    ServerRegistrationManager help");
            outputService.WriteMessage("");
            outputService.WriteMessage("To install a server:");
            outputService.WriteMessage("    ServerRegistrationManager install <path to SharpShell server> <parameters>");
            outputService.WriteMessage("Parameters:");
            outputService.WriteMessage("    -codebase: Optional. Installs a server from a file location, not the GAC.");
            outputService.WriteMessage("");
            outputService.WriteMessage("To uninstall a server:");
            outputService.WriteMessage("    ServerRegistrationManager uninstall <path to SharpShell server>");
            outputService.WriteMessage("");
        }

        /// <summary>
        /// Loads the server types from an assembly.
        /// </summary>
        /// <param name="assemblyPath">The assembly path.</param>
        /// <returns>The SharpShell Server types from the assembly.</returns>
        private static IEnumerable<ISharpShellServer> LoadServerTypes(string assemblyPath)
        {
            //  Create an assembly catalog for the assembly and a container from it.
            var catalog = new AssemblyCatalog(Path.GetFullPath(assemblyPath));
            var container = new CompositionContainer(catalog);

            //  Get all exports of type ISharpShellServer.
            return container.GetExports<ISharpShellServer>().Select(st => st.Value);
        }

        private const string VerbHelp = @"help";
        private const string VerbInstall = @"install";
        private const string VerbUninstall = @"uninstall";

        private const string ParameterCodebase = @"-codebase";
    }
}
