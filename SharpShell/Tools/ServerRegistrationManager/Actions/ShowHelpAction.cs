using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ServerRegistrationManager.OutputService;

namespace ServerRegistrationManager.Actions
{
    /// <summary>
    /// Action to show help.
    /// </summary>
    public static class ShowHelpAction
    {
        /// <summary>
        /// Shows the help message.
        /// </summary>
        public static void Execute(IOutputService outputService)
        {
            var resourceName = string.Format("{0}.Actions.Help.txt", typeof(ShowHelpAction).Assembly.GetName().Name);

            //  Load the resource.
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new InvalidOperationException("Unable to access resource " + resourceName);
                using (var reader = new StreamReader(stream))
                {
                    outputService.WriteMessage(reader.ReadToEnd());
                }
            }

            outputService.WriteMessage("To get help:");
            outputService.WriteMessage("    srm help");
            outputService.WriteMessage("");
            outputService.WriteMessage("To install a server:");
            outputService.WriteMessage("    srm install <path to SharpShell server> <parameters>");
            outputService.WriteMessage("Parameters:");
            outputService.WriteMessage("    -codebase: Optional. Installs a server from a file location, not the GAC.");
            outputService.WriteMessage("    -os[32|64]: Optional. Forces 32 bit or 64 bit installation (Ignores Environment.Is64BitOperatingSystem).");
            outputService.WriteMessage("");
            outputService.WriteMessage("To uninstall a server:");
            outputService.WriteMessage("    srm uninstall <path to SharpShell server>");
            outputService.WriteMessage("");
        }
    }
}
