using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerRegistrationManager.OutputService;
using SharpShell.Configuration;

namespace ServerRegistrationManager.Actions
{
    /// <summary>
    /// Action to show SharpShell config.
    /// </summary>
    public static class ShowConfigAction
    {
        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="outputService">The output service.</param>
        public static void Execute(IOutputService outputService)
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
    }
}
