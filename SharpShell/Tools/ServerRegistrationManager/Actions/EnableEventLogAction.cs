using ServerRegistrationManager.OutputService;
using SharpShell.Diagnostics.Loggers;

namespace ServerRegistrationManager.Actions
{
    /// <summary>
    /// Action to enable the event log.
    /// </summary>
    public static class EnableEventLogAction
    {
        /// <summary>
        /// Enables the event log.
        /// </summary>
        public static void Execute(IOutputService outputService)
        {
            //  SRM runs as an admin, so using the event logger will create the event log.
            //  The event log will be created successfully as we have elevated priviledges.
            var logger = new EventLogLogger();
            logger.LogMessage("Enabling the Event Log for SharpShell.");

            //  Let the user know we're in business.
            outputService.WriteSuccess("Event log enabled.");
        }
    }
}
