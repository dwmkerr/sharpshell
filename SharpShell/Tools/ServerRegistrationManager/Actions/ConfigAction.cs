using System;
using System.Collections.Generic;
using System.Linq;
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
            outputService.WriteMessage($"Logging Mode: {config.LoggingMode}");
            outputService.WriteMessage($"Log Path: {config.LogPath}");
        }

        private static void SetConfig(IOutputService outputService, List<string> parameters)
        {
            //  We must have a key and value.
            if (parameters.Count != 2)
            {
                outputService.WriteError("Incorrect syntax. Use: srm config <setting> <value>");
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
                if (Enum.TryParse(value, true, out LoggingMode mode) == false)
                {
                    const LoggingMode allFlags = LoggingMode.Disabled | LoggingMode.Debug | LoggingMode.EventLog | LoggingMode.File;
                    outputService.WriteError($"Invalid value '{value}'. Acceptable values are: {allFlags}");
                    return;
                }

                //  Set the logging mode.
                config.LoggingMode = mode;

                //  Save back to the registry.
                SystemConfigurationProvider.Save();

                //  Update the user.
                outputService.WriteSuccess($"Set LoggingMode to {mode}");
            }
            else if (string.Compare("LogPath", setting, StringComparison.OrdinalIgnoreCase) == 0)
            {
                //  Set the path.
                config.LogPath = value;

                //  Save back to the registry.
                SystemConfigurationProvider.Save();

                //  Update the user.
                outputService.WriteSuccess($"Set LogPath to {value}");
            }
            else
            {
                //  Show an error.
                outputService.WriteError($"{value} is not a valid config setting. Valid settings are LoggingMode and LogPath.");
            }
        }
    }
}
