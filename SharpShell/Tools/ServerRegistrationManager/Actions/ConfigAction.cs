using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using ServerRegistrationManager.OutputService;
using SharpShell.Configuration;

namespace ServerRegistrationManager.Actions
{
    /// <summary>
    /// Action to show/edit SharpShell config.
    /// </summary>
    public static class ConfigAction
    {
        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="outputService">The output service.</param>
        /// <param name="parameters">The parameters.</param>
        public static void Execute(IOutputService outputService, IEnumerable<string> parameters)
        {
            //  Enumerate the parameters.
            var parametersList = parameters.ToList();

            //  If we have no parameters, we show config.
            if (parametersList.Any() == false)
                ShowConfig(outputService);
            else
                SetConfig(outputService, parametersList);

        }

        private static void ShowConfig(IOutputService outputService)
        {
            //  Get the config.
            var config = SystemConfigurationProvider.Configuration;

            //  If config is not present, let the user know and we're done.
            if (config.IsConfigurationPresent == false)
            {
                outputService.WriteMessage("SharpShell configuration not present on this system.");
                return;
            }

            //  Show the config.
            outputService.WriteMessage(string.Format("Logging Mode: {0}", config.LoggingMode));
            outputService.WriteMessage(string.Format("Log Path: {0}", config.LogPath));
        }

        private static void SetConfig(IOutputService outputService, List<string> parameters)
        {
            //  We must have a key and value.
            if (parameters.Count != 2)
            {
                outputService.WriteError(string.Format("Incorrect syntax. Use: srm config <setting> <value>"));
                return;
            }

            //  Get the setting and value.
            var setting = parameters[0];
            var value = parameters[1];

            //  Get the config.
            var config = SystemConfigurationProvider.Configuration;

            //  Set the setting.
            if (string.Compare("LoggingMode", setting, StringComparison.OrdinalIgnoreCase) == 0)
            {
                //  Try and parse the setting. If we fail, show an error.
                LoggingMode mode;
                if (Enum.TryParse(value, true, out mode) == false)
                {
                    const LoggingMode allFlags = LoggingMode.Disabled | LoggingMode.Debug | LoggingMode.EventLog | LoggingMode.File;
                    outputService.WriteError(string.Format("Invalid value '{0}'. Acceptible values are: {1}", value, allFlags));
                    return;
                }

                //  Set the logging mode.
                config.LoggingMode = mode;

                //  Save back to the registry.
                SystemConfigurationProvider.Save();

                //  Update the user.
                outputService.WriteSuccess(string.Format("Set LoggingMode to {0}", mode));
            }
            else if (string.Compare("LogPath", setting, StringComparison.OrdinalIgnoreCase) == 0)
            {
                //  Set the path.
                config.LogPath = value;

                //  Save back to the registry.
                SystemConfigurationProvider.Save();

                //  Update the user.
                outputService.WriteSuccess(string.Format("Set LogPath to {0}", value));
            }
            else
            {
                //  Show an error.
                outputService.WriteError(string.Format("{0} is not a valid config setting. Valid settings are LoggingMode and LogPath.", value));
            }
        }
    }
}
